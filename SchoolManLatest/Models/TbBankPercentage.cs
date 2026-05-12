using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbBankPercentage
{
    public long BankPercentageId { get; set; }

    public string SchoolName { get; set; } = null!;

    public long SchoolId { get; set; }

    public int Amount { get; set; }

    public bool IsActive { get; set; }

    public DateTime UpdateTimeStamp { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
