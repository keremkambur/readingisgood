using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReadingIsGood.Api.Extensions;
using ReadingIsGood.BusinessLayer.Exceptions;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.Api.Middleware.JwtCheckToRefreshATokenMiddleware
{
    /// <summary>
    /// Used to check if JWT token exists.
    /// Please note that if the JWT is expired, the middleware DOES NOT short circuit the request
    /// </summary>
    public class JwtCheckToRefreshATokenMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public JwtCheckToRefreshATokenMiddleware(RequestDelegate next) => this._next = next;

        // if DI is needed, add them HERE not in the constructor!!!
        /// <summary>
        /// Checks the temporary JWT issued in between OTP generation and verification requests
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="options"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        /// <exception cref="ForbiddenAccessException"></exception>
        public async Task Invoke(
            HttpContext httpContext,
            IOptions<JwtValidationOptions> options,
            IHostEnvironment environment
        )
        {
            var token = httpContext.Request.GetAuthorizationBearerToken();
            new JwtValidator(options.Value).ValidateExpiredToken(token, nameof(Audience.Auth), out var principal);

            httpContext.User = principal;

            await this._next(httpContext).ConfigureAwait(false);
        }
    }
}
