using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PositionTracking.Models
{
    public class MembersViewModel : ProjectModel
    {
        public ICollection<Member> Members { get; set; }

        public MembersViewModel(string projectName, Guid projectId)
            : base(projectName, projectId)
        { }

        public class Member
        {
            public string MemberName { get; set; }
            public string Email { get; set; }
            public string PermissionType { get; set; }
        }

    }


}

