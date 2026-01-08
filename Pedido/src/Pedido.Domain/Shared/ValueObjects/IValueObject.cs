using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Domain.Shared.ValueObjects
{
    public interface IValueObject
    {
        /// <summary>
        /// Compara se este value object é igual a outro.
        /// A comparação é feita pelos valores, não pela referência.
        /// </summary>
        /// <param name="other">O value object a ser comparado.</param>
        /// <returns>True se os value objects são iguais, false caso contrário.</returns>
        bool Equals(object? other);

        /// <summary>
        /// Obtém um código hash para o value object baseado em seus valores.
        /// </summary>
        /// <returns>O código hash do value object.</returns>
        int GetHashCode();
    }
}
