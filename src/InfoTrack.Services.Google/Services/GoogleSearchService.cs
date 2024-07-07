using InfoTrack.Services.Google.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Web;

namespace InfoTrack.Services.Google.Services
{
    public interface IGoogleSearchService
    {
        Task<List<int>> GetGoogleSearch(string keyword, string url);
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

        public async Task<List<int>> GetGoogleSearch(string keyword, string url)
        {
            HttpRequestMessage request = new(HttpMethod.Get, $"search?num=100&q={HttpUtility.UrlEncode(keyword)}");

            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string html = await response.Content.ReadAsStringAsync();
                return GetGooglePosition(html, url);
            }
            else
            {
                _logger.LogError("Error while executing the google search request.");
                throw new GoogleSearchCallFailedException();
            }
        }

        private static List<int> GetGooglePosition(string data, string prefixUrl)
        {
            List<int> result = new();
            string hrefPattern = @"<div[^>]*>\s*<a\s+href=""([^""]*)""";
            Match regexMatch = Regex.Match(data, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            int postion = 1;
            Dictionary<string, int> rankDics = new();
            while (regexMatch.Success)
            {
                Group? collection = regexMatch.Groups[1];
                if(collection.Value.Contains("http") && !collection.Value.Contains("google.co.uk"))
                {
                    rankDics.Add(collection.Value, postion++);
                }

                regexMatch = regexMatch.NextMatch();
            }

            List<int> ranks = rankDics
                .Where(q => q.Key.Contains(prefixUrl))
                .Select(s => s.Value)
                .ToList();

            if(!ranks.Any())
            {
                return new List<int> { 0 };
            }
            return ranks;
        }
    }
}
