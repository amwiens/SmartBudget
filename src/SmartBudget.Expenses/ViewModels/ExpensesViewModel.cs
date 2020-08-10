using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using SmartBudget.Core;
using SmartBudget.Core.Extensions;
using SmartBudget.Core.Services;

using System.Linq;

namespace SmartBudget.Expenses.ViewModels
{
    public class ExpensesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IExpenseService _expenseService;

        public DelegateCommand AddExpenseCommand { get; private set; }

        public ExpensesViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            IExpenseService expenseService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _expenseService = expenseService;

            AddExpenseCommand = new DelegateCommand(AddExpense);
        }

        private void AddExpense()
        {
            _dialogService.ShowAddExpenseDialog(result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    GetExpenses();
                }
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove(RegionNames.ExpensesContent);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetExpenses();
        }

        private async void GetExpenses()
        {
            var expenses = await _expenseService.GetAll();

            if (expenses.Count() > 0)
                _regionManager.RequestNavigate(RegionNames.ExpensesContent, "ExpensesList");
            else
                _regionManager.RequestNavigate(RegionNames.ExpensesContent, "BlankExpenses");
        }
    }
}