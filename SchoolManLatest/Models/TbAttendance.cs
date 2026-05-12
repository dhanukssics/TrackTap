using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbAttendance
{
    public long AttendanceId { get; set; }

    public long StaffId { get; set; }

    public long ClassId { get; set; }

    public long DivisionId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public bool AttendanceData { get; set; }

    public DateOnly TimeStamp { get; set; }

    public Guid AttendanceGuid { get; set; }

    public bool IsActive { get; set; }

    public long StudentId { get; set; }

    public int ShiftStatus { get; set; }

    public virtual TbClass Class { get; set; } = null!;

    public virtual TbDivision Division { get; set; } = null!;

    public virtual TbLogin Staff { get; set; } = null!;

    public virtual TbStudent Student { get; set; } = null!;
}
