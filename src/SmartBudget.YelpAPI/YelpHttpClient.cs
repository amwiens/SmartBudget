using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartBudget.YelpAPI
{
    public class YelpHttpClient : HttpClient
    {
        private readonly string _apiKey;

        public YelpHttpClient(string apiKey)
        {
            this.BaseAddress = new Uri("https://api.yelp.com/v3/");
            _apiKey = apiKey;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Authorization: Bearer {_apiKey}");
            HttpResponseMessage response = await GetAsync($"{uri}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}