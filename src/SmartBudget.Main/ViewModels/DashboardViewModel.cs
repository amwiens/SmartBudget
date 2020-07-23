using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Services;

namespace SmartBudget.Main.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private ISmartBudgetService _smartBudgetService;

        public DashboardViewModel(ISmartBudgetService smartBudgetService)
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
            var accounts = _smartBudgetService.GetAccounts();
        }
    }
}