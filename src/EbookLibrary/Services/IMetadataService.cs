namespace EbookLibrary.Services
{
    public record EbookMetadata(string Title, string Author);

    public interface IMetadataService
    {
        EbookMetadata Extract(string path);
    }
}
