namespace PB.API.RabbitMQ.Email.Messages
{
    public class PagamentoAtualizadoEnvioEmailVO
    {
        public long OrdemId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
