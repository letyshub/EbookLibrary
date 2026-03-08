using EbookLibrary.Models;
using System.Collections.Generic;

namespace EbookLibrary
{
    public interface IDatabaseService
    {
        void Add(Ebook ebook);
        List<Ebook> GetLast(int size);
        List<Ebook> GetByQuery(string query, bool? isRead = null);
        List<Ebook> GetByReadStatus(bool isRead);
        void Update(int id, string title, string author, List<string> tags, bool isRead, int priority);
        void UpdateReadStatus(int id, bool isRead);
        void Delete(int id);
    }
}
