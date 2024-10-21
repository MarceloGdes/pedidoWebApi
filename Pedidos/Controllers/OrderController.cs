using System.Net;
using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.DTOs.Order;
using Pedidos.Application.DTOs.OrderProduct;
using Pedidos.Application.Interfaces;


namespace Pedidos.Api.Controllers;
[Route("api/orders")]
[ApiController]
public class OrderController(IOrderCommand command) : ControllerBase
{

    private IOrderCommand _command = command;

    /// <summary>
    /// Cria um novo pedido.
    /// </summary>
    /// <param name="dto">Dados para criação do pedido.</param>
    
    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
    {
        var orderId = _command.CreateOrder(dto);

        return CreatedAtAction(nameof(CreateOrder), new { id = orderId });
    }

    /// <summary>
    /// Adiciona produtos a um pedido existente.
    /// </summary>
    /// <param name="id">ID do pedido.</param>
    /// <param name="dto">Dados dos produtos a serem adicionados.</param>
 

    [HttpPut("{id}/products/add")]
    public IActionResult AddProducts([FromRoute]int id, [FromBody] UpdateOrderProductDto dto)
    {

        try
        {
            command.AddProducts(id, dto);
            return Ok();
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new { e.StatusCode, message = e.Message});
            
            else if(e.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(new { e.StatusCode, message = e.Message });

            return new ObjectResult(new { e.StatusCode, message = e.Message });
        }
    }

    /// <summary>
    /// Remove produtos de um pedido existente.
    /// </summary>
    /// <param name="id">ID do pedido.</param>
    /// <param name="dto">Dados dos produtos a serem removidos.</param>
 


    [HttpPut("{id}/products/remove")]
    public IActionResult RemoveProducts([FromRoute] int id, [FromBody] UpdateOrderProductDto dto)
    {

        try
        {
            command.RemoveProducts(id, dto);
            return Ok();
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new { e.StatusCode, message = e.Message });

            else if (e.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(new { e.StatusCode, message = e.Message });

            return new ObjectResult(new { e.StatusCode, message = e.Message });
        }
    }


    /// <summary>
    /// Fecha um pedido existente.
    /// </summary>
    /// <param name="id">ID do pedido.</param>

    [HttpPut("{id}/close")]
    public IActionResult CloseOrder([FromRoute] int id)
    {

        try
        {
            command.CloseOrder(id);
            return Ok();
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new { e.StatusCode, message = e.Message });

            else if (e.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(new { e.StatusCode, message = e.Message });

            return new ObjectResult(new { e.StatusCode, message = e.Message });
        }
    }


    /// <summary>
    /// Obtém um pedido e seus produtos pelo ID.
    /// </summary>
    /// <param name="id">ID do pedido.</param>
 
    [HttpGet("{id}")]
    public IActionResult GetOrderById([FromRoute] int id)
    {
        return Ok(_command.GetOrderById(id));

    }


    /// <summary>
    /// Obtém todos os pedidos.
    /// </summary>
    /// <param name="status">Filtra os pedidos pelo Status (closed ou open) - opcional.</param>

    [HttpGet]
    public IActionResult GetOrders([FromQuery] string? status = null)
    {
        
        try
        {

            return Ok(_command.GetOrders(status));

        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new { e.StatusCode, message = e.Message });

            else if (e.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(new { e.StatusCode, message = e.Message });

            return new ObjectResult(new { e.StatusCode, message = e.Message });
        }

    }
}
