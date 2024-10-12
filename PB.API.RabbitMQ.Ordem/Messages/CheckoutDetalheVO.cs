using PB.Integration.RabbitMQ.MessageBus;

namespace PB.API.RabbitMQ.Ordem.Messages
{
    public class CheckoutDetalheVO : BaseMessagem
    {
        public long Id { get; set; }
        public string UsuarioId { get; set; }
        public string PrimeiroNome { get; set; }
        public string Email { get; set; }
        public int CarrinhoTotalItens { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorProduto { get; set; }

    }
}
