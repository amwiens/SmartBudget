using LiveCharts;
using LiveCharts.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Services;

using System.Windows.Media;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountsViewModel : BindableBase, INavigationAware
    {
        private ISmartBudgetService _smartBudgetService;
        private SeriesCollection _cardsBalanceCollection;
        private SeriesCollection _depositBalanceCollection;
        private SeriesCollection _creditBalanceCollection;

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
    }
}