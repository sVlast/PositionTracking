using System;
namespace PositionTracking.Models
{
    public class KeywordDetailViewModel:ProjectModel
    {

        public string Title { get; set; }
        public Guid KeywordId { get; set; }

        public KeywordDetailViewModel(string projectName, Guid projectId)
           : base(projectName, projectId)
        { }

    }
}
