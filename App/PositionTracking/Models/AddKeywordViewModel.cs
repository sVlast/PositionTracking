using System;
namespace PositionTracking.Models
{
    public class AddKeywordViewModel
    {
        public Guid ProjectId { get; set; }

        public string Value { get; set; }
        public string Language { get; set; }
        public string Location { get; set; }

    }
}
