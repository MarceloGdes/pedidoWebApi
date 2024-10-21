using Pedidos.Application.DTOs.Order;
using Pedidos.Application.DTOs.OrderProduct;
using Pedidos.Domain.Models;

namespace Pedidos.Application.Interfaces;
public interface IOrderCommand
{
    int CreateOrder(CreateOrderDto dto);
    void AddProducts(int orderId, UpdateOrderProductDto dto);
    void RemoveProducts(int orderId, UpdateOrderProductDto dto);
    void CloseOrder(int orderId);
    List<OrderDto> GetOrders(string? status);
    OrderDto GetOrderById(int id);
    
}
