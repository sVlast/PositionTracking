using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PositionTracking.Data
{
    public class UserPermission
    {
        public const byte Admin = 255;
        public const byte View = 1;
        public const byte Edit = View | 1 << 1;


        public int UserPermissionId { get; private set; }
        public IdentityUser User { get; private set; }
        public Project Project { get; set; }
        public byte PermissionType { get; set; }
         
        private UserPermission()
        {
        }

        public UserPermission(IdentityUser user, byte permissionType, Project project)
        {
            User = user;
            PermissionType = permissionType;
            Project = project;
        }
    }
}
