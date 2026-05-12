using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbPaymentBillNo
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public long BillNo { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
