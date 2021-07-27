namespace ReadingIsGood.BusinessLayer.Options
{
    public class JwtValidForKindOptions
    {
        public AccessTokenValidForKindOptions AccessToken { get; set; }
        public RefreshTokenValidForKindOptions RefreshToken { get; set; }
    }
}