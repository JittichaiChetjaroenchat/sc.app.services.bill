using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Client.Security;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var securityManager = context.HttpContext.RequestServices.GetService<ISecurityManager>();
                var accessToken = context.HttpContext.Request.GetAccessToken();
                var validateTokenResponse = securityManager.ValidateTokenAsync(context.HttpContext.Request, accessToken).RunSync();
                if (!SecurityClientHelper.IsSuccess(validateTokenResponse))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }

            base.OnActionExecuting(context);
        }
    }
}