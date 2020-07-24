using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;

namespace SmartBudget.Main.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly ISmartBudgetService _smartBudgetService;
        private ObservableCollection<Account> _favoriteAccounts;

        public ObservableCollection<Account> FavoriteAccounts
        {
            get { return _favoriteAccounts; }
            set { SetProperty(ref _favoriteAccounts, value); }
        }

        public bool AreStatistics => false;
        public bool NoStatistics => true;

        public bool AreAccounts => true; // FavoriteAccounts.Count > 0;
        public bool NoAccounts => false; // FavoriteAccounts.Count == 0;

        public bool AreTransactions => false;
        public bool NoTransactions => true;

        public DelegateCommand<Account> AccountSelectedCommand { get; private set; }

        public DashboardViewModel(IRegionManager regionManager,
            ISmartBudgetService smartBudgetService)
        {
            _regionManager = regionManager;
            _smartBudgetService = smartBudgetService;
            AccountSelectedCommand = new DelegateCommand<Account>(AccountSelected);
            GetFavoriteAccounts();
        }

        private void AccountSelected(Account account)
        {
            if (account == null)
                return;

            var p = new NavigationParameters();
            p.Add("area", "Accounts");
            p.Add("account", account);

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
            //_regionManager.RequestNavigate("Content", "Accounts");
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
        }

        private void GetFavoriteAccounts()
        {
            var favoriteAccounts = new ObservableCollection<Account>();
            var accounts = _smartBudgetService.GetAccounts();

            foreach (var account in accounts)
            {
                favoriteAccounts.Add(account);
            }

            for (int i = 0; i < 10; i++)
            {
                favoriteAccounts.Add(new Account { Name = $"Test {i}" });
            }
            FavoriteAccounts = favoriteAccounts;
        }
    }
}