using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Core2WebUI.Core.RabbitMQ
{
    public class PageEntryLogPublisher
    {
        public  void PageEntryLogPublish()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                                    routingKey: "hello",
                                                    basicProperties: null,
                                                    body: body);

                }
            }

        }
    }
}
