using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.Interfaces;

namespace Pedidos.Api.Controllers;
[Route("api/products")]
[ApiController]
public class ProductController(IProductCommand command) : ControllerBase 
{
    private readonly IProductCommand _command = command;

    /// <summary>
    /// Obtém os produtos.
    /// </summary>
 
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(_command.GetProducts());
    }

}
