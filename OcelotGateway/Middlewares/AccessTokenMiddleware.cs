using Microsoft.AspNetCore.Http;
using OcelotGateway.Models;
using OcelotGateway.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Middlewares
{
    public class AccessTokenMiddleware : IMiddleware
    {
        private readonly IAuthService _authService;
        public AccessTokenMiddleware(IAuthService authService)
        {
            _authService = authService;
        }

        //UpstreamPath /cities
        private static ArrayList protectedPaths = new ArrayList {
            "/cities"
        };


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value.ToString();
            if (protectedPaths.Contains(path))
            {
                AuthenticationResponse res = _authService.getAccessToken("").Result;
                string token = res.Token;
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }

            //return;
            await next(context);
        }
    }
}
