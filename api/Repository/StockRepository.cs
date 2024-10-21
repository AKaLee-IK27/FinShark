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

    public async Task<List<Stock>> GetAllAsync()
    {
        return await context.Stocks.ToListAsync();
    }
}
