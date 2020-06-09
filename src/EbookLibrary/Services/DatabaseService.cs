using EbookLibrary.Models;
using LiteDB;
using System.Collections.Generic;

namespace EbookLibrary
{
    public class DatabaseService : IDatabaseService
    {
        public void Add(Ebook ebook)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                ebooks.Insert(ebook);
            }
        }

        public List<Ebook> GetLast(int size)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                return ebooks
                        .Query()
                        .OrderBy(x => x.Id)
                        .Limit(size)
                        .ToList();
            }
        }

        public void Update(int id, string title, string author, List<string> tags)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                var ebook = ebooks.FindById(id);
                ebook.Title = title;
                ebook.Author = author;
                ebook.Tags = tags;
                ebooks.Update(ebook);
            }
        }

        public void Delete(int id)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                ebooks.Delete(id);
            }
        }

        public List<Ebook> GetByQuery(string query)
        {
            var keywords = query.Split(' ');
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                return ebooks
                        .Query()
                        .Where(x => x.Title.Contains(query) || x.Author.Contains(query) || (x.Tags != null && x.Tags.Contains(query)))
                        .ToList();
            }
        }
    }
}
