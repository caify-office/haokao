using ShortUrlService.Domain.Charts;
using ShortUrlService.WebApi.Configurations;
using ShortUrlService.WebApi.Services;
using ShortUrlService.WebApi.Shorteners;

namespace ShortUrlService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
{
    public IConfiguration Configuration => configuration;

    public IWebHostEnvironment WebHostEnvironment => webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>())
                .AddJsonOptions(ops => ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);

        services.Configure<ShortUrlConfig>(Configuration.GetSection("ModuleConfigurations:ShortUrlConfig"));

        services.AddTransient(typeof(Base62Shortener));
        services.AddSingleton<IAccessLogQueue, AccessLogQueue>();
        services.AddHostedService<AccessLogHostedService>();

        services.AddGrpc().AddJsonTranscoding();

        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(Configuration.GetSection("ModuleConfigurations:IpRateLimitingConfig"));
        services.Configure<IpRateLimitPolicies>(Configuration.GetSection("ModuleConfigurations:IpRateLimitPoliciesConfig"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        //services.PostConfigure<LoggerConfiguration>(config =>
        //{
        //    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        //    var elasticSearchConfig = Configuration.GetValue<ElasticSearchConfig>("ModuleConfigurations:ElasticSearchConfig") ?? new();
        //    if (elasticSearchConfig.Enable)
        //    {
        //        config.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticSearchConfig.Uri)
        //        {
        //            AutoRegisterTemplate = elasticSearchConfig.AutoRegisterTemplate,
        //            IndexFormat = $"{elasticSearchConfig.IndexFormatPrefix}-{environment.ToLower()}-{DateTime.Now:yyyy-MM-dd}",
        //            NumberOfReplicas = elasticSearchConfig.NumberOfReplicas,
        //            NumberOfShards = elasticSearchConfig.NumberOfShards,
        //        });
        //    }
        //});

        services.AddSingleton(new IdWorker(1, 1));
        services.AddScoped<IShortUrlChart, DayShortUrlChart>();
        services.AddScoped<IShortUrlChart, WeekShortUrlChart>();
        services.AddScoped<IShortUrlChart, MonthShortUrlChart>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        });

        app.UseGirvsExceptionHandler();

        app.UseRouting();

        app.ConfigureRequestPipeline();

        app.UseIpRateLimiting();

        app.UseGrpcWeb();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
            endpoints.MapGrpcService<ShortUrlGrpcService>().EnableGrpcWeb();
        });
    }
}