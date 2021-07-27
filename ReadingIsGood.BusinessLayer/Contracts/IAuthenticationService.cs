using System;
using System.Threading;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.RequestModels.Auth;
using ReadingIsGood.BusinessLayer.ResponseModels.Auth;
using ReadingIsGood.EntityLayer.Database.Auth;

namespace ReadingIsGood.BusinessLayer.Contracts
{
    public interface IAuthenticationService
    {
        Task RegisterCustomer(RegistrationRequest request, CancellationToken cancellationToken);

        Task<AuthenticationResponse> AuthenticateCustomer(AuthenticationRequest request,
            CancellationToken cancellationToken);

        Task<RefreshLoginResponse> RefreshLogin(RefreshLoginRequest request, CancellationToken cancellationToken);

        Task<User> GetById(Guid uuid);
    }
}