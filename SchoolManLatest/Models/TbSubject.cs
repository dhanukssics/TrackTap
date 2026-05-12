using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSubject
{
    public long SubId { get; set; }

    public long SchoolI { get; set; }

    public string SubjectName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TmeStamp { get; set; }

    public virtual TbSchool SchoolINavigation { get; set; } = null!;

    public virtual ICollection<TbExamSubject> TbExamSubjects { get; set; } = new List<TbExamSubject>();

    public virtual ICollection<TbTimeTable> TbTimeTables { get; set; } = new List<TbTimeTable>();
}
