using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSetting
{
    public int SettingsId { get; set; }

    public long? SchoolId { get; set; }

    public DateTime? FeeStartDate { get; set; }

    public virtual TbSchool? School { get; set; }
}
