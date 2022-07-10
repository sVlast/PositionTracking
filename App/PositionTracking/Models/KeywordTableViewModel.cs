using System;
using System.Collections.Generic;
using PositionTracking.Data;

namespace PositionTracking.Models
{
    public class KeywordTableViewModel : ProjectModel
    {
        public ICollection<Keyword> keywords;
        public KeywordTableViewModel(string projectName, Guid projectId)
            : base(projectName, projectId)
        { }

    public class Keyword
        {
            public string Value { get; set; }
            public string Id { get; set; }
            public string Language { get; set; }
            public string Location { get; set; }
            public ICollection<KeywordRating> Ratings { get; set; }
        }

    public class KeywordRating
        {
            public Guid KeywordRatingId { get; set; }
            public string TimeStamp { get; set; }
            public int Rank { get; set; }
            public SearchEngineType SearchEngine { get; set; }
        }
    }
}