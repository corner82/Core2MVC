using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitLogSubs.Entities.Log;
using RabbitLogSubs.Session;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;

namespace RabbitLogSubs
{
    class Program
    {

        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run app
            serviceProvider.GetService<App>().Run();  
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(AppContext.BaseDirectory)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add console logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole(configuration.GetSection("Logging"))
                .AddSerilog()
                .AddDebug());
            serviceCollection.AddLogging();

            // Add Serilog logging           
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.RollingFile(configuration["Serilog:LogFile"])
            .CreateLogger();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(configuration);

            // Add the App
            serviceCollection.AddTransient<App>();

        }
        

    }

    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IConfigurationRoot _config;

        public App(ILogger<App> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public void Run()
        {
            // Let's test log levels:
            _logger.LogTrace("LogTrace");
            _logger.LogDebug("LogDebug");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            _logger.LogError("LogError");
            _logger.LogCritical("LogCritical");

            var hostName = _config.GetSection("RabbitMQLog").GetSection("HostName").Value;
            var queueName = _config.GetSection("RabbitMQLog").GetSection("PageLogQueue").Value;
            Console.WriteLine(hostName);
            Console.WriteLine(queueName);

            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var pageAccess = JsonConvert.DeserializeObject<PageAccessLogModel>(message);
                    //Console.WriteLine(user);
                    //Console.WriteLine(" [x] Received {0}", message);
                    Console.WriteLine(" [x] Received {0} Host {1} Controller {2} Action {3}", pageAccess.UserName, pageAccess.Host, pageAccess.Controller, pageAccess.Action);
                    _logger.LogDebug($"[x] Received {pageAccess.UserName} Host {pageAccess.Host} Controller {pageAccess.Controller} Action {pageAccess.Action}");
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            
        }
    }
}
