using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core;
using SmartBudget.Core.Dialogs;
using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

namespace SmartBudget.Accounts.ViewModels
{
    public class AccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IAccountService _accountService;

        private Account _account;

        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        public DelegateCommand<int?> EditAccountCommand { get; private set; }
        public DelegateCommand<int?> DeleteAccountCommand { get; private set; }
        public DelegateCommand<int?> AddTransactionCommand { get; private set; }
        public DelegateCommand<int?> ImportTransactionsCommand { get; private set; }

        public AccountViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            IAccountService accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _accountService = accountService;

            EditAccountCommand = new DelegateCommand<int?>(EditAccount);
            DeleteAccountCommand = new DelegateCommand<int?>(DeleteAccount);
            AddTransactionCommand = new DelegateCommand<int?>(AddTransaction);
            ImportTransactionsCommand = new DelegateCommand<int?>(ImportTransactions);
        }

        private void AddTransaction(int? accountId)
        {
            if (accountId == null)
                return;

            _dialogService.ShowAddTransactionDialog(int.Parse(accountId.ToString()), async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    GetTransactions(int.Parse(accountId.ToString()));
                }
            });
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

        private async void DeleteAccount(int? id)
        {
            if (id == null)
                return;

            _dialogService.ShowConfirmDialog("Are you sure you want to delete the account?", async result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    var success = await _accountService.Delete(int.Parse(id.ToString()));

                    _regionManager.RequestNavigate(RegionNames.Content, "Accounts");
                    _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
                }
            });
        }

        private async void ImportTransactions(int? id)
        {
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

            GetTransactions(account.Id);
        }

        private async void GetTransactions(int accountId)
        {
            Account = await _accountService.GetWithTransactions(accountId);

            if (Account.Transactions.Count > 0 || Account.TargetTransactions.Count > 0)
            {
                var p = new NavigationParameters
                {
                    { "accountid", accountId }
                };

                _regionManager.RequestNavigate(RegionNames.TransactionsContent, "Transactions", p);
            }
            else
                _regionManager.RequestNavigate(RegionNames.TransactionsContent, "BlankTransactions");
        }
    }
}