using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppMetadataAPIServer.Controllers;
using AppMetadataAPIServer.InputFormatters;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Parser;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.RequestProcessors;
using AppMetadataAPIServer.Storage;
using AppMetadataAPIServer.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AppMetadataAPIServer
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
            services.AddControllers(_ =>
                _.InputFormatters.Insert(_.InputFormatters.Count, new TextPlainInputFormatter()));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AppMetadataAPIServer", Version = "v1"});
            });

            services.AddSingleton<IPayloadValidator<ApplicationMetadata>, ApplicationMetadataValidator>()
                .AddSingleton<MetadataRequestProcessor, MetadataRequestProcessor>()
                .AddSingleton<IPayloadParser, YamlPayloadParser>()
                .AddSingleton<ICobraSearch<ApplicationMetadataKey,ApplicationMetadata>, CobraSearchEngine>()
                .AddSingleton<ICobraDB<ApplicationMetadataKey, ApplicationMetadata>, CobraDB>()
                .AddSingleton<IQueryContextBuilder, QueryContextBuilder>()
                .AddSingleton<IQueryExecutor, QueryExecutor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppMetadataAPIServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}