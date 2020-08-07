using Prism.Ioc;
using Prism.Modularity;

using SmartBudget.Expenses.Views;

namespace SmartBudget.Expenses
{
    public class ExpensesModule : IModule
    {
        public ExpensesModule()
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ExpensesView>("Expenses");
            containerRegistry.RegisterForNavigation<BlankExpensesView>("BlankExpenses");
            containerRegistry.RegisterForNavigation<ExpensesListView>("ExpensesList");
        }
    }
}