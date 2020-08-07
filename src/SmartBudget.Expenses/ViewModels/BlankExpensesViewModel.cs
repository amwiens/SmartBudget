using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Expenses.ViewModels
{
    public class BlankExpensesViewModel : BindableBase, INavigationAware
    {
        public BlankExpensesViewModel()
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