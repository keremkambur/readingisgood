using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Auth
{
    public class RegistrationRequest : Request
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public override void ValidateAndThrow()
        {
            //TODO add validation!
        }
    }
}