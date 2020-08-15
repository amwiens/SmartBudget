using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Extensions;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.Core.Dialogs
{
    public class AddTransactionDialogViewModel : BindableBase, IDialogAware
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly IPayeeService _payeeService;
        private readonly IDialogService _dialogService;
        private readonly ICategoryService _categoryService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        private ObservableCollection<Payee> _payees;

        public ObservableCollection<Payee> Payees
        {
            get { return _payees; }
            set { SetProperty(ref _payees, value); }
        }

        private Transaction _transaction;

        public Transaction Transaction
        {
            get { return _transaction; }
            set { SetProperty(ref _transaction, value); }
        }

        private string _categoriesMessage;

        public string CategoriesMessage
        {
            get { return _categoriesMessage; }
            set { SetProperty(ref _categoriesMessage, value); }
        }

        private bool _isTransfer = false;

        public bool IsTransfer
        {
            get { return _isTransfer; }
            set { SetProperty(ref _isTransfer, value); }
        }

        private decimal _amount = 0.0M;

        public decimal Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        private TransactionType _transactionType = TransactionType.Expense;

        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set
            {
                SetProperty(ref _transactionType, value);
                IsTransfer = TransactionType == TransactionType.Transfer;
            }
        }

        private ObservableCollection<TransactionCategory> _transactionCategories;

        public ObservableCollection<TransactionCategory> TransactionCategories
        {
            get { return _transactionCategories; }
            set { SetProperty(ref _transactionCategories, value); }
        }

        public Dictionary<TransactionType, string> TransactionTypeCaptions { get; } =
            new Dictionary<TransactionType, string>()
            {
                { TransactionType.Expense, "Expense" },
                { TransactionType.Income, "Income" },
                { TransactionType.Transfer, "Transfer" }
            };

        public Dictionary<int, string> AccountCaptions { get; }

        private Account _account;

        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        private Payee _payee;

        public Payee Payee
        {
            get { return _payee; }
            set { SetProperty(ref _payee, value); }
        }

        private string _newPayee;

        public string NewPayee
        {
            get { return _newPayee; }
            set { SetProperty(ref _newPayee, value); }
        }

        public DelegateCommand AddCategoryCommand { get; }
        public DelegateCommand EditCategoryCommand { get; }
        public DelegateCommand DeleteCategoryCommand { get; }
        public DelegateCommand SaveDialogCommand { get; }
        public DelegateCommand CancelDialogCommand { get; }

        public string Title => "Add Transaction";

        public event Action<IDialogResult> RequestClose;

        public AddTransactionDialogViewModel(ITransactionService transactionService,
            IAccountService accountService,
            IPayeeService payeeService,
            IDialogService dialogService,
            ICategoryService categoryService,
            ITransactionCategoryService transactionCategoryService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _payeeService = payeeService;
            _dialogService = dialogService;
            _categoryService = categoryService;
            _transactionCategoryService = transactionCategoryService;

            TransactionCategories = new ObservableCollection<TransactionCategory>();
            Payees = new ObservableCollection<Payee>();
            GetPayees();

            AccountCaptions = new Dictionary<int, string>();

            Transaction = new Transaction
            {
                Payee = new Payee(),
                TransactionCategories = new ObservableCollection<TransactionCategory>(),
                Date = DateTime.Now
            };

            AddCategoryCommand = new DelegateCommand(AddCategory);
            EditCategoryCommand = new DelegateCommand(EditCategory);
            DeleteCategoryCommand = new DelegateCommand(DeleteCategory);
            SaveDialogCommand = new DelegateCommand(async () => await SaveDialog());
            CancelDialogCommand = new DelegateCommand(CancelDialog);
        }

        private void AddCategory()
        {
            var splitSum = Transaction.TransactionCategories.Sum(x => x.Amount);
            var difference = Amount - splitSum;

            _dialogService.ShowAddEditCategoryToTransactionDialog(0, difference, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var categoryId = result.Parameters.GetValue<int>("categoryid");
                    var amount = result.Parameters.GetValue<decimal>("amount");

                    TransactionCategories.Add(new TransactionCategory
                    {
                        Transaction = Transaction,
                        Category = await _categoryService.Get(categoryId),
                        Amount = amount
                    });

                    CheckCategoryBalance();
                }
            });
        }

        private void EditCategory()
        {
            _dialogService.ShowAddEditCategoryToTransactionDialog(0, 0, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                }
            });
        }

        private void DeleteCategory()
        {
            throw new NotImplementedException();
        }

        private async Task SaveDialog()
        {
            Transaction transaction;
            var result = ButtonResult.OK;
            Payee payee;

            if (Payee is null)
            {
                var newPayee = new Payee
                {
                    Name = NewPayee,
                    Latitude = 0.0M,
                    Longitude = 0.0M
                };
                payee = await _payeeService.Create(newPayee);
            }
            else
                payee = Payee;

            Transaction.AccountId = Account.Id;
            Transaction.TransactionType = TransactionType;
            Transaction.Amount = Amount;
            Transaction.Payee = null;
            Transaction.TransactionCategories = null;
            Transaction.PayeeId = payee.Id;
            if (Transaction.Id == 0)
                transaction = await _transactionService.Create(Transaction);
            else
                transaction = await _transactionService.Update(Transaction.Id, Transaction);

            if (TransactionCategories.Count > 0)
            {
                var success = await _transactionCategoryService.DeleteByTransactionId(transaction.Id);
                if (success)
                {
                    foreach (var transactionCategory in TransactionCategories)
                    {
                        transactionCategory.CategoryId = transactionCategory.Category.Id;
                        transactionCategory.TransactionId = transactionCategory.Transaction.Id;
                        transactionCategory.Category = null;
                        transactionCategory.Transaction = null;
                        await _transactionCategoryService.Create(transactionCategory);
                    }
                }
            }

            RequestClose?.Invoke(new DialogResult(result));
        }

        private void CancelDialog()
        {
            var result = ButtonResult.Cancel;

            RequestClose?.Invoke(new DialogResult(result));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var accountId = parameters.GetValue<int>("accountid");
            var transactionId = parameters.GetValue<int>("transactionid");

            Account = await _accountService.Get(accountId);

            if (transactionId != 0)
            {
                Transaction = await _transactionService.Get(transactionId);
                Amount = Transaction.Amount;
                Payee = Transaction.Payee;
                NewPayee = Transaction.Payee.Name;
                TransactionCategories = new ObservableCollection<TransactionCategory>(Transaction.TransactionCategories);
            }

            CheckCategoryBalance();
            await GetAccounts();
        }

        private async Task GetAccounts()
        {
            var accounts = await _accountService.GetAll();

            foreach (var account in accounts)
            {
                AccountCaptions.Add(account.Id, account.Name);
            }
        }

        private async void GetPayees()
        {
            var payees = await _payeeService.GetAll();

            foreach (var payee in payees)
            {
                Payees.Add(payee);
            }
        }

        private void CheckCategoryBalance()
        {
            var splitSum = TransactionCategories.Sum(x => x.Amount);

            if (splitSum < Amount)
                CategoriesMessage = "You have uncategorized amounts";
            else if (splitSum > Amount)
                CategoriesMessage = "Your categorized amount is greater than your total";
            else
                CategoriesMessage = string.Empty;
        }
    }
}