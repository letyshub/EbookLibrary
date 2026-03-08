using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using System.Collections.Generic;

namespace EbookLibrary.ViewModels
{
    public class AddToListViewModel : Screen
    {
        private readonly IDatabaseService dbService;
        private readonly IEventAggregator eventAggregator;
        private Ebook ebook;
        private IList<ReadingList> lists;

        public AddToListViewModel(IDatabaseService dbService, IEventAggregator eventAggregator)
        {
            this.dbService = dbService;
            this.eventAggregator = eventAggregator;
        }

        public string EbookTitle => ebook?.Title ?? string.Empty;

        public IList<ReadingList> Lists
        {
            get => lists;
            set { lists = value; NotifyOfPropertyChange(() => Lists); }
        }

        public void SetEbook(Ebook e)
        {
            ebook = e;
            NotifyOfPropertyChange(() => EbookTitle);
            Lists = dbService.GetAllReadingLists();
        }

        public void SelectList(ReadingList list)
        {
            dbService.AddToReadingList(list.Id, ebook.Id, 1);
            _ = eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }

        public void Back()
        {
            _ = eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }
    }
}
