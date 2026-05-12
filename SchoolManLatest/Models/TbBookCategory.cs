using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbBookCategory
{
    public long CategoryId { get; set; }

    public string Category { get; set; } = null!;

    public long SchoolId { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool IsActive { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbLibraryBook> TbLibraryBooks { get; set; } = new List<TbLibraryBook>();
}
