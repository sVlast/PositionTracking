using System;
namespace PositionTracking.Models
{
    public abstract class ProjectModel
    {
        public string ProjectName { get; }



        public ProjectModel(string name)
        {
            ProjectName = name;
        }
    }

 
}
