using Pedidos.Domain.Entities;

namespace Pedidos.Application.DTOs.Order;
public class CreateOrderDto
{
    public required string CustomerName { get; set; }

}
