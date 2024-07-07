namespace InfoTrack.DataAccess.Entities
{
    public class Ranking: BaseEntity
    {
        public string? Url { get; set; }

        public string? Keywords { get; set; }

        public int Rank { get; set; }

        public DateTime RankDate { get; set; }
    }
}
