using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Main.Views;

namespace SmartBudget.Main
{
    public class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public MainModule(IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.Topbar, typeof(TopBar));
            _regionManager.RegisterViewWithRegion(RegionNames.Sidebar, typeof(Menu));

            containerRegistry.RegisterForNavigation<Dashboard>();
            containerRegistry.RegisterForNavigation<BlankTransactions>();
            containerRegistry.RegisterForNavigation<BlankAccounts>();
            containerRegistry.RegisterForNavigation<BlankStatistics>();
            containerRegistry.RegisterForNavigation<FavoriteAccounts>();
            containerRegistry.RegisterForNavigation<StatisticsView>();

            _regionManager.RequestNavigate(RegionNames.Content, "Dashboard");
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Dashboard");
        }
    }
}