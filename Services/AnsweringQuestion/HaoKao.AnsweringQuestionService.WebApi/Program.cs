using Girvs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HaoKao.AnsweringQuestionService.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GirvsHostBuilderManager.CreateGrivsHostBuilder<Startup>(args).UseDefaultServiceProvider(options =>
            {
                //we don't validate the scopes, since at the app start and the initial configuration we need
                //to resolve some services (registered as "scoped") through the root container
                options.ValidateScopes = false;
                options.ValidateOnBuild = true;
            }).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}