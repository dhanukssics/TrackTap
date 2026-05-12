using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbCalenderEvent
{
    public long EventId { get; set; }

    public string EventHead { get; set; } = null!;

    public string EventDetails { get; set; } = null!;

    public long SchoolId { get; set; }

    public DateTime EventDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
