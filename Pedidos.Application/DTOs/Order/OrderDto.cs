using System.Text.Json.Serialization;
using Pedidos.Domain.Models;

namespace Pedidos.Application.DTOs.Order;
public class OrderDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OrderProductDto> OrderProducts { get; set; }
    public double Total { get; set; }
    public string status { get; set; }
}
