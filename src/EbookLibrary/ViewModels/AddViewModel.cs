using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using Microsoft.Win32;
using System.Linq;

namespace EbookLibrary.ViewModels
{
    public class AddViewModel : Screen
    {
        private string path;
        private string title;
        private string author;
        private string tags;
        private readonly EbookModel model;
        private readonly IEventAggregator eventAggregator;
        private readonly IFileService fileService;

        public AddViewModel(EbookModel model, IEventAggregator eventAggregator, IFileService fileService)
        {
            this.model = model;
            this.eventAggregator = eventAggregator;
            this.fileService = fileService;
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
            this.eventAggregator.PublishOnUIThread(new DisplayListEbookViewMessage());
        }

        public void Cancel()
        {
            this.eventAggregator.PublishOnUIThread(new DisplayListEbookViewMessage());
        }

        public void SelectFile()
        {
            var fd = new OpenFileDialog();
            fd.Filter = "Ebook files|*.pdf;*.epub;*mobi|All files (*.*)|*.*";
            var result = fd.ShowDialog();
            if (result.Value)
            {
                this.Path = fd.FileName;
            }
        }

        public void OpenFile()
        {
            this.fileService.OpenFile(this.path);
        }
    }
}
