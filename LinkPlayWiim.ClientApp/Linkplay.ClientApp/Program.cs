using Linkplay.ClientApp;
using Linkplay.ClientApp.RestApiClient;

Console.WriteLine("Linkplay/Wiim ClientApp");

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IClientStorage, ClientStorage>();
builder.Services.AddHostedService<DeviceNetworkDiscoveryService>();

var host = builder.Build();
host.Run();

