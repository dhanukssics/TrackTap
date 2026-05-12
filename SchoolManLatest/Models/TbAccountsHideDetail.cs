using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbAccountsHideDetail
{
    public long Id { get; set; }

    public long Schoolid { get; set; }

    public string SchoolName { get; set; } = null!;

    public DateTime EnterDate { get; set; }

    public string VoucherNo { get; set; } = null!;

    public string AccountHeadName { get; set; } = null!;

    public string SubLedger { get; set; } = null!;

    public decimal IncomeAmount { get; set; }

    public decimal ExpenseAmount { get; set; }

    public string TransactionType { get; set; } = null!;

    public string Naration { get; set; } = null!;

    public string FromStatus { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public string VoucherType { get; set; } = null!;
}
