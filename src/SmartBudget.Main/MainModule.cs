using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using SmartBudget.Main.Views;

namespace SmartBudget.Main
{
    public class MainModule : IModule
    {
        public MainModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("Sidebar", typeof(Menu));
            regionManager.RegisterViewWithRegion("Topbar", typeof(TopBar));
            regionManager.RegisterViewWithRegion("Content", typeof(Dashboard));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}