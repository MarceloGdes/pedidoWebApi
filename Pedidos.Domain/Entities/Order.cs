using Pedidos.Domain.Models;

namespace Pedidos.Domain.Entities;
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    public double Total { get; set; }
    public OrderStatusEnum status { get; set; } = OrderStatusEnum.Open;


    public void CalcTotalValue()
    {
        OrderProducts.ForEach(op => Total += (op.Quantity * op.Product.Price));
        
    }

}
