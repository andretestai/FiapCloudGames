using Core.Entities.InsertEntities;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Services.Validator;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Services
{
    public class RabbitConsumerService : BackgroundService
    {
        private const string QueueName = "users-created";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("🟢 RabbitMQ Consumer iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = "rabbitmq",
                    };

                    await using var connection = await factory.CreateConnectionAsync(stoppingToken);
                    await using var channel = await connection.CreateChannelAsync();

                    await channel.QueueDeclareAsync(
                        queue: QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false
                    );

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.ReceivedAsync += async (sender, ea) =>
                    {
                        try
                        {
                            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                            var user = JsonSerializer.Deserialize<UserInsert>(json);

                            Console.WriteLine($"👤 Processando usuário: {user?.Email}");

                            var validator = new UserValidator();
                            var result = validator.Validate(user!);

                            if (!result.IsValid)
                            {
                                Console.WriteLine("❌ Usuário inválido");
                                await channel.BasicRejectAsync(ea.DeliveryTag, false);
                                return;
                            }

                            var insertResult = await SimularInsertBancoAsync(user!);

                            Console.WriteLine($"✅ Inserção concluída: {insertResult}");

                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"🔥 Erro ao processar mensagem: {ex.Message}");
                            await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                        }
                    };

                    await channel.BasicConsumeAsync(
                        queue: QueueName,
                        autoAck: false,
                        consumer: consumer
                    );

                    // mantém o consumer vivo
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ RabbitMQ indisponível, tentando novamente em 5s: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private async Task<bool> SimularInsertBancoAsync(UserInsert user)
        {
            await Task.Delay(300);
            return true;
        }
    }
}
