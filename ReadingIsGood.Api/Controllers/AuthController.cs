using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Exceptions;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.RequestModels.Auth;
using ReadingIsGood.BusinessLayer.ResponseModels.Auth;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.Utils;

namespace ReadingIsGood.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        [Consumes(Constants.MimeType.Json)]
        [ProducesResponseType(typeof(PostResponse), 200)]
        [ProducesResponseType(typeof(PostResponse), 401)]
        public async Task<PostResponse> NewCustomer([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
        {
            var response = new PostResponse(this.HttpContext.TraceIdentifier);

            try
            {
                request.ValidateAndThrow();
                await _authenticationService.RegisterCustomer(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                response.SetError(ex);
            }

            return response;
        }

        [HttpPost("login")]
        [Consumes(Constants.MimeType.Json)]
        [ProducesResponseType(typeof(SingleResponse<AuthenticationResponse>), 200)]
        [ProducesResponseType(typeof(SingleResponse<RefreshLoginResponse>), 401)]
        public async Task<ISingleResponse<AuthenticationResponse>> CustomerLogin([FromBody] AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var response = new SingleResponse<AuthenticationResponse>(this.HttpContext.TraceIdentifier);

            try
            {
                request.ValidateAndThrow();

                response.Model = await _authenticationService.AuthenticateCustomer(request, cancellationToken)
                        .ConfigureAwait(false)
                    ;
            }
            catch (Exception ex)
            {
                response.SetError(ex);
                response.Model = null;
            }

            return response;
        }

        [HttpPost("refresh")]
        [Consumes(Constants.MimeType.Json)]
        [ProducesResponseType(typeof(SingleResponse<RefreshLoginResponse>), 200)]
        [ProducesResponseType(typeof(SingleResponse<RefreshLoginResponse>), 401)]
        public async Task<ISingleResponse<RefreshLoginResponse>> CustomerRefresh([FromBody] RefreshLoginRequest request, CancellationToken cancellationToken)
        {
            var response = new SingleResponse<RefreshLoginResponse>(this.HttpContext.TraceIdentifier);

            try
            {
                request.ValidateAndThrow();

                response.Model = await _authenticationService.RefreshLogin(request, cancellationToken)
                        .ConfigureAwait(false)
                    ;
            }
            catch (Exception ex)
            {
                response.SetError(ex);
                response.Model = null;
            }

            return response;
        }
    }
}