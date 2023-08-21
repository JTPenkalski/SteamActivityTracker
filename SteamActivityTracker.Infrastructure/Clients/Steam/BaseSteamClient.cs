using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamActivityTracker.Core.Options;
using SteamActivityTracker.Infrastructure.Clients.Steam.Models;
using System.Net.Http.Json;

namespace SteamActivityTracker.Infrastructure.Clients.Steam;

public abstract class BaseSteamClient
{
    protected readonly SteamClientOptions config;
    protected readonly ILogger logger;
    protected readonly IHttpClientFactory httpClientFactory;

    public abstract string Interface { get; }

    public BaseSteamClient(
        IOptions<SteamClientOptions> config,
        ILogger logger,
        IHttpClientFactory httpClientFactory
    )
    {
        this.config = config.Value;
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Helper function to make GET requests from a child client.
    /// </summary>
    /// <param name="request">Request information.</param>
    /// <returns>The specified object model, or null if the request failed or didn't return anything.</returns>
    protected virtual async Task<T?> GetAsync<T>(SteamRequest request) where T : SteamClientModel
    {
        T? result = default;

        try
        {
            ValidateRequest(request);

            string requestRoute = await request.WithKey(config.Key).BuildUriAsync();
            string requestUri = $"{config.BaseUrl}{requestRoute}";

            using HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            // For some reason the Steam API inconsistently wraps its responses in an extra layer of JSON
            result = request.WrapResponse
                ? (await response.Content.ReadFromJsonAsync<SteamResponse<T>>())?.Response
                : await response.Content.ReadFromJsonAsync<T>();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "An internal error occurred!");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "An external error occurred!");
        }

        return result;
    }

    private void ValidateRequest(SteamRequest request)
    {
        if (string.IsNullOrWhiteSpace(config.BaseUrl))
            throw new InvalidOperationException($"The Base URL for a Steam Web API request was invalid. Base URL = {config.BaseUrl}");

        request.Validate();
    }
}