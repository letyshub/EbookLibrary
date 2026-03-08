namespace EbookLibrary.Services
{
    public interface ISettingsService
    {
        string Language { get; set; }
        string Theme { get; set; }
        void Save();
    }
}
