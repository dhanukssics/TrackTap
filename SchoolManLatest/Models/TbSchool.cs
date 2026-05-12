using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbSchool
{
    public long SchoolId { get; set; }

    public string SchoolName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? City { get; set; }

    public string? Website { get; set; }

    public string Contact { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public Guid SchoolGuidId { get; set; }

    public bool IsActive { get; set; }

    public string? State { get; set; }

    public string? FilePath { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? BillingFooterMessage { get; set; }

    public int? LibraryDueDays { get; set; }

    public bool? PaymentOption { get; set; }

    public bool SmsActive { get; set; }

    public long? SchoolType { get; set; }

    public virtual ICollection<TbAccountHead> TbAccountHeads { get; set; } = new List<TbAccountHead>();

    public virtual ICollection<TbAccountsHide> TbAccountsHides { get; set; } = new List<TbAccountsHide>();

    public virtual ICollection<TbAssetsLiabilityDatum> TbAssetsLiabilityData { get; set; } = new List<TbAssetsLiabilityDatum>();

    public virtual ICollection<TbAssetsLiabilityId> TbAssetsLiabilityIds { get; set; } = new List<TbAssetsLiabilityId>();

    public virtual ICollection<TbBalanceDummy> TbBalanceDummies { get; set; } = new List<TbBalanceDummy>();

    public virtual ICollection<TbBalance> TbBalances { get; set; } = new List<TbBalance>();

    public virtual ICollection<TbBankBookDatum> TbBankBookData { get; set; } = new List<TbBankBookDatum>();

    public virtual ICollection<TbBankBookId> TbBankBookIds { get; set; } = new List<TbBankBookId>();

    public virtual ICollection<TbBankEntry> TbBankEntries { get; set; } = new List<TbBankEntry>();

    public virtual ICollection<TbBankPercentage> TbBankPercentages { get; set; } = new List<TbBankPercentage>();

    public virtual ICollection<TbBank> TbBanks { get; set; } = new List<TbBank>();

    public virtual ICollection<TbBillCancelAccount> TbBillCancelAccounts { get; set; } = new List<TbBillCancelAccount>();

    public virtual ICollection<TbBiometricDivision> TbBiometricDivisions { get; set; } = new List<TbBiometricDivision>();

    public virtual ICollection<TbBookCategory> TbBookCategories { get; set; } = new List<TbBookCategory>();

    public virtual ICollection<TbBu> TbBus { get; set; } = new List<TbBu>();

    public virtual ICollection<TbCalenderEvent> TbCalenderEvents { get; set; } = new List<TbCalenderEvent>();

    public virtual ICollection<TbCashEntry> TbCashEntries { get; set; } = new List<TbCashEntry>();

    public virtual ICollection<TbCcavenueCourseResponse> TbCcavenueCourseResponses { get; set; } = new List<TbCcavenueCourseResponse>();

    public virtual ICollection<TbCircular> TbCirculars { get; set; } = new List<TbCircular>();

    public virtual ICollection<TbClass> TbClasses { get; set; } = new List<TbClass>();

    public virtual ICollection<TbCommonFeeStudentDiscount> TbCommonFeeStudentDiscounts { get; set; } = new List<TbCommonFeeStudentDiscount>();

    public virtual ICollection<TbDayBookDatum> TbDayBookData { get; set; } = new List<TbDayBookDatum>();

    public virtual ICollection<TbDayBookIdBank> TbDayBookIdBanks { get; set; } = new List<TbDayBookIdBank>();

    public virtual ICollection<TbDayBookId> TbDayBookIds { get; set; } = new List<TbDayBookId>();

    public virtual ICollection<TbDriver> TbDrivers { get; set; } = new List<TbDriver>();

    public virtual ICollection<TbExamPublish> TbExamPublishes { get; set; } = new List<TbExamPublish>();

    public virtual ICollection<TbExam> TbExams { get; set; } = new List<TbExam>();

    public virtual ICollection<TbExpense> TbExpenses { get; set; } = new List<TbExpense>();

    public virtual ICollection<TbFeeAlertDatum> TbFeeAlertData { get; set; } = new List<TbFeeAlertDatum>();

    public virtual ICollection<TbFee> TbFees { get; set; } = new List<TbFee>();

    public virtual ICollection<TbFile> TbFiles { get; set; } = new List<TbFile>();

    public virtual ICollection<TbHomeworkSm> TbHomeworkSms { get; set; } = new List<TbHomeworkSm>();

    public virtual ICollection<TbIncome> TbIncomes { get; set; } = new List<TbIncome>();

    public virtual ICollection<TbLaboratoryCategory> TbLaboratoryCategories { get; set; } = new List<TbLaboratoryCategory>();

    public virtual ICollection<TbLibraryBookSerialNumber> TbLibraryBookSerialNumbers { get; set; } = new List<TbLibraryBookSerialNumber>();

    public virtual ICollection<TbLibraryFine> TbLibraryFines { get; set; } = new List<TbLibraryFine>();

    public virtual ICollection<TbLogin> TbLogins { get; set; } = new List<TbLogin>();

    public virtual ICollection<TbPaymentBillNo> TbPaymentBillNos { get; set; } = new List<TbPaymentBillNo>();

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbSalaryType> TbSalaryTypes { get; set; } = new List<TbSalaryType>();

    public virtual ICollection<TbSchoolModuleMain> TbSchoolModuleMains { get; set; } = new List<TbSchoolModuleMain>();

    public virtual ICollection<TbSchoolSenderId> TbSchoolSenderIds { get; set; } = new List<TbSchoolSenderId>();

    public virtual ICollection<TbSetting> TbSettings { get; set; } = new List<TbSetting>();

    public virtual ICollection<TbSmsHead> TbSmsHeads { get; set; } = new List<TbSmsHead>();

    public virtual ICollection<TbSmsHistory> TbSmsHistories { get; set; } = new List<TbSmsHistory>();

    public virtual ICollection<TbSmsPackage> TbSmsPackages { get; set; } = new List<TbSmsPackage>();

    public virtual ICollection<TbSmtpdetail> TbSmtpdetails { get; set; } = new List<TbSmtpdetail>();

    public virtual ICollection<TbStaffFileCollection> TbStaffFileCollections { get; set; } = new List<TbStaffFileCollection>();

    public virtual ICollection<TbStaffSmshistory> TbStaffSmshistories { get; set; } = new List<TbStaffSmshistory>();

    public virtual ICollection<TbStockIncome> TbStockIncomes { get; set; } = new List<TbStockIncome>();

    public virtual ICollection<TbStockUpdate> TbStockUpdates { get; set; } = new List<TbStockUpdate>();

    public virtual ICollection<TbStudentCurrentBillActivated> TbStudentCurrentBillActivateds { get; set; } = new List<TbStudentCurrentBillActivated>();

    public virtual ICollection<TbStudentFile> TbStudentFiles { get; set; } = new List<TbStudentFile>();

    public virtual ICollection<TbStudentPremotion> TbStudentPremotions { get; set; } = new List<TbStudentPremotion>();

    public virtual ICollection<TbStudent> TbStudents { get; set; } = new List<TbStudent>();

    public virtual ICollection<TbSubject> TbSubjects { get; set; } = new List<TbSubject>();

    public virtual ICollection<TbTeacherFile> TbTeacherFiles { get; set; } = new List<TbTeacherFile>();

    public virtual ICollection<TbTeacher> TbTeachers { get; set; } = new List<TbTeacher>();

    public virtual ICollection<TbTimeTable> TbTimeTables { get; set; } = new List<TbTimeTable>();

    public virtual ICollection<TbTrip> TbTrips { get; set; } = new List<TbTrip>();

    public virtual ICollection<TbUserModuleMain> TbUserModuleMains { get; set; } = new List<TbUserModuleMain>();

    public virtual ICollection<TbVoucherNumber> TbVoucherNumbers { get; set; } = new List<TbVoucherNumber>();

    public virtual ICollection<TbWagesShowsSetting> TbWagesShowsSettings { get; set; } = new List<TbWagesShowsSetting>();
}
