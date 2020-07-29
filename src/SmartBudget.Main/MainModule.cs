using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using SmartBudget.Main.Views;

namespace SmartBudget.Main
{
    public class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Menu>();
            containerRegistry.RegisterForNavigation<TopBar>();
            containerRegistry.RegisterForNavigation<Dashboard>();
            containerRegistry.RegisterForNavigation<BlankTransactions>();
            containerRegistry.RegisterForNavigation<BlankAccounts>();
            containerRegistry.RegisterForNavigation<BlankStatistics>();
            containerRegistry.RegisterForNavigation<FavoriteAccounts>();
            _regionManager.RequestNavigate("Sidebar", "Menu");
        }
    }
}