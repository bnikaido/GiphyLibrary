using GiphyLibrary.Controllers;
using GiphyLibrary.Data;
using GiphyLibrary.Domain;
using GiphyLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace GiphyLibrary
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Setup configuration and dependency injection
            services
                .AddOptions()
                .Configure<GiphyClientConfiguration>(Configuration.GetSection("GiphyClientConfiguration"))
                .AddTransient(s => s.GetRequiredService<IOptions<GiphyClientConfiguration>>().Value)
                .AddSingleton<IGiphyClient, GiphyClient>();

            // Setup database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddSingleton<CloudBlobClient>(_ =>
                {
                    var connectionString = Configuration.GetConnectionString("AzureStorageAccountConnection");
                    if (!CloudStorageAccount.TryParse(connectionString, out var storageAccount))
                    {
                        throw new ConfigurationErrorsException("Unable to parse Azure storage connection string");
                    }
                    
                    return storageAccount.CreateCloudBlobClient();
                })
                .AddSingleton<IBlobStorageQuery, BlobStorageQuery>();

            // Setup authentication
            services
                .AddAuthentication()
                .AddIdentityServerJwt();

            services
                .AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            // Setup user interface
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
