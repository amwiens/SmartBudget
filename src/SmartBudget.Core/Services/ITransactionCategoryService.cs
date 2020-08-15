using SmartBudget.Core.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBudget.Core.Services
{
    public interface ITransactionCategoryService : IDataService<TransactionCategory>
    {
        Task<bool> DeleteByTransactionId(int transactionId);

        Task<IEnumerable<TransactionCategory>> GetByTransactionId(int transactionId);
    }
}