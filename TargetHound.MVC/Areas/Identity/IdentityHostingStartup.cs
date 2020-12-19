using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TargetHound.MVC.Areas.Identity.IdentityHostingStartup))]

namespace TargetHound.MVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}