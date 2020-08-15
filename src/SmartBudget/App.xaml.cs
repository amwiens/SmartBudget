using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

using SmartBudget.Accounts;
using SmartBudget.Core.Dialogs;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services;
using SmartBudget.Expenses;
using SmartBudget.Main;
using SmartBudget.Settings;
using SmartBudget.Views;
using SmartBudget.YelpAPI.Services;

using System.Windows;

namespace SmartBudget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Data Services
            containerRegistry.RegisterSingleton<IAccountService, AccountDataService>();
            containerRegistry.RegisterSingleton<ITransactionService, TransactionDataService>();
            containerRegistry.RegisterSingleton<IPayeeService, PayeeDataService>();
            containerRegistry.RegisterSingleton<IBusinessService, BusinessService>();
            containerRegistry.RegisterSingleton<IExpenseService, ExpenseDataService>();
            containerRegistry.RegisterSingleton<ICategoryService, CategoryDataService>();
            containerRegistry.RegisterSingleton<ITransactionCategoryService, TransactionCategoryDataService>();

            // Dialogs
            containerRegistry.RegisterDialog<ConfirmDialog, ConfirmDialogViewModel>();
            containerRegistry.RegisterDialog<TransactionDialog, TransactionDialogViewModel>();
            containerRegistry.RegisterDialog<AddTransactionDialog, AddTransactionDialogViewModel>();
            containerRegistry.RegisterDialog<ExpenseDialog, ExpenseDialogViewModel>();
            containerRegistry.RegisterDialog<AddExpenseDialog, AddExpenseDialogViewModel>();
            containerRegistry.RegisterDialog<AddEditCategoryToTransaction, AddEditCategoryToTransactionViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<AccountsModule>();
            moduleCatalog.AddModule<ExpensesModule>();
            moduleCatalog.AddModule<SettingsModule>();
        }
    }
}