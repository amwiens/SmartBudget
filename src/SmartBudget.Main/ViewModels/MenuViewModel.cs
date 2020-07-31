using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;

namespace SmartBudget.Main.ViewModels
{
    public class MenuViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

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

        public MenuViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<NavigationEvent>().Subscribe(OnNavigationReceived);
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void OnNavigationReceived(string message)
        {
            DashboardChecked = false;
            AccountsChecked = false;

            switch (message)
            {
                case "Dashboard":
                    DashboardChecked = true;
                    break;

                case "Accounts":
                    AccountsChecked = true;
                    break;
            }
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate(RegionNames.Content, navigatePath);
                _eventAggregator.GetEvent<NavigationEvent>().Publish(navigatePath);
            }
        }

        private void Navigate(string navigatePath, NavigationParameters navigationParameters)
        {
            if (navigatePath != null)
            {
                if (navigationParameters.ContainsKey("account"))
                {
                    _regionManager.RequestNavigate(RegionNames.AccountsContent, "Account", navigationParameters);
                    _eventAggregator.GetEvent<NavigationEvent>().Publish(navigatePath);
                }
                if (navigationParameters.ContainsKey("page"))
                {
                    _regionManager.RequestNavigate(RegionNames.AccountsContent, "AddAccount");
                    _eventAggregator.GetEvent<NavigationEvent>().Publish(navigatePath);
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