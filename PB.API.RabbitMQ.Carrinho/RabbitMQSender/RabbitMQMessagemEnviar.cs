namespace PB.API.RabbitMQ.Carrinho.RabbitMQSender
{
    public class RabbitMQMessagemEnviar : IRabbitMQMessagemEnviar
    {

        private readonly string _hostName;
        private readonly string _senha;
        private readonly string _usuario;
        private readonly int _porta;
        private IConnection _conexao;

        public RabbitMQMessagemEnviar()
        {
            _hostName = "localhost";
            _senha = "guest";
            _usuario = "guest";
            _porta = 5672;
        }

        public void enviarMensagem(BaseMessagem baseMessagem, string filaNome)
        {
            if (ConnectionExists())
            {
                using var channel = _conexao.CreateModel();
                channel.QueueDeclare(queue: filaNome, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray(baseMessagem);
                channel.BasicPublish(
                    exchange: "", routingKey: filaNome, basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _usuario,
                    Password = _senha,
                    Port = _porta
                };
                _conexao = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendMessage(BaseMessagem baseMessagem, string filaNome)
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
            var json = JsonSerializer.Serialize<CheckoutDetalheVO>((CheckoutDetalheVO)baseMessagem, options);
            var corpo = Encoding.UTF8.GetBytes(json);
            return corpo;
        }

        private bool ConnectionExists()
        {
            if (_conexao != null) return true;
            CreateConnection();
            return _conexao != null;
        }
    }
}
