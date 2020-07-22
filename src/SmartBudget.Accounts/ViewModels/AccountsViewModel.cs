using LiveCharts;
using LiveCharts.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using System.Windows.Media;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountsViewModel : BindableBase, INavigationAware
    {
        private SeriesCollection _cardsBalanceCollection;

        public SeriesCollection CardsBalanceCollection
        {
            get { return _cardsBalanceCollection; }
            set { SetProperty(ref _cardsBalanceCollection, value); }
        }

        private SeriesCollection _depositBalanceCollection;

        public SeriesCollection DepositBalanceCollection
        {
            get { return _depositBalanceCollection; }
            set { SetProperty(ref _depositBalanceCollection, value); }
        }

        private SeriesCollection _creditBalanceCollection;

        public SeriesCollection CreditBalanceCollection
        {
            get { return _creditBalanceCollection; }
            set { SetProperty(ref _creditBalanceCollection, value); }
        }

        public AccountsViewModel()
        {
            GetCardsBalance();
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

        private void GetCardsBalance()
        {
            CardsBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<double> { 1400, 1500, 1200, 600, 200 },
                    PointGeometry = null,
                    PointForeground = Brushes.Green
                }
            };

            DepositBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<double> { 1400, 1500, 1200, 600, 200 },
                    PointGeometry = null,
                    PointForeground = Brushes.Yellow
                }
            };

            CreditBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Balance",
                    Values = new ChartValues<double> { 1400, 1500, 1200, 600, 200 },
                    PointGeometry = null,
                    PointForeground = Brushes.Pink
                }
            };
        }
    }
}