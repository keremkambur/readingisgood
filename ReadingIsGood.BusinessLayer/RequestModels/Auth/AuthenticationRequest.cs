using System.Text.Json.Serialization;
using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Auth
{
    public class AuthenticationRequest : Request
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        public override void ValidateAndThrow()
        {
            //TODO add validation!
        }
    }
}