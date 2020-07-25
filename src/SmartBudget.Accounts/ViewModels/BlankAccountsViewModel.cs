using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Accounts.ViewModels
{
    public class BlankAccountsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand AddCardAccountCommand { get; private set; }
        public DelegateCommand AddBankAccountCommand { get; private set; }

        public BlankAccountsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddCardAccountCommand = new DelegateCommand(AddCardAccount);
            AddBankAccountCommand = new DelegateCommand(AddBankAccount);
        }

        private void AddCardAccount()
        {
            var p = new NavigationParameters();
            p.Add("area", "Accounts");
            p.Add("page", "AddAccount");

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }

        private void AddBankAccount()
        {
            var p = new NavigationParameters();
            p.Add("area", "Accounts");
            p.Add("page", "AddAccount");

            _regionManager.RequestNavigate("Sidebar", "Menu", p);
        }
    }
}