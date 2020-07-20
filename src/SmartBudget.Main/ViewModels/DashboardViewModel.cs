using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Main.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        public DashboardViewModel()
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
        }
    }
}