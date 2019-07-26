﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;

namespace BirdyAPI
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
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("Birdy", new Info
                {
                    Title = "Birdy API",
                    Version = "0.0.1"
                });
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                configuration.IncludeXmlComments(xmlPath);
            });

            Configurations.SendGridApiKey = Configuration.GetConnectionString("SendGrid");
            Configurations.BlobStorageApiKey = Configuration.GetConnectionString("BlobStorage");

            services.AddDbContext<BirdyContext>(options =>
                options.UseSqlServer("Server=tcp:birdytest.database.windows.net,1433;Initial Catalog=BirdyDB;Persist Security Info=False;User ID=lol67;Password=Lolilop67;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" /*Configuration.GetConnectionString("AzureDbServer")*/));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(configuration =>
                configuration.SwaggerEndpoint("/swagger/Birdy/swagger.json", "Birdy API"));
            app.UseMvc();
        }
    }
}
