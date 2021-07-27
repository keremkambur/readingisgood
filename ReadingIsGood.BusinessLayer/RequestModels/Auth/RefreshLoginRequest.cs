using System;
using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Auth
{
    public class RefreshLoginRequest : Request
    {
        public string RefreshToken { get; set; }

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