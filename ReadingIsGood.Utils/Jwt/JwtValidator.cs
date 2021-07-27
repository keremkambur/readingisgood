using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ReadingIsGood.Utils.Jwt
{
    public class JwtValidator
    {
        public JwtValidationOptions Options { get; set; }

        public JwtValidator(JwtValidationOptions options)
        {
            this.Options = options;
            this.CheckOptions();
        }

        private void CheckOptions()
        {
            if (this.Options == null)
            {
                throw new ArgumentNullException(nameof(this.Options));
            }

            if (this.Options.Issuer == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.Issuer));
            }

            if (this.Options.ValidationKey == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.ValidationKey));
            }
        }

        public JwtSecurityToken ValidateToken(string token, string audience, out ClaimsPrincipal claimsPrincipal)
            => this.ValidateTokenInternal(token, audience, true, out claimsPrincipal);

        public JwtSecurityToken ValidateExpiredToken(string token, string audience, out ClaimsPrincipal claimsPrincipal)
            => this.ValidateTokenInternal(token, audience, false, out claimsPrincipal);

        private JwtSecurityToken ValidateTokenInternal(string token, string audience, bool validateLifetime, out ClaimsPrincipal claimsPrincipal)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            if (!jwtTokenHandler.CanReadToken(token) || !jwtTokenHandler.CanValidateToken)
            {
                throw new UnauthorizedAccessException("Cannot read JWT token.");
            }

            SecurityToken securityToken;

            try
            {
                claimsPrincipal = jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = this.Options.Issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = this.Options.ValidationKey,

                    ValidateLifetime = validateLifetime,
                    ClockSkew = TimeSpan.FromMinutes(0)
                }, out securityToken);
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException("JWT is invalid.", ex);
            }

            if (claimsPrincipal == null || securityToken == null)
            {
                throw new UnauthorizedAccessException("JWT is invalid.");
            }

            return jwtTokenHandler.ReadJwtToken(token);
        }
    }
}