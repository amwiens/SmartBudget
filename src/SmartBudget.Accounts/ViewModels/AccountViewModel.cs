using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core;
using SmartBudget.Core.Events;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Threading.Tasks;

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

        public DelegateCommand EditAccountCommand { get; private set; }
        public DelegateCommand DeleteAccountCommand { get; private set; }
        public DelegateCommand AddTransactionCommand { get; private set; }
        public DelegateCommand ImportTransactionsCommand { get; private set; }

        public AccountViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            IAccountService accountService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _accountService = accountService;

            EditAccountCommand = new DelegateCommand(async ()=> await EditAccount());
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
            AddTransactionCommand = new DelegateCommand(AddTransaction);
            ImportTransactionsCommand = new DelegateCommand(async ()=> await ImportTransactions());
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var account = new Account();

            if (navigationContext.Parameters.ContainsKey("account"))
                account = navigationContext.Parameters.GetValue<Account>("account");

            GetTransactions(account.Id).Await(TransactionsLoaded, TransactionsLoadedError);
        }

        private void TransactionsLoaded()
        {
        }

        private void TransactionsLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private void AddTransaction()
        {
            _dialogService.ShowAddTransactionDialog(Account.Id, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    GetTransactions(Account.Id).Await(TransactionsLoaded, TransactionsLoadedError);
                }
            });
        }

        private async Task EditAccount()
        {
            var account = await GetAccount(Account.Id);

            var p = new NavigationParameters
            {
                { "page", "EditAccount" },
                { "account", account }
            };

            _regionManager.RequestNavigate(RegionNames.Content, "Accounts", p);
            _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
        }

        private void DeleteAccount()
        {
            _dialogService.ShowConfirmDialog("Are you sure you want to delete the account?", async result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    var success = await AccountDelete(Account.Id);

                    _regionManager.RequestNavigate(RegionNames.Content, "Accounts");
                    _eventAggregator.GetEvent<NavigationEvent>().Publish("Accounts");
                    _eventAggregator.GetEvent<MessageEvent>().Publish("Account deleted");
                }
            });
        }

        private async Task ImportTransactions()
        {
        }

        private async Task GetTransactions(int accountId)
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

        private async Task<Account> GetAccount(int id)
        {
            var account = await _accountService.Get(int.Parse(id.ToString()));
            return account;
        }

        private async Task<bool> AccountDelete(int id)
        {
            var success = await _accountService.Delete(int.Parse(id.ToString()));
            return success;
        }
    }
}