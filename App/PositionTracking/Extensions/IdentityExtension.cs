using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionTracking.Extensions
{
    public static class IdentityExtensions
    {
        public static void AddIdentityError(this ModelStateDictionary modelState, string key, IdentityResult result)
        {
            modelState.TryAddModelError(key, result.Errors.Select(e => e.Description).Aggregate((x, y) => x + "; " + y));
        }
    }

}
