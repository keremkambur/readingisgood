namespace ReadingIsGood.Utils.Jwt
{
    public class JwtResponse
    {
        /// <summary>
        ///     Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        ///     Refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        ///     Expiry time in seconds from the token creation. Also found in the decoded access token as unix timestamp.
        /// </summary>
        public string ExpiresIn { get; set; }
    }
}