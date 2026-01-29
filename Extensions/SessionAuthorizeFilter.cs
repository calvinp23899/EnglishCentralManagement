using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnglishCentralManagement.Extensions
{
    public class SessionAuthorizeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.Session.GetString("USER");

            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult(
                    "Index",
                    "Login",
                    new { area = "Admin" }
                );
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
