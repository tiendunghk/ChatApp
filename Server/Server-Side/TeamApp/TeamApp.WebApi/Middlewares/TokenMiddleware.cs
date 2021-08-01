using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamApp.WebApi.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            context.Request.Headers.TryGetValue("Authorization", out var token);

            var tokenString = token.ToString();

            bool hasAuthorizeAttribute = false;
            if (context.Features.Get<IEndpointFeature>().Endpoint != null)
                hasAuthorizeAttribute = context.Features.Get<IEndpointFeature>().Endpoint.Metadata
                    .Any(m => m is AuthorizeAttribute);

            if (!string.IsNullOrEmpty(tokenString) && string.IsNullOrEmpty(context.Request.Headers["Authorization"]) && hasAuthorizeAttribute)
            {
                context.Request.Headers.Add("Authorization", tokenString);
            }

            if (!hasAuthorizeAttribute)
            {
                context.Request.Headers.Remove("Authorization");
            }

            await _next(context);
        }
    }
}
