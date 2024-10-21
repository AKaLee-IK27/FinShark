using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using FinShark.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> GetAll()
    {
        var stocks = await context.Stocks.ToListAsync();

        var stockDto = stocks.Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    //* GET api/stock/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await context.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();

        await context.Stocks.AddAsync(stockModel);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = stockModel.Id },
            stockModel.ToStockDto()
        );
    }

    //* PUT api/stock/{id}
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateStockRequestDto updateDto
    )
    {
        var stockModel = await context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

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
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        context.Stocks.Remove(stockModel);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
