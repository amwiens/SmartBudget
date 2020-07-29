using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;
using System.Linq;

namespace SmartBudget.Main.ViewModels
{
    public class FavoriteAccountsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly ISmartBudgetService _smartBudgetService;
        private ObservableCollection<Account> _favoriteAccounts;

        public ObservableCollection<Account> FavoriteAccounts
        {
            get { return _favoriteAccounts; }
            set { SetProperty(ref _favoriteAccounts, value); }
        }

        public DelegateCommand<Account> AccountSelectedCommand { get; private set; }
        public DelegateCommand EditAccountCommand { get; private set; }
        public DelegateCommand<Account> DeleteAccountCommand { get; private set; }

        public FavoriteAccountsViewModel(IRegionManager regionManager,
            ISmartBudgetService smartBudgetService)
        {
            _regionManager = regionManager;
            _smartBudgetService = smartBudgetService;

            AccountSelectedCommand = new DelegateCommand<Account>(AccountSelected);
            EditAccountCommand = new DelegateCommand(EditAccount);
            DeleteAccountCommand = new DelegateCommand<Account>(DeleteAccount);
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

        private void EditAccount()
        {
            //if (account == null)
            //    return;

            var p = new NavigationParameters
            {
                { "area", "Accounts" },
                //{ "account", account }
            };

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        private void DeleteAccount(Account account)
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
        }
    }
}