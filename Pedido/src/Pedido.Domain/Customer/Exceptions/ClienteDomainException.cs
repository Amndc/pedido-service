using Pedido.Domain.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Domain.Custumer.Exceptions
{
    internal class ClienteDomainException : DomainException
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CustomerDomainException"/>.
        /// </summary>
        /// <param name="message">A mensagem que descreve o erro.</param>
        public ClienteDomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CustomerDomainException"/>.
        /// </summary>
        /// <param name="message">A mensagem que descreve o erro.</param>
        /// <param name="innerException">A exceção que causou esta exceção.</param>
        public ClienteDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
