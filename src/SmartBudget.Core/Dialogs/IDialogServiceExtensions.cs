using Prism.Services.Dialogs;

using System;

namespace SmartBudget.Core.Dialogs
{
    public static class IDialogServiceExtensions
    {
        public static void ShowConfirmDialog(this IDialogService dialogService,
            string message, Action<IDialogResult> callBack)
        {
            var p = new DialogParameters();
            p.Add("message", message);

            dialogService.ShowDialog("ConfirmDialog", p, callBack);
        }
    }
}