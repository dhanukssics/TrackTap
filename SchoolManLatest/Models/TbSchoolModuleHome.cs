using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSchoolModuleHome
{
    public long Id { get; set; }

    public string MainModule { get; set; } = null!;

    public int OrderName { get; set; }

    public bool IsActive { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual ICollection<TbSchoolModuleDetail> TbSchoolModuleDetails { get; set; } = new List<TbSchoolModuleDetail>();

    public virtual ICollection<TbSchoolSubModule> TbSchoolSubModules { get; set; } = new List<TbSchoolSubModule>();

    public virtual ICollection<TbUserModuleDetail> TbUserModuleDetails { get; set; } = new List<TbUserModuleDetail>();
}
