using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Z.EntityFramework.Plus;

namespace SmartBudget.EntityFramework.Services
{
    public class AccountDataService : IAccountService
    {
        private readonly SmartBudgetDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Account> _nonQueryDataService;

        public AccountDataService(SmartBudgetDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Account>(contextFactory);
        }

        public async Task<Account> Create(Account entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Account> Get(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                Account entity = await context.Accounts
                    .FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
        }

        public async Task<Account> GetWithTransactions(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                Account entity = await context.Accounts
                    .Include(a => a.Transactions)
                    .Include(a => a.TargetTransactions)
                    .FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Account> entities = await context.Accounts
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Account>> GetAllWithTransactions()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Account> entities = await context.Accounts
                    .Include(a => a.Transactions)
                    .Include(a => a.TargetTransactions)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<Account> Update(int id, Account entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }

        public async Task<IEnumerable<Account>> GetAccountDataBeforeDateByType(DateTime date, AccountType accountType)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Account> entities = await context.Accounts
                    .Where(a => a.AccountType == accountType)
                    .IncludeFilter(a => a.Transactions.Where(t => t.Date <= date))
                    .IncludeFilter(a => a.TargetTransactions.Where(t => t.Date <= date))
                    .ToListAsync();
                return entities;
            }
        }
    }
}