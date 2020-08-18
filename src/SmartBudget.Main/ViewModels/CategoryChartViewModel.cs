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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SmartBudget.Main.ViewModels
{
    public class CategoryChartViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITransactionCategoryService _transactionCategoryService;

        private SeriesCollection _monthlyCategoryInformation = new SeriesCollection();

        public SeriesCollection MonthlyCategoryInformation
        {
            get { return _monthlyCategoryInformation; }
            set { SetProperty(ref _monthlyCategoryInformation, value); }
        }

        private ObservableCollection<TransactionCategory> _transactionCategories;

        public ObservableCollection<TransactionCategory> TransactionCategories
        {
            get { return _transactionCategories; }
            set { SetProperty(ref _transactionCategories, value); }
        }

        public string[] Labels { get; set; }
        public Func<decimal, string> Formatter { get; set; }

        public CategoryChartViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ITransactionCategoryService transactionCategoryService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _transactionCategoryService = transactionCategoryService;
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
            GetCategories().Await(CategoriesLoaded, CategoriesLoadedError);
            GetChartData(DateTime.Now.AddMonths(-1));
        }

        private void CategoriesLoaded()
        {
        }

        private void CategoriesLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private async Task GetCategories()
        {
            var transactionCategories = await _transactionCategoryService.GetAll();
            TransactionCategories = new ObservableCollection<TransactionCategory>(transactionCategories);
        }

        private void GetChartData(DateTime date)
        {
            List<ChartData> chartData = TransactionCategories
                .Where(x => x.Transaction.TransactionType == TransactionType.Expense)
                .Where(x => x.Transaction.Date.Month == date.Month)
                .Where(x => x.Transaction.Date.Year == date.Year)
                .GroupBy(x => x.CategoryId)
                .Select(cd => new ChartData
                {
                    Name = cd.Select(x => x.Category.Name).FirstOrDefault(),
                    Amount = cd.Sum(x => x.Transaction.Amount)
                }).ToList();

            foreach (var data in chartData)
            {
                var pieSeries = new PieSeries
                {
                    Title = data.Name,
                    Values = new ChartValues<decimal> { data.Amount }
                };
                MonthlyCategoryInformation.Add(pieSeries);
            }
        }
    }

    public class ChartData
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}