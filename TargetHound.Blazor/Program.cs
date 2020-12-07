namespace TargetHound.Blazor
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TargetHound.Calcualtions;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddTransient<AngleConverter>();
            builder.Services.AddTransient<StraightExtrapolationCalculator>();
            builder.Services.AddTransient<CurveExtrapolationCalculator>();
            builder.Services.AddTransient<PlaneDistanceCalculator>();
            builder.Services.AddTransient<_3DDistanceCalculator>();
            builder.Services.AddTransient<CoordinatesSetter>();
            builder.Services.AddTransient<CurveCalculator>();
            await builder.Build().RunAsync();
        }
    }
}
