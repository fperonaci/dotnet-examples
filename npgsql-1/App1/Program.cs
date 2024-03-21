using Npgsql;

// sudo systemctl start docker
// docker run -e POSTGRES_PASSWORD=postgres -p 5433:5432 -d postgres
// # syntax for port forward is -p <host_port>:<container_port>
// # be sure to use an unused host port
// # netstat -lntu # list ports on linux
// docker exec -it <container_id> /bin/bash  # on linux
// docker exec -it <container_id> /bin/bash  # on windows powershell
// docker exec -it <container_id> //bin/bash # on windows git bash

const string ConnectionString = "Host=localhost;Port=5433;Username=postgres;Password=postgres;";

// NpsqlDataSource.Create(string) = new NpgsqlDataSourceBuilder(string).Build()
// Sometimes you want to use the latter to add, e.g. NetTopologySuite()

using (var source = NpgsqlDataSource.Create(ConnectionString))
{
    source.CreateCommand("DROP DATABASE IF EXISTS testdb;").ExecuteNonQuery();
    source.CreateCommand("CREATE DATABASE testdb;").ExecuteNonQuery();
}

using (var source = NpgsqlDataSource.Create(ConnectionString + "Database=testdb;"))
{
    // Maybe better to open a connection instead of repeteadly using source.CreateCommand ?

    source.CreateCommand("CREATE TABLE things (id INTEGER PRIMARY KEY, name TEXT);").ExecuteNonQuery();
    source.CreateCommand("INSERT INTO things (id, name) VALUES (1, \'fork\');").ExecuteNonQuery();
    source.CreateCommand("INSERT INTO things (id, name) VALUES (3, \'spoon\');").ExecuteNonQuery();
    source.CreateCommand("INSERT INTO things (id, name) VALUES (9, \'knife\');").ExecuteNonQuery();

    using (var reader = source.CreateCommand("SELECT * FROM things;").ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine(reader.GetInt32(0) + " " + reader.GetString(1));
        }
    }

    // Binary writer

    using (var connection = source.OpenConnection())
    {
        var writer = connection.BeginBinaryImport("COPY things (id, name) FROM STDIN (FORMAT BINARY)");
        writer.StartRow();
        writer.Write(10);
        writer.Write("glass");
        writer.StartRow();
        writer.Write(13);
        writer.Write("cup");
        writer.Complete();
    }

    using (var reader = source.CreateCommand("SELECT * FROM things;").ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine(reader.GetInt32(0) + " " + reader.GetString(1));
        }
    }
}
