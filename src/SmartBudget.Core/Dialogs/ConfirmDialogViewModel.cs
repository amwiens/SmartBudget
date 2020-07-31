using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using System;

namespace SmartBudget.Core.Dialogs
{
    public class ConfirmDialogViewModel : BindableBase, IDialogAware
    {
        private string _message;

        public event Action<IDialogResult> RequestClose;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public DelegateCommand YesDialogCommand { get; }
        public DelegateCommand NoDialogCommand { get; }

        public string Title => "Confirm";

        public ConfirmDialogViewModel()
        {
            YesDialogCommand = new DelegateCommand(YesDialog);
            NoDialogCommand = new DelegateCommand(NoDialog);
        }

        private void YesDialog()
        {
            var result = ButtonResult.Yes;

            RequestClose?.Invoke(new DialogResult(result));
        }

        private void NoDialog()
        {
            var result = ButtonResult.No;

            RequestClose?.Invoke(new DialogResult(result));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }
    }
}