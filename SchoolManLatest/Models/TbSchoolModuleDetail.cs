using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSchoolModuleDetail
{
    public long Id { get; set; }

    public long SchoolModuleId { get; set; }

    public long MainId { get; set; }

    public long SchoolSubModuleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public long SchoolId { get; set; }

    public virtual TbSchoolModuleHome Main { get; set; } = null!;

    public virtual TbSchoolModuleMain SchoolModule { get; set; } = null!;

    public virtual TbSchoolSubModule SchoolSubModule { get; set; } = null!;
}
