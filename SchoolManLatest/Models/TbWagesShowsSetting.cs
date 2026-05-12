using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbWagesShowsSetting
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public bool IsWagesShows { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStap { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
