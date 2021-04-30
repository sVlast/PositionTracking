using System;
using System.Collections.Generic;

namespace PositionTracking.Data
{
    public class Project
    {
        public Guid ProjectId { get; private set; }
        public string Name { get; set; }
        public string Paths { get; set; } //odvajati patheve sa Space
        public ICollection<Keyword> Keywords { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }


        


        public Project()
        {
           // ProjectId = Guid.NewGuid() ;     
        }
    }
}
