using System.IO;

namespace SmartBudget.Core
{
    public class App
    {
        public static DataAccess.SmartBudgetContext CreateDatabase()
        {
            // Database
            string dbLocation = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "smartBudget.db");
            System.Diagnostics.Debug.WriteLine($"Database location: {dbLocation}");
            DataAccess.SmartBudgetContext ctx = DataAccess.SmartBudgetContext.Create(dbLocation);
            return ctx;
        }
    }
}