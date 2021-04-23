using System;
using System.Collections.Generic;

namespace PositionTracking.Data
{
    public class KeywordEntry
    {
        public int KeywordEntryId { get;private set; }
        public string Language { get; set; }
        public string Location { get; set; }
        public IList<KeywordRating> Ratings { get; set; }

        public KeywordEntry()
        {
        }
    }
}
