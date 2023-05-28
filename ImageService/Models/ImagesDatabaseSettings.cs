namespace ImageService.Models;

public class ImagesDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ImagesCollectionName { get; set; } = null!;
}