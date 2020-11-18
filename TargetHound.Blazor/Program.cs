namespace TargetHound.Blazor
{
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Services;
    using TargetHound.Services.Interfaces;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<IProjectService, ProjectService>();

            await builder.Build().RunAsync();
        }
    }
}
