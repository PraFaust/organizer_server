using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using organizer_server.Models;                 // пространство имен моделей
using Microsoft.EntityFrameworkCore;        // пространство имен EntityFramework
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace organizer_server
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://clientorganizer.azurewebsites.net")
                                        .AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
            });

            string connection = Configuration.GetConnectionString("DBConnection");
            services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=login}/{loginData?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=register}/{registrationData?}");
            });
            // for API controller routes
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
