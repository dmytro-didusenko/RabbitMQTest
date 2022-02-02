using Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Publisher
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

            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(GetUserData()));

            channel.BasicPublish(
                exchange: "",
                routingKey: "dev-queue",
                basicProperties: null,
                body: messageBody);

            Console.WriteLine("Your message was sent");

            Console.ReadLine();
        }

        public static User GetUserData()
        {
            var user = new User();

            Console.Write("Enter User's name: ");
            user.Name = Console.ReadLine();
            Console.Write("Enter User's age: ");
            user.Age = Convert.ToInt32(Console.ReadLine());

            return user;
        }
    }
}
