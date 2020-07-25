using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Accounts.ViewModels
{
    public class AddAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public AddAccountViewModel(IRegionManager regionManager)
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
        }
    }
}