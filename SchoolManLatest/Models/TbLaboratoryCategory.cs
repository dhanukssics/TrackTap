using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbLaboratoryCategory
{
    public long CategoryId { get; set; }

    public string LaboratoryName { get; set; } = null!;

    public long SchoolId { get; set; }

    public bool IsActive { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbStockUpdate> TbStockUpdates { get; set; } = new List<TbStockUpdate>();
}
