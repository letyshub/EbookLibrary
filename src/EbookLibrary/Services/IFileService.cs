namespace EbookLibrary
{
    public interface IFileService
    {
        string SaveFile(string path);
        void DeleteFile(string path);
        void OpenFile(string path);
    }
}
