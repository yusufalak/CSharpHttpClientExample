using Commons.Components;
using Commons.ExceptionService;
using Commons.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json.Serialization;



namespace CSharpHttpClientExample
{
    public class Startup
    {
        private static readonly string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:ss";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter(DATE_FORMAT));
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter2(DATE_FORMAT));
            });

            services.AddHttpClient(Configuration);

            services.AddSingleton(factory => log4net.LogManager.GetLogger(GetType()));

            services.TryAddSingleton<IExceptionService, ExceptionService>();

        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseSubChannelExceptionHandler();
            app.UseSubChannelHttpRequestMiddleware();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with url endpoints must be made through a url client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

        }
    }
}
