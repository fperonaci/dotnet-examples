using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Npgsql;

// docker run -e POSTGRES_PASSWORD=postgres -p 5433:5432 -d postgis/postgis

const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;";

var WKBWriter = new WKBWriter();

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

    using (var writer = conn.BeginBinaryImport("COPY data (geom) FROM STDIN (FORMAT BINARY)"))
    {
        var point = new Point(1.3d, 9.7d);
        writer.StartRow();
        writer.Write(WKBWriter.Write(point));
        // writer.Write(point); works as well ? it seems so
        writer.Complete();
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
