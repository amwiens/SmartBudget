using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.Generic;

namespace SmartBudget.Core.Dialogs
{
    public class AddTransactionDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        private Transaction _transaction;

        public Transaction Transaction
        {
            get { return _transaction; }
            set { SetProperty(ref _transaction, value); }
        }

        public Dictionary<TransactionType, string> TransactionTypeCaptions { get; } =
            new Dictionary<TransactionType, string>()
            {
                { TransactionType.Expense, "Expense" },
                { TransactionType.Income, "Income" },
                { TransactionType.Transfer, "Transfer" }
            };

        private Account _account;

        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        public DelegateCommand SaveDialogCommand { get; }
        public DelegateCommand CancelDialogCommand { get; }

        public string Title => "Add Transaction";

        public event Action<IDialogResult> RequestClose;

        public AddTransactionDialogViewModel(ITransactionService transactionService,
            IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;

            Transaction = new Transaction();

            SaveDialogCommand = new DelegateCommand(SaveDialog);
            CancelDialogCommand = new DelegateCommand(CancelDialog);
        }

        private void SaveDialog()
        {
            var result = ButtonResult.OK;

            Transaction.AccountId = Account.Id;
            var transaction = _transactionService.Create(Transaction);

            RequestClose?.Invoke(new DialogResult(result));
        }

        private void CancelDialog()
        {
            var result = ButtonResult.Cancel;

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
            var accountId = parameters.GetValue<int>("accountid");
            Account = await _accountService.Get(accountId);
        }
    }
}