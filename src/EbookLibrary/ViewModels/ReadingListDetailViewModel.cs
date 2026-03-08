using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace EbookLibrary.ViewModels
{
    public class ReadingListDetailViewModel : Screen
    {
        private readonly IDatabaseService dbService;
        private readonly IFileService fileService;
        private readonly IEventAggregator eventAggregator;
        private ReadingList currentList;
        private IList<ReadingListItemDisplay> items;
        private bool isAddingBook;
        private string searchQuery;
        private IList<Ebook> searchResults;

        public ReadingListDetailViewModel(IDatabaseService dbService, IFileService fileService, IEventAggregator eventAggregator)
        {
            this.dbService = dbService;
            this.fileService = fileService;
            this.eventAggregator = eventAggregator;
            searchResults = new List<Ebook>();
        }

        public string ListName => currentList?.Name ?? string.Empty;

        public IList<ReadingListItemDisplay> Items
        {
            get => items;
            set { items = value; NotifyOfPropertyChange(() => Items); }
        }

        public bool IsAddingBook
        {
            get => isAddingBook;
            set { isAddingBook = value; NotifyOfPropertyChange(() => IsAddingBook); }
        }

        public string SearchQuery
        {
            get => searchQuery;
            set { searchQuery = value; NotifyOfPropertyChange(() => SearchQuery); }
        }

        public IList<Ebook> SearchResults
        {
            get => searchResults;
            set { searchResults = value; NotifyOfPropertyChange(() => SearchResults); }
        }

        public void SetList(ReadingList list)
        {
            currentList = list;
            NotifyOfPropertyChange(() => ListName);
            LoadItems();
        }

        private void LoadItems()
        {
            if (currentList == null) return;
            currentList = dbService.GetReadingList(currentList.Id);
            var allEbooks = dbService.GetAll().ToDictionary(e => e.Id);
            Items = currentList.Entries
                .Where(e => allEbooks.ContainsKey(e.EbookId))
                .OrderByDescending(e => e.Priority)
                .Select(e => new ReadingListItemDisplay
                {
                    EbookId = e.EbookId,
                    Title = allEbooks[e.EbookId].Title,
                    Author = allEbooks[e.EbookId].Author,
                    Path = allEbooks[e.EbookId].Path,
                    Priority = e.Priority
                })
                .ToList();
        }

        public void ShowAddBook()
        {
            SearchQuery = string.Empty;
            SearchResults = new List<Ebook>();
            IsAddingBook = true;
        }

        public void HideAddBook()
        {
            IsAddingBook = false;
            SearchResults = new List<Ebook>();
        }

        public void SearchBooks()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;
            var existing = currentList.Entries.Select(e => e.EbookId).ToHashSet();
            SearchResults = dbService.GetByQuery(SearchQuery)
                .Where(e => !existing.Contains(e.Id))
                .ToList();
        }

        public void AddBook(Ebook ebook)
        {
            dbService.AddToReadingList(currentList.Id, ebook.Id, 1);
            HideAddBook();
            LoadItems();
        }

        public void RemoveEntry(ReadingListItemDisplay item)
        {
            dbService.RemoveFromReadingList(currentList.Id, item.EbookId);
            LoadItems();
        }

        public void CyclePriority(ReadingListItemDisplay item)
        {
            var newPriority = (item.Priority + 1) % 4;
            dbService.UpdateReadingListEntryPriority(currentList.Id, item.EbookId, newPriority);
            LoadItems();
        }

        public void OpenEbook(ReadingListItemDisplay item)
        {
            fileService.OpenFile(item.Path);
        }

        public void Back()
        {
            _ = eventAggregator.PublishOnUIThreadAsync(new DisplayReadingListsViewMessage());
        }
    }
}
