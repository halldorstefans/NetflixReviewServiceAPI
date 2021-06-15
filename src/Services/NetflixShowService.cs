using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NetflixReviewService.Models;
using System.Text.Json;
using NetflixReviewService.Services.Interfaces;
using Microsoft.Extensions.Options;
using NetflixReviewService.Helpers;

namespace NetflixReviewService.Services
{
    public class NetflixShowService : IShowService
    {
        private readonly HttpClient client;
        private readonly ITokenService tokenService;
        private readonly string showsUrl;
        public NetflixShowService(ITokenService tokenService, IOptions<NetflixSettings> netflixSettings, HttpClient httpClient)
        {
            this.tokenService = tokenService;
            this.showsUrl = netflixSettings.Value.AppBaseUrl + "Shows/";
            this.client = httpClient;
        }

        public async Task<NetflixData> GetShows()
        {
            try
            {
                var shows = new NetflixData();

                var token = await tokenService.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.GetAsync(showsUrl);
                
                if(res.IsSuccessStatusCode)
                {
                    var json = res.Content.ReadAsStringAsync().Result;
                    shows = JsonSerializer.Deserialize<NetflixData>(json);
                }

                return shows;
            }
            catch
            {
                //Log
                throw;
            }
        }

        public async Task<ShowModel> GetShow(string id)
        {
            try
            {
                var show = new ShowModel();
                var token = await tokenService.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.GetAsync(showsUrl + id);
                
                if(res.IsSuccessStatusCode)
                {
                    var json = res.Content.ReadAsStringAsync().Result;
                    show = JsonSerializer.Deserialize<ShowModel>(json);
                }

                return show;
            }
            catch
            {
                //Log
                throw;
            }
        }
    }
}
