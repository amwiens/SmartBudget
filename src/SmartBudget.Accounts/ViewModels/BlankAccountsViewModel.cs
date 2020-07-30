using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Events;

namespace SmartBudget.Accounts.ViewModels
{
    public class BlankAccountsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand AddAccountCommand { get; private set; }

        public BlankAccountsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            AddAccountCommand = new DelegateCommand(AddAccount);
        }

        private void AddAccount()
        {
            _regionManager.RequestNavigate("AccountsContent", "AddAccount");
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Dashboard");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

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