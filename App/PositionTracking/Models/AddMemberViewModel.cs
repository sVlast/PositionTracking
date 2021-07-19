using PositionTracking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionTracking.Models
{
    public class AddMemberViewModel
    {
        public string MemberEmail { get; set; }
        public UserRole UserRole { get; set; }
        public Guid ProjectId { get; set; }
    }
}