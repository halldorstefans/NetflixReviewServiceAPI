using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetflixReviewService.Services.Interfaces;
using NetflixReviewService.Helpers;

namespace NetflixReviewService.Services
{
  public class NetflixTokenService : ITokenService
  {
    private readonly HttpClient client;
    private NetflixToken token = new NetflixToken();
    private readonly IOptions<NetflixSettings> netflixSettings;

    public NetflixTokenService(IOptions<NetflixSettings> netflixSettings, HttpClient httpClient)
    {
        this.netflixSettings = netflixSettings;
        this.client = httpClient;
    }

    public async Task<string> GetToken()
    {
        this.token = await this.GetNewAccessToken();
    
        return token.AccessToken;
    }

    private async Task<NetflixToken> GetNewAccessToken()
    {
        var token = new NetflixToken();

        var client_id = netflixSettings.Value.ClientId;
        var client_secret = netflixSettings.Value.ClientSecret;
        var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

        var postMessage = new Dictionary<string, string>();
        postMessage.Add("grant_type", "client_credentials");
        postMessage.Add("scope", netflixSettings.Value.Scope);

        var request = new HttpRequestMessage(HttpMethod.Post, netflixSettings.Value.TokenUrl)
        {
            Content = new FormUrlEncodedContent(postMessage)
        };

        var response = await client.SendAsync(request);
        if(response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            token = JsonSerializer.Deserialize<NetflixToken>(json);
        }
        else
        {
            //Log
            throw new ApplicationException("Unable to retrieve access token from Netflix");
        }

        return token;
    }

    private class NetflixToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
  }
}
