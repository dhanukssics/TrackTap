using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbClass
{
    public long ClassId { get; set; }

    public long SchoolId { get; set; }

    public string Class { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public Guid ClassGuild { get; set; }

    public bool IsActive { get; set; }

    public int ClassOrder { get; set; }

    public bool PublishStatus { get; set; }

    public long AcademicYearId { get; set; }

    public virtual TbAcademicYear AcademicYear { get; set; } = null!;

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbAttendance> TbAttendances { get; set; } = new List<TbAttendance>();

    public virtual ICollection<TbDivision> TbDivisions { get; set; } = new List<TbDivision>();

    public virtual ICollection<TbExamPublish> TbExamPublishes { get; set; } = new List<TbExamPublish>();

    public virtual ICollection<TbExam> TbExams { get; set; } = new List<TbExam>();

    public virtual ICollection<TbFeeClass> TbFeeClasses { get; set; } = new List<TbFeeClass>();

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbStudentPremotion> TbStudentPremotions { get; set; } = new List<TbStudentPremotion>();

    public virtual ICollection<TbStudent> TbStudents { get; set; } = new List<TbStudent>();

    public virtual ICollection<TbTeacherClass> TbTeacherClasses { get; set; } = new List<TbTeacherClass>();

    public virtual ICollection<TbTimeTable> TbTimeTables { get; set; } = new List<TbTimeTable>();
}
