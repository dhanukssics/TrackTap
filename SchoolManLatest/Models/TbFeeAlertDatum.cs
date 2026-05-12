using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbFeeAlertDatum
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public DateTime AlertDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
