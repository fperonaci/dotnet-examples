using NetTopologySuite.Geometries;
using Npgsql;

// docker run -e POSTGRES_PASSWORD=postgres -p 5433:5432 -d postgis/postgis

const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;";

using (var source = NpgsqlDataSource.Create(ConnectionString))
{
    source.CreateCommand("DROP DATABASE IF EXISTS testdb;").ExecuteNonQuery();
    source.CreateCommand("CREATE DATABASE testdb;").ExecuteNonQuery();
}

var builder = new NpgsqlDataSourceBuilder(ConnectionString + "Database=testdb;");
builder.UseNetTopologySuite();

using (var source = builder.Build())
{
    source.CreateCommand("CREATE EXTENSION IF NOT EXISTS postgis;").ExecuteNonQuery();
    source.CreateCommand("CREATE TABLE data (geom GEOMETRY);").ExecuteNonQuery();

    using var conn = builder.Build().OpenConnection();

    using (var cmd = new NpgsqlCommand("INSERT INTO data (geom) VALUES ($1)", conn))
    {
        var point = new Point(1.2d, 9.8d);
        cmd.Parameters.Add(new() { Value = point });
        cmd.ExecuteNonQuery();
    }

    using (var cmd = new NpgsqlCommand("SELECT geom FROM data", conn))
    {
        using var reader = cmd.ExecuteReader();
        reader.Read();
        var point = reader.GetFieldValue<Point>(0);
        Console.WriteLine(point.X);
        Console.WriteLine(point.Y);
    }
}
