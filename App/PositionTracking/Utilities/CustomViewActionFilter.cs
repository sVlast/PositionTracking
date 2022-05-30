using Microsoft.AspNetCore.Mvc.Filters;

namespace PositionTracking.Utilities
{
    public class MyCustomViewActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            base.OnActionExecuting(context);
        }
    }
}
