using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

using Tributech.SensorManager.Application.Extensions;
using Tributech.SensorManager.Domain.Extensions;

namespace Tributech.SensorManager.Api;

public static class Extensions
{
    // MvcBuilder addjsonoptions
    public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder, Action<JsonOptions>? setupAction = null)
    {
        //builder.Services.Configure(setupAction);
        builder.AddJsonOptions(options =>
        {
            // Configure the JSON serializer options here
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.IgnoreNullValues = true;

            options.JsonSerializerOptions
                .ConfigureApplicationJsonOptions()
                .ConfigureDomainJsonOptions()
                ;

            // Add more configuration options as needed

            setupAction?.Invoke(options);
        });

        return builder;
    }

    // addkeycloakauthentication
    public static IServiceCollection AddKeycloakAuthenticationOld(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IClaimsTransformation, KeycloakRolesClaimsTransformation>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
        })
        .AddOpenIdConnect(options =>
        {
            options.Authority = "http://localhost:8085/realms/customer";
            options.ClientId = "customer-api";
            options.ClientSecret = "D5GZg98lOguiLrXiEMe22dapOrhCpZa1";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.Scope.Add("openid");
            options.Scope.Add("roles");
            options.Scope.Add("profile");

            //options.CallbackPath = "/api/account/login"; // Update callback path
            options.CallbackPath = "/login-callback"; // Update callback path
            options.SignedOutCallbackPath = "/logout-callback"; // Update signout callback path
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "preferred_username",
                RoleClaimType = "roles"
            };
            options.ClaimActions.MapJsonKey("roles", "roles");

            options.RequireHttpsMetadata = false; // development

            options.Events.OnRedirectToIdentityProvider = context =>
            {
                // Modify scopes here if needed
                context.ProtocolMessage.Scope += " email"; // Add email scope dynamically
                return Task.CompletedTask;
            };

            options.Events.OnTokenValidated = ctx =>
            {
                var payload = ctx.SecurityToken.Payload;

                //var (_, roles) = payload.Where(x => x.Key == "role").FirstOrDefault();
                //var element = roles as JsonElement?;
                //var element.Value.Deserialize<string[]>();

                return Task.CompletedTask;
            };

            options.Events.OnTokenResponseReceived = ctx =>
            {
                var payload = ctx.TokenEndpointResponse.AccessToken;
                try
                {
                    //var decoded = JwtPayload.Deserialize(payload);
                    DecodeJwt(ctx);
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }

                return Task.CompletedTask;
            };

            //Microsoft.AspNetCore.Authentication.OAuth.Claims.DeleteClaimAction ac;
            //Microsoft.AspNetCore.Authentication.OpenIdConnect.Claims.UniqueJsonKeyClaimAction bv;

            //options.ClaimActions.Clear();
            //options.ClaimActions.Add(new CustomClaimAction("resource_access", "JSON"));
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        });
        return services;
    }

    private static void DecodeJwt(TokenResponseReceivedContext ctx)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(ctx.TokenEndpointResponse.AccessToken);
        var claims = token.Claims;

        // roles from resource_access.customer-api.roles
        var roles = claims.Where(c => c.Type == "resource_access").Select(c => c.Value).ToList();
    }

    public static void UseKeycloakAuthOld(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        // Add routes for callback handling
        app.Map("/login-callback", loginCallbackApp =>
        {
            loginCallbackApp.Run(async context =>
            {
                // Handle the callback from Keycloak after successful authentication
                await context.Response.WriteAsync("Authentication successful");
            });
        });

        app.Map("/logout-callback", logoutCallbackApp =>
        {
            logoutCallbackApp.Run(async context =>
            {
                // Handle the callback from Keycloak after sign-out
                await context.Response.WriteAsync("Sign-out successful");
            });
        });

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllerRoute(
        //        name: "default",
        //        pattern: "{controller=Home}/{action=Index}/{id?}");

        //    // Add route for Keycloak authentication callback
        //    endpoints.MapControllerRoute(
        //        name: "login-callback",
        //        pattern: "login-callback",
        //        defaults: new { controller = "Account", action = "LoginCallback" });
        //});

        // Top-level:
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Add route for Keycloak authentication callback
        app.MapControllerRoute(
            name: "login-callback",
            pattern: "login-callback",
            defaults: new { controller = "Account", action = "LoginCallback" });
    }

    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(GetPublicKey()),
                ValidateIssuer = false,
                ValidateAudience = false,
                // Set clock skew to zero if you want exact token expiration timing, optional
                //ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
        });

        return services;
    }

    private static RSAParameters GetPublicKey()
    {
        const string rawPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxl2wB87BCgSFXO2MjwV5qeSZ6vD9j2bMHgUt3fzTTc/1GoEasczGGM0p8T2oNAHPxqPF9RmT98hiR11LNQq+knEbwUUwAXXXO5OPtvW4+lXPMRnJ6iGaben+eFPehf+2YyUphyi7AYfCWypbREhFm5aZMHsYo9Ux+0LC2xNVhovCagcomPxWcns7HOsN5htn88LL9Jlx4PvDs2S9gLt2f4mmLCA4y7nVP8oBSf/ohm01i42EOBTwS4BT92uJ9SbV+9gnZPr6pkB8Olb/lZ0EzTRpzzgKqDLW7sqenj4UnnTfqPF9AY58xLqzvLHHZSxGBXE1pKwKPinWUaN35LKpEwIDAQAB";
        const string publicKey = $"-----BEGIN PUBLIC KEY-----\n{rawPublicKey}\n-----END PUBLIC KEY-----";

        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKey.ToCharArray());
        return rsa.ExportParameters(false);
    }

    public static void UseKeycloakAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}

public class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        //var identity = (ClaimsIdentity)principal.Identity;

        //// Add custom claims
        //identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

        return principal;
    }
}

public class CustomClaimAction : ClaimAction
{
    public CustomClaimAction(string claimType, string valueType) : base(claimType, valueType)
    {
    }

    public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
    {
        // take roles and map them to identity
        var roles = userData.GetProperty("resource_access").GetProperty("customer-api").GetProperty("roles").EnumerateArray();
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString(), issuer));
        }
    }
}