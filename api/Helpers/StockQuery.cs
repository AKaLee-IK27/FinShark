namespace api.Helpers;

public class StockQuery
{
    public string? Symbol { get; set; }
    public string? CompanyName { get; set; }
    public string? SortBy { get; set; }
    public bool IsSortDescending { get; set; } = false;
}
