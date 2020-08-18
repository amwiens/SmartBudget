using LiveCharts;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SmartBudget.Main.ViewModels
{
    public class CategoryChartViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICategoryService _categoryService;

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

        public CategoryChartViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ICategoryService categoryService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _categoryService = categoryService;
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
    }
}