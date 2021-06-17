using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Middlewares
{
    public class AccessTokenOcelotMiddleware : OcelotPipelineConfiguration
    {
        public AccessTokenOcelotMiddleware()
        {
            PreAuthenticationMiddleware = async (ctx, next) =>
            {
                await ProcessRequest(ctx, next);
            };
        }

        public async Task ProcessRequest(HttpContext context, System.Func<Task> next)
        {
            string token = "";
            context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjgxYzMwNmExLTJhNWMtNGM5NC1mMzhhLTA4ZDkyY2RmNDhkMiIsInJvbGUiOiJSZWd1bGFyIHVzZXIiLCJuYmYiOjE2MjM3NjI0NDksImV4cCI6MTYyMzc2OTY0OSwiaWF0IjoxNjIzNzYyNDQ5fQ.Isgc8rMlXpoyE9_Zg4ywiQKunslB4BE8BmCwxSpZeFw";
            //return;
            await next.Invoke();
        }
    }
}
