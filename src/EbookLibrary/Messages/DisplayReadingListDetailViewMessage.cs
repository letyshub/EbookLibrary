using EbookLibrary.Models;

namespace EbookLibrary.Messages
{
    public class DisplayReadingListDetailViewMessage
    {
        public ReadingList ReadingList { get; }
        public DisplayReadingListDetailViewMessage(ReadingList readingList) => ReadingList = readingList;
    }
}
