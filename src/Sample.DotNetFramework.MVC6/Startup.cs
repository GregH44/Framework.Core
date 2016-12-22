using Framework.Core;
using Framework.Core.Attributes;
using Framework.Core.Configuration;
using Framework.Core.Extensions;
using Framework.Core.DAL.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.DotNetFramework.MVC6.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

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
            services.AddMvc();

            var model = new GenericModelBuilder(Configuration).InitializeDataModels();
            services.AddDbContext(
                Configuration["ConnectionStrings:DefaultConnection"],
                GetType().Namespace,
                model);

            // Exception handler service
            services.AddScoped<ExceptionHandlerAttribute>();
            // CorrelationId manager service
            services.AddScoped<CorrelationIdAttribute>();

            // Application services
            ConfigureAppServices.Configure(ref services);

            // Configuration service
            services.AddSingleton(typeof(IConfigurationRoot), imp => Configuration);

            FrameworkManager.Initialize(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile(string.Format("{0}-{1}.txt", Path.Combine(env.WebRootPath, env.ApplicationName), "{Date}"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                loggerFactory.AddDebug();
                app.UseExceptionHandler("/Transverse/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });

            using (var context = app.ApplicationServices.GetService<DataBaseContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}
