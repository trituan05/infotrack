using InfoTrack.DataAccess.UnitOfWork;
using InfoTrack.Services.Google.Services;
using System.Text.RegularExpressions;

namespace InfoTrack.WebApp.Business.Managers
{
    public interface IDashboardManager
    {
        Task<List<int>> SearchByKeyword(string keyword, string url);
    }

    internal class DashboardManager : IDashboardManager
    {
        private readonly IGoogleSearchService _googleSearchService;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardManager(IGoogleSearchService googleSearchService, IUnitOfWork unitOfWork)
        {
            _googleSearchService = googleSearchService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<int>> SearchByKeyword(string keyword, string url)
        {
            List<int> result = new() { 0 };
            string data = await _googleSearchService.GetGoogleSearch(keyword);
            Dictionary<string, int> searchResult = GetGooglePosition(data);
            if(searchResult.Any())
            {
                result = searchResult
                    .Where(q => q.Key.Contains(url))
                    .Select(s => s.Value)
                    .ToList();

                await StoreRanking(keyword, url, result.First());
            }
            return result;
        }

        private async Task StoreRanking(string keyword, string url, int position)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Rankings.StoreRankData(keyword, url, position);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
            finally
            {
                await _unitOfWork.DisposeTransactionAsync();
            }
        }

        private static Dictionary<string, int> GetGooglePosition(string data)
        {
            Dictionary<string, int> result = new();
            string hrefPattern = @"<div[^>]*>\s*<a\s+href=""([^""]*)""";
            Match regexMatch = Regex.Match(data, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            int index = 1;
            while (regexMatch.Success)
            {
                Group? collection = regexMatch.Groups[1];
                if (!collection.Value.Contains("google.co.uk") && collection.Value.Contains("http"))
                {
                    result.Add(collection.Value.Replace("/url?q=", ""), index);
                    index++;
                }
                regexMatch = regexMatch.NextMatch();
            }

            return result;
        }
    }
}
