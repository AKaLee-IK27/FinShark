namespace api.Helpers;

public class StockQuery
{
    public string? Symbol { get; set; }
    public string? CompanyName { get; set; }
    public string? SortBy { get; set; }
    public bool IsSortDescending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
