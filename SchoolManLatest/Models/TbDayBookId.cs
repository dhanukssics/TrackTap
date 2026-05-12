using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbDayBookId
{
    public long Id { get; set; }

    public long SchoolId { get; set; }

    public long ExpenseId { get; set; }

    public long IncomeId { get; set; }

    public virtual TbSchool School { get; set; } = null!;
}
