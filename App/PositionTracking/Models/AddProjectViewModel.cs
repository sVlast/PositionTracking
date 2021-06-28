using System;
using PositionTracking.Data;

namespace PositionTracking.Models
{
    public class AddProjectViewModel: ProjectModel
    {

        public AddProjectViewModel(string projectName, Guid projectId)
            : base(projectName, projectId)
        { }
    }
}
