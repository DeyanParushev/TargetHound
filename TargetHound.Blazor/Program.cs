namespace TargetHound.Blazor
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    
    using TargetHound.Blazor.Services;
    using TargetHound.Calcualtions;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient(
                "TargetHound",
                    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddApiAuthorization();
            builder.Services.AddSingleton<IStateService, StateService>();
            builder.Services.AddTransient<AngleConverter>();
            builder.Services.AddTransient<StraightExtrapolationCalculator>();
            builder.Services.AddTransient<CurveExtrapolationCalculator>();
            builder.Services.AddTransient<PlaneDistanceCalculator>();
            builder.Services.AddTransient<_3DDistanceCalculator>();
            builder.Services.AddTransient<CoordinatesSetter>();
            builder.Services.AddTransient<CurveCalculator>();
            builder.Services.AddTransient<Extrapolator>();
            await builder.Build().RunAsync();
        }
    }
}
