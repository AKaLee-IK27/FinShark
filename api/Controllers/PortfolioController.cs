using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController(
    UserManager<AppUser> userManager,
    IStockRepository stockRepo,
    IPortfolioRepository portfolioRepo
) : ControllerBase
{
    private readonly UserManager<AppUser> userManager = userManager;
    private readonly IStockRepository stockRepo = stockRepo;
    private readonly IPortfolioRepository portfolioRepo = portfolioRepo;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);

        if (appUser == null)
            return Unauthorized(username);

        var userPortfolios = await portfolioRepo.GetUserPortfolioAsync(appUser);

        return Ok(userPortfolios);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);
        if (appUser == null)
            return Unauthorized(username);

        var stock = await stockRepo.GetBySymbolAsync(symbol);
        if (stock == null)
            return NotFound("Stock not found");
        var userPortfolio = await portfolioRepo.GetUserPortfolioAsync(appUser);

        if (userPortfolio.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Stock already in portfolio");

        var portfolioModel = new Portfolio { AppUserId = appUser.Id, StockId = stock.Id };

        await portfolioRepo.CreateAsync(portfolioModel);
        if (portfolioModel == null)
            return StatusCode(500, "Failed to add stock to portfolio");

        return CreatedAtAction(nameof(GetAll), portfolioModel);
    }
}
