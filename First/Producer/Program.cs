using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable((msg) => System.Diagnostics.Debug.WriteLine($"SERILOG: {msg}"));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.WithCorrelationId()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elastic:changeme@elasticsearch:9200"))
                    {
                        AutoRegisterTemplate = true,
                        OverwriteTemplate = true,
                        DetectElasticsearchVersion = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                        NumberOfReplicas = 1,
                        NumberOfShards = 2,

                        RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
                        FailureCallback = e => Console.WriteLine("------------ Unable to submit event:" + e.MessageTemplate),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                               EmitEventFailureHandling.WriteToFailureSink |
                                               EmitEventFailureHandling.RaiseCallback,
                        FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null, null),
                    })
            )
            .ConfigureWebHostDefaults(webBuilder =>
                {
                   webBuilder.UseStartup<Startup>();
                })
                ;
    }
}
