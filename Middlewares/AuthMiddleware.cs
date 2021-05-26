using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Zembil.Middlewares
{
    class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authToken = context.Request.Query["x-Bearer-Token"];
            if (!string.IsNullOrWhiteSpace(authToken))
            {
                if (authToken == "2kejn2o3ind3")
                {
                    await _next(context);
                }

            }
            else
            {
                context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("invalid calling tenant");
                return;
            }

        }
    }
}