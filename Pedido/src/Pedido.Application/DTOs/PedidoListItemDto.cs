using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Application.DTOs
{
    /// <summary>
    /// DTO para listagem de pedidos (visão resumida)
    /// </summary>
    public class PedidoListItemDto
    {
        public Guid Id { get; set; }
        public Guid? ClienteId { get; set; }
        public string? ClienteNome { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ItemsCount { get; set; }
    }
}
