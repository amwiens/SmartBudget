namespace SmartBudget.YelpAPI
{
    public class YelpHttpClientFactory
    {
        private readonly string _apiKey;

        public YelpHttpClientFactory(string apiKey)
        {
            _apiKey = apiKey;
        }

        public YelpHttpClient CreateHttpClient()
        {
            return new YelpHttpClient(_apiKey);
        }
    }
}