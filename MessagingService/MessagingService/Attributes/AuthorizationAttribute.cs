using Microsoft.AspNetCore.Mvc.Filters;
using MessagingService.Exceptions;
using System;
using System.Net;

namespace MessagingService.Attributes
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _mockToken = "sYQbf4ypSZqrszH4544MzHy8BhK2PCbrzLDVfpmvLNebTVYQ8UfFXAsvsv2D9weHzZ24wAKgPbCEy5xV6TXLB4ZrvpGynjbk6PVhGMZrkWVbvrkXgG399xG68YUg2dvh";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
                throw new BusinessException("Not authorized.", HttpStatusCode.Unauthorized);

            string tokenString = authorizationHeader["Bearer ".Length..];

            if (tokenString != _mockToken)
                throw new BusinessException("Not authorized.", HttpStatusCode.Unauthorized);
        }
    }
}
