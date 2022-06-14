using System;
using System.Collections.Generic;

namespace PositionTracking.Models
{
    public class UserProjectsViewModel
    {
        public ICollection<UserPermission> UserPermissions { get; set; }

        public class UserPermission
        {
            public int Id { get; set; }
            public string User { get; set; }
            public string Project { get; set; }
            public string Permission { get; set; }
        }
    }
}
