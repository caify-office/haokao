using Girvs.AuthorizePermission.Extensions;
using Girvs.DynamicWebApi;
using Girvs.Infrastructure.Extensions;
using HaoKao.CorrectionNotebookService.Application.Workers;
using HaoKao.CorrectionNotebookService.Domain.Services;
using HaoKao.CorrectionNotebookService.Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;

namespace HaoKao.CorrectionNotebookService.WebApi;

public class Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
{
    public IConfiguration Configuration => configuration;

    public IWebHostEnvironment WebHostEnvironment => webHostEnvironment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
        services.AddControllersWithAuthorizePermissionFilter(options => options.Filters.Add<GirvsModelStateInvalidFilter>());
        // .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
        services.ConfigureApplicationServices(Configuration, WebHostEnvironment);

        services.AddScoped<ILargeLanguageModel, QianFanAppBuilder>();
        services.AddScoped<ILargeLanguageModel, QianFanLLM>();
        services.AddScoped<ILargeLanguageModel, QwenLLM>();
        services.AddScoped<IImageEnhancement, TencentCloudImageService>();
        services.AddScoped<IImageEnhancement, TextInImageService>();
        services.AddScoped<IImageDemoire, TextInImageService>();
        services.AddScoped<IImageDewarp, TextInImageService>();
        services.AddScoped<IOcrService, AliyunOcrService>();
        services.AddScoped<IImageRecognition, AliyunOcrService>();

        services.AddScoped(typeof(IServiceFactory<>), typeof(ServiceFactory<>));

        services.AddSingleton<IQuestionQueue, QuestionQueue>();
        services.AddHostedService<QuestionHostedService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseGirvsExceptionHandler();
        app.UseRouting();
        app.ConfigureRequestPipeline();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.ConfigureEndpointRouteBuilder();
        });
    }
}