using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController(
    UserManager<AppUser> userManager,
    IStockRepository stockRepo,
    IPortfolioRepository portfolioRepo,
    IFMPService fmpService
) : ControllerBase
{
    private readonly UserManager<AppUser> userManager = userManager;
    private readonly IStockRepository stockRepo = stockRepo;
    private readonly IPortfolioRepository portfolioRepo = portfolioRepo;
    private readonly IFMPService fmpService = fmpService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        if (string.IsNullOrEmpty(username))
            return Unauthorized($"Username '{username}' not found in claims.");

        var appUser = await userManager.FindByNameAsync(username);

        if (appUser == null)
            return Unauthorized($"User '{username}' not found.");

        var userPortfolio = await portfolioRepo.GetUserPortfolioAsync(appUser);

        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        if (string.IsNullOrEmpty(username))
            return Unauthorized($"Username '{username}' not found in claims.");

        var appUser = await userManager.FindByNameAsync(username);
        if (appUser == null)
            return Unauthorized($"User '{username}' not found.");

        var stock = await stockRepo.GetBySymbolAsync(symbol);

        if (stock == null)
        {
            stock = await fmpService.FindStockBySymbolAsync(symbol);
            if (stock == null)
                return BadRequest("Stock does not exist");
            else
            {
                await stockRepo.CreateAsync(stock);
            }
        }

        if (stock == null)
            return NotFound("Stock not found");
        var userPortfolio = await portfolioRepo.GetUserPortfolioAsync(appUser);

        if (userPortfolio.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Stock already in portfolio");

        var portfolioModel = new Portfolio { AppUserId = appUser.Id, StockId = stock.Id };

        await portfolioRepo.CreateAsync(portfolioModel);
        if (portfolioModel == null)
            return StatusCode(500, "Failed to add stock to portfolio");

        return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var userName = User.GetUsername();
        if (string.IsNullOrEmpty(userName))
            return Unauthorized($"Username '{userName}' not found in claims.");

        var appUser = await userManager.FindByNameAsync(userName);

        if (appUser == null)
            return Unauthorized($"User '{userName}' not found.");

        var userPortfolio = await portfolioRepo.GetUserPortfolioAsync(appUser);

        var filteredStock = userPortfolio
            .Where(p => p.Symbol.ToLower() == symbol.ToLower())
            .ToList();
        if (filteredStock.Count() == 1)
        {
            await portfolioRepo.DeletePortfolioAsync(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not found in your portfolio");
        }

        return Ok();
    }
}
