namespace EbookLibrary.Models
{
    public class ReadingListItemDisplay
    {
        public int EbookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Path { get; set; }
        public int Priority { get; set; }
    }
}
