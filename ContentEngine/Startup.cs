﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Microsoft.AspNet.Diagnostics;

namespace ContentEngine
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure json output serialisation to convert all enums to strings, not output their integer values
            services.AddMvc().AddJsonOptions(o => {
                o.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            });

            services.AddApiVersioning();

            RegisterDependencies(services);
        }

        // Configure solution wide DI custom classes
        private void RegisterDependencies(IServiceCollection services) {
            services.AddSingleton(Configuration);
            RegisterGenericPersistence(services);
            RegisterAzureTableAsPersistenceProvider(services);
        }

        private void RegisterGenericPersistence(IServiceCollection services)
        {
            services.AddSingleton<Persistence.IDataUpdater, Persistence.JsonDataUpdater>();
        }

        private void RegisterAzureTableAsPersistenceProvider(IServiceCollection services)
        {
            services.AddSingleton<Persistence.Azure.ITableStorage, Persistence.Azure.Implementation.TableStorage>();
            services.AddSingleton<Persistence.Azure.ILinkReader, Persistence.Azure.Implementation.LinkReader>();
            services.AddSingleton<Persistence.IContentReader, Persistence.Azure.Implementation.ContentReader>();
            services.AddSingleton<Persistence.IContentWriter, Persistence.Azure.Implementation.ContentWriter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, new SwaggerUiOwinSettings() { });
            }

            app.UseMvc();
        }
    }
}
