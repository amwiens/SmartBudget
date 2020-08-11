using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using SmartBudget.Core;
using SmartBudget.Core.Dialogs;
using SmartBudget.Core.Events;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartBudget.Accounts.ViewModels
{
    public class TransactionsListViewModel : BindableBase, INavigationAware
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

        public DelegateCommand<Transaction> TransactionSelectedCommand { get; private set; }
        public DelegateCommand<int?> EditTransactionCommand { get; private set; }
        public DelegateCommand<int?> DeleteTransactionCommand { get; private set; }

        public TransactionsListViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ITransactionService transactionService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _transactionService = transactionService;

            Transactions = new ObservableCollection<Transaction>();

            TransactionSelectedCommand = new DelegateCommand<Transaction>(TransactionSelected);
            EditTransactionCommand = new DelegateCommand<int?>(EditTransaction);
            DeleteTransactionCommand = new DelegateCommand<int?>(DeleteTransaction);
        }

        private void EditTransaction(int? id)
        {
            throw new NotImplementedException();
        }

        private void DeleteTransaction(int? id)
        {
            if (id == null)
                return;

            _dialogService.ShowConfirmDialog("Are you sure you want to delete the transaction?", async result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    var success = await _transactionService.Delete(int.Parse(id.ToString()));

                    var p = new NavigationParameters
                    {
                        { "page", "Account" },
                        { "account", _accountId }
                    };

                    _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
                    _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
                }
            });
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