using System;
using PositionTracking.Data;

namespace PositionTracking.Models
{
    public class AddProjectViewModel
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public AddProjectViewModel(string name, Guid id)
        {
            ProjectName = name;
            ProjectId = id;
        }
    }
}
