using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PositionTracking.Data
{
    public class UserPermission
    {
        public int UserPermissionId { get; private set; }
        public ICollection<IdentityUser> User { get; set; }
        public byte PermissionType { get; set; }

        public UserPermission()
        {
        }
    }
}
