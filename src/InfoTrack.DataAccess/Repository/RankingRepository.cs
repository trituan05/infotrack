using AutoMapper;
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
        private readonly IMapper _mapper;

        public RankingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
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
