using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbStudentDetailedDetail
{
    public long Id { get; set; }

    public long StudentId { get; set; }

    public bool IsActive { get; set; }

    public DateTime TimeStamp { get; set; }

    public string? GuardianName { get; set; }

    public string? RelationShip { get; set; }

    public string? Occupation { get; set; }

    public string? GuardianAddress { get; set; }

    public string? ContactLandLine { get; set; }

    public string? ContactOfficeLine { get; set; }

    public string? PreviousSchoolName { get; set; }

    public string? PreviousStandard { get; set; }

    public DateTime? PreviousDateOfAdmission { get; set; }

    public DateTime? PreviousDateOfLeaving { get; set; }

    public string? TcFilepath { get; set; }

    public string? TcNumber { get; set; }

    public string? TcDate { get; set; }

    public string? PlaceOfbirth { get; set; }

    public string? DobcertificateFilePath { get; set; }

    public long? ReligionId { get; set; }

    public long? CategoryId { get; set; }

    public string? Caste { get; set; }

    public int? NationalityId { get; set; }

    public string? MotherTongue { get; set; }

    public string? PermanentBodyMark1 { get; set; }

    public string? PermanentBodyMark2 { get; set; }

    public string? BoardingPoint { get; set; }

    public string? KnownLanguage1 { get; set; }

    public string? KnownLanguage2 { get; set; }

    public string? Taluk { get; set; }

    public string? RevenueDistrict { get; set; }

    public int? PanchayatiRajSystemId { get; set; }

    public string? DistrictPanchayath { get; set; }

    public string? BlockPanchayath { get; set; }

    public int? InstructionMediumId { get; set; }

    public int? FirstLanguagePaper1 { get; set; }

    public int? FirstLanguagePaper2 { get; set; }

    public int? ThirdLanguage { get; set; }

    public bool? IsVaccinated { get; set; }

    public DateTime? VaccinatedDate { get; set; }

    public int? LearingDisabilityId { get; set; }

    public int? EconomicalStatus { get; set; }

    public virtual TbCategory? Category { get; set; }

    public virtual TbReligion? Religion { get; set; }

    public virtual TbStudent Student { get; set; } = null!;
}
