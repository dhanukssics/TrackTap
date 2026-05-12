using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStudentCurrentBillActivated
{
    public long BillActiveId { get; set; }

    public long StudentId { get; set; }

    public long SchoolId { get; set; }

    public bool? BillingForStudent { get; set; }

    public long? BillingForStudentUser { get; set; }

    public DateTime? BillingForStudentDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual TbStudent Student { get; set; } = null!;
}
