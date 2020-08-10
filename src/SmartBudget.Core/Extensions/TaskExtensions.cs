using System;
using System.Threading.Tasks;

namespace SmartBudget.Core.Extensions
{
    public static class TaskExtensions
    {
        public async static void Await(this Task task, Action completedCallback, Action<Exception> errorCallBack)
        {
            try
            {
                await task;
                completedCallback?.Invoke();
            }
            catch (Exception ex)
            {
                errorCallBack?.Invoke(ex);
            }
        }
    }
}