namespace PB.API.RabbitMQ.Ordem.MessageConsumer
{
    public class RabbitMQPaymentConsumer 
    {
        private IConnection _conexao;
        private IModel _channel;
        private const string ExchangeName = "DirectPaymentUpdateExchange";
        private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

    }
}
