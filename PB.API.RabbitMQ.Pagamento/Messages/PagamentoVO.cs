using PB.Integration.RabbitMQ.MessageBus;

namespace PB.API.RabbitMQ.Pagamento.Messages
{
    public class PagamentoVO : BaseMessagem
    {
        public long OrderId { get; set; }
        public string Nome { get; set; }
        public string CartaoNumero { get; set; }
        public string CVV { get; set; }
        public string ExpiraMesAno { get; set; }
        public decimal valorCompra { get; set; }
        public string Email { get; set; }
    }
}
