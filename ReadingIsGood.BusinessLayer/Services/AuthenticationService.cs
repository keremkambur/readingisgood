using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Exceptions;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.Options;
using ReadingIsGood.BusinessLayer.RequestModels.Auth;
using ReadingIsGood.BusinessLayer.ResponseModels.Auth;
using ReadingIsGood.EntityLayer.Database.Auth;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.EntityLayer.QueryModels;
using ReadingIsGood.Utils.Extensions;
using ReadingIsGood.Utils.Identity;
using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class AuthenticationService : InternalService<AuthenticationServiceOptions>, IAuthenticationService
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IBusinessObject businessObject,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<AuthenticationServiceOptions> options
        ) : base(logger, businessObject, options)
        {
            _jwtOptions = jwtOptions.Value;
            _jwtGenerator = new JwtGenerator(_jwtOptions);

            if (string.IsNullOrWhiteSpace(Options.Salt))
                throw new ArgumentNullException(
                    $"[{nameof(AuthenticationServiceOptions.Salt)}] is required. Please configure the service options properly [{nameof(AuthenticationServiceOptions)}]");
        }

        public Task RegisterCustomer(RegistrationRequest request, CancellationToken cancellationToken)
        {
            BusinessObject.AuthRepository.UserCrudOperations.Create(request.ToNewUser(Options.Salt));
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Authenticates user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponse> AuthenticateCustomer(AuthenticationRequest request,
            CancellationToken cancellationToken)
        {
            var user = BusinessObject.AuthRepository.QueryUserFromLogin(request.Email, request.Password,
                Options.Salt);

            if (user == null)
            {
                throw new ForbiddenAccessException("User does not exist");
            }

            var newRefreshToken = _jwtOptions.GenerateRefreshToken();

            if (!BusinessObject.AuthRepository.CreateNewRefreshTokenFromLogin(
                user.Uuid,
                user.ClientId,
                newRefreshToken,
                _jwtOptions.RefreshTokenValidForKind(nameof(JwtValidForKind.Customer)).ToDateTimeFromUtcNow()
            ))
            {
                throw new ForbiddenAccessException("Failed to authenticate user.");
            }

            return new AuthenticationResponse
            {
                ClientId = user.ClientId,
                Token = BuildJwtToken(user, UserType.Customer, JwtValidForKind.Customer.ToAudience(), newRefreshToken)
            };
        }

        /// <summary>
        ///     Refreshes token of user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RefreshLoginResponse> RefreshLogin(RefreshLoginRequest request, CancellationToken cancellationToken)
        {
            var newRefreshToken = _jwtOptions.GenerateRefreshToken();

            var user = BusinessObject.AuthRepository.QueryAndCreateNewRefreshTokenFromLogin(
                request.RefreshToken,
                request.ClientId,
                newRefreshToken,
                _jwtOptions.RefreshTokenValidForKind(nameof(JwtValidForKind.Customer)).ToDateTimeFromUtcNow());

            if (user == null)
            {
                throw new ForbiddenAccessException("Failed to authenticate user.");
            }

            return new RefreshLoginResponse
            {
                Token = BuildJwtToken(user, UserType.Customer, JwtValidForKind.Customer.ToAudience(), newRefreshToken)
            };
        }

        public async Task<User> GetById(Guid uuid)
        {
            return this.BusinessObject.AuthRepository.UserCrudOperations.QuerySingle(x => x.Uuid == uuid);
        }

        private JwtResponse BuildJwtToken(QueryUserFromLoginResponse user, UserType userType, List<string> audiences,
            string refreshToken = null)
        {
            if (user == null)
            {
                throw new ForbiddenAccessException("User does not exist.");
            }

            string jwtValidForKind;

            var claimsHelper = new ClaimsIdentityHelper(_jwtGenerator.Options.Issuer);

            var claims = new List<Claim>
            {
                claimsHelper.CreateClaim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()), // unique identifier for the jwt
                claimsHelper.CreateClaim(ClaimsIdentityHelper.ClaimsTypes.ClientId,
                    user?.ClientId.ToString()), // unique identifier for client (login device)
                claimsHelper.CreateClaim(JwtRegisteredClaimNames.Sub,
                    user?.Uuid.ToString()), // unique identifier for the principal (user)
                claimsHelper.CreateClaim(JwtRegisteredClaimNames.Iat,
                    _jwtGenerator.Options.IssuedAt.ToUnixEpochInSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            claims.Add(claimsHelper.CreateClaim(ClaimsIdentityHelper.ClaimsTypes.UserType, userType.ToString()));

            audiences.ForEach(audience => claims.Add(claimsHelper.CreateClaim(JwtRegisteredClaimNames.Aud, audience)));

            return _jwtGenerator.GetResponse(
                claims.ToArray(),
                userType == UserType.Customer
                    ? nameof(JwtValidForKind.Customer)
                    : nameof(JwtValidForKind.Admin),
                refreshToken);
        }
    }
}