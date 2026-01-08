using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pedido.Application.Commands;
using Pedido.Application.DTOs;
using Pedido.Application.Queries;



namespace Pedido.Api.Controllers
{
    [ApiController]
    [Route("api/v1/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(IMediator mediator, ILogger<PedidosController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePedidoDto request)
        {
            var command = new CreatePedidoCommand
            {
                ClienteId = request.ClienteId,
                Items = request.Items.Select(item => new CreatePedidoItemCommand
                {
                    ProdutoId = item.ProdutoId,
                    Quant = item.Quant
                }).ToList()
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? clienteId,
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetPedidosQuery
            {
                ClienteId = clienteId,
                Status = status,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetPedidoByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Order);
        }

        [HttpPut("{id:guid}/status")]
      //  [Authorize]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdatePedidoStatusDto request)
        {
            var command = new UpdatePedidoStatusCommand
            {
                Id = id,
                Status = request.Status
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id:guid}/status")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var query = new GetPedidoByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.Success)
                return NotFound();

            return Ok(new PedidoStatusDto
            {
                OrderId = result.Order.Id,
                Status = result.Order.Status,
                CreatedAt = result.Order.CreatedAt
            });
        }
    }
}
