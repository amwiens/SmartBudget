using LiveCharts;
using LiveCharts.Wpf;

using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Events;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SmartBudget.Main.ViewModels
{
    public class MonthlyIncomeExpenseChartViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITransactionService _transactionService;
        private SeriesCollection _monthlyTransactionInformation;

        public SeriesCollection MonthlyTransactionInformation
        {
            get { return _monthlyTransactionInformation; }
            set { SetProperty(ref _monthlyTransactionInformation, value); }
        }

        private ObservableCollection<Transaction> _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        public string[] Labels { get; set; }
        public Func<decimal, string> Formatter { get; set; }

        public MonthlyIncomeExpenseChartViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ITransactionService transactionService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _transactionService = transactionService;
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
            GetTransactions().Await(TransactionsLoaded, TransactionsLoadedError);

            MonthlyTransactionInformation = new SeriesCollection();
            MonthlyTransactionInformation.Add(new ColumnSeries
            {
                Title = "Income",
                Values = new ChartValues<decimal>
                {
                    GetMonthlyIncome(DateTime.Now.AddMonths(-5)),
                    GetMonthlyIncome(DateTime.Now.AddMonths(-4)),
                    GetMonthlyIncome(DateTime.Now.AddMonths(-3)),
                    GetMonthlyIncome(DateTime.Now.AddMonths(-2)),
                    GetMonthlyIncome(DateTime.Now.AddMonths(-1)),
                    GetMonthlyIncome(DateTime.Now),
                },
                Fill = new SolidColorBrush(Color.FromRgb(223, 245, 210))
            });

            MonthlyTransactionInformation.Add(new ColumnSeries
            {
                Title = "Expenses",
                Values = new ChartValues<decimal>
                {
                    GetMonthlyExpenses(DateTime.Now.AddMonths(-5)),
                    GetMonthlyExpenses(DateTime.Now.AddMonths(-4)),
                    GetMonthlyExpenses(DateTime.Now.AddMonths(-3)),
                    GetMonthlyExpenses(DateTime.Now.AddMonths(-2)),
                    GetMonthlyExpenses(DateTime.Now.AddMonths(-1)),
                    GetMonthlyExpenses(DateTime.Now),
                },
                Fill = new SolidColorBrush(Color.FromRgb(255, 239, 215))
            });

            Labels = new[]
            {
                DateTime.Now.AddMonths(-5).ToString("MMM"),
                DateTime.Now.AddMonths(-4).ToString("MMM"),
                DateTime.Now.AddMonths(-3).ToString("MMM"),
                DateTime.Now.AddMonths(-2).ToString("MMM"),
                DateTime.Now.AddMonths(-1).ToString("MMM"),
                DateTime.Now.ToString("MMM")
            };
            Formatter = value => value.ToString("C");
        }

        private void TransactionsLoaded()
        {
        }

        private void TransactionsLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private async Task GetTransactions()
        {
            var transactions = await _transactionService.GetAll();
            Transactions = new ObservableCollection<Transaction>(transactions);
        }

        private decimal GetMonthlyIncome(DateTime date)
        {
            var incomeTransactions = Transactions
                .Where(x => x.TransactionType == TransactionType.Income)
                .Where(x => x.Date.Month == date.Month)
                .Where(x => x.Date.Year == date.Year);
            return incomeTransactions.Sum(x => x.Amount);
        }

        private decimal GetMonthlyExpenses(DateTime date)
        {
            var incomeTransactions = Transactions
                .Where(x => x.TransactionType == TransactionType.Expense)
                .Where(x => x.Date.Month == date.Month)
                .Where(x => x.Date.Year == date.Year);
            return incomeTransactions.Sum(x => x.Amount);
        }
    }
}