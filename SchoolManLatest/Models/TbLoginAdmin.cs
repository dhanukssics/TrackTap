using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbLoginAdmin
{
    public long AdminId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }
}
