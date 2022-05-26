namespace PocApi.Models;

public class ComicStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ComicsCollectionName { get; set; } = null!;
}