using System.Collections.Generic;

namespace EbookLibrary.Models
{
    public class Ebook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public string Path { get; set; }
    }
}
