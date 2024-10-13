namespace PB.API.RabbitMQ.Pagamento.MessageConsumer
{
    public class RabbitMQPagamentoConsumir : BackgroundService
    {
        private IConnection _conexao;
        private IModel _channel;
        private IRabbitMQMessagemEnviar _rabbitMQMessagemEnviar;


        public RabbitMQPagamentoConsumir(
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
            _channel.QueueDeclare(queue: "ordempagamentoprocessofila", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PagamentoVO vo = JsonSerializer.Deserialize<PagamentoVO>(content);
                ProcessPayment(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("ordempagamentoprocessofila", false, consumer);
            return Task.CompletedTask;
        }


        private async Task ProcessPayment(PagamentoVO vo)
        {
            var resultado = this.ProcessarPagament();

            AtualizarPagamentoVO pagamentoResultado = new()
            {
                Status = resultado,
                OrdemId = vo.OrderId,
                Email = vo.Email
            };

            try
            {
                _rabbitMQMessagemEnviar.EnviarMensagem(pagamentoResultado);
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }

        private bool ProcessarPagament()
        {
            return true;
        }
    }
}
