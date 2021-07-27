using System.Text;
using ReadingIsGood.BusinessLayer.RequestModels.Auth;
using ReadingIsGood.EntityLayer.Database.Auth;
using ReadingIsGood.Utils.Crypto;

namespace ReadingIsGood.BusinessLayer.Extensions
{
    public static class RegistrationRequestExtensions
    {
        public static User ToNewUser(this RegistrationRequest request, string salt)
        {
            return new()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHashed = PasswordHelper.GenerateHashedPassword(request.Password, Encoding.ASCII.GetBytes(salt))
            };
        }
    }
}