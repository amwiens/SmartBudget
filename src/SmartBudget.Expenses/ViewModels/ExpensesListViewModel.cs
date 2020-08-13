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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBudget.Expenses.ViewModels
{
    public class ExpensesListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IExpenseService _expenseService;

        private ObservableCollection<Expense> _monthlyExpenses;

        public ObservableCollection<Expense> MonthlyExpenses
        {
            get { return _monthlyExpenses; }
            set { SetProperty(ref _monthlyExpenses, value); }
        }

        private ObservableCollection<Expense> _yearlyExpenses;

        public ObservableCollection<Expense> YearlyExpenses
        {
            get { return _yearlyExpenses; }
            set { SetProperty(ref _yearlyExpenses, value); }
        }

        private ObservableCollection<Expense> _otherExpenses;

        public ObservableCollection<Expense> OtherExpenses
        {
            get { return _otherExpenses; }
            set { SetProperty(ref _otherExpenses, value); }
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

            MonthlyExpenses = new ObservableCollection<Expense>();
            YearlyExpenses = new ObservableCollection<Expense>();
            OtherExpenses = new ObservableCollection<Expense>();

            ExpenseSelectedCommand = new DelegateCommand<Expense>(ExpenseSelected);
        }

        private void ExpenseSelected(Expense expense)
        {
            _dialogService.ShowExpenseDialog(expense.Id, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    _regionManager.RequestNavigate(RegionNames.Content, "Expenses");
                    _eventAggregator.GetEvent<NavigationEvent>().Publish("Expenses");
                }
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetExpenses().Await(ExpensesLoaded, ExpensesLoadedError);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void ExpensesLoaded()
        {
        }

        private void ExpensesLoadedError(Exception ex)
        {
            _eventAggregator.GetEvent<ExceptionEvent>().Publish(ex);
        }

        private async Task GetExpenses()
        {
            var expenses = await _expenseService.GetAll();

            foreach (var expense in expenses.Where(e => e.Recurrence == ExpenseRecurrence.Monthly && (e.EndDate is null || e.EndDate > DateTime.Now) && e.StartDate.AddMonths(-1) < DateTime.Now).OrderBy(e => e.StartDate.Day))
            {
                MonthlyExpenses.Add(new Expense
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Amount = expense.Amount,
                    StartDate = expense.StartDate,
                    EndDate = expense.EndDate,
                    IsEndless = expense.IsEndless,
                    Recurrence = expense.Recurrence,
                    Note = expense.Note
                });
            }

            foreach (var expense in expenses.Where(e => e.Recurrence == ExpenseRecurrence.Yearly && (e.EndDate is null || e.EndDate > DateTime.Now) && e.StartDate.AddMonths(-1) < DateTime.Now).OrderBy(e => e.StartDate.Day).OrderBy(e => e.StartDate.Month))
            {
                YearlyExpenses.Add(new Expense
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Amount = expense.Amount,
                    StartDate = expense.StartDate,
                    EndDate = expense.EndDate,
                    IsEndless = expense.IsEndless,
                    Recurrence = expense.Recurrence,
                    Note = expense.Note
                });
            }

            foreach (var expense in expenses.Where(e => e.Recurrence != ExpenseRecurrence.Monthly && e.Recurrence != ExpenseRecurrence.Yearly && (e.EndDate is null || e.EndDate > DateTime.Now) && e.StartDate.AddMonths(-1) < DateTime.Now))
            {
                OtherExpenses.Add(new Expense
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Amount = expense.Amount,
                    StartDate = expense.StartDate,
                    EndDate = expense.EndDate,
                    IsEndless = expense.IsEndless,
                    Recurrence = expense.Recurrence,
                    Note = expense.Note
                });
            }
        }
    }
}