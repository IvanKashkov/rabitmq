﻿using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.RabbitMQ
{
    public class RabbitMqOptions
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }

        public RabbitMqOptions(IConfiguration configuration)
        {
            configuration.Bind(nameof(RabbitMqOptions), this);
        }
    }
}
