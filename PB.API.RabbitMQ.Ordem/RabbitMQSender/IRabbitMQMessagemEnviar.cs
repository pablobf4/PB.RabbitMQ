namespace PB.API.RabbitMQ.Ordem.RabbitMQSender
{
    public interface IRabbitMQMessagemEnviar
    {
        void EnviarMensagem(BaseMessagem baseMessagem, string filaNome);
    }
}
