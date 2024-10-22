using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using FinShark.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository(ApplicationDBContext context) : IStockRepository
{
    private readonly ApplicationDBContext context = context;

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await context.Stocks.AddAsync(stockModel);
        await context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return null;
        }

        context.Stocks.Remove(stockModel);
        await context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<List<Stock>> GetAllAsync(StockQuery query)
    {
        var stocks = context.Stocks.Include(s => s.Comments).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsSortDescending
                    ? stocks.OrderByDescending(s => s.Symbol)
                    : stocks.OrderBy(s => s.Symbol);
            }
            else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsSortDescending
                    ? stocks.OrderByDescending(s => s.CompanyName)
                    : stocks.OrderBy(s => s.CompanyName);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<bool> StockExists(int id)
    {
        return await context.Stocks.AnyAsync(s => s.Id == id);
    }

    public async Task<Stock?> UpdateAsync(int id, CreateStockDto stockDto)
    {
        var exsitingStock = await context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (exsitingStock == null)
        {
            return null;
        }

        exsitingStock.Symbol = stockDto.Symbol;
        exsitingStock.CompanyName = stockDto.CompanyName;
        exsitingStock.Purchase = stockDto.Purchase;
        exsitingStock.LastDiv = stockDto.LastDiv;
        exsitingStock.Industry = stockDto.Industry;
        exsitingStock.MarketCap = stockDto.MarketCap;

        await context.SaveChangesAsync();
        return exsitingStock;
    }
}
