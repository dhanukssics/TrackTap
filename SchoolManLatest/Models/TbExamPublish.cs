using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbExamPublish
{
    public long ExamPublishId { get; set; }

    public long SchoolId { get; set; }

    public long ClassId { get; set; }

    public long DivisionId { get; set; }

    public long ExamId { get; set; }

    public string ExamName { get; set; } = null!;

    public DateTime ExamDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbClass Class { get; set; } = null!;

    public virtual TbDivision Division { get; set; } = null!;

    public virtual TbExam Exam { get; set; } = null!;

    public virtual TbSchool School { get; set; } = null!;
}
