using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSalesStock
{
    public long StockId { get; set; }

    public long CategoryId { get; set; }

    public string Item { get; set; } = null!;

    public decimal Price { get; set; }

    public string SalesId { get; set; } = null!;

    public long SchoolId { get; set; }

    public long UserId { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public int? StockQuantity { get; set; }

    public long StudentId { get; set; }

    public long? Billno { get; set; }
}
