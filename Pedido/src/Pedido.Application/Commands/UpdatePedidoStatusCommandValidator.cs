using Pedido.Domain.ValueObjects;
using FluentValidation;


namespace Pedido.Application.Commands
{
    public class UpdatePedidoStatusCommandValidator : AbstractValidator<UpdatePedidoStatusCommand>
    {
        public UpdatePedidoStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do pedido é obrigatório"); RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status é obrigatório")
                .Must(BeValidStatus).WithMessage("Status inválido. Valores válidos: Pending, Processing, Ready, Completed, Cancelled, Paid, AwaitingPayment");
        }
        private bool BeValidStatus(string status)
        {
            return Enum.TryParse(typeof(PedidoStatus), status, true, out _);
        }
    }
}
