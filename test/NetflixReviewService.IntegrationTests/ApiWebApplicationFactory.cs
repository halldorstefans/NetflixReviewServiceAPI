using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetflixReviewService.Services;
using NetflixReviewService.Services.Interfaces;

namespace NetflixReviewService.IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public IConfiguration Configuration { get; private set; }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>        
            {            
                Configuration = new ConfigurationBuilder().Build();
                config.AddConfiguration(Configuration);        
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<IShowService, NetflixShowService>();
                services.AddScoped<ITokenService, NetflixTokenService>();
            });
        }
    }
}