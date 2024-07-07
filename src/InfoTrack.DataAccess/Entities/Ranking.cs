namespace InfoTrack.DataAccess.Entities
{
    public class Ranking: BaseEntity
    {
        public string Url { get; set; } = default!;

        public string Keywords { get; set; } = default!;

        public int Rank { get; set; }

        public DateTime RankDate { get; set; }
    }
}
