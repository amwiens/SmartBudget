using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Core.Dialogs
{
    public class AddExpenseDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IExpenseService _expenseService;

        private Expense _expense = new Expense();

        public Expense Expense
        {
            get { return _expense; }
            set { SetProperty(ref _expense, value); }
        }

        private ExpenseRecurrence _expenseRecurrence = ExpenseRecurrence.Monthly;

        public ExpenseRecurrence ExpenseRecurrence
        {
            get { return _expenseRecurrence; }
            set
            {
                SetProperty(ref _expenseRecurrence, value);
            }
        }

        public Dictionary<ExpenseRecurrence, string> ExpenseRecurrenceCaptions { get; } =
            new Dictionary<ExpenseRecurrence, string>()
            {
                { ExpenseRecurrence.Daily, "Daily" },
                { ExpenseRecurrence.DailyWithoutWeekend, "Daily w/o weekend" },
                { ExpenseRecurrence.Weekly, "Weekly" },
                { ExpenseRecurrence.Monthly, "Monthly" },
                { ExpenseRecurrence.Yearly, "Yearly" },
                { ExpenseRecurrence.Biweekly, "Biweekly" },
                { ExpenseRecurrence.Bimonthly, "Bimonthly" },
                { ExpenseRecurrence.Quarterly, "Quarterly" },
                { ExpenseRecurrence.Biannually, "Biannually" }
            };

        public DelegateCommand SaveDialogCommand { get; }
        public DelegateCommand CancelDialogCommand { get; }

        public string Title => "Add Transaction";

        public event Action<IDialogResult> RequestClose;

        public AddExpenseDialogViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;

            SaveDialogCommand = new DelegateCommand(async () => await SaveDialog());
            CancelDialogCommand = new DelegateCommand(CancelDialog);
        }

        private async Task SaveDialog()
        {
            var result = ButtonResult.OK;

            var expense = await _expenseService.Create(Expense);

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

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}