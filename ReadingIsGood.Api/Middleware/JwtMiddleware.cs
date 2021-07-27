using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IOptions<JwtIssuerOptions> issuerOptions, IAuthenticationService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, issuerOptions.Value, authService, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, JwtIssuerOptions issuerOptions, IAuthenticationService authService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidIssuer = issuerOptions.Issuer,
                    IssuerSigningKey = issuerOptions.SigningCredentials.Key,
                    ValidAudience = nameof(Audience.Auth),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // attach user to context on successful jwt validation
                context.Items["User"] = authService.GetById(Guid.Parse(userId));
            }
            catch
            {

            }
        }
    }
}
