using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.Generic;

namespace SmartBudget.Accounts.ViewModels
{
    public class EditAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly ISmartBudgetService _smartBudgetService;

        private Account _account;

        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        public Dictionary<AccountType, string> AccountTypeCaptions { get; } =
            new Dictionary<AccountType, string>()
            {
                { AccountType.Card, "Credit Card" },
                { AccountType.Bank, "Bank Account" },
                { AccountType.Credit, "Credit Account" }
            };


        public DelegateCommand UpdateAccountCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public EditAccountViewModel(IRegionManager regionManager,
            ISmartBudgetService smartBudgetService)
        {
            _regionManager = regionManager;
            _smartBudgetService = smartBudgetService;

            UpdateAccountCommand = new DelegateCommand(UpdateAccount);
            CancelCommand = new DelegateCommand(CancelAddAccount);
        }

        private async void UpdateAccount()
        {
            var newAccount = await _smartBudgetService.UpdateAccountAsync(Account);

            var p = new NavigationParameters
            {
                { "area", "Accounts" },
                { "account", newAccount }
            };

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        private void CancelAddAccount()
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
            var account = new Account();

            if (navigationContext.Parameters.ContainsKey("account"))
                account = navigationContext.Parameters.GetValue<Account>("account");

            Account = _smartBudgetService.GetAccountItem(account.Id);
        }
    }
}