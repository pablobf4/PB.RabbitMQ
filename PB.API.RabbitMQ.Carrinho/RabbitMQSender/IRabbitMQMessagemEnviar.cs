namespace PB.API.RabbitMQ.Carrinho.RabbitMQSender
{
    public interface IRabbitMQMessagemEnviar
    {
        void enviarMensagem(BaseMessagem baseMessagem, string filaNome);
    }
}
