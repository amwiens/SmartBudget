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

namespace SmartBudget.Accounts.ViewModels
{
    public class AddAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccountService _accountService;

        private AccountType _accountType;

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

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _accountNumber;

        public string AccountNumber
        {
            get { return _accountNumber; }
            set { SetProperty(ref _accountNumber, value); }
        }

        private decimal _rate;

        public decimal Rate
        {
            get { return _rate; }
            set { SetProperty(ref _rate, value); }
        }

        private DateTime _startDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime _endDate = DateTime.Now;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        private decimal _startingAmount;

        public decimal StartingAmount
        {
            get { return _startingAmount; }
            set { SetProperty(ref _startingAmount, value); }
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

            SaveAccountCommand = new DelegateCommand(SaveAccount);
            CancelCommand = new DelegateCommand(CancelAddAccount);
        }

        private async void SaveAccount()
        {
            var account = new Account
            {
                AccountType = AccountType,
                Name = Name,
                AccountNumber = AccountNumber,
                Rate = Rate,
                StartDate = StartDate,
                EndDate = EndDate,
                StartingAmount = StartingAmount
            };

            var newAccount = await _accountService.Create(account);

            var p = new NavigationParameters
            {
                { "area", "Accounts" },
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}