using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Events;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Threading.Tasks;

namespace SmartBudget.Core.Dialogs
{
    public class TransactionDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ITransactionService _transactionService;
        private readonly IDialogService _dialogService;

        private Transaction _transaction;

        public Transaction Transaction
        {
            get { return _transaction; }
            set { SetProperty(ref _transaction, value); }
        }

        public string Title => "Transaction";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand EditTransactionCommand { get; }
        public DelegateCommand DeleteTransactionCommand { get; }
        public DelegateCommand CloseDialogCommand { get; }

        public TransactionDialogViewModel(IEventAggregator eventAggregator,
            ITransactionService transactionService,
            IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _transactionService = transactionService;
            _dialogService = dialogService;

            EditTransactionCommand = new DelegateCommand(async () => await EditTransaction());
            DeleteTransactionCommand = new DelegateCommand(async () => await DeleteTransaction());
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private async Task EditTransaction()
        {
            _dialogService.ShowAddTransactionDialog(Transaction.AccountId, Transaction.Id, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    Transaction = await _transactionService.Get(Transaction.Id);
                }
            });
        }

        private async Task DeleteTransaction()
        {
            var result = ButtonResult.OK;

            if (await TransactionDelete(Transaction.Id))
            {
                _eventAggregator.GetEvent<MessageEvent>().Publish("Transaction deleted");
                RequestClose?.Invoke(new DialogResult(result));
            }
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

        public async Task<bool> TransactionDelete(int id)
        {
            var result = await _transactionService.Delete(id);
            return result;
        }
    }
}