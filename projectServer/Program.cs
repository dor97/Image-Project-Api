using projectServer;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables().Build();

            try
            {
                CreateHostBuilder(configuration).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {

            }
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("WebHostListenUrl");

            return Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    //logging.AddConsole();
                    logging.AddConsole(options =>
                    {
                        options.FormatterName = "custom";
                    });
                    logging.AddConsoleFormatter<CustomConsoleFormatter, CustomConsoleFormatterOptions>(options =>
                    {
                        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff";
                        options.CustomPrefix = "Log: ";
                    });
                })
                //.UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()                   
                    .UseUrls(url);
                    //.UseEnvironment();
                });
        }
    }
}
