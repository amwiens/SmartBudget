using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartBudget.Main.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly ISmartBudgetService _smartBudgetService;
        private ObservableCollection<Account> _favoriteAccounts;
        private bool _areAccounts;
        private bool _noAccounts;

        public ObservableCollection<Account> FavoriteAccounts
        {
            get { return _favoriteAccounts; }
            set { SetProperty(ref _favoriteAccounts, value); }
        }

        public bool AreStatistics => false;
        public bool NoStatistics => true;

        public bool AreAccounts
        {
            get { return _areAccounts; }
            set { SetProperty(ref _areAccounts, value); }
        }

        public bool NoAccounts
        {
            get { return _noAccounts; }
            set { SetProperty(ref _noAccounts, value); }
        }

        public bool AreTransactions => false;
        public bool NoTransactions => true;

        public DelegateCommand<Account> AccountSelectedCommand { get; private set; }
        public DelegateCommand AllReportsCommand { get; private set; }
        public DelegateCommand AllAccountsCommand { get; private set; }
        public DelegateCommand<Account> EditAccountCommand { get; private set; }


        public DashboardViewModel(IRegionManager regionManager,
            ISmartBudgetService smartBudgetService)
        {
            _regionManager = regionManager;
            _smartBudgetService = smartBudgetService;
            AllReportsCommand = new DelegateCommand(AllReports);
            AllAccountsCommand = new DelegateCommand(AllAccounts);
            AccountSelectedCommand = new DelegateCommand<Account>(AccountSelected);
            EditAccountCommand = new DelegateCommand<Account>(EditAccount);
        }

        private void EditAccount(Account account)
        {
            if (account == null)
                return;

            var p = new NavigationParameters
            {
                { "area", "Accounts" },
                { "account", account }
            };

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        private void AllReports()
        {
            var p = new NavigationParameters
            {
                { "area", "Reports" }
            };

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        private void AllAccounts()
        {
            var p = new NavigationParameters
            {
                { "area", "Accounts" }
            };

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        private void AccountSelected(Account account)
        {
            if (account == null)
                return;

            var p = new NavigationParameters
            {
                { "area", "Accounts" },
                { "account", account }
            };

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetFavoriteAccounts();
        }

        private void GetFavoriteAccounts()
        {
            var favoriteAccounts = new ObservableCollection<Account>();
            var accounts = _smartBudgetService.GetAccounts().Where(a => a.Favorite == true);

            foreach (var account in accounts)
            {
                favoriteAccounts.Add(account);
            }

            FavoriteAccounts = favoriteAccounts;

            if (FavoriteAccounts.Count > 0)
            {
                NoAccounts = false;
                AreAccounts = true;
            }
            else
            {
                NoAccounts = true;
                AreAccounts = false;
            }
        }
    }
}