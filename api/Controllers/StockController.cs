using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using FinShark.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController(IStockRepository stockRepo) : ControllerBase
{
    private readonly IStockRepository stockRepo = stockRepo;

    //* GET api/stock?symbol=ABC&companyName=Company&sortBy=Symbol&isSortDescending=true

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] StockQuery query)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stocks = await stockRepo.GetAllAsync(query);

        var stocksDto = stocks.Select(s => s.ToStockDto()).ToList();

        return Ok(stocksDto);
    }

    //* GET api/stock/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var stock = await stockRepo.GetByIdAsync(id);

        if (stock == null)
            return NotFound("Stock not found");

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateStockDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await stockRepo.UpdateAsync(id, updateDto);

        if (stockModel == null)
            return NotFound("Stock not found");

        return Ok(stockModel.ToStockDto());
    }

    //* DELETE api/stock/{id}
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await stockRepo.DeleteAsync(id);

        if (stockModel == null)
            return NotFound("Stock not found");

        return NoContent();
    }
}
