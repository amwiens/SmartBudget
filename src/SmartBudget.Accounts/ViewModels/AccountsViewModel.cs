using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountsViewModel : BindableBase, INavigationAware
    {
        public AccountsViewModel()
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