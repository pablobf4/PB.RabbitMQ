namespace PB.API.RabbitMQ.Pagamento.Messages
{
    public class AtualizarPagamentoVO : BaseMessagem
    {
        public long OrdemId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
} 
