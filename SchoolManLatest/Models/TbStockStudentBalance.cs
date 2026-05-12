using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStockStudentBalance
{
    public long BalanceId { get; set; }

    public long StudentId { get; set; }

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }
}
