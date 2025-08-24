using System.Text;
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
                                                           }).
        AddEntityFrameworkStores<AppDbContext>().
        AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
                                   {
                                       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                       options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                                   }).
        AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             // 🔒 Standard checks
                             ValidateIssuer = true , ValidateAudience = true , ValidateLifetime = true , ValidateIssuerSigningKey = true ,

                             // 👇 from your config (IOptions<JwtOptions>)
                             ValidIssuer      = JwtOptions.Issuer , ValidAudience = JwtOptions.Audience ,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey)) ,

                             // ⏰ Ensure token expires exactly as intended
                             ClockSkew = TimeSpan.Zero , // default = 5 minutes; set to 0 for strict expiration

                             // ✅ Extra safety
                             RequireExpirationTime = true ,  // token *must* have exp claim
                             RequireSignedTokens   = true ,  // token must be signed
                             ValidateActor         = false , // only needed if you use "act" claim
                             ValidateTokenReplay   = false , // enable if you track jti in DB/Redis for replay protection
                             SaveSigninToken       = false   // don't keep token instance in memory
                         };

// Debug why it's 401
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

// 🔹 OpenAPI + Bearer security in the document
builder.Services.AddOpenApi(options =>
                            {
                                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                            });

builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();


app.UseCors(a => a.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // 1) Apply migrations
    var db = services.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    // 2) Seed roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

    var roles = new[]
    {
        "Doctor" ,
        "Secretary" ,
        "Patient"
    };

    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<long>(role));

    // 3) Seed users (Doctor, Secretary, Patient)
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await SeedHelper.SeedUserAsync(
        userManager ,
        phone: "0911000001" , // will normalize to PhoneNumber +98911000001
        fullName: "Dr. Example" ,
        email: "doctor@example.com" ,
        password: "Doctor#1234" ,
        role: "Doctor");

    await SeedHelper.SeedUserAsync(
        userManager ,
        phone: "0911000002" ,
        fullName: "Sec. Example" ,
        email: "secretary@example.com" ,
        password: "Secretary#1234" ,
        role: "Secretary");

    await SeedHelper.SeedUserAsync(
        userManager ,
        phone: "0911000003" ,
        fullName: "Mr. Patient" ,
        email: "patient@example.com" ,
        password: "Patient#1234" ,
        role: "Patient");
}

app.MapOpenApi("/openapi/v1.json");

app.MapScalarApiReference("/docs" ,
                          options =>
                          {
                              options.WithTitle("Clinic Management API").
                                      WithSidebar(true).
                                      WithDarkMode(true).
                                      WithDefaultOpenAllTags(false).
                                      WithOpenApiRoutePattern("/openapi/{documentName}.json") // where Scalar fetches docs
                                      .
                                      AddDocument("v1" , "Production API"); // documentName = "v1"
                          });

app.MapGet("/" , () => Results.Redirect("/docs"));

app.Run();