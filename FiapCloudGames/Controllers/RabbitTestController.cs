using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitTestController : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq"
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "fiap-events",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = new
            {
                App = "FiapCloudGames",
                Evento = "TESTE_RABBITMQ",
                Data = DateTime.UtcNow
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "fiap-events",
                body: body
            );

            return Ok("Mensagem enviada para o RabbitMQ");
        }
    }
}
