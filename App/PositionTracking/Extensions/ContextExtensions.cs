using Microsoft.AspNetCore.Http;
using PositionTracking.Data;
using System;

namespace PositionTracking.Extensions
{
    public static class ContextExtensions
    {
        public static Languages GetLanguage(this HttpContext context)
        {
            return context.Request.Cookies.TryGetValue("Lang", out string lang) && Enum.TryParse<Languages>(lang,out Languages result) ? result : Languages.en;
        }
    }
}
