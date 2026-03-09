using EbookLibrary.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

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

        public void Update(int id, string title, string author, List<string> tags, bool isRead, int priority)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                var ebook = ebooks.FindById(id);
                ebook.Title = title;
                ebook.Author = author;
                ebook.Tags = tags;
                ebook.IsRead = isRead;
                ebook.Priority = priority;
                ebooks.Update(ebook);
            }
        }

        public void UpdateReadStatus(int id, bool isRead)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                var ebook = ebooks.FindById(id);
                ebook.IsRead = isRead;
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

        public int Count()
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                return db.GetCollection<Ebook>("ebooks").Count();
            }
        }

        public List<Ebook> GetByQuery(string query, bool? isRead = null)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                var q = ebooks.Query()
                        .Where(x => x.Title.Contains(query) || x.Author.Contains(query) || (x.Tags != null && x.Tags.Any(t => t.Contains(query))));

                if (isRead.HasValue)
                    q = q.Where(x => x.IsRead == isRead.Value);

                return q.ToList();
            }
        }

        public List<Ebook> GetByReadStatus(bool isRead)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var ebooks = db.GetCollection<Ebook>("ebooks");
                return ebooks.Query()
                        .Where(x => x.IsRead == isRead)
                        .OrderByDescending(x => x.Priority)
                        .ToList();
            }
        }

        public List<Ebook> GetAll()
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                return db.GetCollection<Ebook>("ebooks").FindAll().ToList();
            }
        }

        public List<ReadingList> GetAllReadingLists()
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                return db.GetCollection<ReadingList>("readinglists").FindAll().ToList();
            }
        }

        public ReadingList GetReadingList(int id)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                return db.GetCollection<ReadingList>("readinglists").FindById(id);
            }
        }

        public void AddReadingList(string name)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                db.GetCollection<ReadingList>("readinglists").Insert(new ReadingList { Name = name });
            }
        }

        public void DeleteReadingList(int id)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                db.GetCollection<ReadingList>("readinglists").Delete(id);
            }
        }

        public void AddToReadingList(int listId, int ebookId, int priority)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var lists = db.GetCollection<ReadingList>("readinglists");
                var list = lists.FindById(listId);
                if (list.Entries.Any(e => e.EbookId == ebookId)) return;
                list.Entries.Add(new ReadingListEntry { EbookId = ebookId, Priority = priority });
                lists.Update(list);
            }
        }

        public void RemoveFromReadingList(int listId, int ebookId)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var lists = db.GetCollection<ReadingList>("readinglists");
                var list = lists.FindById(listId);
                list.Entries.RemoveAll(e => e.EbookId == ebookId);
                lists.Update(list);
            }
        }

        public void UpdateReadingListEntryPriority(int listId, int ebookId, int priority)
        {
            using (var db = new LiteDatabase(Properties.EbookLibrary.Default.ConnectionString))
            {
                var lists = db.GetCollection<ReadingList>("readinglists");
                var list = lists.FindById(listId);
                var entry = list.Entries.FirstOrDefault(e => e.EbookId == ebookId);
                if (entry != null) entry.Priority = priority;
                lists.Update(list);
            }
        }
    }
}
