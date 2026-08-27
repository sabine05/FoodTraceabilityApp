public class RawMaterial
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string BatchNumber { get; set; } = string.Empty;

    public DateTime ReceptionDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int SupplierId { get; set; }
}
