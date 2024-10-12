﻿using PB.API.RabbitMQ.Ordem.Messages;
using PB.API.RabbitMQ.Ordem.RabbitMQSender;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


namespace PB.API.RabbitMQ.Ordem.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private IConnection _conexao;
        private IModel _channel;
        private IRabbitMQMessagemEnviar _rabbitMQMessagemEnviar;

        public RabbitMQCheckoutConsumer(
            IRabbitMQMessagemEnviar rabbitMQMessagemEnviar)
        {
            _rabbitMQMessagemEnviar = rabbitMQMessagemEnviar;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            _conexao = factory.CreateConnection();
            _channel = _conexao.CreateModel();
            _channel.QueueDeclare(queue: "checkoutfila", false, false, false, arguments: null);  
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutDetalheVO vo = JsonSerializer.Deserialize<CheckoutDetalheVO>(content);
                ProcessOrder(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutfila", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CheckoutDetalheVO vo)
        {
            var orderId = GerarNumeroPedido(1, 100000);
            PagamentoVO pagamento = new()
            {
               OrderId = orderId,
               CVV = "123",
               CartaoNumero = "411111111111",
               Email = "pb@gmail.com",
               Nome = vo.PrimeiroNome,
               valorCompra = vo.ValorProduto,
               ExpiraMesAno = "11/10/2024"
            };
            try
            {
               // _rabbitMQMessagemEnviar.EnviarMensagem(pagamento, "ordemPagamentoprocessofila");
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }

        public int GerarNumeroPedido(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max); 
        }
    }
}
