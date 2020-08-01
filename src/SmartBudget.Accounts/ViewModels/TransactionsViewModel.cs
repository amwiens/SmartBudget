using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

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
        private readonly ITransactionService _transactionService;

        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        public DelegateCommand AddAccountCommand { get; private set; }

        public TransactionsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ITransactionService transactionService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _transactionService = transactionService;

            Transactions = new ObservableCollection<Transaction>();

            AddAccountCommand = new DelegateCommand(AddAccount);
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