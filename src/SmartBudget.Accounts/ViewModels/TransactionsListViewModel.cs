﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core.Events;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.Accounts.ViewModels
{
    public class TransactionsListViewModel : BindableBase, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly ITransactionService _transactionService;
        private readonly IEventAggregator _eventAggregator;
        private int _accountId;

        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        public DelegateCommand<Transaction> TransactionSelectedCommand { get; private set; }

        public TransactionsListViewModel(IDialogService dialogService,
            IEventAggregator eventAggregator,
            ITransactionService transactionService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _transactionService = transactionService;

            Transactions = new ObservableCollection<Transaction>();

            TransactionSelectedCommand = new DelegateCommand<Transaction>(TransactionSelected);
        }

        private void TransactionSelected(Transaction transaction)
        {
            _dialogService.ShowTransactionDialog(transaction.Id, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    GetTransactions(_accountId).Await(TransactionsLoaded, TransactionsLoadedError);
                }
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("accountid"))
                _accountId = navigationContext.Parameters.GetValue<int>("accountid");

            GetTransactions(_accountId).Await(TransactionsLoaded, TransactionsLoadedError);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void TransactionsLoaded()
        {
        }

        private void TransactionsLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private async Task GetTransactions(int accountId)
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