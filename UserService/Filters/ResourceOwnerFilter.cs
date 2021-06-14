using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Extensions;

namespace UserService.Filters
{
    public class ResourceOwnerFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                Guid id = Guid.Empty;
                if (context.ActionArguments.ContainsKey("userId"))
                {
                    //TODO: HttpContext extension is not working
                    id = (Guid)context.ActionArguments["userId"];
                    string userId = context.HttpContext.User.Claims.Single(x => x.Type == "id").Value;
                    if (!userId.Equals(id.ToString())){
                        context.Result = new BadRequestObjectResult("You do not own the resource, action is restricted to the owner of the resource");
                        return;
                    }
                }
                else
                {
                    context.Result = new BadRequestObjectResult("Bad id parameter");
                    return;
                }
            }
            catch (Exception e)
            {
                context.Result = new BadRequestObjectResult(e.Message);
                return;
            }

            await next();
        }
    }
}
