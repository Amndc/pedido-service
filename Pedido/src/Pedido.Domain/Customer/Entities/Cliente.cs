using Pedido.Domain.Custumer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pedido.Domain.Custumer.Entities
{
    public class Cliente
    {
        public Guid Id { get; }
        public string Cpf { get; }
        public Name Nome { get; private set; }


        public Cliente(Guid id, string cpf, Name nome)
        {
            Id = id;
            Cpf = cpf;
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        }

    }
}
