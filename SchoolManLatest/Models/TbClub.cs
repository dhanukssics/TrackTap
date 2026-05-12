using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbClub
{
    public long Id { get; set; }

    public string ClubName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }
}
