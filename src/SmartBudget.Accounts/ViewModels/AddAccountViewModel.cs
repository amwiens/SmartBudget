using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Accounts.ViewModels
{
    public class AddAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccountService _accountService;

        private AccountType _accountType = AccountType.Bank;

        public AccountType AccountType
        {
            get { return _accountType; }
            set { SetProperty(ref _accountType, value); }
        }

        public Dictionary<AccountType, string> AccountTypeCaptions { get; } =
            new Dictionary<AccountType, string>()
            {
                { AccountType.Card, "Credit Card" },
                { AccountType.Bank, "Bank Account" },
                { AccountType.Credit, "Credit Account" }
            };

        private Account _account;

        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        public DelegateCommand SaveAccountCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public AddAccountViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IAccountService accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            Account = new Account
            {
                StartDate = DateTime.Now
            };

            SaveAccountCommand = new DelegateCommand(async () => await SaveAccount());
            CancelCommand = new DelegateCommand(CancelAddAccount);
        }

        private async Task SaveAccount()
        {
            Account.AccountType = AccountType;
            var newAccount = await CreateAccount(Account);

            var p = new NavigationParameters
            {
                { "area", "Accounts" },
                { "account", newAccount }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
            _eventAggregator.GetEvent<MessageEvent>().Publish("Account created");
        }

        private void CancelAddAccount()
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
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private async Task<Account> CreateAccount(Account account)
        {
            var newAccount = await _accountService.Create(account);
            return newAccount;
        }
    }
}