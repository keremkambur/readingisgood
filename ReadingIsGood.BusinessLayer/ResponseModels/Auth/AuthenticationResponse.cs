using System;
using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Auth
{
    public class AuthenticationResponse
    {
        public Guid ClientId { get; set; }
        public JwtResponse Token { get; set; }
    }
}