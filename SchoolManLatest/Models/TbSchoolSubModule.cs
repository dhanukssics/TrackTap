using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSchoolSubModule
{
    public long Id { get; set; }

    public long MainId { get; set; }

    public string SchoolSubModule { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual TbSchoolModuleHome Main { get; set; } = null!;

    public virtual ICollection<TbSchoolModuleDetail> TbSchoolModuleDetails { get; set; } = new List<TbSchoolModuleDetail>();

    public virtual ICollection<TbUserModuleDetail> TbUserModuleDetails { get; set; } = new List<TbUserModuleDetail>();
}
