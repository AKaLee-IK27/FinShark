using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace api.Services;

public class FMPService(HttpClient httpClient, IConfiguration config) : IFMPService
{
    private readonly HttpClient httpClient = httpClient;
    private readonly IConfiguration config = config;

    public async Task<Stock> FindStockBySymbolAsync(string symbol)
    {
        try
        {
            var result = await httpClient.GetAsync(
                $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={config["FMPKey"]}"
            );
            if (!result.IsSuccessStatusCode)
                throw new Exception("Stock not found");

            var content = await result.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
            if (tasks.IsNullOrEmpty())
                throw new Exception("Stock not found");

            if (tasks == null || !tasks.Any())
                throw new Exception("Stock not found");

            var stock = tasks.First();
            if (stock == null)
                throw new Exception("Stock not found");
            return stock.ToStockFromFMP();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
