using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Something.Application;
using Something.Domain;
using Something.Persistence;

namespace Something.API
{
    public class Startup
    {
        readonly string DevSomething3AllowSpecificOrigins = "_devSomething3AllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: DevSomething3AllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:44380")
                                      .AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Custom.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });
            services.AddAuthorization();
            services.AddDbContext<AppDbContext>(
                options => options.UseInMemoryDatabase(nameof(Something.API))
                );
            services.AddSingleton<ISomethingFactory, SomethingFactory>();
            services.AddScoped<ISomethingCreateInteractor, SomethingCreateInteractor>();
            services.AddScoped<ISomethingReadInteractor, SomethingReadInteractor>();
            services.AddScoped<ISomethingPersistence, SomethingPersistence>();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseCors(DevSomething3AllowSpecificOrigins);
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
