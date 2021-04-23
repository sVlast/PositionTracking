using System;
namespace PositionTracking.Data
{
    public class KeywordRating
    {
        public int KeywordRatingId {get;private set;}
        public DateTime TimeStamp { get; set; }
        public int Rank { get; set; }
        public string SearchEngine { get; set; }

        public KeywordRating()
        {
        }
    }
}
