﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core.Dialogs;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;
using System.Linq;

namespace SmartBudget.Accounts.ViewModels
{
    public class TransactionsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly ITransactionService _transactionService;
        private int _accountId;

        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        public DelegateCommand AddAccountCommand { get; private set; }
        public DelegateCommand<Transaction> TransactionSelectedCommand { get; private set; }

        public TransactionsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ITransactionService transactionService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _transactionService = transactionService;

            Transactions = new ObservableCollection<Transaction>();

            AddAccountCommand = new DelegateCommand(AddAccount);
            TransactionSelectedCommand = new DelegateCommand<Transaction>(TransactionSelected);
        }

        private void TransactionSelected(Transaction transaction)
        {
            _dialogService.ShowTransactionDialog(transaction.Id, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    GetTransactions(_accountId);
                }
            });
        }

        private void AddAccount()
        {
            _regionManager.RequestNavigate("AccountsContent", "AddAccount");
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("accountid"))
                _accountId = navigationContext.Parameters.GetValue<int>("accountid");

            GetTransactions(_accountId);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private async void GetTransactions(int accountId)
        {
            var transactions = await _transactionService.GetByAccountId(accountId);
            foreach (var transaction in transactions.OrderByDescending(t => t.Id).OrderByDescending(t => t.Date))
            {
                Transactions.Add(new Transaction
                {
                    WorkingAccountId = _accountId,
                    Id = transaction.Id,
                    Payee = transaction.Payee,
                    Date = transaction.Date,
                    Amount = transaction.Amount,
                    IsCleared = transaction.IsCleared,
                    TransactionType = transaction.TransactionType,
                    Note = transaction.Note,
                    AccountId = transaction.AccountId,
                    Account = transaction.Account,
                    TargetAccountId = transaction.TargetAccountId,
                    TargetAccount = transaction.TargetAccount,
                });
            }
        }
    }
}