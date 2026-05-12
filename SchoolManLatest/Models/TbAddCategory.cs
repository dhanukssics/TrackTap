using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbAddCategory
{
    public long? CategoryId { get; set; }

    public long? SchoolId { get; set; }

    public string? Item { get; set; }

    public string? Unit { get; set; }

    public int Id { get; set; }

    public DateTime TimeStamp { get; set; }

    public long UserId { get; set; }
}
