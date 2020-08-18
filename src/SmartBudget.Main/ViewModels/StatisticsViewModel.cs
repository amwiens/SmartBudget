using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;

namespace SmartBudget.Main.ViewModels
{
    public class StatisticsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public StatisticsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
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
            _regionManager.RequestNavigate(RegionNames.StatisticsMonthlyIncomeExpense, "MonthlyIncomeExpenseChartView");
            _regionManager.RequestNavigate(RegionNames.StatisticsTransactionCategories, "CategoryChartView");
        }
    }
}