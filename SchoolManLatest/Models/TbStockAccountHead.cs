using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStockAccountHead
{
    public long AccountId { get; set; }

    public string AccHeadName { get; set; } = null!;

    public long SchoolId { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool? ForBill { get; set; }
}
