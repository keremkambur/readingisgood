using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Auth
{
    public class AuthenticationRequest : Request
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public override void ValidateAndThrow()
        {
            //TODO add validation!
        }
    }
}