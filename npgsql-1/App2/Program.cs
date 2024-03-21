using Npgsql;

// This does not work anymore

// docker build -t herenow .
// docker run -d -p 5433:5432 herenow

var connectionString =
  "Host=localhost;" +
  "Port=5433;" +
  "Username=postgres;" +
  "Password=postgres;" +
  "Database=testdb;";

using var connection = new NpgsqlConnection(connectionString);

connection.Open();

var cmd = connection.CreateCommand();

cmd.CommandText =
  "create type mood as enum ('happy', 'sad');" +
  "create table people (age integer, cmood mood);";

// cmd.ExecuteNonQuery();

// connection.ReloadTypes();

var copyCmd =
  "copy people (age, cmood)" +
  "from stdin (format binary)";

using var writer = connection.BeginBinaryImport(copyCmd);

writer.StartRow();
writer.Write(12);
writer.Write(Mood.Happy);
writer.Complete();

enum Mood
{
  Null = 0,
  Sad = 1,
  Happy = 2
}
