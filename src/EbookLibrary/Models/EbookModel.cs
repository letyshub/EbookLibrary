using System.Collections.Generic;

namespace EbookLibrary.Models
{
    public class EbookModel
    {
        private readonly IFileService fileService;
        private readonly IDatabaseService dbService;

        public EbookModel(IFileService fileService, IDatabaseService dbService)
        {
            this.fileService = fileService;
            this.dbService = dbService;
        }

        public void Add(Ebook ebook)
        {
            ebook.Path = this.fileService.SaveFile(ebook.Path);
            this.dbService.Add(ebook);
        }

        public List<Ebook> GetLast(int size)
        {
            return this.dbService.GetLast(size);
        }

        public void Update(int id, string title, string author, List<string> tags, bool isRead, int priority)
        {
            this.dbService.Update(id, title, author, tags, isRead, priority);
        }

        public void ToggleReadStatus(int id, bool isRead)
        {
            this.dbService.UpdateReadStatus(id, isRead);
        }

        public IList<Ebook> GetByReadStatus(bool isRead)
        {
            return this.dbService.GetByReadStatus(isRead);
        }

        public void Delete(int id, string path)
        {
            this.fileService.DeleteFile(path);
            this.dbService.Delete(id);
        }

        public IList<Ebook> Get(string query, bool? isRead = null)
        {
            return this.dbService.GetByQuery(query, isRead);
        }
    }
}
