using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;

namespace SmartBudget.Expenses.ViewModels
{
    public class ExpensesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public ExpensesViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove(RegionNames.ExpensesContent);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}