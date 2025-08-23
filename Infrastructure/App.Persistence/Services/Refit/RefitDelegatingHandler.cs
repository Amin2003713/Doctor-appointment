#region

    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Headers;
    using App.Applications.Users.Queries.GetUserInfo;
    using App.Common.Exceptions;
    using App.Common.General;
    using App.Common.Utilities.LifeTime;
    using MediatR;
    using Microsoft.Extensions.Localization;

#endregion

    namespace App.Persistence.Services.Refit;

    public class RefitDelegatingHandler : DelegatingHandler ,
                                                   ITransientDependency
    {
        private readonly IMediator                                _mediator;
        private readonly IStringLocalizer<RefitDelegatingHandler> _localizer;

        public RefitDelegatingHandler(
            IMediator                                mediator ,
            IStringLocalizer<RefitDelegatingHandler> localizer)
        {
            _mediator  = mediator;
            _localizer = localizer;

            InnerHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false ,
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request , CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request ,            nameof(request));
            ArgumentNullException.ThrowIfNull(request.RequestUri , nameof(request.RequestUri));

            try
            {
                return await base.SendAsync(request , cancellationToken);
            }
            catch (HttpRequestException)
            {
                throw ShiftyException.Create(_localizer["NetworkError"]);
            }
            catch (TaskCanceledException)
            {
                throw ShiftyException.Create(_localizer["TimeoutError"]);
            }
            catch (Exception)
            {
                throw ShiftyException.Create(_localizer["UnHandlerError"]);
            }
        }


        private bool IsTokenExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken   = jwtHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.Now;
        }
    }