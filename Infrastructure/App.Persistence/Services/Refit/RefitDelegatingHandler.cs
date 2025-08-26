#region

    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Headers;
    using App.Applications.Users.Queries.GetUserInfo;
    using App.Common.Exceptions;
    using App.Common.General;
    using App.Common.Utilities.LifeTime;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Localization;

#endregion

    namespace App.Persistence.Services.Refit;

    public class RefitDelegatingHandler : DelegatingHandler,
        ITransientDependency
    {
        private readonly IStringLocalizer<RefitDelegatingHandler> _localizer;
        private readonly IServiceScopeFactory                     _scopeFactory;

        public RefitDelegatingHandler(
            IStringLocalizer<RefitDelegatingHandler> localizer,
            IServiceScopeFactory                     scopeFactory)
        {
            _localizer         = localizer;
            _scopeFactory = scopeFactory;

            InnerHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request,            nameof(request));
            ArgumentNullException.ThrowIfNull(request.RequestUri, nameof(request.RequestUri));

            try
            {
                await AddTokenHeaderAsync(request.Headers);
                return await base.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException)
            {
                throw ShiftyException.Create(_localizer["NetworkError"]);
            }
            catch (TaskCanceledException)
            {
                throw ShiftyException.Create(_localizer["TimeoutError"]);
            }
            catch (Exception e)
            {
                throw ShiftyException.Create(_localizer["UnHandlerError"]);
            }
        }

        private async Task AddTokenHeaderAsync(HttpRequestHeaders headers)
        {
            try

            {
                using var scope    = _scopeFactory.CreateScope();
                var       mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var       user     = await mediator.Send(new GetUserInfoQuery());


                if (!string.IsNullOrEmpty(user?.Token) && !IsTokenExpired(user.Token))
                    headers.Authorization = new AuthenticationHeaderValue(ApplicationConstants.Headers.Token, user.Token);
            }

            catch (Exception e)

            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool IsTokenExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken   = jwtHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.Now;
        }
    }