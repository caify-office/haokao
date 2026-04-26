using System;
using System.IO;
using System.Linq;
using Girvs;
using Girvs.Configuration;
using Girvs.Infrastructure;
using Girvs.Infrastructure.Extensions;
using HaoKao.YarpGateway.Services;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HaoKao.YarpGateway;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment):IGirvsStartup
{
    public IConfiguration Configuration { get; } = configuration;

    public IWebHostEnvironment WebHostEnvironment { get; } = webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
               "v1",
               new OpenApiInfo { Title = AppDomain.CurrentDomain.FriendlyName, Version = "v1" }
           );
            // TODO:һ  Ҫ    true
            c.DocInclusionPredicate((docName, description) => true);

            c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer"
            });

            var basePath = Path.GetDirectoryName(typeof(Startup).Assembly.Location) ??
                           string.Empty;

            var xmlFiles = Directory.GetFiles(basePath, "*.xml");
            foreach (var xmlFile in xmlFiles)
            {
                c.IncludeXmlComments(xmlFile);
            }

            c.AddServer(new OpenApiServer { Url = "/", Description = "Yarp Api Gateway Swagger" });

            c.CustomOperationIds(apiDesc =>
                {
                    if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerAction) return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                    return string.Empty;
                }
            );
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        services.AddCors(options =>
        {
            // this defines a CORS policy called "default"
            options.AddPolicy("default", policy =>
            {
                policy.SetIsOriginAllowed(origin => true)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseGirvsExceptionHandler();
        app.UseCors("default");

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.ConfigObject.Urls = new SwaggerEndpointEnumerator();
        });
       

        app.UseRouting();

        app.ConfigureRequestPipeline(env);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/",
                             async context => { await context.Response.WriteAsync("Welcome to ScsApiGateway!"); });
            endpoints.MapSwagger("{documentName}/api-docs");
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}