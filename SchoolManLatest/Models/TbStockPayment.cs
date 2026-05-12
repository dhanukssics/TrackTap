using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStockPayment
{
    public long PaymentId { get; set; }

    public decimal Amount { get; set; }

    public bool IsPaid { get; set; }

    public long StockId { get; set; }

    public long StudentId { get; set; }

    public long ClassId { get; set; }

    public long SchoolId { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool IsActive { get; set; }

    public Guid? PaymentGuid { get; set; }

    public long? BillNo { get; set; }

    public long PaymentType { get; set; }

    public int? BillType { get; set; }

    public long? IssuedPerson { get; set; }

    public long? PartialPaidParentBillNo { get; set; }

    public int? PaymentMode { get; set; }

    public string? ChequeNumber { get; set; }

    public DateTime? ChequeDate { get; set; }

    public long? BankId { get; set; }

    public decimal? MaxAmount { get; set; }

    public decimal? Discount { get; set; }

    public string SalesId { get; set; } = null!;
}
