﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;
using System.Linq;

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

        public DelegateCommand AllReportsCommand { get; private set; }
        public DelegateCommand AllAccountsCommand { get; private set; }

        public DashboardViewModel(IRegionManager regionManager,
            ISmartBudgetService smartBudgetService)
        {
            _regionManager = regionManager;
            _smartBudgetService = smartBudgetService;

            AllReportsCommand = new DelegateCommand(AllReports);
            AllAccountsCommand = new DelegateCommand(AllAccounts);
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

            _regionManager.RequestNavigate("DashboardStatistics", "BlankStatistics");
            _regionManager.RequestNavigate("DashboardTransactions", "BlankTransactions");
            if (FavoriteAccounts.Count > 0)
                _regionManager.RequestNavigate("DashboardFavoriteAccounts", "FavoriteAccounts");
            else
                _regionManager.RequestNavigate("DashboardFavoriteAccounts", "BlankAccounts");
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
        }
    }
}