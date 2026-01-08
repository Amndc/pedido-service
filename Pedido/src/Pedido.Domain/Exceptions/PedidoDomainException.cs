using Pedido.Domain.Shared.Exceptions;


namespace Pedido.Domain.Exceptions
{
    /// <summary>
    /// Exceção de domínio específica para entidades de pedido.
    /// </summary>
    public class PedidoDomainException : DomainException
    {
        public PedidoDomainException(string message) : base(message)
        {
        }
    }
}
