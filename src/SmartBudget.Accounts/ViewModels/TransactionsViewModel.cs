using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core.Dialogs;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;

namespace SmartBudget.Accounts.ViewModels
{
    public class TransactionsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly ITransactionService _transactionService;

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
                //if (result.Result == ButtonResult.Yes)
                //{
                //    var success = await _accountService.Delete(int.Parse(id.ToString()));

                //    _regionManager.RequestNavigate(RegionNames.Content, "Accounts");
                //    _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
                //}
            });
        }

        private void AddAccount()
        {
            _regionManager.RequestNavigate("AccountsContent", "AddAccount");
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            int accountId = 0;

            if (navigationContext.Parameters.ContainsKey("accountid"))
                accountId = navigationContext.Parameters.GetValue<int>("accountid");

            var transactions = await _transactionService.GetByAccountId(accountId);
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}