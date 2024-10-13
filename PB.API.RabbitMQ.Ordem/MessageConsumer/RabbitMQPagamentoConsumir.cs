namespace PB.API.RabbitMQ.Ordem.MessageConsumer
{
    public class RabbitMQPagamentoConsumir : BackgroundService
    {
        private IConnection _conexao;
        private IModel _channel;
        private const string ExchangeName = "DirectPaymentUpdateExchange";
        private const string PaymentOrderUpdateQueueName = "PagamentoOrdemAtualizarFilaNome";


        public RabbitMQPagamentoConsumir()
        {

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _conexao = factory.CreateConnection();
            _channel = _conexao.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);
            _channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PagamentoOrdem");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                AtualizarPagamentoResultadoVO vo = JsonSerializer.Deserialize<AtualizarPagamentoResultadoVO>(content);
                AtualizarPagamentoStatus(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(PaymentOrderUpdateQueueName, false, consumer);
            return Task.CompletedTask;
        }


        private async Task AtualizarPagamentoStatus(AtualizarPagamentoResultadoVO vo)
        {
            try
            {
            //logica para atualizar status
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
