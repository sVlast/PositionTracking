using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PositionTracking.Models
{
    public class ProjectsViewModel
    {
        public ICollection<Project> Projects { get; set; }

        public LanguageDictionary Dictionary { get; set; }

        public class Project
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int NumerOfKeywords { get; set; }
            public string Role { get; set; }
           // public string Change { get; set; }

        }


    }
}