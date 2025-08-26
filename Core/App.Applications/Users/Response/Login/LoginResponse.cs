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

            string Get(params string[] types)
            {
                return jwtToken.Claims.FirstOrDefault(c => types.Contains(c.Type))?.Value ?? string.Empty;
            }

            // ID: prefer sub, then nameidentifier
            var id = Get(JwtRegisteredClaimNames.Sub , ClaimTypes.NameIdentifier);

            // Full display name comes from ClaimTypes.Name (you issued it as FullName or fallback)
            var userName = Get(ClaimTypes.Name);

            // Custom claims you added
            var firstName = Get("first_name");
            var lastName  = Get("last_name");
            var profile   = Get("profile");
            var address   = Get("address");

            var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            return new UserInfo
            {
                UserName = userName ,
                Id                                                 = id ,
                FirstName = firstName ,
                LastName = lastName ,
                Profile  = string.IsNullOrWhiteSpace(profile) ? null : profile ,
                Address = (string.IsNullOrWhiteSpace(address) ? null : address)! ,

                LastLoginDate = string.Empty ,
                PhoneNumber = Get(ClaimTypes.MobilePhone) ,
                RolesList = roles ,
                Token = Token
            };
        }
    }