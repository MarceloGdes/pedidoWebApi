
using Pedidos.Application.DTOs.Product;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Mappers;
public class ProductMapper : IMapper<ProductDto, CreateProductDto, Product>
{
    public ProductDto MapToDto(Product entity)
    {
        return new ProductDto(entity.Id, entity.Name, entity.Price);
    }

    public Product MapToEntity(CreateProductDto dto)
    {
        return new Product(dto.Name, dto.Price);
    }
}
