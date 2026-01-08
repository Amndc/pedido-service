using Pedido.Application.DTOs;


namespace Pedido.Api.Examples.Swagger
{
    ///// <summary>
    ///// Exemplo de request para criação de pedido
    ///// </summary>
    //public class CreatePedidoDtoExample : IExamplesProvider<CreatePedidoDto>
    //{
    //    public CreatePedidoDto GetExamples()
    //    {
    //        return new CreatePedidoDto
    //        {
    //            ClienteId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
    //            Items = new List<CreatePedidoItemDto>
    //            {
    //               new()
    //               {
    //                   ProdutoId = Guid.Parse("e47ac10b-58cc-4372-a567-0e02b2c3d123"),
    //                   Quant = 2,
    //                   Observation = "Sem cebola"
    //               },
    //               new()
    //               {
    //                   ProdutoId = Guid.Parse("d47ac10b-58cc-4372-a567-0e02b2c3d456"),
    //                   Quant = 1,
    //                   Observation = null
    //               }
    //            }
    //        };
    //    }
    //}

    ///// <summary>
    ///// Exemplo de response para pedido
    ///// </summary>
    //public class PedidoDtoExample : IExamplesProvider<PedidoDto>
    //{
    //    public PedidoDto GetExamples()
    //    {
    //        return new PedidoDto
    //        {
    //            Id = Guid.Parse("a47ac10b-58cc-4372-a567-0e02b2c3d789"),
    //            ClienteId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
    //            Status = "Pending",
    //            TotalPrice = 45.90m,
    //            CreatedAt = DateTime.UtcNow,
    //            UpdatedAt = DateTime.UtcNow,
    //            Items = new List<PedidoItemDto>
    //        {
    //            new()
    //            {
    //                Id = Guid.Parse("b47ac10b-58cc-4372-a567-0e02b2c3d111"),
    //                ProdutoId = Guid.Parse("e47ac10b-58cc-4372-a567-0e02b2c3d123"),
    //                ProdutoNome = "X-Bacon",
    //                Quant = 2,
    //                UnitPrice = 15.95m,
    //                SubTotal = 31.90m,
    //                Observation = "Sem cebola"
    //            },
    //            new()
    //            {
    //                Id = Guid.Parse("c47ac10b-58cc-4372-a567-0e02b2c3d222"),
    //                ProdutoId = Guid.Parse("d47ac10b-58cc-4372-a567-0e02b2c3d456"),
    //                ProdutoNome = "Coca-Cola 350ml",
    //                Quant = 1,
    //                UnitPrice = 14.00m,
    //                SubTotal = 14.00m,
    //                Observation = null
    //            }
    //        }
    //        };
    //    }
    //}
}
