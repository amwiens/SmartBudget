using SmartBudget.Core.DataAccess;

namespace SmartBudget.Core.Services
{
    public class SmartBudgetService : ISmartBudgetService
    {
        public SmartBudgetService()
        {
            this.context = App.CreateDatabase();
        }

        private SmartBudgetDbContext context;
        public static string ActionAdd => "add";
        public static string ActionUpdate => "update";
        public static string ActionDelete => "delete";
    }
}