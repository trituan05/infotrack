using InfoTrack.Services.Google.Exceptions;
using Microsoft.Extensions.Logging;
using System.Web;

namespace InfoTrack.Services.Google.Services
{
    public interface IGoogleSearchService
    {
        Task<string> GetGoogleSearch(string keyword);
    }

    internal class GoogleSearchService: IGoogleSearchService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;

        public GoogleSearchService(
            ILogger<GoogleSearchService> logger,
            HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<string> GetGoogleSearch(string keyword)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"search?num=100&q={HttpUtility.UrlEncode(keyword)}");

            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError("Error while executing the google search request.");
                throw new GoogleSearchCallFailedException();
            }
        }
    }
}
