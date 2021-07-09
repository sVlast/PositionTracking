using System;
namespace PositionTracking.Models
{
    public abstract class ProjectModel
    {
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }



        public ProjectModel(string name, Guid id)
        {
            ProjectName = name;
            ProjectId = id;
        }

        protected ProjectModel()
        { }
    }

 
}