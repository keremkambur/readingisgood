using System;
using System.Text.Json.Serialization;
using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Auth
{
    public class RefreshLoginRequest : Request
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("clientId")]
        public Guid ClientId { get; set; }

        public override void ValidateAndThrow()
        {
            if (string.IsNullOrWhiteSpace(RefreshToken))
            {
                //throw new RequestParameterException(nameof(this.RefreshToken), "Refresh token is required.");
            }
        }
    }
}