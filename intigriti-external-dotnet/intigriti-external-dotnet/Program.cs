using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using IdentityModel.Client;

Console.Write("Client ID: ");
var clientId = Console.ReadLine();
Console.Write("Client SECRET: ");
var clientSecret = Console.ReadLine();
var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://login.intigriti.com");
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
});
const string url = "https://api.intigriti.com/external/v1.2/submissions";
var apiRequest = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri(url),
    Headers = {{"Authorization", $"Bearer {tokenResponse.AccessToken}"}}
};
var apiResponse = await client.SendAsync(apiRequest);
var apiResponseContent = await apiResponse.Content.ReadAsStringAsync();

Console.WriteLine("");
Console.WriteLine("API call succeeded!");
Console.WriteLine("All your submissions:");
Console.WriteLine("");
Console.WriteLine(FormatJsonText(apiResponseContent));

// Helper to pretty format json
static string FormatJsonText(string jsonString)
{
    using var doc = JsonDocument.Parse(jsonString, new JsonDocumentOptions {AllowTrailingCommas = true});

    var memoryStream = new MemoryStream();
    using (var utf8JsonWriter = new Utf8JsonWriter(memoryStream, new JsonWriterOptions {Indented = true}))
    {
        doc.WriteTo(utf8JsonWriter);
    }

    return new System.Text.UTF8Encoding().GetString(memoryStream.ToArray());
}