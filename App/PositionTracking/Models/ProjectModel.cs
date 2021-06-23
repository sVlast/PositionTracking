using System;
namespace PositionTracking.Models
{
    public abstract class ProjectModel
    {
        public string ProjectName { get; }
        public Guid ProjectId { get;}



        public ProjectModel(string name, Guid id)
        {
            ProjectName = name;
            ProjectId = id;


        }
    }

 
}
