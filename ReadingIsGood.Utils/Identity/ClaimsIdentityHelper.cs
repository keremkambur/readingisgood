using System.Security.Claims;

namespace ReadingIsGood.Utils.Identity
{
    public class ClaimsIdentityHelper
    {
        protected string Issuer;

        public ClaimsIdentityHelper(string issuer)
        {
            Issuer = issuer;
        }

        public Claim CreateClaim(string type, string value, string valueType = ClaimValueTypes.String)
        {
            return new(type, value, valueType, Issuer);
        }

        public static class ClaimsTypes
        {
            /// <summary>
            ///     type of the user
            /// </summary>
            public const string UserType = "http://reading-is-good.getir.de/api/2021/07/auth/claims/user-type";

            /// <summary>
            ///     unique login id (device id); used to identify and follow refresh token path
            /// </summary>
            public const string ClientId = "http://reading-is-good.getir.de/api/2021/07/auth/claims/client-id";
        }
    }
}