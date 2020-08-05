using SmartBudget.Core.Models;

using System.Threading.Tasks;

namespace SmartBudget.Core.Services
{
    public interface IBusinessService
    {
        public Task<Payee> BusinessesSearch(string term, string location = null, decimal latitude = 0M, decimal longitude = 0M);
    }
}