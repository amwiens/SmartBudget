using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using SmartBudget.Core.Events;
using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System;
using System.Threading.Tasks;

namespace SmartBudget.Core.Dialogs
{
    public class ExpenseDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IExpenseService _expenseService;

        private Expense _expense;

        public Expense Expense
        {
            get { return _expense; }
            set { SetProperty(ref _expense, value); }
        }

        public string Title => "Expense";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand EditExpenseCommand { get; }
        public DelegateCommand DeleteExpenseCommand { get; }
        public DelegateCommand CloseDialogCommand { get; }

        public ExpenseDialogViewModel(IEventAggregator eventAggregator,
            IExpenseService expenseService)
        {
            _eventAggregator = eventAggregator;
            _expenseService = expenseService;

            EditExpenseCommand = new DelegateCommand(EditExpense);
            DeleteExpenseCommand = new DelegateCommand(async () => await DeleteExpense());
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private void EditExpense()
        {
            throw new NotImplementedException();
        }

        private async Task DeleteExpense()
        {
            var result = ButtonResult.OK;

            if (await ExpenseDelete(Expense.Id))
            {
                _eventAggregator.GetEvent<MessageEvent>().Publish("Expense deleted");
                RequestClose?.Invoke(new DialogResult(result));
            }
        }

        private void CloseDialog()
        {
            var result = ButtonResult.No;

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
            var expenseId = parameters.GetValue<int>("expenseid");
            Expense = await _expenseService.Get(expenseId);
        }

        public async Task<bool> ExpenseDelete(int id)
        {
            var result = await _expenseService.Delete(id);
            return result;
        }
    }
}