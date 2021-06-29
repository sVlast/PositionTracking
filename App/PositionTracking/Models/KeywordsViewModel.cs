using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PositionTracking.Models
{
    public class KeywordsViewModel : ProjectModel
    {
        public ICollection<Keyword> Keywords { get; set; }

        public KeywordsViewModel(string projectName, Guid projectId)
            :base(projectName, projectId)
        { }

        public class Keyword
        {
            public string Value { get; set; }
            public string LanguageLocation { get; set; }
            public int Rating { get; set; }
            public string Id { get; set; }
        }
       
    }


}
