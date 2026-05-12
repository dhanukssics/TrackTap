using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbCircular
{
    public long CircularId { get; set; }

    public long SchoolId { get; set; }

    public int LoginType { get; set; }

    public long UserId { get; set; }

    public DateTime CircularDate { get; set; }

    public string Description { get; set; } = null!;

    public string? FilePath { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual TbLogin User { get; set; } = null!;
}
