using PB.Integration.RabbitMQ.MessageBus;

namespace PB.API.RabbitMQ.Carrinho.Menssages
{
    public class CheckoutDetalheVO : BaseMessagem
    {
        public long Id { get; set; }
        public string UsuarioId { get; set; }
        public string PrimeiroNome { get; set; }
        public string Email { get; set; }
        public int CarrinhoTotalItens { get; set; }
        public long ProdutotId { get; set; }
    }
}
