using EbookLibrary.Models;

namespace EbookLibrary.Messages
{
    public class DisplayEditEbookViewMessage
    {
        public DisplayEditEbookViewMessage(Ebook ebook)
        {
            this.Ebook = ebook;
        }

        public Ebook Ebook { get; private set; }
    }
}
