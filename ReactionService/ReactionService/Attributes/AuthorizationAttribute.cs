using Microsoft.AspNetCore.Mvc.Filters;
using ReactionService.Exceptions;
using System;

namespace ReactionService.Attributes
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _mockToken = "DKbU5sk7djks2X9F4mSY4H5dRLrQ5R32";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
                throw new BusinessException("Not authorized", 401);

            string tokenString = authorizationHeader.Substring("Bearer ".Length);

            if (tokenString != _mockToken)
                throw new BusinessException("Not authorized", 401);
        }
    }
}
