using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSchoolModuleMain
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public string SchoolName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbSchoolModuleDetail> TbSchoolModuleDetails { get; set; } = new List<TbSchoolModuleDetail>();
}
