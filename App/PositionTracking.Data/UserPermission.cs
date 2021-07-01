using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PositionTracking.Data
{
    public class UserPermission
    {



        public int UserPermissionId { get; private set; }
        public IdentityUser User { get; private set; }
        public Project Project { get; set; }
        public UserRole PermissionType { get; set; }
         
        private UserPermission()
        {
        }

        public UserPermission(IdentityUser user, UserRole permissionType, Project project)
        {
            User = user;
            PermissionType = permissionType;
            Project = project;
        }
    }
}
