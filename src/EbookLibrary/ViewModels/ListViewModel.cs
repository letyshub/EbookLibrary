using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using System.Collections.Generic;

namespace EbookLibrary.ViewModels
{
    public class ListViewModel : Screen
    {
        private readonly EbookModel model;
        private readonly IEventAggregator eventAggregator;
        private string query;
        private IList<Ebook> ebooks;
        private IFileService fileService;

        public ListViewModel(EbookModel model, IEventAggregator eventAggregator, IFileService fileService)
        {
            this.model = model;
            this.eventAggregator = eventAggregator;
            this.fileService = fileService;
            this.Ebooks = this.model.GetLast(10);
        }

        public IList<Ebook> Ebooks
        {
            get { return ebooks; }
            set
            {
                this.ebooks = value;
                NotifyOfPropertyChange(() => Ebooks);
            }
        }

        public string Query
        {
            get { return query; }
            set
            {
                this.query = value;
                NotifyOfPropertyChange(() => Query);
            }
        }

        public void AddEbook()
        {
            this.eventAggregator.PublishOnUIThread(new DisplayAddEbookViewMessage());
        }

        public void SearchEbook()
        {
            this.Ebooks = string.IsNullOrEmpty(this.query) ? this.model.GetLast(25) : this.model.Get(this.query);
        }

        public void EditEbook(Ebook ebook)
        {
            this.eventAggregator.PublishOnUIThread(new DisplayEditEbookViewMessage(ebook));
        }

        public void OpenEbook(Ebook ebook)
        {
            this.fileService.OpenFile(ebook.Path);
        }
    }
}
