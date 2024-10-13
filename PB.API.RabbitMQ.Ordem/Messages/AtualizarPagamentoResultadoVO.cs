namespace PB.API.RabbitMQ.Ordem.Messages
{
    public class AtualizarPagamentoResultadoVO : BaseMessagem
    {
        public long OrdemId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
