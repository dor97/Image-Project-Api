using System.IO;
using Microsoft.Extensions.Configuration;
using projectServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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

/*
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/
