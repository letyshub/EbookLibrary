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

        internal void Update(int id, string title, string author, List<string> tags)
        {
            this.dbService.Update(id, title, author, tags);
        }

        internal void Delete(int id, string path)
        {
            this.fileService.DeleteFile(path);
            this.dbService.Delete(id);
        }

        internal IList<Ebook> Get(string query)
        {
            return this.dbService.GetByQuery(query);
        }
    }
}
