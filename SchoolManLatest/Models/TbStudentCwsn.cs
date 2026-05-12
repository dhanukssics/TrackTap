using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStudentCwsn
{
    public long Id { get; set; }

    public string CwsnData { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }
}
