using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using EbookLibrary.Services;
using Microsoft.Win32;
using System.Linq;
using System.Threading.Tasks;

namespace EbookLibrary.ViewModels
{
    public class AddViewModel : Screen
    {
        private string path;
        private string title;
        private string author;
        private string tags;
        private bool isLoading;
        private readonly EbookModel model;
        private readonly IEventAggregator eventAggregator;
        private readonly IFileService fileService;
        private readonly IMetadataService metadataService;

        public AddViewModel(EbookModel model, IEventAggregator eventAggregator, IFileService fileService, IMetadataService metadataService)
        {
            this.model = model;
            this.eventAggregator = eventAggregator;
            this.fileService = fileService;
            this.metadataService = metadataService;
        }

        public string Title
        {
            get { return title; }
            set
            {
                this.title = value;
                NotifyOfPropertyChange(() => Title);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                this.author = value;
                NotifyOfPropertyChange(() => Author);
            }
        }

        public string Tags
        {
            get { return tags; }
            set
            {
                this.tags = value;
                NotifyOfPropertyChange(() => Tags);
            }
        }

        public string Path
        {
            get { return path; }
            set
            {
                this.path = value;
                NotifyOfPropertyChange(() => Path);
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            private set { isLoading = value; NotifyOfPropertyChange(() => IsLoading); }
        }

        public bool CanSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Path) &&
                    !string.IsNullOrWhiteSpace(this.Title);
            }
        }

        public void Save()
        {
            var ebook = new Ebook
            {
                Author = this.author,
                Title = this.title,
                Path = this.path,
                Tags = this.tags?.Split(',').ToList()
            };
            this.model.Add(ebook);
            _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }

        public void Cancel()
        {
            _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }

        public async void SelectFile()
        {
            var fd = new OpenFileDialog();
            fd.Filter = "Ebook files|*.pdf;*.epub;*.mobi|All files (*.*)|*.*";
            var result = fd.ShowDialog();
            if (result.Value)
            {
                this.Path = fd.FileName;
                IsLoading = true;
                var meta = await Task.Run(() => this.metadataService.Extract(fd.FileName));
                IsLoading = false;
                if (string.IsNullOrWhiteSpace(this.Title) && !string.IsNullOrWhiteSpace(meta.Title))
                    this.Title = meta.Title;
                if (string.IsNullOrWhiteSpace(this.Author) && !string.IsNullOrWhiteSpace(meta.Author))
                    this.Author = meta.Author;
            }
        }

        public void OpenFile()
        {
            this.fileService.OpenFile(this.path);
        }
    }
}
