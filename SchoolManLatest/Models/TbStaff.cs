using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStaff
{
    public long StaffId { get; set; }

    public long UserId { get; set; }

    public string StaffName { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime Dob { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public decimal? SalaryAmount { get; set; }

    public decimal? Pfpercentage { get; set; }

    public decimal? Esipercentage { get; set; }

    public bool? IsPermanent { get; set; }

    public long? UserType { get; set; }

    public DateTime? Doj { get; set; }

    public virtual ICollection<TbStaffFileCollection> TbStaffFileCollections { get; set; } = new List<TbStaffFileCollection>();

    public virtual TbLogin User { get; set; } = null!;
}
