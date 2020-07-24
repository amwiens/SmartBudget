using LiveCharts;
using LiveCharts.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountsViewModel : BindableBase, INavigationAware
    {
        private ISmartBudgetService _smartBudgetService;
        private SeriesCollection _cardsBalanceCollection;
        private SeriesCollection _depositBalanceCollection;
        private SeriesCollection _creditBalanceCollection;

        private ObservableCollection<Account> _cardAccounts;
        private ObservableCollection<Account> _bankAccounts;
        private ObservableCollection<Account> _creditAccounts;

        private bool _areAccounts;
        private bool _noAccounts;

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

        public bool AreAccounts
        {
            get { return _areAccounts; }
            set { SetProperty(ref _areAccounts, value); }
        }

        public bool NoAccounts
        {
            get { return _noAccounts; }
            set { SetProperty(ref _noAccounts, value); }
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

        public SeriesCollection CreditBalanceCollection
        {
            get { return _creditBalanceCollection; }
            set { SetProperty(ref _creditBalanceCollection, value); }
        }

        public AccountsViewModel(ISmartBudgetService smartBudgetService)
        {
            CardAccounts = new ObservableCollection<Account>();
            BankAccounts = new ObservableCollection<Account>();
            CreditAccounts = new ObservableCollection<Account>();

            _smartBudgetService = smartBudgetService;
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
            GetCardsBalance();
            GetAccounts();
        }

        private void GetCardsBalance()
        {
            CardsBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<double> { 1400, 1500, 1200, 600, 200 },
                    PointGeometry = null,
                    Fill = new SolidColorBrush(Color.FromRgb(223, 245, 210)), // fill color
                    Stroke = new SolidColorBrush(Color.FromRgb(109, 210, 48)) // line color
                }
            };

            DepositBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<double> { 1400, 1500, 1200, 600, 200 },
                    PointGeometry = null,
                    Fill = new SolidColorBrush(Color.FromRgb(255, 239, 215)), // fill color
                    Stroke = new SolidColorBrush(Color.FromRgb(255, 171, 43)) // line color
                }
            };

            CreditBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<double> { 1400, 1500, 1200, 600, 200 },
                    PointGeometry = null,
                    Fill = new SolidColorBrush(Color.FromRgb(255, 218, 233)), // fill color
                    Stroke = new SolidColorBrush(Color.FromRgb(254, 77, 151)) // line color
                }
            };
        }

        private void GetAccounts()
        {
            var accounts = _smartBudgetService.GetAccounts();

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

            if (CardAccounts.Count > 0 || BankAccounts.Count > 0 || CreditAccounts.Count > 0)
            {
                NoAccounts = false;
                AreAccounts = true;
            }
            else
            {
                NoAccounts = true;
                AreAccounts = false;
            }
        }
    }
}