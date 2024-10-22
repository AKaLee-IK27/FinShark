using api.Interfaces;
using api.Models;
using FinShark.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository(ApplicationDBContext context) : IPortfolioRepository
{
    private readonly ApplicationDBContext context = context;

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await context.Portfolios.AddAsync(portfolio);
        await context.SaveChangesAsync();

        return portfolio;
    }

    public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
    {
        return await context
            .Portfolios.Where(p => p.AppUserId == user.Id)
            .Select(p => new Stock
            {
                Id = p.StockId,
                Symbol = p.Stock.Symbol,
                CompanyName = p.Stock.CompanyName,
                Purchase = p.Stock.Purchase,
                LastDiv = p.Stock.LastDiv,
                Industry = p.Stock.Industry,
                MarketCap = p.Stock.MarketCap,
            })
            .ToListAsync();
    }
}
