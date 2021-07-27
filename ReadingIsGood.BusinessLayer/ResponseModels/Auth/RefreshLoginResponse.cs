using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Auth
{
    public class RefreshLoginResponse
    {
        public JwtResponse Token { get; set; }
    }
}