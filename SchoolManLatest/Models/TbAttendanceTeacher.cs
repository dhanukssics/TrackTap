using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbAttendanceTeacher
{
    public int AttendanceId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public bool AttendanceData { get; set; }

    public DateOnly TimeStamp { get; set; }

    public Guid AttendanceGuid { get; set; }

    public bool IsActive { get; set; }

    public long TeacherId { get; set; }

    public int ShiftStatus { get; set; }

    public virtual TbTeacher Teacher { get; set; } = null!;
}
