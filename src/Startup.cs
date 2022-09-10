using Dapper.FluentMap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UsersAuthExample.Extensions.DependencyInjections;
using UsersAuthExample.Mappings.Dapper;

namespace UsersAuthExample
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
            FluentMapper.Initialize(config => { 
                config.AddMap(new UserDtoMap()); 
            });

            services.AddMvcCore().AddApiExplorer();

            services.RegisterDependency();
            services.AddApiVersioning();

            services.AddControllers();

            //Add ApiVersion in swagger
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            //services.AddSwaggerGen(options =>
            //{
            //    options.IncludeXmlComments(xmlFilePath);
            //});


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UsersAuthExample", Version = "v1" });
            });
            //services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsersAuthExample v1"));

                //app.UseSwagger(options =>
                //{
                //    options.PreSerializeFilters.Add((swagger, req) =>
                //    {
                //        swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://{req.Host}" } };
                //    });
                //});

                //app.UseSwaggerUI(options =>
                //{
                //    foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
                //    {
                //        options.SwaggerEndpoint($"../swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
                //        options.DefaultModelsExpandDepth(-1);
                //        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                //    }
                //});
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerConfigureOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = "My Test API",
                    Version = desc.ApiVersion.ToString(),
                });
            }
        }
    }

}
