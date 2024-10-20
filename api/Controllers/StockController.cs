using api.Mappers;
using FinShark.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext context;

    public StockController(ApplicationDBContext context)
    {
        this.context = context;
    }

    // GET api/stock
    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = context.Stocks.ToList().Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    // GET api/stock/{id}
    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }
}
