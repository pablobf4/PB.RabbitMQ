namespace PB.API.RabbitMQ.Pagamento.RabbitMQSender
{
    public class RabbitMQMessagemEnviar : IRabbitMQMessagemEnviar
    {
        private readonly string _hostNome;
        private readonly string _senha;
        private readonly string _usuarioNome;
        private IConnection _connection;
        private const string ExchangeName = "DirectPagamentoUpdate_Exchange";
        private const string PagamentoEmailAtualizarFilaName = "PagamentoEmailAtualizarFilaNome";
        private const string PagamentoOrdemAtualizarFilaName = "PagamentoOrdemAtualizarFilaNome";


        public RabbitMQMessagemEnviar()
        {
            _hostNome = "localhost";
            _senha = "guest";
            _usuarioNome = "guest";
        }

        public void EnviarMensagem(BaseMessagem BaseMessagem)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);
                channel.QueueDeclare(PagamentoEmailAtualizarFilaName, false, false, false, null);
                channel.QueueDeclare(PagamentoOrdemAtualizarFilaName, false, false, false, null);

                channel.QueueBind(PagamentoEmailAtualizarFilaName, ExchangeName, "PagamentoEmail");
                channel.QueueBind(PagamentoOrdemAtualizarFilaName, ExchangeName, "PagamentoOrdem");

                byte[] body = GetMessageAsByteArray(BaseMessagem);
                channel.BasicPublish(
                    exchange: ExchangeName, "PagamentoEmail", basicProperties: null, body: body);
                channel.BasicPublish(
                    exchange: ExchangeName, "PagamentoOrdem", basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessagem BaseMessagem)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<AtualizarPagamentoVO>((AtualizarPagamentoVO)BaseMessagem, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostNome,
                    UserName = _senha,
                    Password = _usuarioNome
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;
            CreateConnection();
            return _connection != null;
        }
    }
}
