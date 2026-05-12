using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStockVoucherNumber
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public long ReceiptVoucher { get; set; }

    public long PaymentVoucher { get; set; }

    public long ContraVoucher { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }
}
