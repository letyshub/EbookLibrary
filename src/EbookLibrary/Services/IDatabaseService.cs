using EbookLibrary.Models;
using System.Collections.Generic;

namespace EbookLibrary
{
    public interface IDatabaseService
    {
        void Add(Ebook ebook);
        List<Ebook> GetLast(int size);
        List<Ebook> GetByQuery(string query);
        void Update(int id, string title, string author, List<string> tags);
        void Delete(int id);
    }
}
