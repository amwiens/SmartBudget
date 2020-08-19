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

        private ObservableCollection<string> _dates = new ObservableCollection<string>();

        public ObservableCollection<string> Dates
        {
            get { return _dates; }
            set { SetProperty(ref _dates, value); }
        }

        private string _selectedDate;

        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                SetProperty(ref _selectedDate, value);
                UpdateChart(value);
            }
        }

        public CategoryChartViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ITransactionCategoryService transactionCategoryService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _transactionCategoryService = transactionCategoryService;

            eventAggregator.GetEvent<RefreshChartEvent>().Subscribe(OnRefreshChartReceived);
        }

        private void OnRefreshChartReceived(string message)
        {
            if (message == "Refresh")
                GetCategories().Await(CategoriesLoaded, CategoriesLoadedError);
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
        }

        private void CategoriesLoaded()
        {
            AddDatesToDropdown();
            SelectedDate = Dates.FirstOrDefault();
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
            MonthlyCategoryInformation.Clear();

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

        private void AddDatesToDropdown()
        {
            foreach (var transactionCategory in TransactionCategories.Where(x => x.Transaction.TransactionType == TransactionType.Expense).OrderByDescending(x => x.Transaction.Date))
            {
                var dateString = $"{transactionCategory.Transaction.Date:MMMM} {transactionCategory.Transaction.Date.Year}";
                if (!Dates.Contains(dateString))
                    Dates.Add(dateString);
            }
        }

        private void UpdateChart(string value)
        {
            string[] splitString = value.Split(" ");

            switch (splitString[0])
            {
                case "January":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 1, 1));
                    break;

                case "February":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 2, 1));
                    break;

                case "March":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 3, 1));
                    break;

                case "April":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 4, 1));
                    break;

                case "May":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 5, 1));
                    break;

                case "June":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 6, 1));
                    break;

                case "July":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 7, 1));
                    break;

                case "August":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 8, 1));
                    break;

                case "September":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 9, 1));
                    break;

                case "October":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 10, 1));
                    break;

                case "November":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 11, 1));
                    break;

                case "December":
                    GetChartData(new DateTime(int.Parse(splitString[1]), 12, 1));
                    break;
            }
        }
    }

    public class ChartData
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}