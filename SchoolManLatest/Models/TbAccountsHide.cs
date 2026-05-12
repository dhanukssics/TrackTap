using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbAccountsHide
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public bool IsAccountHide { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStap { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
