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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Accounts.ViewModels
{
    public class EditAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccountService _accountService;

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
            IAccountService accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            UpdateAccountCommand = new DelegateCommand(async () => await UpdateAccount());
            CancelCommand = new DelegateCommand(CancelAccount);
        }

        private async Task UpdateAccount()
        {
            var newAccount = await UpdateAccount(Account.Id, Account);

            var p = new NavigationParameters
            {
                { "page", "Account" },
                { "account", newAccount }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
            _eventAggregator.GetEvent<MessageEvent>().Publish("Account updated");
        }

        private void CancelAccount()
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
            var account = new Account();

            if (navigationContext.Parameters.ContainsKey("account"))
                account = navigationContext.Parameters.GetValue<Account>("account");

            GetAccount(account.Id).Await(AccountLoaded, AccountLoadedError);
        }

        private void AccountLoaded()
        {
        }

        private void AccountLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        public async Task GetAccount(int id)
        {
            Account = await _accountService.Get(id);
        }

        public async Task<Account> UpdateAccount(int accountId, Account account)
        {
            var newAccount = await _accountService.Update(accountId, account);
            return newAccount;
        }
    }
}