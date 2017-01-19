using Framework.Core;
using Framework.Core.Configuration;
using Framework.Core.DAL.Infrastructure;
using Framework.Core.Extensions;
using Framework.Core.DAL.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Sample.DotNetFramework.MVC6
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = ConfigurationManager.Configure(env.ContentRootPath);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            
            services.AddDbContext<DatabaseContext>(
                "ConnectionStrings:DefaultConnection",
                GetType().Namespace);

            services.Initialize();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(string.Format("{0}-{1}.txt", Path.Combine(env.WebRootPath, env.ApplicationName), "{Date}"), LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            /* If you want to use EF code first on startup, add the code below
             */
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //serviceScope.ServiceProvider.GetService<DatabaseContext>().MigrateDatabaseAndSeedData("D:\\SqlScripts");
                serviceScope.ServiceProvider.GetService<DatabaseContext>().MigrateDatabase();
            }
        }
    }
}
