using System;
using Microsoft.AspNetCore.Identity;

namespace PositionTracking.Data
{
    public class User : IdentityUser
    {
        public int UserId { get; private set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }



        public User()
        {
        }
    }
}
