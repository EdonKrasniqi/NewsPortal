using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using Core.Interfaces;
using Microsoft.AspNetCore.Builder;

namespace Portal.Middlewares
{
    public class AppAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AppAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IAuthorizationInterface authorizationManager)
        {
            if (httpContext?.Request?.Method == HttpMethods.Options)
            {
                await _next.Invoke(httpContext);
                return;
            }

            var user = httpContext?.User;
            if (user == null)
            {
                await _next.Invoke(httpContext!);
                return;
            }

            await authorizationManager.InitializeAsync(CancellationToken.None);
            await _next.Invoke(httpContext!);
        }
    }

    public static class UserResourcesMiddlewareExtension
    {
        public static IApplicationBuilder UseAppAuthentication(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<AppAuthenticationMiddleware>();

            return builder;
        }
    }
}

