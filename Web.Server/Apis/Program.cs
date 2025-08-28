using System.Text;
using Api.Endpoints.Constants;
using Api.Endpoints.Context;
using Api.Endpoints.Models.User;
using Api.Endpoints.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddIdentity<AppUser , IdentityRole<long>>(options =>
    {
        options.Password.RequiredLength         = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase       = false;
        options.Password.RequireLowercase       = false;
        options.Password.RequireDigit           = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            
            ValidateIssuer = true ,
            ValidateAudience = true ,
            ValidateLifetime = true ,
            ValidateIssuerSigningKey = true ,

            
            ValidIssuer      = JwtOptions.Issuer ,
            ValidAudience = JwtOptions.Audience ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey)) ,

            
            ClockSkew = TimeSpan.Zero , 

            
            RequireExpirationTime = true ,  
            RequireSignedTokens   = true ,  
            ValidateActor         = false , 
            ValidateTokenReplay   = false , 
            SaveSigninToken       = false   
        };


        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT auth failed: " + ctx.Exception.Message);
                return Task.CompletedTask;
            } ,
            OnChallenge = ctx => Task.CompletedTask
        };
    });

builder.Services.AddControllers();


builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.UseCors(a => a.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    
    var db = services.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

    var roles = new[]
    {
        "Doctor" ,
        "Secretary" ,
        "Patient"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<long>(role));
    }

    
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await SeedHelper.SeedUserAsync(
        userManager ,
        "0911000001" , 
        "Dr. Example" ,
        "doctor@example.com" ,
        "Doctor#1234" ,
        "Doctor");

    await SeedHelper.SeedUserAsync(
        userManager ,
        "0911000002" ,
        "Sec. Example" ,
        "secretary@example.com" ,
        "Secretary#1234" ,
        "Secretary");

    await SeedHelper.SeedUserAsync(
        userManager ,
        "0911000003" ,
        "Mr. Patient" ,
        "patient@example.com" ,
        "Patient#1234" ,
        "Patient");
}

app.MapOpenApi("/openapi/v1.json");

app.MapScalarApiReference("/docs" ,
    options =>
    {
        options.WithTitle("Clinic Management API")
            .WithSidebar()
            .WithDarkMode()
            .WithDefaultOpenAllTags(false)
            .WithOpenApiRoutePattern("/openapi/{documentName}.json") 
            .AddDocument("v1" , "Production API");                   
    });

app.MapGet("/" , () => Results.Redirect("/docs"));

app.Run();