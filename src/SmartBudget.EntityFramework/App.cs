using System;
using System.IO;

namespace SmartBudget.EntityFramework
{
    public class App
    {
        public static SmartBudgetDbContext CreateDatabase()
        {
            // Database
            string dbLocation = Path.Combine(Environment.CurrentDirectory, "smartBudget.db");
            System.Diagnostics.Debug.WriteLine($"Database location: {dbLocation}");
            SmartBudgetDbContext ctx = SmartBudgetDbContext.Create(dbLocation);
            return ctx;
        }
    }
}