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
    public class FavoriteAccountsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccountService _accountService;
        private ObservableCollection<Account> _favoriteAccounts;

        public ObservableCollection<Account> FavoriteAccounts
        {
            get { return _favoriteAccounts; }
            set { SetProperty(ref _favoriteAccounts, value); }
        }

        public DelegateCommand<Account> AccountSelectedCommand { get; private set; }

        public FavoriteAccountsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IAccountService accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            AccountSelectedCommand = new DelegateCommand<Account>(AccountSelected);
        }

        private void AccountSelected(Account account)
        {
            if (account == null)
                return;

            var p = new NavigationParameters
            {
                { "page", "Account" },
                { "account", account }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
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
            GetFavoriteAccounts().Await(FavoriteAccountsLoaded, FavoriteAccountsLoadedError);
        }

        private void FavoriteAccountsLoaded()
        {
        }

        private void FavoriteAccountsLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private async Task GetFavoriteAccounts()
        {
            var favoriteAccounts = new ObservableCollection<Account>();
            var accounts = await _accountService.GetAll();

            foreach (var account in accounts.Where(a => a.Favorite == true))
            {
                favoriteAccounts.Add(account);
            }

            FavoriteAccounts = favoriteAccounts;
        }
    }
}