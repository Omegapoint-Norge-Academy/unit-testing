namespace ConsumerBank.Services.Options;

public class DbOptions
{
    public DbOptions(string database, string dbUsername, string dbPassword)
    {
        DbPassword = dbPassword;
        DbUsername = dbUsername;
        Database = database;
    }
    
    public string Database { get; set; }
    public string DbUsername { get; set; }
    public string DbPassword  { get; set; }

}