using System.Collections.Generic;

namespace EbookLibrary.Models
{
    public class ReadingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ReadingListEntry> Entries { get; set; } = new();
    }

    public class ReadingListEntry
    {
        public int EbookId { get; set; }
        public int Priority { get; set; }
    }
}
