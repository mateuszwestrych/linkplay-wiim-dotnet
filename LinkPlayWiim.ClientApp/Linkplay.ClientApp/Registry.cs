using Linkplay.ClientApp.RestApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace Linkplay.ClientApp;

public static class Registry
{
    public static void AddLinkPlayClient(this IServiceCollection services)
    {
        services
            .AddHttpClient(RestApiClient.RestApiClient.ClientName, client =>
            {
                client.Timeout = TimeSpan.FromSeconds(5);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                // Allowing Untrusted SSL Certificates
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
                return handler;
            });
        
        
        services.AddTransient<IRestApiClient, RestApiClient.RestApiClient>();
    }
}