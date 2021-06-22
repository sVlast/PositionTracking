using Microsoft.AspNetCore.Identity;
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
        public IEnumerable<UserPermission> UserPermissions { get; private set; }



        private Project()
        { }

        public Project(IdentityUser user, byte permissionType)
        {
            UserPermissions = new List<UserPermission>();
            AddUserPermission(user, permissionType);
        }

        public void AddUserPermission(IdentityUser user, byte permissionType)
        {
            ((ICollection<UserPermission>)UserPermissions).Add(new UserPermission(user, permissionType, this));
        }
    }
}
