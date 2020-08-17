using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.Main.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private ObservableCollection<Account> _favoriteAccounts;

        public ObservableCollection<Account> FavoriteAccounts
        {
            get { return _favoriteAccounts; }
            set { SetProperty(ref _favoriteAccounts, value); }
        }

        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        public DelegateCommand AllReportsCommand { get; private set; }
        public DelegateCommand AllAccountsCommand { get; private set; }

        public DashboardViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IAccountService accountService,
            ITransactionService transactionService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;
            _transactionService = transactionService;

            AllReportsCommand = new DelegateCommand(AllReports);
            AllAccountsCommand = new DelegateCommand(AllAccounts);
        }

        private void AllReports()
        {
            _regionManager.RequestNavigate(RegionNames.Content, "Reports");
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Reports");
        }

        private void AllAccounts()
        {
            _regionManager.RequestNavigate(RegionNames.Content, "Accounts");
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove(RegionNames.DashboardFavoriteAccounts);
            _regionManager.Regions.Remove(RegionNames.DashboardStatistics);
            _regionManager.Regions.Remove(RegionNames.DashboardTransactions);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetFavoriteAccounts().Await(FavoriteAccountsLoaded, FavoriteAccountsLoadedError);
            GetTransactions().Await(TransactionsLoaded, TransactionsLoadedError);

            // Load statistics view
            if (Transactions.Count > 0)
                _regionManager.RequestNavigate(RegionNames.DashboardStatistics, "StatisticsView");
            else
                _regionManager.RequestNavigate(RegionNames.DashboardStatistics, "BlankStatistics");

            // Load transactions view
            _regionManager.RequestNavigate(RegionNames.DashboardTransactions, "BlankTransactions");

            // Load favorite accounts view
            if (FavoriteAccounts.Count > 0)
                _regionManager.RequestNavigate(RegionNames.DashboardFavoriteAccounts, "FavoriteAccounts");
            else
                _regionManager.RequestNavigate(RegionNames.DashboardFavoriteAccounts, "BlankAccounts");
        }

        private void FavoriteAccountsLoaded()
        {
        }

        private void FavoriteAccountsLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private void TransactionsLoaded()
        {
        }

        private void TransactionsLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private async Task GetFavoriteAccounts()
        {
            var accounts = await _accountService.GetAll();
            FavoriteAccounts = new ObservableCollection<Account>(accounts.Where(a => a.Favorite == true));
        }

        private async Task GetTransactions()
        {
            var transactions = await _transactionService.GetAll();
            Transactions = new ObservableCollection<Transaction>(transactions);
        }
    }
}