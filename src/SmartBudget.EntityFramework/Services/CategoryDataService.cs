using Microsoft.EntityFrameworkCore;

using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.EntityFramework.Services.Common;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.EntityFramework.Services
{
    public class CategoryDataService : ICategoryService
    {
        private readonly SmartBudgetDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Category> _nonQueryDataService;

        public CategoryDataService(SmartBudgetDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Category>(contextFactory);
        }

        public async Task<Category> Create(Category entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Category> Get(int id)
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                Category entity = await context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            using (SmartBudgetDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Category> entities = await context.Categories
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<Category> Update(int id, Category entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}