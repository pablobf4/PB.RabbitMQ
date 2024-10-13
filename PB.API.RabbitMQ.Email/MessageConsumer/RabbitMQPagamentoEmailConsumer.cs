using PB.API.RabbitMQ.Email.Messages;
using System.Text;
using System.Text.Json;

namespace PB.API.RabbitMQ.Email.MessageConsumer
{
    public class RabbitMQPagamentoEmailConsumer : BackgroundService
    {
        private IConnection _conexao;
        private IModel _channel;
        private const string ExchangeNome = "DirectPaymentUpdateExchange";
        private const string PagamentoEmailAtualizarFilaNome = "PagamentoEmailAtualizarFilaNome";
        public RabbitMQPagamentoEmailConsumer()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            _conexao = factory.CreateConnection();
            _channel = _conexao.CreateModel();

            _channel.ExchangeDeclare(ExchangeNome, ExchangeType.Direct);
            _channel.QueueDeclare(queue: PagamentoEmailAtualizarFilaNome, false, false, false, null);
            _channel.QueueBind(PagamentoEmailAtualizarFilaNome, ExchangeNome, "PagamentoEmail");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PagamentoAtualizadoEnvioEmailVO vo = JsonSerializer.Deserialize<PagamentoAtualizadoEnvioEmailVO>(content);
                EnviarEmailPagamento(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(PagamentoEmailAtualizarFilaNome, false, consumer);
            return Task.CompletedTask;
        }


        private async Task EnviarEmailPagamento(PagamentoAtualizadoEnvioEmailVO vo)
        {
            try
            {
                //logica do email
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }

        
    }
}
