namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

/// <summary>
/// Contains all necessary data to make a Steam Web API request.
/// </summary>
public class SteamRequest
{
    public const string VERSION_1 = "v0001";
    public const string VERSION_2 = "v0002";
    public const string QUERY_PARAM_KEY = "key";

    public required string Interface { get; init; }

    public required string Endpoint { get; init; }

    public string Version { get; init; } = VERSION_1;

    public IDictionary<string, string> Query { get; init; } = new Dictionary<string, string>();

    public bool WrapResponse { get; init; } = true;

    /// <summary>
    /// Converts this request into a string format to make an API request.
    /// Does not include Base Url.
    /// </summary>
    /// <returns>A string representation of this request.</returns>
    public async Task<string> BuildUriAsync()
    {
        string requestUri = $"{Interface}/{Endpoint}/{Version}/";

        if (Query.Any())
        {
            using HttpContent content = new FormUrlEncodedContent(Query);
            string query = await content.ReadAsStringAsync();

            requestUri += $"?{query}";
        }

        return requestUri;
    }

    /// <summary>
    /// Appends the API key query string to the URI.
    /// </summary>
    /// <param name="apiKey">The key required for most Steam API calls.</param>
    /// <returns>This <see cref="SteamRequest"/> instance.</returns>
    public SteamRequest WithKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException($"The API Key for a Steam Web API request was invalid. Key = {apiKey}");

        Query.Add(QUERY_PARAM_KEY, apiKey);

        return this;
    }

    /// <summary>
    /// Builds a query string for the URI.
    /// </summary>
    /// <param name="query">The key-value pairs to use in the query string.</param>
    /// <returns>This <see cref="SteamRequest"/> instance.</returns>
    public SteamRequest WithQuery(params (string key, string value)[] query)
    {
        foreach (var (key, value) in query)
        {
            Query.Add(key, value);
        }

        return this;
    }

    /// <summary>
    /// Validates this request for valid inputs.
    /// </summary>
    /// <returns>True, if successful. Errors will be thrown otherwise.</returns>
    /// <exception cref="InvalidOperationException" />
    public bool Validate()
    {
        var testProperties = new (string name, string value, Func<string, bool> validation)[]
        {
            (nameof(Interface), Interface, string.IsNullOrWhiteSpace),
            (nameof(Endpoint), Endpoint, string.IsNullOrWhiteSpace),
            (nameof(Version), Version, x => x == "v1" || x == "v2")
        };

        foreach (var (name, value, validation) in testProperties)
        {
            if (validation(value))
                throw new InvalidOperationException($"The {name} for a Steam Web API request was invalid. {name} = {value}");
        }

        return true;
    }
}
