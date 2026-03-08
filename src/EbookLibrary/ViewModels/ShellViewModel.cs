using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using EbookLibrary.Messages;

namespace EbookLibrary.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<DisplayAddEbookViewMessage>, IHandle<DisplayListEbookViewMessage>, IHandle<DisplayEditEbookViewMessage>, IHandle<DisplaySettingsViewMessage>
    {
        private readonly IEventAggregator eventAggregator;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _ = ActivateItemAsync(IoC.Get<ListViewModel>(), CancellationToken.None);
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        public async Task HandleAsync(DisplayAddEbookViewMessage message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<AddViewModel>(), cancellationToken);
        }

        public async Task HandleAsync(DisplayListEbookViewMessage message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<ListViewModel>(), cancellationToken);
        }

        public async Task HandleAsync(DisplayEditEbookViewMessage message, CancellationToken cancellationToken)
        {
            var vm = IoC.Get<EditViewModel>();
            vm.SetEbook(message.Ebook);
            await ActivateItemAsync(vm, cancellationToken);
        }

        public async Task HandleAsync(DisplaySettingsViewMessage message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SettingsViewModel>(), cancellationToken);
        }
    }
}
