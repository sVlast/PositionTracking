using PositionTracking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionTracking.Models
{
    public class AddMemberViewModel
    {
        public string MemberName { get; set; }
        public UserRole userRole { get; set; }

    }
}
