using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ReadingIsGood.Utils.Jwt
{
    public class JwtIssuerOptions : JwtValidationOptions
    {
        /// <summary>
        ///     "nbf" (Not Before) Claim (default is UTC NOW, minus 5 seconds)
        /// </summary>
        /// <remarks>
        ///     The "nbf" (not before) claim identifies the time before which the JWT
        ///     MUST NOT be accepted for processing.  The processing of the "nbf"
        ///     claim requires that the current date/time MUST be after or equal to
        ///     the not-before date/time listed in the "nbf" claim.  Implementers MAY
        ///     provide for some small leeway, usually no more than a few minutes, to
        ///     account for clock skew.  Its value MUST be a number containing a
        ///     NumericDate value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public DateTime NotBefore => DateTime.UtcNow.AddSeconds(-5);

        /// <summary>
        ///     "iat" (Issued At) Claim (default is UTC NOW)
        /// </summary>
        /// <remarks>
        ///     The "iat" (issued at) claim identifies the time at which the JWT was
        ///     issued.  This claim can be used to determine the age of the JWT.  Its
        ///     value MUST be a number containing a NumericDate value.  Use of this
        ///     claim is OPTIONAL.
        /// </remarks>
        public DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        ///     Set the timespan the access token will be valid for (default is 5 min/300 seconds)
        /// </summary>
        public IDictionary<string, TimeSpan> AccessTokenValidFor { get; set; } = new Dictionary<string, TimeSpan>();

        /// <summary>
        ///     Set the timespan the refresh token will be valid for (default is 5 days)
        /// </summary>
        public IDictionary<string, TimeSpan> RefreshTokenValidFor { get; set; } = new Dictionary<string, TimeSpan>();

        /// <summary>
        ///     The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        ///     returns the valid for time for the respective access token kind; defaults to 5 minutes
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns></returns>
        public TimeSpan AccessTokenValidForKind(string kind)
        {
            return AccessTokenValidFor.ContainsKey(kind)
                ? AccessTokenValidFor[kind]
                : TimeSpan.FromMinutes(5);
        }

        /// <summary>
        ///     returns the valid for time for the respective refresh token kind; defaults to 5 minutes
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns></returns>
        public TimeSpan RefreshTokenValidForKind(string kind)
        {
            return RefreshTokenValidFor.ContainsKey(kind)
                ? RefreshTokenValidFor[kind]
                : TimeSpan.FromDays(5);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}+{Guid.NewGuid()}"));
        }

        /// <summary>
        ///     "exp" (Expiration Time) Claim (returns IssuedAt + ValidFor)
        /// </summary>
        /// <param name="kind"></param>
        /// <remarks>
        ///     The "exp" (expiration time) claim identifies the expiration time on
        ///     or after which the JWT MUST NOT be accepted for processing.  The
        ///     processing of the "exp" claim requires that the current date/time
        ///     MUST be before the expiration date/time listed in the "exp" claim.
        ///     Implementers MAY provide for some small leeway, usually no more than
        ///     a few minutes, to account for clock skew.  Its value MUST be a number
        ///     containing a NumericDate value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public DateTime Expiration(string kind)
        {
            return IssuedAt.Add(AccessTokenValidForKind(kind));
        }
    }
}