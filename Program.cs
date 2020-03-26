using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.AspNetCore.Components;

namespace SyscafeAppPwa
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                       builder => builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());

                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("https://localhost:44309/").WithHeaders("*").WithMethods("GET, POST, PUT, DELETE, OPTIONS"));
            });
            builder.Services.AddBaseAddressHttpClient();

            builder.Services.AddScoped<HttpClient>(s =>
                {
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri)
                    };
                });
            

            await builder.Build().RunAsync();
        }
    }
}
