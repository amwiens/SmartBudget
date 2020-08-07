using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;

using System.Collections.ObjectModel;

namespace SmartBudget.Expenses.ViewModels
{
    public class ExpensesListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IExpenseService _expenseService;

        private ObservableCollection<Expense> _expenses;

        public ObservableCollection<Expense> Expenses
        {
            get { return _expenses; }
            set { SetProperty(ref _expenses, value); }
        }

        public DelegateCommand<Expense> ExpenseSelectedCommand { get; private set; }

        public ExpensesListViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            IExpenseService expenseService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _expenseService = expenseService;

            Expenses = new ObservableCollection<Expense>();

            ExpenseSelectedCommand = new DelegateCommand<Expense>(ExpenseSelected);
        }

        private void ExpenseSelected(Expense obj)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetExpenses();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private async void GetExpenses()
        {
            var expenses = await _expenseService.GetAll();
            foreach (var expense in expenses)
            {
                Expenses.Add(new Expense
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Amount = expense.Amount,
                    StartDate = expense.StartDate,
                    EndDate = expense.EndDate,
                    IsEndless = expense.IsEndless,
                    Recurrence = expense.Recurrence
                });
            }
        }
    }
}