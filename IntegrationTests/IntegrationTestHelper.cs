using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Tributech.SensorManager.Api;

namespace IntegrationTests;

internal class IntegrationTestHelper
{
    public static ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                // use in-memory database
                new("ConnectionStrings:DefaultConnection", "DataSource=:memory:"),
                new("UseInMemoryDatabase", "true")
            ])
            .Build();

        Program.ConfigureServices(services, configuration);
        var sp = services.BuildServiceProvider();
        return sp;
    }
}