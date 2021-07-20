using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PositionTracking.WebApi.Controllers
{
    public class AllowCrossSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.HttpContext?.Response != null)
                actionExecutedContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuted(actionExecutedContext);
        }
    }

}
