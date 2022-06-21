using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PositionTracking
{
    internal class CustomPasswordHasher : IPasswordHasher<IdentityUser>
    {
        public string HashPassword(IdentityUser user, string password)
        {
            using (SHA256 mysHA256 = SHA256.Create())
            {
                byte[] hashValue = mysHA256.ComputeHash(Encoding.UTF8.GetBytes(password + user.Email));
                
                return Convert.ToBase64String(hashValue);
            }
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            if(hashedPassword == HashPassword(user,providedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}