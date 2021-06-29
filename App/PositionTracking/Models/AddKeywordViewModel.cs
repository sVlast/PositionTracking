using System;
using PositionTracking.Data;


namespace PositionTracking.Models
{
    public class AddKeywordViewModel
    {
        public Guid ProjectId { get; set; }

        public string Value { get; set; }
        public Languages Language { get; set; }
        public Countries Location { get; set; }

    }
}
