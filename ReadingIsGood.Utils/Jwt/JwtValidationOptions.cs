using Microsoft.IdentityModel.Tokens;

namespace ReadingIsGood.Utils.Jwt
{
    public class JwtValidationOptions
    {
        public SecurityKey ValidationKey { get; set; }

        /// <summary>
        ///     "iss" (Issuer) Claim
        /// </summary>
        /// <remarks>
        ///     The "iss" (issuer) claim identifies the principal that issued the
        ///     JWT.  The processing of this claim is generally application specific.
        ///     The "iss" value is a case-sensitive string containing a StringOrURI
        ///     value.  Use of this claim is OPTIONAL.
        /// </remarks>
        public string Issuer { get; set; }
    }
}