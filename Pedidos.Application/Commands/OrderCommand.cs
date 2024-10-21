
using System.Net;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application.DTOs;
using Pedidos.Application.DTOs.Order;
using Pedidos.Application.DTOs.OrderProduct;
using Pedidos.Application.DTOs.Product;
using Pedidos.Application.Interfaces;
using Pedidos.Application.Mappers;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Models;
using Pedidos.Infra.Database.Context;

namespace Pedidos.Application.Commands;

public class OrderCommand(ApiContext context) : IOrderCommand
{
    private readonly ApiContext _context = context;
    private readonly ProductMapper _prodcutMapper = new();
    private readonly OrderMapper _orderMapper = new();


    public int CreateOrder(CreateOrderDto dto)
    {
        var order = _orderMapper.MapToEntity(dto);

        _context.Add(order);
        _context.SaveChanges();
        return order.Id;

    }
    

    public void AddProducts(int orderId, UpdateOrderProductDto dto)
    {


        var order = GetOrder(orderId);
        
        if (order.status == OrderStatusEnum.Closed)
        {
            throw new HttpRequestException("Pedido está fechado e não pode ser editado.", null, HttpStatusCode.BadRequest);
        }

        var product = GetProduct(dto.ProductId);

        var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == product.Id);

        ValidateUpdateOrderDto(dto);

        if (orderProduct == null)
        {
            var newOrderProduct = new OrderProduct
            {
                Order = order,
                Product = product,
                Quantity = dto.Quantity
            };
       

            _context.OrderProducts.Add(newOrderProduct);
        }else
        {
            orderProduct.Quantity += dto.Quantity;
            _context.OrderProducts.Update(orderProduct);
        }


        order.CalcTotalValue();
        _context.SaveChanges();

        
    }

    public void RemoveProducts(int orderId, UpdateOrderProductDto dto)
    {
        

        var order = GetOrder(orderId);

        if (order.status == OrderStatusEnum.Closed)
        {
            throw new HttpRequestException("Pedido está fechado e não pode ser editado.", null, HttpStatusCode.BadRequest);
        }

        

        var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == dto.ProductId);

        if (orderProduct == null)
        {
            throw new HttpRequestException($"O Produto id {dto.ProductId} não encontrado no pedido id {orderId}, verifique.", null, HttpStatusCode.NotFound);
        }
        else
        {
            ValidateUpdateOrderDto(dto);

            orderProduct.Quantity -= dto.Quantity;

            if(orderProduct.Quantity < 1)
            {
                _context.OrderProducts.Remove(orderProduct);
            }else
            {
                _context.OrderProducts.Update(orderProduct);
            }

        }


        order.CalcTotalValue();
        _context.SaveChanges();


    }

    public void CloseOrder(int orderId)
    {
        var order = GetOrder(orderId);

        if (order.status == OrderStatusEnum.Closed)
        {
            throw new HttpRequestException("O pedido já está fechado.", null, HttpStatusCode.BadRequest);
        }

        var orderProduct = order.OrderProducts.FirstOrDefault();

        if (orderProduct == null)
        {
            throw new HttpRequestException($"O Pedido não pode ser fechado, devido a ausencia de produtos", null, HttpStatusCode.BadRequest);
        }

        order.status = OrderStatusEnum.Closed;
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public OrderDto GetOrderById(int id)
    {
        var order = GetOrder(id);

        var orderDto = _orderMapper.MapToDto(order);

            
        orderDto.OrderProducts = order.OrderProducts.Select(op => new OrderProductDto
        {
            Product = _prodcutMapper.MapToDto(op.Product),
            Quantity = op.Quantity,
        }).ToList();
         
        return orderDto;
    }

    public List<OrderDto> GetOrders(string? status)
    {
        List <OrderDto> orders = new List<OrderDto>();

        if (!string.IsNullOrEmpty(status))
        {
            //Tenta converter a string em um const do enum
            if(Enum.TryParse<OrderStatusEnum>(status, true, out var parsedStatus))
            {
                orders = _context.Orders
                    .Where(o => o.status == parsedStatus)
                    .Select(o => _orderMapper.MapToDto(o))
                    .ToList();
            }else
            {
                throw new HttpRequestException($"O parametro '{status}' passado como status do pedido, é inválido ou inexistente.", null, HttpStatusCode.BadRequest);
            }
        }else
        {
            orders = _context.Orders.Select(o => _orderMapper.MapToDto(o)).ToList();
        }
        
        

        return orders;
    }


    private Product GetProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            throw new HttpRequestException($"Produto id {id} não encontrado, verifique se o id do produto está correto.", null, HttpStatusCode.NotFound);
        }

        return product;
    }

    private Order GetOrder(int id)
    {
        var order = _context.Orders
           .Include(o => o.OrderProducts)
           .ThenInclude(op => op.Product)
           .FirstOrDefault(o => o.Id == id);


        if (order == null)
        {
            throw new HttpRequestException("Pedido não encontrado, verifique se o id do pedido está correto.", null, HttpStatusCode.NotFound);

        }

        return order;


    }

    private void ValidateUpdateOrderDto(UpdateOrderProductDto dto)
    {
        if(dto.Quantity < 1)
            throw new HttpRequestException("A quantidade de produtos deve ser maior ou igual a 1", null, HttpStatusCode.BadRequest);
    }
    
}
