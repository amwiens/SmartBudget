﻿using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

using SmartBudget.Accounts;
using SmartBudget.Core.Dialogs;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services;
using SmartBudget.Main;
using SmartBudget.Views;

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

            // Dialogs
            containerRegistry.RegisterDialog<ConfirmDialog, ConfirmDialogViewModel>();
            containerRegistry.RegisterDialog<TransactionDialog, TransactionDialogViewModel>();
            containerRegistry.RegisterDialog<AddTransactionDialog, AddTransactionDialogViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<AccountsModule>();
        }
    }
}