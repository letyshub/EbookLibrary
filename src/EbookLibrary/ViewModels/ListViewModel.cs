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
        private readonly IFileService fileService;
        private string query;
        private IList<Ebook> ebooks;
        private bool? readFilter = null;

        public ListViewModel(EbookModel model, IEventAggregator eventAggregator, IFileService fileService)
        {
            this.model = model;
            this.eventAggregator = eventAggregator;
            this.fileService = fileService;
            LoadEbooks();
        }

        public IList<Ebook> Ebooks
        {
            get { return ebooks; }
            set
            {
                ebooks = value;
                NotifyOfPropertyChange(() => Ebooks);
            }
        }

        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                NotifyOfPropertyChange(() => Query);
            }
        }

        public bool IsFilterAll => readFilter == null;
        public bool IsFilterToRead => readFilter == false;
        public bool IsFilterRead => readFilter == true;

        public void AddEbook()
        {
            _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayAddEbookViewMessage());
        }

        public void SearchEbook()
        {
            LoadEbooks();
        }

        public void ShowAll()
        {
            readFilter = null;
            NotifyFilterProperties();
            LoadEbooks();
        }

        public void ShowToRead()
        {
            readFilter = false;
            NotifyFilterProperties();
            LoadEbooks();
        }

        public void ShowRead()
        {
            readFilter = true;
            NotifyFilterProperties();
            LoadEbooks();
        }

        public void ToggleRead(Ebook ebook)
        {
            ebook.IsRead = !ebook.IsRead;
            this.model.ToggleReadStatus(ebook.Id, ebook.IsRead);
            LoadEbooks();
        }

        public void EditEbook(Ebook ebook)
        {
            _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayEditEbookViewMessage(ebook));
        }

        public void OpenEbook(Ebook ebook)
        {
            this.fileService.OpenFile(ebook.Path);
        }

        private void LoadEbooks()
        {
            if (string.IsNullOrEmpty(query))
                Ebooks = readFilter.HasValue ? model.GetByReadStatus(readFilter.Value) : model.GetLast(25);
            else
                Ebooks = model.Get(query, readFilter);
        }

        private void NotifyFilterProperties()
        {
            NotifyOfPropertyChange(() => IsFilterAll);
            NotifyOfPropertyChange(() => IsFilterToRead);
            NotifyOfPropertyChange(() => IsFilterRead);
        }
    }
}
