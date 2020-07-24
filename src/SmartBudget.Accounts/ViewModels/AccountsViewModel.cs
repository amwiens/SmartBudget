using LiveCharts;
using LiveCharts.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;
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

        public bool AreAccounts => CardAccounts?.Count > 0 || BankAccounts?.Count > 0 || CreditAccounts?.Count > 0;
        public bool NoAccounts => CardAccounts?.Count == 0 || BankAccounts?.Count == 0 || CreditAccounts?.Count == 0;

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
            var favoriteAccounts = new ObservableCollection<Account>();
            var accounts = _smartBudgetService.GetAccounts();

            foreach (var account in accounts)
            {
                favoriteAccounts.Add(account);
            }

            for (int i = 0; i < 10; i++)
            {
                favoriteAccounts.Add(new Account { Name = $"Test {i}" });
            }
            CardAccounts = favoriteAccounts;
        }
    }
}