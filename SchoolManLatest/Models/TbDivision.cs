using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbDivision
{
    public long DivisionId { get; set; }

    public long ClassId { get; set; }

    public string Division { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public Guid DivisionGuid { get; set; }

    public bool IsActive { get; set; }

    public virtual TbClass Class { get; set; } = null!;

    public virtual ICollection<TbAttendance> TbAttendances { get; set; } = new List<TbAttendance>();

    public virtual ICollection<TbBiometricDivision> TbBiometricDivisions { get; set; } = new List<TbBiometricDivision>();

    public virtual ICollection<TbExamPublish> TbExamPublishes { get; set; } = new List<TbExamPublish>();

    public virtual ICollection<TbExam> TbExams { get; set; } = new List<TbExam>();

    public virtual ICollection<TbFeeClass> TbFeeClasses { get; set; } = new List<TbFeeClass>();

    public virtual ICollection<TbStudentPremotion> TbStudentPremotionFromDivisionNavigations { get; set; } = new List<TbStudentPremotion>();

    public virtual ICollection<TbStudentPremotion> TbStudentPremotionToDivisionNavigations { get; set; } = new List<TbStudentPremotion>();

    public virtual ICollection<TbStudent> TbStudents { get; set; } = new List<TbStudent>();

    public virtual ICollection<TbTeacherClass> TbTeacherClasses { get; set; } = new List<TbTeacherClass>();

    public virtual ICollection<TbTimeTable> TbTimeTables { get; set; } = new List<TbTimeTable>();
}
