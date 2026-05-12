using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStockBalance
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public DateTime CurrentDate { get; set; }

    public int SourceId { get; set; }

    public long? BankId { get; set; }

    public decimal Opening { get; set; }

    public decimal Closing { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }
}
