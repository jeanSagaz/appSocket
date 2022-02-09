using appSignalRApi.Hubs;
using appSignalRApi.Hubs.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appSignalRApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "appSignalRApi", Version = "v1" });
            });

            services.AddSignalR();
            services.AddCors(options =>
            {
                //options.AddDefaultPolicy(builder =>
                //{
                //    builder.WithOrigins("file:///C:/Users/Usuario/Desktop/socket/signalR/index.html")
                //        .AllowCredentials();
                //});

                options.AddPolicy(name: "CorsPolicy",
                              builder =>
                              {
                                  builder.WithOrigins("*")
                                       .AllowAnyMethod()
                                       .AllowAnyHeader();
                                       //.AllowCredentials();
                              });
            });
            
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddSingleton<IHubNotificationHelper, HubNotificationHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "appSignalRApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");
            //app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notifications");
                endpoints.MapControllers();
            });
        }
    }
}
