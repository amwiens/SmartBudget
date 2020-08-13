using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Settings.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        public SettingsViewModel()
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