﻿using Prism.Services.Dialogs;

using System;

namespace SmartBudget.Core.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowConfirmDialog(this IDialogService dialogService,
            string message, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters
            {
                { "message", message }
            };

            dialogService.ShowDialog("ConfirmDialog", p, callBack);
        }

        public static void ShowTransactionDialog(this IDialogService dialogService,
            int transactionId, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters
            {
                { "transactionid", transactionId }
            };

            dialogService.ShowDialog("TransactionDialog", p, callBack);
        }

        public static void ShowAddTransactionDialog(this IDialogService dialogService,
            int accountId, int transactionId, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters
            {
                { "accountid", accountId },
                { "transactionid", transactionId }
            };

            dialogService.ShowDialog("AddTransactionDialog", p, callBack);
        }

        public static void ShowAddEditCategoryToTransactionDialog(this IDialogService dialogService,
            int categoryId, decimal amount, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters
            {
                { "categoryid", categoryId },
                { "amount", amount }
            };

            dialogService.ShowDialog("AddEditCategoryToTransaction", p, callBack);
        }

        public static void ShowExpenseDialog(this IDialogService dialogService,
            int expenseId, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters
            {
                { "expenseid", expenseId }
            };

            dialogService.ShowDialog("ExpenseDialog", p, callBack);
        }

        public static void ShowAddExpenseDialog(this IDialogService dialogService,
            Action<IDialogResult> callBack)
        {
            var p = new DialogParameters();

            dialogService.ShowDialog("AddExpenseDialog", p, callBack);
        }
    }
}