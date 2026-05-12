using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbLogin
{
    public long UserId { get; set; }

    public long SchoolId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool DisableStatus { get; set; }

    public Guid LoginGuid { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbAssetsLiabilityDatum> TbAssetsLiabilityData { get; set; } = new List<TbAssetsLiabilityDatum>();

    public virtual ICollection<TbAttendance> TbAttendances { get; set; } = new List<TbAttendance>();

    public virtual ICollection<TbBankBookDatum> TbBankBookData { get; set; } = new List<TbBankBookDatum>();

    public virtual ICollection<TbBankEntry> TbBankEntries { get; set; } = new List<TbBankEntry>();

    public virtual ICollection<TbCashEntry> TbCashEntries { get; set; } = new List<TbCashEntry>();

    public virtual ICollection<TbCircular> TbCirculars { get; set; } = new List<TbCircular>();

    public virtual ICollection<TbCommonFeeStudentDiscount> TbCommonFeeStudentDiscounts { get; set; } = new List<TbCommonFeeStudentDiscount>();

    public virtual ICollection<TbDayBookDatum> TbDayBookData { get; set; } = new List<TbDayBookDatum>();

    public virtual ICollection<TbExam> TbExams { get; set; } = new List<TbExam>();

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbStaffSmshistory> TbStaffSmshistories { get; set; } = new List<TbStaffSmshistory>();

    public virtual ICollection<TbStaff> TbStaffs { get; set; } = new List<TbStaff>();

    public virtual ICollection<TbStockUpdate> TbStockUpdates { get; set; } = new List<TbStockUpdate>();

    public virtual ICollection<TbTeacher> TbTeachers { get; set; } = new List<TbTeacher>();

    public virtual ICollection<TbUserAllotedMenu> TbUserAllotedMenus { get; set; } = new List<TbUserAllotedMenu>();
}
