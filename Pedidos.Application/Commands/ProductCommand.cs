using Pedidos.Domain.Entities;
using Pedidos.Application.Interfaces;
using Pedidos.Infra.Database.Context;
using Pedidos.Application.DTOs.Product;
using Pedidos.Application.Mappers;

namespace Pedidos.Application.Commands;
public class ProductCommand(ApiContext context) : IProductCommand
{
    private readonly ApiContext _context = context;
    public List<ProductDto> GetProducts()
    {

        //Mapeando os produtos da base de dados para o DTO.
        return _context.Products.Select(p => new ProductMapper().MapToDto(p)).ToList();
      
    }

}
