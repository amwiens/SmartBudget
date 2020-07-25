using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Models;

namespace SmartBudget.Main.ViewModels
{
    public class MenuViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        private bool _dashboardChecked = false;

        public bool DashboardChecked
        {
            get { return _dashboardChecked; }
            set { SetProperty(ref _dashboardChecked, value); }
        }

        private bool _accountsChecked = false;

        public bool AccountsChecked
        {
            get { return _accountsChecked; }
            set { SetProperty(ref _accountsChecked, value); }
        }

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                var p = new NavigationParameters();
                p.Add("Title", navigatePath);

                _regionManager.RequestNavigate("Content", navigatePath);
                _regionManager.RequestNavigate("Topbar", "TopBar", p);

                switch (navigatePath)
                {
                    case "Dashboard":
                        DashboardChecked = true;
                        AccountsChecked = false;
                        break;

                    case "Accounts":
                        DashboardChecked = false;
                        AccountsChecked = true;
                        break;

                    default:
                        throw new System.Exception("Value not found");
                }
            }
        }

        private void Navigate(string navigatePath, NavigationParameters navigationParameters)
        {
            if (navigatePath != null)
            {
                if (navigationParameters.ContainsKey("account"))
                {
                    var p = new NavigationParameters();
                    p.Add("Title", navigatePath);

                    _regionManager.RequestNavigate("Content", "Account", navigationParameters);
                    _regionManager.RequestNavigate("Topbar", "TopBar", p);

                    DashboardChecked = false;
                    AccountsChecked = true;
                }
                if (navigationParameters.ContainsKey("page"))
                {
                    var p = new NavigationParameters();
                    p.Add("Title", navigatePath);

                    _regionManager.RequestNavigate("Content", "AddAccount");
                    _regionManager.RequestNavigate("Topbar", "TopBar", p);

                    DashboardChecked = false;
                    AccountsChecked = true;
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var area = string.Empty;
            var p = new NavigationParameters();

            if (navigationContext.Parameters.ContainsKey("area"))
            {
                area = navigationContext.Parameters.GetValue<string>("area");
            }
            if (navigationContext.Parameters.ContainsKey("account"))
            {
                Account account = navigationContext.Parameters.GetValue<Account>("account");
                p.Add("account", account);
            }
            if (navigationContext.Parameters.ContainsKey("page"))
            {
                string page = navigationContext.Parameters.GetValue<string>("page");
                p.Add("page", page);
            }

            switch (area)
            {
                case "Accounts":
                    if (p.Count > 0)
                        Navigate("Accounts", p);
                    else
                        Navigate("Accounts");
                    break;

                default:
                    Navigate("Dashboard");
                    break;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}