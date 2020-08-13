using Prism.Ioc;
using Prism.Modularity;

using SmartBudget.Settings.Views;

namespace SmartBudget.Settings
{
    public class SettingsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SettingsView>("Settings");
        }
    }
}