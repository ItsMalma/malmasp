namespace Malmasp.Options;

public class PostgresqlOptions
{
    public required string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 5432;
    private string _database = string.Empty;
    public string Database {
        get => _database == string.Empty ? Username : _database;
        set => _database = value;
    }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = "PGPASSWORD";
    public string Passfile { get; set; } = string.Empty;
}