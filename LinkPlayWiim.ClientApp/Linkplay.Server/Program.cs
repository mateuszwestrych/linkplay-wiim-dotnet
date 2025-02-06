using Linkplay.ClientApp;
using Linkplay.ClientApp.DeviceCatalogs;
using Linkplay.ClientApp.RestApiClient;
using Linkplay.Server.DeviceDiscovery;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddSingleton<DeviceCatalog>();
builder.Services.AddSingleton<IDeviceCatalogViewer>(p=>p.GetRequiredService<DeviceCatalog>());
builder.Services.AddSingleton<IDeviceCatalogRegistrer>(p=>p.GetRequiredService<DeviceCatalog>());

builder.Services.AddTransient<IDeviceConnector, DeviceConnector>();
builder.Services.AddHostedService<DeviceNetworkDiscoveryService>();

builder.Services.AddLinkPlayClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/devices/list", (IDeviceCatalogViewer deviceCatalogViewer) =>
    {
        return deviceCatalogViewer.GetDevices();
    })
    .WithName("GetDevices");

app.Run();