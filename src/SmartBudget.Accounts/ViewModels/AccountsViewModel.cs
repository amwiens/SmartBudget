using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;
using System.Linq;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService<Account> _accountService;

        private ObservableCollection<Account> _cardAccounts;
        private ObservableCollection<Account> _bankAccounts;
        private ObservableCollection<Account> _creditAccounts;

        public ObservableCollection<Account> CardAccounts
        {
            get { return _cardAccounts; }
            set { SetProperty(ref _cardAccounts, value); }
        }

        public ObservableCollection<Account> BankAccounts
        {
            get { return _bankAccounts; }
            set { SetProperty(ref _bankAccounts, value); }
        }

        public ObservableCollection<Account> CreditAccounts
        {
            get { return _creditAccounts; }
            set { SetProperty(ref _creditAccounts, value); }
        }

        public AccountsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDataService<Account> accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            CardAccounts = new ObservableCollection<Account>();
            BankAccounts = new ObservableCollection<Account>();
            CreditAccounts = new ObservableCollection<Account>();
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
            GetAccounts();

            if (CardAccounts.Count > 0 || BankAccounts.Count > 0 || CreditAccounts.Count > 0)
                _regionManager.RequestNavigate(RegionNames.AccountsContent, "AllAccounts");
            else
                _regionManager.RequestNavigate(RegionNames.AccountsContent, "BlankAccounts");
        }

        private async void GetAccounts()
        {
            var accounts = await _accountService.GetAll();

            foreach (var account in accounts.Where(a => a.AccountType == AccountType.Card))
            {
                CardAccounts.Add(account);
            }
            foreach (var account in accounts.Where(a => a.AccountType == AccountType.Bank))
            {
                BankAccounts.Add(account);
            }
            foreach (var account in accounts.Where(a => a.AccountType == AccountType.Credit))
            {
                CreditAccounts.Add(account);
            }
        }
    }
}