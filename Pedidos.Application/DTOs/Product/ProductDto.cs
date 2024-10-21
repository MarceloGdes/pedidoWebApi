namespace Pedidos.Application.DTOs.Product;
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }

    public ProductDto(int id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
