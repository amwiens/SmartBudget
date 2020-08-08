using Prism.Services.Dialogs;

using System;

namespace SmartBudget.Core.Dialogs
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
            int accountId, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters
            {
                { "accountid", accountId }
            };

            dialogService.ShowDialog("AddTransactionDialog", p, callBack);
        }

        public static void ShowAddExpenseDialog(this IDialogService dialogService,
            Action<IDialogResult> callBack)
        {
            var p = new DialogParameters();

            dialogService.ShowDialog("AddExpenseDialog", p, callBack);
        }
    }
}