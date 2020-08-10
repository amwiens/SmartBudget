using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;

namespace SmartBudget.Main.ViewModels
{
    public class MenuViewModel : BindableBase
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

        private bool _expensesChecked = false;

        public bool ExpensesChecked
        {
            get { return _expensesChecked; }
            set { SetProperty(ref _expensesChecked, value); }
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

                case "Expenses":
                    ExpensesChecked = true;
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
    }
}