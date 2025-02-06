using Linkplay.ClientApp.RestApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace Linkplay.ClientApp;

public static class Registry
{
    public static void AddLinkPlayClient(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IRestApiClient, RestApiClient.RestApiClient>();
    }
}