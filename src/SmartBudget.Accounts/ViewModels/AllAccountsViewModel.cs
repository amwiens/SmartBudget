using LiveCharts;
using LiveCharts.Wpf;

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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SmartBudget.Accounts.ViewModels
{
    public class AllAccountsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccountService _accountService;

        private SeriesCollection _cardsBalanceCollection;
        private SeriesCollection _depositBalanceCollection;
        private SeriesCollection _creditBalanceCollection;

        private ObservableCollection<Account> _cardAccounts;
        private ObservableCollection<Account> _bankAccounts;
        private ObservableCollection<Account> _creditAccounts;

        public DelegateCommand<Account> AccountSelectedCommand { get; private set; }
        public DelegateCommand AddAccountCommand { get; private set; }

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

        public SeriesCollection CardsBalanceCollection
        {
            get { return _cardsBalanceCollection; }
            set { SetProperty(ref _cardsBalanceCollection, value); }
        }

        public SeriesCollection DepositBalanceCollection
        {
            get { return _depositBalanceCollection; }
            set { SetProperty(ref _depositBalanceCollection, value); }
        }

        private decimal _cardsBalance;

        public decimal CardsBalance
        {
            get { return _cardsBalance; }
            set { SetProperty(ref _cardsBalance, value); }
        }

        private decimal _depositBalance;

        public decimal DepositBalance
        {
            get { return _depositBalance; }
            set { SetProperty(ref _depositBalance, value); }
        }

        private decimal _creditBalance;

        public decimal CreditBalance
        {
            get { return _creditBalance; }
            set { SetProperty(ref _creditBalance, value); }
        }

        public SeriesCollection CreditBalanceCollection
        {
            get { return _creditBalanceCollection; }
            set { SetProperty(ref _creditBalanceCollection, value); }
        }

        public List<string> Labels { get; set; }

        public AllAccountsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IAccountService accountService)
        {
            CardAccounts = new ObservableCollection<Account>();
            BankAccounts = new ObservableCollection<Account>();
            CreditAccounts = new ObservableCollection<Account>();
            Labels = new List<string>();

            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            AccountSelectedCommand = new DelegateCommand<Account>(AccountSelected);
            AddAccountCommand = new DelegateCommand(AddAccount);
        }

        private void AddAccount()
        {
            var p = new NavigationParameters
            {
                { "page", "AddAccount" }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
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
            GetAccounts();
            if (CardAccounts.Count > 0)
                GetCardsBalance();
            if (BankAccounts.Count > 0)
                GetBankBalance();
            if (CreditAccounts.Count > 0)
                GetCreditBalance();


            for (int i = 30; i > 0; i--)
            {
                var newDate = DateTime.Now.AddDays(-i);
                Labels.Add(newDate.ToShortDateString());
            }
        }

        private async void GetCardsBalance()
        {
            CardsBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = await GetAccountBalanceChartValues(AccountType.Card),
                    PointGeometry = null,
                    Fill = new SolidColorBrush(Color.FromRgb(223, 245, 210)), // fill color
                    Stroke = new SolidColorBrush(Color.FromRgb(109, 210, 48)) // line color
                }
            };

            CardsBalance = CardAccounts.Sum(a => a.Balance);
        }

        private async void GetBankBalance()
        {
            DepositBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = await GetAccountBalanceChartValues(AccountType.Bank),
                    PointGeometry = null,
                    Fill = new SolidColorBrush(Color.FromRgb(255, 239, 215)), // fill color
                    Stroke = new SolidColorBrush(Color.FromRgb(255, 171, 43)) // line color
                }
            };

            DepositBalance = BankAccounts.Sum(a => a.Balance);
        }

        private async void GetCreditBalance()
        {
            CreditBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = await GetAccountBalanceChartValues(AccountType.Credit),
                    PointGeometry = null,
                    Fill = new SolidColorBrush(Color.FromRgb(255, 218, 233)), // fill color
                    Stroke = new SolidColorBrush(Color.FromRgb(254, 77, 151)) // line color
                }
            };

            CreditBalance = CreditAccounts.Sum(a => a.Balance);
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

        private async Task<ChartValues<decimal>> GetAccountBalanceChartValues(AccountType accountType)
        {
            var chartValues = new ChartValues<decimal>();

            for (int i = 30; i > 0; i--)
            {
                var newDate = DateTime.Now.AddDays(-i);

                var accounts = await _accountService.GetAccountDataBeforeDateByType(newDate, accountType);
                var sum = accounts.Sum(a => a.Balance);
                chartValues.Add(sum);
            }

            return chartValues;
        }
    }
}