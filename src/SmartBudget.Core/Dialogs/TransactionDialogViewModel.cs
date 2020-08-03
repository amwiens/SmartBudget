using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;

namespace SmartBudget.Core.Dialogs
{
    public class TransactionDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ITransactionService _transactionService;

        private Transaction _transaction;

        public Transaction Transaction
        {
            get { return _transaction; }
            set { SetProperty(ref _transaction, value); }
        }

        public string Title => "Transaction";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand CloseDialogCommand { get; }

        public TransactionDialogViewModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;

            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private void CloseDialog()
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

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var transactionId = parameters.GetValue<int>("transactionid");
            Transaction = await _transactionService.Get(transactionId);
        }
    }
}