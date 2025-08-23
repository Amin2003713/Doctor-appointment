using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Applications.Users.Commands.Logout;
using App.Applications.Users.Queries.GetUserInfo;
using App.Applications.Users.Requests.Login;
using App.Common.Utilities.LifeTime;
using App.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace App.Persistence.Services.AuthState;

public class ClientStateProvider : AuthenticationStateProvider, IScopedDependency
{
    private readonly IMediator _mediator;
    private readonly NavigationManager _navigationManager;

    public ClientStateProvider(IMediator mediator, NavigationManager navigationManager)
    {
        _mediator = mediator;
        _navigationManager = navigationManager;
        // ❌ هیچ عملیات async یا .Result/.Wait() در سازنده انجام نده
    }

    public UserInfo? User { get;  set; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // بار اول از سرور/استوریج بگیر (کاملاً async)
            User ??= await _mediator.Send(new GetUserInfoQuery());

            // کاربر لاگین نیست
            if (User?.Token is null || User.RefreshToken is null)
            {
                _navigationManager.NavigateTo("/login");
                return Anonymous();
            }

            // اگر توکن هنوز معتبر است
            if (!IsTokenExpired(User.Token))
            {
                var principal = CreatePrincipal(User.Token);
                return new AuthenticationState(principal);
            }

            // تلاش برای رفرش
            var refreshedState = await RefreshTokenAsync(User);
            if (refreshedState is not null)
            {
                // تغییر وضعیت را به UI اطلاع بده
                NotifyAuthenticationStateChanged(Task.FromResult(refreshedState));
                return refreshedState;
            }

            // رفرش ناموفق
            _navigationManager.NavigateTo("/login");
            return Anonymous();
        }
        catch
        {
            _navigationManager.NavigateTo("/login");
            return Anonymous();
        }
    }

    private static AuthenticationState Anonymous()
        => new(new ClaimsPrincipal(new ClaimsIdentity()));

    private static bool IsTokenExpired(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        // همیشه با UTC مقایسه کن
        return jwt.ValidTo <= DateTime.UtcNow;
    }

    private async Task<AuthenticationState?> RefreshTokenAsync(UserInfo user)
    {
        try
        {
            var login = await _mediator.Send(new RefreshTokenRequest(user.RefreshToken!, user.Token!));

            // اگه نیاز داری رفرش‌توکن جدید رو هم ذخیره کنی، همینجا انجام بده
            User = new UserInfo
            {
                Token = login.Token,
                RefreshToken = login.RefreshToken ?? user.RefreshToken,
                // سایر فیلدها...
            };

            var principal = CreatePrincipal(login.Token);
            return new AuthenticationState(principal);
        }
        catch
        {
            return null;
        }
    }

    private static ClaimsPrincipal CreatePrincipal(string jwtToken)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        var identity = new ClaimsIdentity(jwt.Claims, authenticationType: "jwt");
        return new ClaimsPrincipal(identity);
    }

    public async Task Logout()
    {
        if (User?.Token is not null)
        {
            try { await _mediator.Send(new LogoutCommand(User.Token)); } catch { /* ignore */ }
        }

        User = null;
        var anon = Task.FromResult(Anonymous());
        NotifyAuthenticationStateChanged(anon);
        _navigationManager.NavigateTo("/login");
    }

    // اختیاری: وقتی از بیرون (مثلاً بعد از Login) توکن گرفتی، صدا بزن
    public Task SetUserAsync(UserInfo? user)
    {
        User = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}
