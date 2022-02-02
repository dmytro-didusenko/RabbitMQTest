using Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "dev-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (User, us) =>
            {
                var user = us.Body.ToArray();
                var message = JsonSerializer.Deserialize<User>(Encoding.UTF8.GetString(user));
                Console.WriteLine($"User name is {message.Name} and he has {message.Age} years old");
            };

            channel.BasicConsume(
                queue: "dev-queue",
                autoAck: true,
                consumer: consumer);

            Console.ReadLine();

        }
    }
}
