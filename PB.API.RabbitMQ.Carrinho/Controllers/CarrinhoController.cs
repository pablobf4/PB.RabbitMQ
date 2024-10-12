namespace PB.API.RabbitMQ.Carrinho.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private IRabbitMQMessagemEnviar _rabbitMQMessagemEnviar;

        public CarrinhoController(IRabbitMQMessagemEnviar rabbitMQMessagemEnviar)
        {
            _rabbitMQMessagemEnviar = rabbitMQMessagemEnviar ??
                throw new ArgumentNullException(nameof(rabbitMQMessagemEnviar));
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutDetalheVO>> Checkout(CheckoutDetalheVO vo)
        {
            vo.DataCriacao = DateTime.Now;

            // RabbitMQ logica - enviar mensagem
            _rabbitMQMessagemEnviar.enviarMensagem(vo, "checkoutfila");

            return Ok(vo);
        }
    }
}
