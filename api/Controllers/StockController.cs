using api.Dtos.Stock;
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

    //* GET api/stock
    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = context.Stocks.ToList().Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    //* GET api/stock/{id}
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
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
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

    //* PUT api/stock/{id}
    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap = updateDto.MarketCap;

        context.SaveChanges();

        return NoContent();
    }

    //* DELETE api/stock/{id}
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stockModel = context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        context.Stocks.Remove(stockModel);
        context.SaveChanges();

        return NoContent();
    }
}
