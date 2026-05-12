using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbExam
{
    public long ExamId { get; set; }

    public long SchoolId { get; set; }

    public long UserId { get; set; }

    public long ClassId { get; set; }

    public long DivisionId { get; set; }

    public string ExamName { get; set; } = null!;

    public DateTime ExamDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbClass Class { get; set; } = null!;

    public virtual TbDivision Division { get; set; } = null!;

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbExamPublish> TbExamPublishes { get; set; } = new List<TbExamPublish>();

    public virtual ICollection<TbExamSubject> TbExamSubjects { get; set; } = new List<TbExamSubject>();

    public virtual ICollection<TbStudentMark> TbStudentMarks { get; set; } = new List<TbStudentMark>();

    public virtual TbLogin User { get; set; } = null!;
}
