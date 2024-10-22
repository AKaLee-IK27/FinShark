using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock;

public class UpdateStockDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot be more than 10 characters")]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(50, ErrorMessage = "Company name cannot be more than 50 characters")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [Range(1, 1e9, ErrorMessage = "Purchase must be greater than 0")]
    public decimal Purchase { get; set; }

    [Required]
    [Range(0.001, 100, ErrorMessage = "Last Div must be greater than 0")]
    public decimal LastDiv { get; set; }

    [Required]
    [MaxLength(10, ErrorMessage = "Industry cannot be more than 10 characters")]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [Range(1, 5e9, ErrorMessage = "Market cap must be greater than 0")]
    public long MarketCap { get; set; }
}
