using Caliburn.Micro;
using EbookLibrary.Messages;
using EbookLibrary.Models;
using System.Linq;
using System.Text;

namespace EbookLibrary.ViewModels
{
    public class EditViewModel : Screen
    {
        private int id;
        private string title;
        private string author;
        private string tags;
        private readonly EbookModel model;
        private readonly IEventAggregator eventAggregator;
        private readonly IWindowManager windowManager;
        private readonly IFileService fileService;

        public EditViewModel(EbookModel model, IEventAggregator eventAggregator, IWindowManager windowManager, IFileService fileService)
        {
            this.model = model;
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
            this.fileService = fileService;
        }

        public void SetEbook(Ebook ebook)
        {
            this.id = ebook.Id;
            this.Path = ebook.Path;
            this.title = ebook.Title;
            this.author = ebook.Author;

            if (!ebook.Tags.IsNullOrEmpty())
            {
                var sb = new StringBuilder();
                sb.AppendJoin(',', ebook.Tags);
                this.tags = sb.ToString();
            }
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

        public string Path { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(this.Title);

        public void Save()
        {
            this.model.Update(id, title, author, this.tags?.Split(',').ToList());
            this.eventAggregator.PublishOnUIThread(new DisplayListEbookViewMessage());
        }

        public void Open()
        {
            this.fileService.OpenFile(this.Path);
        }

        public void Delete()
        {
            var confirmVm = new ConfirmationDialogViewModel("Are you sure you want to delete this ebook?");
            var result = this.windowManager.ShowDialog(confirmVm);

            if (result.Value)
            {
                this.model.Delete(this.id, this.Path);
                this.eventAggregator.PublishOnUIThread(new DisplayListEbookViewMessage());
            }
        }

        public void Cancel()
        {
            this.eventAggregator.PublishOnUIThread(new DisplayListEbookViewMessage());
        }
    }
}
