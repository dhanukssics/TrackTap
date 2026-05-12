using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbPushDatum
{
    public long? SchoolId { get; set; }

    public string LegacyNumber { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public string? PlayStore { get; set; }

    public virtual TbSchool? School { get; set; }
}
