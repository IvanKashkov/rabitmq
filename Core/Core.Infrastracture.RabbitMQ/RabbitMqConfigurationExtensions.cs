using System;
using System.Reflection;
using Core.Infrastructure.RabbitMQ.Messages;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.RabbitMQ
{
    public static class RabbitMqConfigurationExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration, params QueueSettings[] queueRegistrations)
        {
            var consumersAssembly = Assembly.GetCallingAssembly();
            var options = new RabbitMqOptions(configuration);

            services.AddRabbitMq(options, consumersAssembly, queueRegistrations);

            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services,
            RabbitMqOptions options,
            Assembly consumersAssembly,
            params QueueSettings[] queueRegistrations)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(consumersAssembly);
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(options.Host, options.VirtualHost, h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });

                    foreach (var queueRegistration in queueRegistrations)
                    {
                        config.ReceiveEndpoint(queueRegistration.Name, e =>
                        {
                            foreach (var type in queueRegistration.Types)
                                e.ConfigureConsumer(context, type);

                            e.PrefetchCount = queueRegistration.PrefetchCount;
                            e.UseMessageRetry(y => y.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)));
                        });
                    }
                });
            });

            services.AddMassTransitHostedService();
            services.AddScoped<IPublishService, PublishService>();

            return services;
        }
    }
}
