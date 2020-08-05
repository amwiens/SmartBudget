using SmartBudget.Core.Models;
using SmartBudget.Core.Services;
using SmartBudget.YelpAPI.Results;

using System.Threading.Tasks;

namespace SmartBudget.YelpAPI.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly YelpHttpClientFactory _httpClientFactory;

        public BusinessService(YelpHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Payee> BusinessesSearch(string term, string location = null, decimal latitude = 0M, decimal longitude = 0M)
        {
            using (YelpHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = $"businesses/search?term={term}&location={location}";

                BusinessesSearch businesses = await client.GetAsync<BusinessesSearch>(uri);

                return new Payee();
            }
        }
    }
}