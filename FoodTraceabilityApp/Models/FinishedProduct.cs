public class FinishedProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string InternalBatchNumber { get; set; } = string.Empty;
    
    public DateTime ProductionDate { get; set; }

    public int Quantity { get; set; }
}   