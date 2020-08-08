using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System;
using System.IO;

namespace SmartBudget.EntityFramework
{
    public class SmartBudgetDbContextFactory : IDesignTimeDbContextFactory<SmartBudgetDbContext>
    {
        public SmartBudgetDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<SmartBudgetDbContext>();
            var dbPath = Path.Combine(Environment.CurrentDirectory, "smartBudget.db");
            options.UseSqlite(dbPath);
            _ = SmartBudgetDbContext.Create(dbPath);

            return new SmartBudgetDbContext(options.Options, dbPath);
        }
    }
}