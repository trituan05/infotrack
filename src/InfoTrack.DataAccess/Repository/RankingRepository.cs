using InfoTrack.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace InfoTrack.DataAccess.Repository
{
    public interface IRankingRepository 
    {
        Task StoreRankData(string url, string keywords, int position);
        Task<List<Models.WebApp.Models.Ranking>> GetReport(DateTime from, DateTime to);
    }

    internal class RankingRepository : IRankingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RankingRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task StoreRankData(string url, string keywords, int position)
        {
            _dbContext.Rankings.Add(new Entities.Ranking
            {
                Url = url,
                Keywords = keywords,
                Rank = position,
                RankDate = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Models.WebApp.Models.Ranking>> GetReport(DateTime from, DateTime to)
        {
            var query = await _dbContext.Rankings
                    .Where(q => from.Date <= q.RankDate && q.RankDate <= to.AddDays(1).Date)
                    .Select(s => new Models.WebApp.Models.Ranking
                    {
                        Url = s.Url,
                        Keywords = s.Keywords,
                        Rank = s.Rank
                    })
                    .ToListAsync();

            return query;
        }
    }
}
