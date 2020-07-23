using System;
using System.IO;

namespace SmartBudget.Core
{
    public class App
    {
        public static DataAccess.SmartBudgetDbContext CreateDatabase()
        {
            // Database
            string dbLocation = Path.Combine(Environment.CurrentDirectory, "smartBudget.db");
            System.Diagnostics.Debug.WriteLine($"Database location: {dbLocation}");
            DataAccess.SmartBudgetDbContext ctx = DataAccess.SmartBudgetDbContext.Create(dbLocation);
            return ctx;
        }
    }
}