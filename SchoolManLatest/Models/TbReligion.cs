using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbReligion
{
    public long Id { get; set; }

    public string ReligionName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual ICollection<TbStudentDetailedDetail> TbStudentDetailedDetails { get; set; } = new List<TbStudentDetailedDetail>();
}
