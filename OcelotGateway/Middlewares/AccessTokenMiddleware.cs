using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Middlewares
{
    public class AccessTokenMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string token = "";
            context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjgxYzMwNmExLTJhNWMtNGM5NC1mMzhhLTA4ZDkyY2RmNDhkMiIsInJvbGUiOiJSZWd1bGFyIHVzZXIiLCJuYmYiOjE2MjM4ODgwODYsImV4cCI6MTYyMzg5NTI4NiwiaWF0IjoxNjIzODg4MDg2fQ.P2yw3JSMx-3WYOXZzoj1ABeuQtyWYhD5fcbmli0XgvI";
            //return;
            await next(context);
        }
    }
}
