using DanceCup.Api.Domain;

namespace DanceCup.Api;

public class Startup : IStartup
{
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        
        throw new NotImplementedException();
    }

    public void Configure(IApplicationBuilder app)
    {
        throw new NotImplementedException();
    }
}