using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Linkplay.ClientApp;
using Linkplay.ClientApp.DeviceCatalogs;
using Linkplay.ClientApp.RestApiClient;
using Linkplay.Server;
using Linkplay.Server.DeviceDiscovery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DeviceCatalog>();
builder.Services.AddSingleton<IDeviceCatalogViewer>(p=>p.GetRequiredService<DeviceCatalog>());
builder.Services.AddSingleton<IDeviceCatalogRegistrer>(p=>p.GetRequiredService<DeviceCatalog>());
builder.Services.AddSingleton<IDeviceCatalogFinder>(p=>p.GetRequiredService<DeviceCatalog>());

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

// Endpoints

app.MapGet("/devices", (IDeviceCatalogViewer catalogViewer) => catalogViewer.GetDevices())
    .WithName("GetDevices");

app.MapPost("/devices/{deviceId}/play", async (IDeviceCatalogFinder deviceFinder,
    IDeviceConnector deviceConnector,
    [FromRoute] string deviceId,
    [FromBody] PlayUrlRequest request,
    CancellationToken cancellationToken) =>
{
    var finalUrl = request.Url;
    
    string ExtractFileId(string url)
    {
        var match = Regex.Match(url, @"drive\.google\.com/file/d/([^/]+)/");
        return match.Success ? match.Groups[1].Value : null;
    }
    
    if (request.Url.Contains("drive") && request.Url.Contains("google.com"))
    {       
        var fileId = ExtractFileId(request.Url);
        finalUrl = $"https://drive.google.com/uc?export=download&id={fileId}";
    }
    
    var result = await deviceFinder
        .TryGetDevice(deviceId)
        .Bind(async device => await device.PlayUrlAsync(finalUrl, deviceConnector, cancellationToken));

    return result.IsSuccess ? Results.Ok() : Results.BadRequest();
}).WithName("PlayUrlOnDevice");

app.Run();