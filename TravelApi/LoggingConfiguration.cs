using Serilog;
using Serilog.Core;

namespace TravelApi
{
    public static class LoggingConfiguration
    {
        private static Microsoft.Extensions.Logging.ILogger _logger;
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            // instantiate and configure logging. Using serilog here, to log to console and a text-file.
            LoggerFactory loggerFactory = new LoggerFactory();
            Logger loggerConfig = new LoggerConfiguration()
                //.WriteTo.Console()
                .WriteTo.File("logs\\travel_api.log", rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            loggerFactory.AddSerilog(loggerConfig);

            // create logger and put it to work.
            var logProvider = loggerFactory.CreateLogger("TravelApI");

            _logger = logProvider;
            services.AddSingleton<ILoggerFactory>(loggerFactory);
            services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(logProvider);

            return services;
        }

        public static Microsoft.Extensions.Logging.ILogger GetLogger()
        {
            return _logger;
        }
    }
}
