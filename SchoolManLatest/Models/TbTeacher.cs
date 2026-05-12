using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbTeacher
{
    public long TeacherId { get; set; }

    public string TeacherSpecialId { get; set; } = null!;

    public string TeacherName { get; set; } = null!;

    public long SchoolId { get; set; }

    public string ContactNumber { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime TimeStamp { get; set; }

    public Guid TeacherGuid { get; set; }

    public bool IsActive { get; set; }

    public string? FilePath { get; set; }

    public long UserId { get; set; }

    public decimal? SalaryAmount { get; set; }

    public decimal? Pfpercentage { get; set; }

    public decimal? Esipercentage { get; set; }

    public bool? IsPermanent { get; set; }

    public long? UserType { get; set; }

    public DateTime? Doj { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbAllMessage> TbAllMessages { get; set; } = new List<TbAllMessage>();

    public virtual ICollection<TbAttendanceTeacher> TbAttendanceTeachers { get; set; } = new List<TbAttendanceTeacher>();

    public virtual ICollection<TbMessage> TbMessages { get; set; } = new List<TbMessage>();

    public virtual ICollection<TbTeacherClass> TbTeacherClasses { get; set; } = new List<TbTeacherClass>();

    public virtual ICollection<TbTeacherFile> TbTeacherFiles { get; set; } = new List<TbTeacherFile>();

    public virtual ICollection<TbTimeTable> TbTimeTables { get; set; } = new List<TbTimeTable>();

    public virtual TbLogin User { get; set; } = null!;
}
