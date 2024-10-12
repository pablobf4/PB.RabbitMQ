namespace PB.API.RabbitMQ.Ordem.RabbitMQSender
{
    public class RabbitMQMessagemEnviar : IRabbitMQMessagemEnviar
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _conexao;

        public RabbitMQMessagemEnviar()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void EnviarMensagem(BaseMessagem baseMessagem, string filaNome)
        {
            if (ConnectionExists())
            {
                using var channel = _conexao.CreateModel();
                channel.QueueDeclare(queue: filaNome, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray(baseMessagem);
                channel.BasicPublish(exchange: "", routingKey: filaNome, basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessagem baseMessagem)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<PagamentoVO>((PagamentoVO)baseMessagem, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };
                _conexao = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_conexao != null) return true;
            CreateConnection();
            return _conexao != null;
        }
    }
}
