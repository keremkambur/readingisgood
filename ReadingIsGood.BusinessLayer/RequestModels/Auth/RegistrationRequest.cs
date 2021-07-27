using System.Text.Json.Serialization;
using ReadingIsGood.BusinessLayer.RequestModels.Base;
using ReadingIsGood.BusinessLayer.Exceptions;

namespace ReadingIsGood.BusinessLayer.RequestModels.Auth
{
    public class RegistrationRequest : Request
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        public override void ValidateAndThrow()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                throw new RequestParameterException(FirstName);
            }

        }
    }
}