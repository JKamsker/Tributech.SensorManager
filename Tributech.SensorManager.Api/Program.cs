using FluentValidation;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

using Tributech.SensorManager.Application;
using Tributech.SensorManager.Application.Behavior;
using Tributech.SensorManager.Infrastructure;

namespace Tributech.SensorManager.Api;

public class Program
{
    public static void Main(string[] args)
    {
        // Set threadlanguage to english
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        Testjwt();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddControllers()
            .ConfigureJsonOptions();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddMediatR(builder =>
        {
            builder.RegisterServicesFromAssemblyContaining<ApplicationStub>();
            builder.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(typeof(ApplicationStub).Assembly);

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddKeycloakAuthentication(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseKeycloakAuth();

        app.MapControllers();

        app.Run();
    }

    private static void Testjwt()
    {
        var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJpN01DOGtqTnVvTzRJazB6ZEU5WF9CZWYwUk1fcGtXSEVHV01DUlBTQzdzIn0.eyJleHAiOjE3MTUyOTY3ODYsImlhdCI6MTcxNTI5NjQ4NiwiYXV0aF90aW1lIjoxNzE1Mjk1NjQyLCJqdGkiOiJjOTJkYTVlNy03NjgwLTRjMTItYTYwZC0zNTYzNDMzOWI2MmUiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjgwODUvcmVhbG1zL2N1c3RvbWVyIiwiYXVkIjpbImN1c3RvbWVyLWFwaSIsInJlYWxtLW1hbmFnZW1lbnQiXSwic3ViIjoiMTJjNWY0ODktMDJkZC00ZjkxLWFhYzktZGUxMjFiNjMzZmU4IiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiY3VzdG9tZXItYXBpIiwibm9uY2UiOiI2Mzg1MDg5MzI4NjE5MDUzMzMuTnpnME1tWTFZVEV0TldVMk1TMDBZamN4TFdFeFlqY3RaVE5qWWpRelpUVXpOekU1TkdKaFpUZ3pZakV0TldFMVppMDBaakU1TFRoak5EY3RZalZtTWpVM01ERTBaalppIiwic2Vzc2lvbl9zdGF0ZSI6ImNkNTRjY2QyLTFhOTAtNGVjZi1hOGU4LTI5NTk4N2FjN2JkZSIsImFjciI6IjAiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDo1MDgwIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NTQ0MyJdLCJyZXNvdXJjZV9hY2Nlc3MiOnsicmVhbG0tbWFuYWdlbWVudCI6eyJyb2xlcyI6WyJtYW5hZ2UtdXNlcnMiLCJ2aWV3LXVzZXJzIiwicXVlcnktZ3JvdXBzIiwicXVlcnktdXNlcnMiXX0sImN1c3RvbWVyLWFwaSI6eyJyb2xlcyI6WyJBZG1pbiJdfX0sInNjb3BlIjoib3BlbmlkIGVtYWlsIHByb2ZpbGUiLCJzaWQiOiJjZDU0Y2NkMi0xYTkwLTRlY2YtYThlOC0yOTU5ODdhYzdiZGUiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwibmFtZSI6IkFkbWluIEN1c3RvbWVyIiwicHJlZmVycmVkX3VzZXJuYW1lIjoiYWRtaW5AdHJpYnV0ZWNoLmlvIiwiZ2l2ZW5fbmFtZSI6IkFkbWluIiwiZmFtaWx5X25hbWUiOiJDdXN0b21lciIsImVtYWlsIjoiYWRtaW5AdHJpYnV0ZWNoLmlvIn0.prRqvOOIijnPPE7xOYtpcMZTiXvbLi9CVT0_DRXB5ZBK3T9AvgEAfdP-VZ5xEMBrZ7tGdxkwXjqTz2Faw_bbkews-w5TFzfYB6UGbZWs0NktyzkkMRzpkjE19SR0JtzfBW6fan6dOrnYd5g7-WK3hLEv0mSLRrukOdIt10FswNjFdxNs6QJBjtAk7Bt7iot21tZvKyEUKyQN3w8dU6FBegw_NVuhY1warAdqt2rSM12N80npfzxbOgvJgdG_ngkZMCBSigtpxQRqBI6axhEyVsDHrVaPLtgBMN8ZJloUpLougPmieDS9LbLMEoPOQpsSkOnqObZHG0vQu7tH-wH4Ew";
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = token.Claims;

        var roles = claims
            .Where(c => c.Type == "resource_access")
            //.Select(c => c.Value)
            .ToList();
    }
}