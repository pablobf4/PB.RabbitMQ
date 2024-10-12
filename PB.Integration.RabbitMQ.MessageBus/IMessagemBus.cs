namespace PB.Integration.RabbitMQ.MessageBus
{
     public interface IMessagemBus
    {
        Task PublicarMessagem(BaseMessagem messagem, string filaNome);
    }
}
