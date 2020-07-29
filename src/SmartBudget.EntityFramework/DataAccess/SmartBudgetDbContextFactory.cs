using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System.IO;

namespace SmartBudget.EntityFramework.DataAccess
{
    public class SmartBudgetDbContextFactory : IDesignTimeDbContextFactory<SmartBudgetDbContext>
    {
        public SmartBudgetDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<SmartBudgetDbContext>();
            options.UseSqlite(Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "smartBudget.db"));

            return new SmartBudgetDbContext(options.Options);
        }
    }
}