using MediatR;


namespace Pedido.Application.Commands
{
    public class CreatePedidoCommand : IRequest<CreatePedidoCommandResult>
    {
        public Guid? ClienteId { get; set; }
        public string? ClienteNome { get; set; }
        public List<CreatePedidoItemCommand> Items { get; set; } = new();
    }

    public class CreatePedidoItemCommand
    {
        public Guid ProdutoId { get; set; }
        public int Quant { get; set; }
        public string ProdutoNome { get; set; } = default!;
        public decimal UnitPrice { get; set; }
    }

    public class CreatePedidoCommandResult
    {
        public Guid Id { get; set; }
        public Guid? ClienteId { get; set; }
        public string? ClienteName { get; set; }
        public List<CreatePedidoItemCommandResult> Items { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePedidoItemCommandResult
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quant { get; set; }
        public decimal SubTotal { get; set; }
    }


}
