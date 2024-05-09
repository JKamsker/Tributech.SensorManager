using FluentValidation;

using System.Globalization;
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}