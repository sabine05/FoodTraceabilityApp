namespace FoodTraceabilityApp.Models;

public class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public int Phone { get; set; }

    public string Address { get; set; } = string.Empty;
}