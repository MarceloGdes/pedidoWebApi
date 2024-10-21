
using Pedidos.Application.DTOs.Product;

namespace Pedidos.Application.DTOs;
public class OrderProductDto
{
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
}
