using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using System.Collections.Generic;

namespace EbookLibrary.ViewModels
{
    public class ReadingListsViewModel : Screen
    {
        private readonly IDatabaseService dbService;
        private readonly IEventAggregator eventAggregator;
        private IList<ReadingList> lists;
        private string newListName;

        public ReadingListsViewModel(IDatabaseService dbService, IEventAggregator eventAggregator)
        {
            this.dbService = dbService;
            this.eventAggregator = eventAggregator;
            Lists = dbService.GetAllReadingLists();
        }

        public IList<ReadingList> Lists
        {
            get => lists;
            set { lists = value; NotifyOfPropertyChange(() => Lists); }
        }

        public string NewListName
        {
            get => newListName;
            set
            {
                newListName = value;
                NotifyOfPropertyChange(() => NewListName);
                NotifyOfPropertyChange(() => CanCreateList);
            }
        }

        public bool CanCreateList => !string.IsNullOrWhiteSpace(newListName);

        public void CreateList()
        {
            if (!CanCreateList) return;
            dbService.AddReadingList(newListName.Trim());
            NewListName = string.Empty;
            Lists = dbService.GetAllReadingLists();
        }

        public void DeleteList(ReadingList list)
        {
            dbService.DeleteReadingList(list.Id);
            Lists = dbService.GetAllReadingLists();
        }

        public void OpenList(ReadingList list)
        {
            _ = eventAggregator.PublishOnUIThreadAsync(new DisplayReadingListDetailViewMessage(list));
        }

        public void Back()
        {
            _ = eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }
    }
}
