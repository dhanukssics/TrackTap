using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbCategory
{
    public long Id { get; set; }

    public string? CategoryName { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual ICollection<TbStudentDetailedDetail> TbStudentDetailedDetails { get; set; } = new List<TbStudentDetailedDetail>();
}
