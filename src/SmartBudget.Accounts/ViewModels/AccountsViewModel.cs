using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
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
        private readonly IAccountService _accountService;

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
            IAccountService accountService)
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
            _regionManager.Regions.Remove(RegionNames.TransactionsContent);
            _regionManager.Regions.Remove(RegionNames.AccountsContent);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("page"))
            {
                switch (navigationContext.Parameters.GetValue<string>("page"))
                {
                    case "Account":
                        _regionManager.RequestNavigate(RegionNames.AccountsContent, "Account", navigationContext.Parameters);
                        break;

                    case "AddAccount":
                        _regionManager.RequestNavigate(RegionNames.AccountsContent, "AddAccount", navigationContext.Parameters);
                        break;

                    case "EditAccount":
                        _regionManager.RequestNavigate(RegionNames.AccountsContent, "EditAccount", navigationContext.Parameters);
                        break;

                    case "AllAccounts":
                    default:
                        ShowAllAccounts();
                        break;
                }
            }
            else
            {
                ShowAllAccounts();
            }
        }

        private void ShowAllAccounts()
        {
            GetAccounts();

            if (CardAccounts.Count > 0 || BankAccounts.Count > 0 || CreditAccounts.Count > 0)
            {
                _regionManager.RequestNavigate(RegionNames.AccountsContent, "AllAccounts");
                _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
            }
            else
            {
                _regionManager.RequestNavigate(RegionNames.AccountsContent, "BlankAccounts");
                _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
            }
        }

        private async void GetAccounts()
        {
            var accounts = await _accountService.GetAllWithTransactions();

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