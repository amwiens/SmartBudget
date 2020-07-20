using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Navigate("Dashboard");
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