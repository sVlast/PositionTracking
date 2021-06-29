using System;
namespace PositionTracking.Data
{
    public class KeywordRating
    {
        public Guid KeywordRatingId {get;private set;}
        public DateTime TimeStamp { get; set; }
        public int Rank { get; set; }
        public SearchEngineType SearchEngine { get; set; }

        public KeywordRating()
        {
        }
        public KeywordRating(int rank,SearchEngineType searchEngine)
        {
            Rank = rank;
            TimeStamp = DateTime.UtcNow;
            SearchEngine = searchEngine;
        }
    }
}
