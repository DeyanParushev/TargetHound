namespace TargetHound.MVC
{
    using System.IO;
    using System.Reflection;
    using System.Text.Json;
    
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using SendgridEmailInAspNetCore.Services;
   
    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.DTOs;
    using TargetHound.MVC.Models;
    using TargetHound.Services;
    using TargetHound.Services.Automapper;
    using TargetHound.Services.Interfaces;
    using TargetHound.Services.Messages;
    using TargetHound.SharedViewModels.ViewModels;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TargetHoundContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), 
                ServiceLifetime.Singleton);

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
               .AddRoles<ApplicationRole>()
               .AddEntityFrameworkStores<TargetHoundContext>();

            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
                .AddRazorRuntimeCompilation();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = new JsonSerializerOptions().PropertyNameCaseInsensitive;
            });

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<IClientInvitationsService, ClientInvitationsService>();
            services.AddTransient<IContractorService, ContractorService>();
            services.AddTransient<ICollarService, CollarService>();
            services.AddTransient<ITargetService, TargetService>();
            services.AddTransient<IBoreholeService, BoreholeService>();

            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ProjectViewModel).GetTypeInfo().Assembly,
                typeof(ProjectDTO).GetTypeInfo().Assembly);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseBlazorFrameworkFiles();

            app.UseRouting();

            app.UseResponseCaching();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "FilesCSV")),
                RequestPath = "/ExportCSV"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
