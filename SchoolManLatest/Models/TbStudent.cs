using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStudent
{
    public long StudentId { get; set; }

    public long SchoolId { get; set; }

    public string StudentSpecialId { get; set; } = null!;

    public string StundentName { get; set; } = null!;

    public string ParentName { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? ContactNumber { get; set; }

    public string? ClasssNumber { get; set; }

    public long ClassId { get; set; }

    public long DivisionId { get; set; }

    public long BusId { get; set; }

    public string? TripNo { get; set; }

    public string? FilePath { get; set; }

    public DateTime TimeStamp { get; set; }

    public Guid? StudentGuid { get; set; }

    public bool IsActive { get; set; }

    public long? ParentId { get; set; }

    public string? State { get; set; }

    public string? Gender { get; set; }

    public string? BloodGroup { get; set; }

    public string? ParentEmail { get; set; }

    public string? PostalCode { get; set; }

    public DateTime? Dob { get; set; }

    public string? Aadhaar { get; set; }

    public string? BioNumber { get; set; }

    public bool? IsSamrtPhoneUser { get; set; }

    public string? Religion { get; set; }

    public string? ReligionCast { get; set; }

    public string? Category { get; set; }

    public DateTime? Doj { get; set; }

    public virtual TbBu Bus { get; set; } = null!;

    public virtual TbClass Class { get; set; } = null!;

    public virtual TbDivision Division { get; set; } = null!;

    public virtual TbParent? Parent { get; set; }

    public virtual TbSchool School { get; set; } = null!;

    public virtual ICollection<TbAttendance> TbAttendances { get; set; } = new List<TbAttendance>();

    public virtual ICollection<TbCommonFeeStudentDiscount> TbCommonFeeStudentDiscounts { get; set; } = new List<TbCommonFeeStudentDiscount>();

    public virtual ICollection<TbDeletedFeeStudent> TbDeletedFeeStudents { get; set; } = new List<TbDeletedFeeStudent>();

    public virtual ICollection<TbFeeDiscount> TbFeeDiscounts { get; set; } = new List<TbFeeDiscount>();

    public virtual ICollection<TbFeeDue> TbFeeDues { get; set; } = new List<TbFeeDue>();

    public virtual ICollection<TbFeeStudent> TbFeeStudents { get; set; } = new List<TbFeeStudent>();

    public virtual ICollection<TbLibraryBookStudent> TbLibraryBookStudents { get; set; } = new List<TbLibraryBookStudent>();

    public virtual ICollection<TbMessage> TbMessages { get; set; } = new List<TbMessage>();

    public virtual ICollection<TbOtpmessage> TbOtpmessages { get; set; } = new List<TbOtpmessage>();

    public virtual ICollection<TbParentMessage> TbParentMessages { get; set; } = new List<TbParentMessage>();

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbSmsHistory> TbSmsHistories { get; set; } = new List<TbSmsHistory>();

    public virtual ICollection<TbStockStudentPaidAmount> TbStockStudentPaidAmounts { get; set; } = new List<TbStockStudentPaidAmount>();

    public virtual ICollection<TbStudentBalance> TbStudentBalances { get; set; } = new List<TbStudentBalance>();

    public virtual ICollection<TbStudentCurrentBillActivated> TbStudentCurrentBillActivateds { get; set; } = new List<TbStudentCurrentBillActivated>();

    public virtual ICollection<TbStudentDetailedDetail> TbStudentDetailedDetails { get; set; } = new List<TbStudentDetailedDetail>();

    public virtual ICollection<TbStudentFile> TbStudentFiles { get; set; } = new List<TbStudentFile>();

    public virtual ICollection<TbStudentMark> TbStudentMarks { get; set; } = new List<TbStudentMark>();

    public virtual ICollection<TbStudentPaidAmount> TbStudentPaidAmounts { get; set; } = new List<TbStudentPaidAmount>();

    public virtual ICollection<TbStudentPremotion> TbStudentPremotions { get; set; } = new List<TbStudentPremotion>();
}
