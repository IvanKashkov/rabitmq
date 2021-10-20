using System;
using GreenPipes.Configurators;

namespace Core.Infrastructure.RabbitMQ
{
    public class QueueSettings
    {
        public string Name { get; }

        public ushort PrefetchCount { get; }

        public Type[] Types { get; }

        public Action<IRetryConfigurator> RetryConfig = null;

        public QueueSettings(string name, ushort prefetchCount, params Type[] types)
        {
            Name = name;
            PrefetchCount = prefetchCount;
            Types = types;
        }

        public QueueSettings(
            string name,
            ushort prefetchCount,
            Action<IRetryConfigurator> retryConfig = null,
            params Type[] types)
            : this(name, prefetchCount, types)
        {
            RetryConfig = retryConfig;
        }
    }
}
