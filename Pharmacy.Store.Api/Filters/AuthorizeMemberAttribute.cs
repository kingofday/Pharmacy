using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Pharmacy.API
{
    /// <summary>
    /// کلاسی برای فیلتر کردن درخواست ها بر اساس سطح دسترسی
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthAttribute : ActionFilterAttribute, IAsyncActionFilter
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            bool skipAuthorization = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            if (!skipAuthorization)
            {
                //var values = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "token").Value;
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new { IsSuccessFull = false, Status = 401, Message = "UnAuthorized Access" });
                }
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}