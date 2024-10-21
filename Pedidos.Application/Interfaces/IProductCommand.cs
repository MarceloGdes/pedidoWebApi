
using Pedidos.Application.DTOs.Product;

namespace Pedidos.Application.Interfaces;
public interface IProductCommand
{
    List<ProductDto> GetProducts();
}
