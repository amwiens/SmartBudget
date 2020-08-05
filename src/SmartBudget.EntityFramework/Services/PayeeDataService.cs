using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services.Common;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.EntityFramework.Services
{
    public class PayeeDataService : IPayeeService
    {
        private readonly SmartBudgetDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Payee> _nonQueryDataService;

        public PayeeDataService(SmartBudgetDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Payee>(contextFactory);
        }

        public async Task<Payee> Create(Payee entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Payee> Get(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                Payee entity = await context.Payees
                    .FirstOrDefaultAsync(p => p.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Payee>> GetAll()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Payee> entities = await context.Payees
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<Payee> Update(int id, Payee entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}