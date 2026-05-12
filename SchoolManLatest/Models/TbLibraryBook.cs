using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbLibraryBook
{
    public long BookId { get; set; }

    public long CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int Status { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool IsActive { get; set; }

    public long SerialNumber { get; set; }

    public decimal? RandomNumber { get; set; }

    public virtual TbBookCategory Category { get; set; } = null!;

    public virtual ICollection<TbLibraryBookStudent> TbLibraryBookStudents { get; set; } = new List<TbLibraryBookStudent>();
}
