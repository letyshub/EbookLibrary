using Caliburn.Micro;

namespace EbookLibrary.ViewModels
{
    public class ConfirmationDialogViewModel : Screen
    {
        public ConfirmationDialogViewModel(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }

        public void Confirm()
        {
            this.TryClose(true);
        }

        public void Cancel()
        {
            this.TryClose(false);
        }
    }
}
