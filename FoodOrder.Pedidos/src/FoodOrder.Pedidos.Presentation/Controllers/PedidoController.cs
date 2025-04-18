using FoodOrder.Pedidos.Application.Feature.Pedidos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Pedidos.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Route("ListarPedidos")]
        public async Task<IActionResult> ListarPedidos()
        {
            var query = new PedidosCollectionQuery();
            var cliente = await _mediator.Send(query);
            return Ok(cliente);
        }

        [HttpPut]
        [Route("AtualizarStatusPedido")]
        public async Task<IActionResult> AtualizarStatusPedido([FromBody] UpdateStatusPedidoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("AtualizarStatuspagamento")]
        public async Task<IActionResult> AtualizarStatuspagamento([FromBody] UpdateStatusPagamentoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
