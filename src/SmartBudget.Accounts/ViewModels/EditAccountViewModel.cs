﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.Generic;

namespace SmartBudget.Accounts.ViewModels
{
    public class EditAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService<Account> _accountService;

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
            IEventAggregator eventAggregator,
            IDataService<Account> accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            UpdateAccountCommand = new DelegateCommand(UpdateAccount);
            CancelCommand = new DelegateCommand(CancelAddAccount);
        }

        private async void UpdateAccount()
        {
            var newAccount = await _accountService.Update(Account.Id, Account);

            var p = new NavigationParameters
            {
                { "page", "Account" },
                { "account", newAccount }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
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

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var account = new Account();

            if (navigationContext.Parameters.ContainsKey("account"))
                account = navigationContext.Parameters.GetValue<Account>("account");

            Account = await _accountService.Get(account.Id);
        }
    }
}