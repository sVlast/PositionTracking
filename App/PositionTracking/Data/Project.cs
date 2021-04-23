using System;
using System.Collections.Generic;

namespace PositionTracking.Data
{
    public class Project
    {
        public Guid ProjectId { get; private set; }
        public string Name { get; set; }
        public IList<Uri> Paths { get; set; }
        public IList<Keyword> Keywords { get; set; }
        public IList<UserPermission> UserPermissions { get; set; }


        


        public Project()
        {
           // ProjectId = Guid.NewGuid() ;     
        }
    }
}
