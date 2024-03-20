using Npgsql;

// sudo systemctl start docker
// docker run -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres
// docker exec -it <container_id> /bin/bash

const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;";

using (var source = NpgsqlDataSource.Create(ConnectionString))
{
    source.CreateCommand("DROP DATABASE IF EXISTS testdb;").ExecuteNonQuery();
    source.CreateCommand("CREATE DATABASE testdb;").ExecuteNonQuery();
}
