using InfoTrack.DataAccess.DbContexts;

namespace InfoTrack.DataAccess.Repository
{
    public interface IRankingRepository 
    {
        Task StoreRankData(string url, string keywords, int position);
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
    }
}
