using System;
using System.Collections.Generic;
namespace PositionTracking.Data
{
    public class UserPermission
    {
        public int UserPermissionId { get; private set; }
        public IList<User> User { get; set; }
        public byte PermissionType { get; set; }

        public UserPermission()
        {
        }
    }
}
