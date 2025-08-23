using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options =>
    {
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // 🔒 Standard checks
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // 👇 from your config (IOptions<JwtOptions>)
            ValidIssuer = JwtOptions.Issuer,
            ValidAudience = JwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey)),

            // ⏰ Ensure token expires exactly as intended
            ClockSkew = TimeSpan.Zero,   // default = 5 minutes; set to 0 for strict expiration

            // ✅ Extra safety
            RequireExpirationTime = true, // token *must* have exp claim
            RequireSignedTokens = true,   // token must be signed
            ValidateActor = false,        // only needed if you use "act" claim
            ValidateTokenReplay = false,  // enable if you track jti in DB/Redis for replay protection
            SaveSigninToken = false       // don't keep token instance in memory
        };
    });


builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
    var roles = new[]
    {
        "Doctor",
        "Secretary",
        "Patient"
    };

    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<long>(role));
}


app.Run();