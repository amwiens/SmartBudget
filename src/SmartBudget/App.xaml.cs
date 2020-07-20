using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

using SmartBudget.Accounts;
using SmartBudget.Main;
using SmartBudget.Views;

using System.Windows;

namespace SmartBudget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<AccountsModule>();
        }
    }
}