using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadingIsGood.Utils.Jwt
{
    public class JwtGenerator
    {
        public JwtGenerator(JwtIssuerOptions options)
        {
            Options = options;
            CheckOptions();
        }

        public JwtIssuerOptions Options { get; }

        /// <summary>
        ///     "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        /// <remarks>
        ///     The "jti" (JWT ID) claim provides a unique identifier for the JWT.
        ///     The identifier value MUST be assigned in a manner that ensures that
        ///     there is a negligible probability that the same value will be
        ///     accidentally assigned to a different data object; if the application
        ///     uses multiple issuers, collisions MUST be prevented among values
        ///     produced by different issuers as well.  The "jti" claim can be used
        ///     to prevent the JWT from being replayed.  The "jti" value is a case-
        ///     sensitive string.  Use of this claim is OPTIONAL.
        /// </remarks>
        public static Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

        private void CheckOptions()
        {
            if (Options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
        }

        public JwtSecurityToken GetSecurityToken(IEnumerable<Claim> claims, string validForKind)
        {
            return new(
                Options.Issuer,
                claims: claims,
                notBefore: Options.NotBefore,
                expires: Options.Expiration(validForKind),
                signingCredentials: Options.SigningCredentials
            );
        }

        public string GetAccessToken(Claim[] claims, string validForKind)
        {
            return new JwtSecurityTokenHandler().WriteToken(GetSecurityToken(claims, validForKind));
        }

        public JwtResponse GetResponse(Claim[] claims, string validForKind, string refreshToken = null)
        {
            return new()
            {
                AccessToken = GetAccessToken(claims, validForKind),
                RefreshToken = refreshToken ?? Options.GenerateRefreshToken(),
                ExpiresIn = ((int) Options.AccessTokenValidForKind(validForKind).TotalSeconds).ToString()
            };
        }
    }
}