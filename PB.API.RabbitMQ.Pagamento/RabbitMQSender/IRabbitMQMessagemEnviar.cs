namespace PB.API.RabbitMQ.Pagamento.RabbitMQSender
{
    public interface IRabbitMQMessagemEnviar
    {
        void EnviarMensagem(BaseMessagem baseMessagem);

    }
}
