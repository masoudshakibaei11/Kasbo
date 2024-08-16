

namespace Kasbo.Models;

public class Chart : BaseEntity<Guid>
{
    public int StockId { get; set; }
    public int Open { get; set; }
    public int Close { get; set; }
    public int High { get; set; }
    public int Low { get; set; }
    public double Avg { get; set; }
}