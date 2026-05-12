using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbParent
{
    public long ParentId { get; set; }

    public string ParentName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public Guid ParentGuid { get; set; }

    public bool IsActive { get; set; }

    public string? State { get; set; }

    public string? FilePath { get; set; }

    public string? PostalCode { get; set; }

    public string? MotherName { get; set; }

    public virtual ICollection<TbCcavenueCourseResponse> TbCcavenueCourseResponses { get; set; } = new List<TbCcavenueCourseResponse>();

    public virtual ICollection<TbParentMessage> TbParentMessages { get; set; } = new List<TbParentMessage>();

    public virtual ICollection<TbStudent> TbStudents { get; set; } = new List<TbStudent>();
}
