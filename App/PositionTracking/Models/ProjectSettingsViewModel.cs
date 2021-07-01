using System;
namespace PositionTracking.Models
{
    public class ProjectSettingsViewModel: ProjectModel
    {
        public string Domain { get; set; }


        public ProjectSettingsViewModel(string projectName, Guid projectId)
            : base(projectName, projectId)
        { }


        public ProjectSettingsViewModel()
        { }
    }
}
