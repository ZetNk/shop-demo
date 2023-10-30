using System.Threading.RateLimiting;
using Amazon.S3;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Shop.API.Data;
using Shop.API.Interfaces;
using Shop.API.Models;
using Shop.API.Services;
using Shop.API.Validators;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var AllowSpecificOrigins = "_AllowSpecificOrigins";

services.AddCors(o => o.AddPolicy(AllowSpecificOrigins, builder => { builder.AllowAnyOrigin(); }));


//add Db
services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie().AddGoogle(options =>
    {
        var googleAuthNSection = configuration.GetSection("Authentication:Google");

        if (googleAuthNSection == null) throw new Exception("missing google auth detalis");

        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
        options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
    });

//rate limitation 20 requests per minute, per authenticated username
services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(), partition =>
                new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 20,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1)
                }));
});

services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<IValidator<CartItem>, CartItemValidator>();
services.AddScoped<ICartService, CartService>();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    var authorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth");
    var tokenUrl = new Uri("https://oauth2.googleapis.com/token");
    c.AddOAuth2(authorizationUrl, tokenUrl);
});

// add aws
var awsOptions = configuration.GetAWSOptions();
services.AddDefaultAWSOptions(awsOptions);
services.AddAWSService<IAmazonS3>();

var app = builder.Build();


// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var googleAuthNSection = configuration.GetSection("Authentication:Google");
    string[] scopes = {"https://www.googleapis.com/auth/userinfo.email"};

    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop.API v1");
    c.OAuthClientId(googleAuthNSection["ClientId"]);
    c.OAuthClientSecret(googleAuthNSection["ClientSecret"]);
    c.OAuthAppName("Shop Api");
    c.OAuthScopes(scopes);
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
});

app.UseCors(AllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetService<DataContext>().Database.Migrate();
}

app.Run();