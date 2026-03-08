using EbookLibrary.Models;

namespace EbookLibrary.Messages
{
    public class DisplayAddToListViewMessage
    {
        public Ebook Ebook { get; }
        public DisplayAddToListViewMessage(Ebook ebook) => Ebook = ebook;
    }
}
