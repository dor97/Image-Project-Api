using projectServer;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Console.WriteLine(environment);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
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
                    .UseConfiguration(configuration)
                    .UseUrls(url);
                    //.UseEnvironment();
                });
        }
    }
}
