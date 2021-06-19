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
        private readonly IAuthenticationService _authService;
        public AccessTokenMiddleware(IAuthenticationService authService)
        {
            _authService = authService;
        }

        //UpstreamPath /cities
        private static readonly string[] protectedPaths = new string[] {
            "/cities",
            "/roles",
            "/personalUsers",
            "/corporationUsers"
        };


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value.ToString();
            if (protectedPaths.Any(s => path.Contains(s)))
            {
                string publicToken = context.Request.Headers["Authorization"];
                if (!String.IsNullOrEmpty(publicToken))
                {
                    AuthenticationResponse res = _authService.getAccessToken(new Guid(publicToken)).Result;
                    if (res.Succes)
                    {
                        string token = res.Token;
                        context.Request.Headers["Authorization"] = "Bearer " + token;
                    }
                }
               
            }

            await next(context);
        }
    }
}
