using System;
namespace PositionTracking.Models
{
    public class ProjectSettingsViewModel: ProjectModel
    {
        public string Domain { get; set; }
        public string ProjectImage { get; set; }


        public ProjectSettingsViewModel(string projectName, Guid projectId)
            : base(projectName, projectId)
        { }

        public ProjectSettingsViewModel(string projectName, Guid projectId,string ProjectImage)
            : base(projectName, projectId)
        {
            this.ProjectImage = ProjectImage;
        }


        public ProjectSettingsViewModel()
        { }
    }
}
