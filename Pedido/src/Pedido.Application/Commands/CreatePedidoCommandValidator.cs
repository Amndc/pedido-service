using FluentValidation;

namespace Pedido.Application.Commands
{
    public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
    {
        public CreatePedidoCommandValidator()
        {
            // CustomerId é opcional para pedidos anônimos
            // Quando informado, deve ser um GUID válido (não vazio)
            RuleFor(x => x.ClienteId)
                .Must(customerId => !customerId.HasValue || customerId.Value != Guid.Empty)
                .WithMessage("Quando informado, o ID do cliente deve ser um GUID válido");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Os itens do pedido são obrigatórios")
                .Must(items => items != null && items.Count > 0).WithMessage("O pedido deve ter pelo menos um item");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProdutoId)
                    .NotEmpty().WithMessage("O ID do produto é obrigatório");

                item.RuleFor(x => x.Quant)
                    .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero");
            });
        }
    }
}
