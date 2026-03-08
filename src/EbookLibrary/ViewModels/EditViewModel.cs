using System.Threading.Tasks;
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
        private bool isRead;
        private int priority;
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

            this.isRead = ebook.IsRead;
            this.priority = ebook.Priority;
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

        public bool IsRead
        {
            get { return isRead; }
            set
            {
                this.isRead = value;
                NotifyOfPropertyChange(() => IsRead);
            }
        }

        public int Priority
        {
            get { return priority; }
            set
            {
                this.priority = value;
                NotifyOfPropertyChange(() => Priority);
            }
        }

        public string Path { get; private set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(this.Title);

        public void Save()
        {
            this.model.Update(id, title, author, this.tags?.Split(',').ToList(), this.isRead, this.priority);
            _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }

        public void Open()
        {
            this.fileService.OpenFile(this.Path);
        }

        public async Task Delete()
        {
            var confirmVm = new ConfirmationDialogViewModel("Are you sure you want to delete this ebook?");
            var result = await this.windowManager.ShowDialogAsync(confirmVm);

            if (result == true)
            {
                this.model.Delete(this.id, this.Path);
                _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
            }
        }

        public void Cancel()
        {
            _ = this.eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }
    }
}
