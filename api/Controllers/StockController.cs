using api.Dtos.Stock;
using api.Interfaces;
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
    private readonly IStockRepository stockRepo;

    public StockController(IStockRepository stockRepo)
    {
        this.stockRepo = stockRepo;
    }

    //* GET api/stock
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await stockRepo.GetAllAsync();

        var stockDto = stocks.Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    //* GET api/stock/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await stockRepo.GetByIdAsync(id);

        if (stock == null)
        {
            return NotFound("Stock not found");
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();

        await stockRepo.CreateAsync(stockModel);

        return CreatedAtAction(
            nameof(GetById),
            new { id = stockModel.Id },
            stockModel.ToStockDto()
        );
    }

    //* PUT api/stock/{id}
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateStockDto updateDto)
    {
        var stockModel = await stockRepo.UpdateAsync(id, updateDto);

        if (stockModel == null)
        {
            return NotFound("Stock not found");
        }

        return Ok(stockModel.ToStockDto());
    }

    //* DELETE api/stock/{id}
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await stockRepo.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound("Stock not found");
        }

        return NoContent();
    }
}
