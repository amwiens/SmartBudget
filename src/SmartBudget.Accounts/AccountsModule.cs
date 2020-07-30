using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using SmartBudget.Accounts.Views;

namespace SmartBudget.Accounts
{
    public class AccountsModule : IModule
    {
        public AccountsModule(IRegionManager regionManager)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AccountsView>("Accounts");
            containerRegistry.RegisterForNavigation<AccountView>("Account");
            containerRegistry.RegisterForNavigation<BlankAccounts>("BlankAccounts");
            containerRegistry.RegisterForNavigation<AddAccountView>("AddAccount");
        }
    }
}