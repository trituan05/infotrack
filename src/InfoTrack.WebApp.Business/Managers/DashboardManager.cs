using InfoTrack.DataAccess.UnitOfWork;
using InfoTrack.Services.Google.Services;

namespace InfoTrack.WebApp.Business.Managers
{
    public interface IDashboardManager
    {
        Task<List<int>> SearchByKeyword(string keyword, string url);
        Task<List<Models.WebApp.Models.Ranking>> GetDailyReport();
        Task<List<Models.WebApp.Models.Ranking>> GetWeeklyReport();
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
            List<int> data = await _googleSearchService.GetGoogleSearch(keyword, url);
            await StoreRanking(keyword, url, data.First());
            return data;
        }

        public async Task<List<Models.WebApp.Models.Ranking>> GetDailyReport()
        {
            var result = await _unitOfWork.Rankings.GetReport(DateTime.UtcNow, DateTime.UtcNow);
            return result;
        }

        public async Task<List<Models.WebApp.Models.Ranking>> GetWeeklyReport()
        {
            var result = await _unitOfWork.Rankings.GetReport(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
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
    }
}
