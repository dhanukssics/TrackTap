using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbCommonFeeStudentDiscount
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public long StudentId { get; set; }

    public long FeeId { get; set; }

    public DateTime DiscountAllowFeeDate { get; set; }

    public long UserId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? TimeStamp { get; set; }

    public decimal? OriginalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public virtual TbFee Fee { get; set; } = null!;

    public virtual TbSchool School { get; set; } = null!;

    public virtual TbStudent Student { get; set; } = null!;

    public virtual TbLogin User { get; set; } = null!;
}
