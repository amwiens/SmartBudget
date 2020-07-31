using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService<Account> _accountService;

        private Account _account;

        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private decimal _balance = 0.00M;

        public decimal Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        public DelegateCommand<int?> EditAccountCommand { get; private set; }

        public AccountViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDataService<Account> accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _accountService = accountService;

            EditAccountCommand = new DelegateCommand<int?>(EditAccount);
        }

        private async void EditAccount(int? id)
        {
            if (id == null)
                return;

            var account = await _accountService.Get(int.Parse(id.ToString()));

            var p = new NavigationParameters
            {
                { "page", "EditAccount" },
                { "account", account }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var account = new Account();

            if (navigationContext.Parameters.ContainsKey("account"))
                account = navigationContext.Parameters.GetValue<Account>("account");

            Account = await _accountService.Get(account.Id);
        }
    }
}