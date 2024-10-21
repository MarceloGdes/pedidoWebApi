

using Pedidos.Application.DTOs.Order;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Mappers;
public class OrderMapper : IMapper<OrderDto, CreateOrderDto, Order>
{
    public OrderDto MapToDto(Order order)
    {   

        var orderDto = new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            status = Enum.GetName(order.status),
            Total = order.Total,
            OrderProducts = null
        };

        return orderDto;
    }



    public Order MapToEntity(CreateOrderDto dto)
    {
        return new Order
        {
            CustomerName = dto.CustomerName
        };
    }


}
