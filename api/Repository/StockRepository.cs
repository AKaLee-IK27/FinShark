using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using FinShark.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext context;

    public StockRepository(ApplicationDBContext context)
    {
        this.context = context;
    }

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

    public async Task<List<Stock>> GetAllAsync()
    {
        return await context.Stocks.Include(s => s.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
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
