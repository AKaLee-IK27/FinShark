using api.DTOs.Stock;
using api.Mappers;
using api.Models;
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

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequest stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();

        context.Stocks.Add(stockModel);
        context.SaveChanges();

        return CreatedAtAction(
            nameof(GetById),
            new { id = stockModel.Id },
            stockModel.ToStockDto()
        );
    }
}
