using Caliburn.Micro;
using EbookLibrary.Messages;

namespace EbookLibrary.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<DisplayAddEbookViewMessage>, IHandle<DisplayListEbookViewMessage>, IHandle<DisplayEditEbookViewMessage>
    {
        private readonly IEventAggregator eventAggregator;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            ActivateItem(IoC.Get<ListViewModel>());
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        public void Handle(DisplayAddEbookViewMessage message)
        {
            ActivateItem(IoC.Get<AddViewModel>());
        }

        public void Handle(DisplayListEbookViewMessage message)
        {
            ActivateItem(IoC.Get<ListViewModel>());
        }

        public void Handle(DisplayEditEbookViewMessage message)
        {
            var vm = IoC.Get<EditViewModel>();
            vm.SetEbook(message.Ebook);
            ActivateItem(vm);
        }
    }
}
