using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace Linkplay.ClientApp.RestApiClient;

public interface IRestApiClient
{
    Task<Result> GetAsync(string url, CancellationToken cancellationToken = default);
    Task<Result<T?>> GetAsync<T>(string url, CancellationToken cancellationToken = default);
}

public class RestApiClient : IRestApiClient
{
    private readonly HttpClient _httpClient;

    public RestApiClient(  IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("WiimApiClient");
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<Result> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
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