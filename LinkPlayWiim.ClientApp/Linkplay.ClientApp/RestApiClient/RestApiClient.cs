using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Linkplay.ClientApp.RestApiClient;

public interface IRestApiClient
{
    Task<Result> GetAsync(string url, CancellationToken cancellationToken = default);
    Task<Result<T?>> GetAsync<T>(string url, CancellationToken cancellationToken = default);
}

public class RestApiClient : IRestApiClient
{
    private readonly ILogger<RestApiClient> _logger;
    public const string ClientName = "LinkplayRestApiClient";
    
    private readonly HttpClient _httpClient;
    
    

    public RestApiClient(IHttpClientFactory httpClientFactory, ILogger<RestApiClient> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(ClientName);
    }

    public async Task<Result> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogTrace("GET URL: {0}", url);
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return Result.Success();
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<T?>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogTrace("GET URL: {0}", url);
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
        
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return Result.Success(JsonConvert.DeserializeObject<T>(content!));
        }
        catch (Exception ex)
        {
            return Result.Failure<T?>(ex.Message);
        }
    }
}