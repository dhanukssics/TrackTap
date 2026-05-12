using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStudentFile
{
    public long Id { get; set; }

    public long StudentId { get; set; }

    public long SchoolId { get; set; }

    public int FileType { get; set; }

    public DateTime ReceivingDate { get; set; }

    public string Description { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual TbStudent Student { get; set; } = null!;
}
