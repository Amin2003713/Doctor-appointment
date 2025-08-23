#region

    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using App.Domain.Users;

    namespace App.Applications.Users.Response.Login;

#endregion

    public class LoginResponse
    {
        public string Token { get; set; }


        public UserInfo CreateUser()
        {
            var handler  = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(Token);

            return new UserInfo
            {
                // UserName رو از ClaimTypes.Name می‌گیریم
                UserName      = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty,

                // Id رو از Sub یا NameIdentifier می‌گیریم
                Id            = jwtToken.Claims.FirstOrDefault(c =>
                                        c.Type == JwtRegisteredClaimNames.Sub ||
                                        c.Type == ClaimTypes.NameIdentifier)
                                    ?.Value ??
                                string.Empty,

                // چون توی توکن FirstName / LastName ست نمی‌شه، اینجا خالی می‌ذاریم
                FirstName     = string.Empty,
                LastName      = string.Empty,

                // profile هم ست نشده → خالی
                Profile       = null,

                // LastLoginDate هم ست نشده → خالی
                LastLoginDate = string.Empty,

                // PhoneNumber هم ست نشده → خالی
                PhoneNumber   = null,

                // Roles اگه ست می‌کنی در توکن، اینجا هم قابل خوندنه
                RolesList     = jwtToken.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList(),

                Token         = Token
            };
        }
    }