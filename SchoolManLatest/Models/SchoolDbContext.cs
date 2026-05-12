using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrackTap.Models;

public partial class SchoolDbContext : DbContext
{
    public SchoolDbContext()
    {
    }

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OurPatient> OurPatients { get; set; }

    public virtual DbSet<Parentdetail> Parentdetails { get; set; }

    public virtual DbSet<StudentDetail> StudentDetails { get; set; }

    public virtual DbSet<StudentRem> StudentRems { get; set; }

    public virtual DbSet<TbAcademicYear> TbAcademicYears { get; set; }

    public virtual DbSet<TbAccountHead> TbAccountHeads { get; set; }

    public virtual DbSet<TbAccountsHide> TbAccountsHides { get; set; }

    public virtual DbSet<TbAccountsHideDetail> TbAccountsHideDetails { get; set; }

    public virtual DbSet<TbAddCategory> TbAddCategories { get; set; }

    public virtual DbSet<TbAllMessage> TbAllMessages { get; set; }

    public virtual DbSet<TbAssetsLiabilityDatum> TbAssetsLiabilityData { get; set; }

    public virtual DbSet<TbAssetsLiabilityId> TbAssetsLiabilityIds { get; set; }

    public virtual DbSet<TbAttendance> TbAttendances { get; set; }

    public virtual DbSet<TbAttendanceTeacher> TbAttendanceTeachers { get; set; }

    public virtual DbSet<TbBalance> TbBalances { get; set; }

    public virtual DbSet<TbBalanceDummy> TbBalanceDummies { get; set; }

    public virtual DbSet<TbBank> TbBanks { get; set; }

    public virtual DbSet<TbBankBookDatum> TbBankBookData { get; set; }

    public virtual DbSet<TbBankBookId> TbBankBookIds { get; set; }

    public virtual DbSet<TbBankEntry> TbBankEntries { get; set; }

    public virtual DbSet<TbBankEntryTest> TbBankEntryTests { get; set; }

    public virtual DbSet<TbBankPercentage> TbBankPercentages { get; set; }

    public virtual DbSet<TbBillCancelAccount> TbBillCancelAccounts { get; set; }

    public virtual DbSet<TbBiometricDivision> TbBiometricDivisions { get; set; }

    public virtual DbSet<TbBookCategory> TbBookCategories { get; set; }

    public virtual DbSet<TbBu> TbBus { get; set; }

    public virtual DbSet<TbCalenderEvent> TbCalenderEvents { get; set; }

    public virtual DbSet<TbCashEntry> TbCashEntries { get; set; }

    public virtual DbSet<TbCashEntryTest> TbCashEntryTests { get; set; }

    public virtual DbSet<TbCategory> TbCategories { get; set; }

    public virtual DbSet<TbCcavenueCourseResponse> TbCcavenueCourseResponses { get; set; }

    public virtual DbSet<TbCircular> TbCirculars { get; set; }

    public virtual DbSet<TbClass> TbClasses { get; set; }

    public virtual DbSet<TbClassList> TbClassLists { get; set; }

    public virtual DbSet<TbClub> TbClubs { get; set; }

    public virtual DbSet<TbCommonFeeStudentDiscount> TbCommonFeeStudentDiscounts { get; set; }

    public virtual DbSet<TbDayBookDatum> TbDayBookData { get; set; }

    public virtual DbSet<TbDayBookId> TbDayBookIds { get; set; }

    public virtual DbSet<TbDayBookIdBank> TbDayBookIdBanks { get; set; }

    public virtual DbSet<TbDeletedFeeStudent> TbDeletedFeeStudents { get; set; }

    public virtual DbSet<TbDeviceToken> TbDeviceTokens { get; set; }

    public virtual DbSet<TbDivision> TbDivisions { get; set; }

    public virtual DbSet<TbDriver> TbDrivers { get; set; }

    public virtual DbSet<TbExam> TbExams { get; set; }

    public virtual DbSet<TbExamPublish> TbExamPublishes { get; set; }

    public virtual DbSet<TbExamSubject> TbExamSubjects { get; set; }

    public virtual DbSet<TbExpense> TbExpenses { get; set; }

    public virtual DbSet<TbFee> TbFees { get; set; }

    public virtual DbSet<TbFeeAlertDatum> TbFeeAlertData { get; set; }

    public virtual DbSet<TbFeeClass> TbFeeClasses { get; set; }

    public virtual DbSet<TbFeeDiscount> TbFeeDiscounts { get; set; }

    public virtual DbSet<TbFeeDue> TbFeeDues { get; set; }

    public virtual DbSet<TbFeeStudent> TbFeeStudents { get; set; }

    public virtual DbSet<TbFile> TbFiles { get; set; }

    public virtual DbSet<TbHomeworkSm> TbHomeworkSms { get; set; }

    public virtual DbSet<TbIncome> TbIncomes { get; set; }

    public virtual DbSet<TbLaboratoryCategory> TbLaboratoryCategories { get; set; }

    public virtual DbSet<TbLibraryBook> TbLibraryBooks { get; set; }

    public virtual DbSet<TbLibraryBookSerialNumber> TbLibraryBookSerialNumbers { get; set; }

    public virtual DbSet<TbLibraryBookStudent> TbLibraryBookStudents { get; set; }

    public virtual DbSet<TbLibraryFine> TbLibraryFines { get; set; }

    public virtual DbSet<TbLogin> TbLogins { get; set; }

    public virtual DbSet<TbLoginAdmin> TbLoginAdmins { get; set; }

    public virtual DbSet<TbMenuList> TbMenuLists { get; set; }

    public virtual DbSet<TbMessage> TbMessages { get; set; }

    public virtual DbSet<TbOtpmessage> TbOtpmessages { get; set; }

    public virtual DbSet<TbParent> TbParents { get; set; }

    public virtual DbSet<TbParentMessage> TbParentMessages { get; set; }

    public virtual DbSet<TbPayment> TbPayments { get; set; }

    public virtual DbSet<TbPaymentBillNo> TbPaymentBillNos { get; set; }

    public virtual DbSet<TbPaymentTest> TbPaymentTests { get; set; }

    public virtual DbSet<TbPurchase> TbPurchases { get; set; }

    public virtual DbSet<TbPushDatum> TbPushData { get; set; }

    public virtual DbSet<TbReligion> TbReligions { get; set; }

    public virtual DbSet<TbResetPassword> TbResetPasswords { get; set; }

    public virtual DbSet<TbSalaryType> TbSalaryTypes { get; set; }

    public virtual DbSet<TbSalesStock> TbSalesStocks { get; set; }

    public virtual DbSet<TbSchool> TbSchools { get; set; }

    public virtual DbSet<TbSchoolModuleDetail> TbSchoolModuleDetails { get; set; }

    public virtual DbSet<TbSchoolModuleHome> TbSchoolModuleHomes { get; set; }

    public virtual DbSet<TbSchoolModuleMain> TbSchoolModuleMains { get; set; }

    public virtual DbSet<TbSchoolSenderId> TbSchoolSenderIds { get; set; }

    public virtual DbSet<TbSchoolSubModule> TbSchoolSubModules { get; set; }

    public virtual DbSet<TbSetting> TbSettings { get; set; }

    public virtual DbSet<TbSmsHead> TbSmsHeads { get; set; }

    public virtual DbSet<TbSmsHistory> TbSmsHistories { get; set; }

    public virtual DbSet<TbSmsPackage> TbSmsPackages { get; set; }

    public virtual DbSet<TbSmtpdetail> TbSmtpdetails { get; set; }

    public virtual DbSet<TbStaff> TbStaffs { get; set; }

    public virtual DbSet<TbStaffFileCollection> TbStaffFileCollections { get; set; }

    public virtual DbSet<TbStaffSmshistory> TbStaffSmshistories { get; set; }

    public virtual DbSet<TbState> TbStates { get; set; }

    public virtual DbSet<TbStockAccountHead> TbStockAccountHeads { get; set; }

    public virtual DbSet<TbStockBalance> TbStockBalances { get; set; }

    public virtual DbSet<TbStockBankEntry> TbStockBankEntries { get; set; }

    public virtual DbSet<TbStockCashEntry> TbStockCashEntries { get; set; }

    public virtual DbSet<TbStockIncome> TbStockIncomes { get; set; }

    public virtual DbSet<TbStockPayment> TbStockPayments { get; set; }

    public virtual DbSet<TbStockPaymentBillNo> TbStockPaymentBillNos { get; set; }

    public virtual DbSet<TbStockStudentBalance> TbStockStudentBalances { get; set; }

    public virtual DbSet<TbStockStudentPaidAmount> TbStockStudentPaidAmounts { get; set; }

    public virtual DbSet<TbStockSubLedgerDatum> TbStockSubLedgerData { get; set; }

    public virtual DbSet<TbStockUpdate> TbStockUpdates { get; set; }

    public virtual DbSet<TbStockVoucherNumber> TbStockVoucherNumbers { get; set; }

    public virtual DbSet<TbStudent> TbStudents { get; set; }

    public virtual DbSet<TbStudentBalance> TbStudentBalances { get; set; }

    public virtual DbSet<TbStudentCurrentBillActivated> TbStudentCurrentBillActivateds { get; set; }

    public virtual DbSet<TbStudentCwsn> TbStudentCwsns { get; set; }

    public virtual DbSet<TbStudentDetailedDetail> TbStudentDetailedDetails { get; set; }

    public virtual DbSet<TbStudentFile> TbStudentFiles { get; set; }

    public virtual DbSet<TbStudentMark> TbStudentMarks { get; set; }

    public virtual DbSet<TbStudentPaidAmount> TbStudentPaidAmounts { get; set; }

    public virtual DbSet<TbStudentPremotion> TbStudentPremotions { get; set; }

    public virtual DbSet<TbSubLedgerDatum> TbSubLedgerData { get; set; }

    public virtual DbSet<TbSubject> TbSubjects { get; set; }

    public virtual DbSet<TbTeacher> TbTeachers { get; set; }

    public virtual DbSet<TbTeacherClass> TbTeacherClasses { get; set; }

    public virtual DbSet<TbTeacherFile> TbTeacherFiles { get; set; }

    public virtual DbSet<TbTimeTable> TbTimeTables { get; set; }

    public virtual DbSet<TbTravel> TbTravels { get; set; }

    public virtual DbSet<TbTrip> TbTrips { get; set; }

    public virtual DbSet<TbUserAllotedMenu> TbUserAllotedMenus { get; set; }

    public virtual DbSet<TbUserModuleDetail> TbUserModuleDetails { get; set; }

    public virtual DbSet<TbUserModuleMain> TbUserModuleMains { get; set; }

    public virtual DbSet<TbVoucherNumber> TbVoucherNumbers { get; set; }

    public virtual DbSet<TbWagesShowsSetting> TbWagesShowsSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=50.63.166.171;Database=DB_SchoolMan_2025;User Id=sa;Password=WKO#$@@12345JK;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OurPatient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("our_patients");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("      Address");
            entity.Property(e => e.AdmissionNumberOfStudent).HasColumnName("AdmissionNumber of Student");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("      City");
            entity.Property(e => e.ContactNumber).HasColumnName("  ContactNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("      Email");
            entity.Property(e => e.ParentName).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName(" Password");
        });

        modelBuilder.Entity<Parentdetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Parentdetail");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("      Address");
            entity.Property(e => e.AdmissionNumberOfStudent)
                .HasMaxLength(255)
                .HasColumnName("AdmissionNumber of Student");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("      City");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(255)
                .HasColumnName("  ContactNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("      Email");
            entity.Property(e => e.ParentName).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName(" Password");
        });

        modelBuilder.Entity<StudentDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("StudentDetail");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AdmissionNumber).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Class)
                .HasMaxLength(255)
                .HasColumnName(" Class");
            entity.Property(e => e.ContactNumber).HasMaxLength(255);
            entity.Property(e => e.ParentName).HasMaxLength(255);
            entity.Property(e => e.State).HasMaxLength(255);
            entity.Property(e => e.StudentName).HasMaxLength(255);
        });

        modelBuilder.Entity<StudentRem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Student_Rem");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Address1).HasMaxLength(255);
            entity.Property(e => e.Class).HasColumnName(" Class");
            entity.Property(e => e.ParentName).HasMaxLength(255);
            entity.Property(e => e.State).HasMaxLength(255);
            entity.Property(e => e.StudentName).HasMaxLength(255);
        });

        modelBuilder.Entity<TbAcademicYear>(entity =>
        {
            entity.HasKey(e => e.YearId).HasName("PK__tb_Acade__C33A18CD714B8396");

            entity.ToTable("tb_AcademicYear");
        });

        modelBuilder.Entity<TbAccountHead>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tb_Accou__349DA5A6F2C525EF");

            entity.ToTable("tb_AccountHead");

            entity.Property(e => e.ForBill).HasDefaultValue(false);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbAccountHeads)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Accoun__Schoo__7B5B524B");
        });

        modelBuilder.Entity<TbAccountsHide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Accou__3214EC07FF909DB1");

            entity.ToTable("tb_AccountsHide");

            entity.Property(e => e.TimeStap).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbAccountsHides)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_AccountsHide_tb_School");
        });

        modelBuilder.Entity<TbAccountsHideDetail>(entity =>
        {
            entity.ToTable("tb_AccountsHideDetails");

            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.ExpenseAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IncomeAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbAddCategory>(entity =>
        {
            entity.ToTable("tb_AddCategory");

            entity.Property(e => e.Item).HasMaxLength(50);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<TbAllMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId);

            entity.ToTable("tb_AllMessage");

            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TbAllMessages)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_AllMessage_tb_Teacher");
        });

        modelBuilder.Entity<TbAssetsLiabilityDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Asset__3214EC0763283C1B");

            entity.ToTable("tb_AssetsLiabilityData");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Head).WithMany(p => p.TbAssetsLiabilityData)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Assets__HeadI__7D0E9093");

            entity.HasOne(d => d.School).WithMany(p => p.TbAssetsLiabilityData)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Assets__Schoo__7E02B4CC");

            entity.HasOne(d => d.User).WithMany(p => p.TbAssetsLiabilityData)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Assets__UserI__7EF6D905");
        });

        modelBuilder.Entity<TbAssetsLiabilityId>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Asset__3214EC07EAB1EADB");

            entity.ToTable("tb_AssetsLiabilityId");

            entity.HasOne(d => d.School).WithMany(p => p.TbAssetsLiabilityIds)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Assets__Schoo__7FEAFD3E");
        });

        modelBuilder.Entity<TbAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__tb_Atten__8B69261CC7BF5B5F");

            entity.ToTable("tb_Attendance");

            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbAttendances)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Attend__Class__7D439ABD");

            entity.HasOne(d => d.Division).WithMany(p => p.TbAttendances)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Attend__Divis__7E37BEF6");

            entity.HasOne(d => d.Staff).WithMany(p => p.TbAttendances)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Attend__Staff__7F2BE32F");

            entity.HasOne(d => d.Student).WithMany(p => p.TbAttendances)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Attend__Stude__00200768");
        });

        modelBuilder.Entity<TbAttendanceTeacher>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__tb_Atten__57FA49141EB9F266");

            entity.ToTable("tb_AttendanceTeacher");

            entity.Property(e => e.AttendanceId).HasColumnName("Attendance_Id");
            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TbAttendanceTeachers)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Attend__Teach__3BEAD8AC");
        });

        modelBuilder.Entity<TbBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Balan__3214EC07D7323DF8");

            entity.ToTable("tb_Balance");

            entity.Property(e => e.Closing).HasColumnType("money");
            entity.Property(e => e.CurrentDate).HasColumnType("datetime");
            entity.Property(e => e.Opening).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbBalances)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Balanc__Schoo__3EA749C6");
        });

        modelBuilder.Entity<TbBalanceDummy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Balan__3214EC076054FB30");

            entity.ToTable("tb_BalanceDummy");

            entity.Property(e => e.Closing).HasColumnType("money");
            entity.Property(e => e.CurrentDate).HasColumnType("datetime");
            entity.Property(e => e.Opening).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbBalanceDummies)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Balanc__Schoo__72E607DB");
        });

        modelBuilder.Entity<TbBank>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__tb_Banks__AA08CB13EF4F6A69");

            entity.ToTable("tb_Banks");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbBanks)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Banks__School__7849DB76");
        });

        modelBuilder.Entity<TbBankBookDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_BankB__3214EC07DCB54DEB");

            entity.ToTable("tb_BankBookData");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ChequeDate).HasColumnType("datetime");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Bank).WithMany(p => p.TbBankBookData)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankBo__BankI__72910220");

            entity.HasOne(d => d.Head).WithMany(p => p.TbBankBookData)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankBo__HeadI__73852659");

            entity.HasOne(d => d.School).WithMany(p => p.TbBankBookData)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankBo__Schoo__74794A92");

            entity.HasOne(d => d.Subledger).WithMany(p => p.TbBankBookData)
                .HasForeignKey(d => d.SubledgerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankBo__Suble__756D6ECB");

            entity.HasOne(d => d.User).WithMany(p => p.TbBankBookData)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankBo__UserI__76619304");
        });

        modelBuilder.Entity<TbBankBookId>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_BankB__3214EC07B456AAB0");

            entity.ToTable("tb_BankBookId");

            entity.HasOne(d => d.School).WithMany(p => p.TbBankBookIds)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankBo__Schoo__7755B73D");
        });

        modelBuilder.Entity<TbBankEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_BankE__3214EC0700C0E3D8");

            entity.ToTable("tb_BankEntry");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ChequeDate).HasColumnType("datetime");
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Bank).WithMany(p => p.TbBankEntries)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankEn__BankI__39E294A9");

            entity.HasOne(d => d.Head).WithMany(p => p.TbBankEntries)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankEn__HeadI__38EE7070");

            entity.HasOne(d => d.School).WithMany(p => p.TbBankEntries)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankEn__Schoo__3BCADD1B");

            entity.HasOne(d => d.User).WithMany(p => p.TbBankEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BankEn__UserI__3AD6B8E2");
        });

        modelBuilder.Entity<TbBankEntryTest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tb_BankEntryTest");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BankId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillNo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CancelStatus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DataFromStatus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.HeadId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Id).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsActive).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Migration).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModeType).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SchoolId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VoucherNumber).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TbBankPercentage>(entity =>
        {
            entity.HasKey(e => e.BankPercentageId).HasName("PK_BankPercentage");

            entity.ToTable("tb_BankPercentage");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UpdateTimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbBankPercentages)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_BankPercentage_tb_School");
        });

        modelBuilder.Entity<TbBillCancelAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_BillC__3214EC073ADF4CEB");

            entity.ToTable("tb_BillCancelAccounts");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CancelDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.TbBillCancelAccounts)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BillCa__ItemI__090A5324");

            entity.HasOne(d => d.School).WithMany(p => p.TbBillCancelAccounts)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BillCa__Schoo__08162EEB");
        });

        modelBuilder.Entity<TbBiometricDivision>(entity =>
        {
            entity.HasKey(e => e.BioDivId).HasName("PK__tb_Biome__96471CF25AA05802");

            entity.ToTable("tb_BiometricDivision");

            entity.HasOne(d => d.Division).WithMany(p => p.TbBiometricDivisions)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Biomet__Divis__02925FBF");

            entity.HasOne(d => d.School).WithMany(p => p.TbBiometricDivisions)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Biomet__Schoo__019E3B86");
        });

        modelBuilder.Entity<TbBookCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tb_BookC__19093A0B7E293B51");

            entity.ToTable("tb_BookCategory");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbBookCategories)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_BookCa__Schoo__01142BA1");
        });

        modelBuilder.Entity<TbBu>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PK__tb_Bus__6A0F60B54A54E2F5");

            entity.ToTable("tb_Bus");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbBus)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Bus__SchoolId__4E88ABD4");
        });

        modelBuilder.Entity<TbCalenderEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__tb_Calen__7944C8100114BB87");

            entity.ToTable("tb_CalenderEvent");

            entity.Property(e => e.EventDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbCalenderEvents)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Calend__Schoo__02FC7413");
        });

        modelBuilder.Entity<TbCashEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_CashE__3214EC070BB12DFC");

            entity.ToTable("tb_CashEntry");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Head).WithMany(p => p.TbCashEntries)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_CashEn__HeadI__4277DAAA");

            entity.HasOne(d => d.School).WithMany(p => p.TbCashEntries)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_CashEn__Schoo__4460231C");

            entity.HasOne(d => d.User).WithMany(p => p.TbCashEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_CashEn__UserI__436BFEE3");
        });

        modelBuilder.Entity<TbCashEntryTest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tb_CashEntryTest");

            entity.Property(e => e.AdvanceStatus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillNo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CancelStatus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DataFromStatus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.HeadId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Id).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsActive).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Migration).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReverseStatus).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SchoolId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VoucherNumber).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TbCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Categ__3214EC073B0323C9");

            entity.ToTable("tb_Category");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbCcavenueCourseResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__tb_Ccave__1AAA646C654850E2");

            entity.ToTable("tb_CcavenueCourseResponse");

            entity.Property(e => e.Amount).HasColumnType("money");

            entity.HasOne(d => d.Parent).WithMany(p => p.TbCcavenueCourseResponses)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__tb_Ccaven__Paren__03F0984C");

            entity.HasOne(d => d.School).WithMany(p => p.TbCcavenueCourseResponses)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_tb_CcavenueCourseResponse_tb_School");
        });

        modelBuilder.Entity<TbCircular>(entity =>
        {
            entity.HasKey(e => e.CircularId).HasName("PK__tb_Circu__C019C86EEE10B049");

            entity.ToTable("tb_Circular");

            entity.Property(e => e.CircularDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("USerId");

            entity.HasOne(d => d.School).WithMany(p => p.TbCirculars)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Circul__Schoo__04E4BC85");

            entity.HasOne(d => d.User).WithMany(p => p.TbCirculars)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Circul__USerI__05D8E0BE");
        });

        modelBuilder.Entity<TbClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__tb_Class__CB1927C053877ED6");

            entity.ToTable("tb_Class");

            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.AcademicYear).WithMany(p => p.TbClasses)
                .HasForeignKey(d => d.AcademicYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Class__Academ__6BE40491");

            entity.HasOne(d => d.School).WithMany(p => p.TbClasses)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Class__School__2A4B4B5E");
        });

        modelBuilder.Entity<TbClassList>(entity =>
        {
            entity.HasKey(e => e.ClassNameId).HasName("PK__tb_Class__71AFB784ED6853A0");

            entity.ToTable("tb_ClassList");
        });

        modelBuilder.Entity<TbClub>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Clubs__3214EC07C0F44B27");

            entity.ToTable("tb_Clubs");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbCommonFeeStudentDiscount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Commo__3214EC07514B9663");

            entity.ToTable("tb_CommonFeeStudentDiscount");

            entity.Property(e => e.DiscountAllowFeeDate).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OriginalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbCommonFeeStudentDiscounts)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Common__FeeId__4119A21D");

            entity.HasOne(d => d.School).WithMany(p => p.TbCommonFeeStudentDiscounts)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Common__Schoo__3F3159AB");

            entity.HasOne(d => d.Student).WithMany(p => p.TbCommonFeeStudentDiscounts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Common__Stude__40257DE4");

            entity.HasOne(d => d.User).WithMany(p => p.TbCommonFeeStudentDiscounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Common__UserI__420DC656");
        });

        modelBuilder.Entity<TbDayBookDatum>(entity =>
        {
            entity.HasKey(e => e.DayBookId).HasName("PK__tb_DayBo__91C4FAA7BD099D7D");

            entity.ToTable("tb_DayBookData");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Head).WithMany(p => p.TbDayBookData)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_DayBoo__HeadI__07C12930");

            entity.HasOne(d => d.School).WithMany(p => p.TbDayBookData)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_DayBoo__Schoo__08B54D69");

            entity.HasOne(d => d.SubLedger).WithMany(p => p.TbDayBookData)
                .HasForeignKey(d => d.SubLedgerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_DayBoo__SubLe__09A971A2");

            entity.HasOne(d => d.User).WithMany(p => p.TbDayBookData)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_DayBoo__UserI__0A9D95DB");
        });

        modelBuilder.Entity<TbDayBookId>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_DayBo__3214EC07F118A9E8");

            entity.ToTable("tb_DayBookId");

            entity.Property(e => e.ExpenseId).HasDefaultValue(1L);
            entity.Property(e => e.IncomeId).HasDefaultValue(1L);

            entity.HasOne(d => d.School).WithMany(p => p.TbDayBookIds)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_DayBoo__Schoo__0B91BA14");
        });

        modelBuilder.Entity<TbDayBookIdBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_DayBo__3214EC0738CCFF76");

            entity.ToTable("tb_DayBookIdBank");

            entity.HasOne(d => d.School).WithMany(p => p.TbDayBookIdBanks)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_DayBoo__Schoo__12C8C788");
        });

        modelBuilder.Entity<TbDeletedFeeStudent>(entity =>
        {
            entity.HasKey(e => e.DletedFeeId).HasName("PK__tb_Delet__267CB18FD976DEBC");

            entity.ToTable("tb_DeletedFeeStudent");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.FeeClass).WithMany(p => p.TbDeletedFeeStudents)
                .HasForeignKey(d => d.FeeClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Delete__FeeCl__5E54FF49");

            entity.HasOne(d => d.Student).WithMany(p => p.TbDeletedFeeStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Delete__Stude__5F492382");
        });

        modelBuilder.Entity<TbDeviceToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__tb_Devic__658FEEEABC6C1D6C");

            entity.ToTable("tb_DeviceToken");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbDivision>(entity =>
        {
            entity.HasKey(e => e.DivisionId).HasName("PK__tb_Divis__20EFC6A86E07F199");

            entity.ToTable("tb_Division");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbDivisions)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Divisi__Class__2B3F6F97");
        });

        modelBuilder.Entity<TbDriver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("PK__tb_Drive__F1B1CD048E4AEE42");

            entity.ToTable("tb_Driver");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbDrivers)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Driver__Schoo__0D7A0286");
        });

        modelBuilder.Entity<TbExam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__tb_Exams__297521C73289DC39");

            entity.ToTable("tb_Exams");

            entity.Property(e => e.ExamDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbExams)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Exams__ClassI__0E6E26BF");

            entity.HasOne(d => d.Division).WithMany(p => p.TbExams)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Exams__Divisi__0F624AF8");

            entity.HasOne(d => d.School).WithMany(p => p.TbExams)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Exams__School__10566F31");

            entity.HasOne(d => d.User).WithMany(p => p.TbExams)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Exams__UserId__114A936A");
        });

        modelBuilder.Entity<TbExamPublish>(entity =>
        {
            entity.HasKey(e => e.ExamPublishId);

            entity.ToTable("tb_ExamPublish");

            entity.Property(e => e.ExamDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbExamPublishes)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_ExamPublish_tb_Class");

            entity.HasOne(d => d.Division).WithMany(p => p.TbExamPublishes)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_ExamPublish_tb_Division");

            entity.HasOne(d => d.Exam).WithMany(p => p.TbExamPublishes)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_ExamPublish_tb_Exams");

            entity.HasOne(d => d.School).WithMany(p => p.TbExamPublishes)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_ExamPublish_tb_School");
        });

        modelBuilder.Entity<TbExamSubject>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("PK__tb_ExamS__4D9BB84A98E7A377");

            entity.ToTable("tb_ExamSubjects");

            entity.Property(e => e.ExamDate)
                .HasDefaultValueSql("(getdate())", "ExamDate")
                .HasColumnType("datetime");
            entity.Property(e => e.ExternalMark).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InternalMarks).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Mark).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubjectId).HasDefaultValue(1L, "SubjectId");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Exam).WithMany(p => p.TbExamSubjects)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_ExamSu__ExamI__123EB7A3");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.TbExamSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_ExamSu__Subje__477199F1");
        });

        modelBuilder.Entity<TbExpense>(entity =>
        {
            entity.ToTable("tb_Expense");

            entity.Property(e => e.AccountHead).IsUnicode(false);
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())", "DF_tb_Expense_Date")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_tb_Expense_IsActive");
            entity.Property(e => e.Particular).IsUnicode(false);

            entity.HasOne(d => d.School).WithMany(p => p.TbExpenses)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_Expense_tb_School");
        });

        modelBuilder.Entity<TbFee>(entity =>
        {
            entity.HasKey(e => e.FeeId).HasName("PK__tb_Fee__B387B2291CE2D411");

            entity.ToTable("tb_Fee");

            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.FeeStartDate).HasColumnType("datetime");
            entity.Property(e => e.FineAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbFees)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Fee__SchoolId__14270015");
        });

        modelBuilder.Entity<TbFeeAlertDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_FeeAl__3214EC072ADAC85F");

            entity.ToTable("tb_FeeAlertData");

            entity.Property(e => e.AlertDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbFeeAlertData)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeAle__Schoo__151B244E");
        });

        modelBuilder.Entity<TbFeeClass>(entity =>
        {
            entity.HasKey(e => e.FeeClassId).HasName("PK__tb_FeeCl__7BDF497F987AF1B5");

            entity.ToTable("tb_FeeClass");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbFeeClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeCla__Class__160F4887");

            entity.HasOne(d => d.Division).WithMany(p => p.TbFeeClasses)
                .HasForeignKey(d => d.DivisionId)
                .HasConstraintName("FK__tb_FeeCla__Divis__01342732");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbFeeClasses)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeCla__FeeId__17036CC0");
        });

        modelBuilder.Entity<TbFeeDiscount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__tb_FeeDi__E43F6D96AB5DC13A");

            entity.ToTable("tb_FeeDiscount");

            entity.Property(e => e.DiscountAmount).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbFeeDiscounts)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeDis__FeeId__17F790F9");

            entity.HasOne(d => d.Student).WithMany(p => p.TbFeeDiscounts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeDis__Stude__18EBB532");
        });

        modelBuilder.Entity<TbFeeDue>(entity =>
        {
            entity.HasKey(e => e.FeeDuesId).HasName("PK__tb_FeeDu__AA032D34EBEE603A");

            entity.ToTable("tb_FeeDues");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbFeeDues)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeDue__FeeId__19DFD96B");

            entity.HasOne(d => d.Student).WithMany(p => p.TbFeeDues)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeDue__Stude__1AD3FDA4");
        });

        modelBuilder.Entity<TbFeeStudent>(entity =>
        {
            entity.HasKey(e => e.FeeStudentId).HasName("PK__tb_FeeSt__FC46064E6748A623");

            entity.ToTable("tb_FeeStudent");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbFeeStudents)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeStu__FeeId__1BC821DD");

            entity.HasOne(d => d.Student).WithMany(p => p.TbFeeStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_FeeStu__Stude__1CBC4616");
        });

        modelBuilder.Entity<TbFile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__tb_File__6F0F98BF505EA6B0");

            entity.ToTable("tb_File");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbFiles)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK__tb_File__SchoolI__1DB06A4F");
        });

        modelBuilder.Entity<TbHomeworkSm>(entity =>
        {
            entity.HasKey(e => e.HomeworkSmsId).HasName("PK__tb_Homew__E3C4B4633E86B71B");

            entity.ToTable("tb_HomeworkSms");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Head).WithMany(p => p.TbHomeworkSms)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Homewo__HeadI__7DCDAAA2");

            entity.HasOne(d => d.School).WithMany(p => p.TbHomeworkSms)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Homewo__Schoo__7EC1CEDB");
        });

        modelBuilder.Entity<TbIncome>(entity =>
        {
            entity.ToTable("tb_Income");

            entity.Property(e => e.AccountHead).IsUnicode(false);
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())", "DF_tb_Income_Date")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_tb_Income_IsActive");
            entity.Property(e => e.Particular).IsUnicode(false);

            entity.HasOne(d => d.School).WithMany(p => p.TbIncomes)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_Income_tb_School");
        });

        modelBuilder.Entity<TbLaboratoryCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tb_Labor__19093A0BC5AAB112");

            entity.ToTable("tb_LaboratoryCategory");

            entity.HasOne(d => d.School).WithMany(p => p.TbLaboratoryCategories)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Labora__Schoo__1F98B2C1");
        });

        modelBuilder.Entity<TbLibraryBook>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__tb_Libra__3DE0C207186CE9E2");

            entity.ToTable("tb_LibraryBook");

            entity.Property(e => e.RandomNumber).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TbLibraryBooks)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Librar__Categ__208CD6FA");
        });

        modelBuilder.Entity<TbLibraryBookSerialNumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Libra__3214EC074DDD4B1C");

            entity.ToTable("tb_LibraryBookSerialNumber");

            entity.HasOne(d => d.School).WithMany(p => p.TbLibraryBookSerialNumbers)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Librar__Schoo__345EC57D");
        });

        modelBuilder.Entity<TbLibraryBookStudent>(entity =>
        {
            entity.HasKey(e => e.StudentBookId).HasName("PK__tb_Libra__F916A6E4021FC721");

            entity.ToTable("tb_LibraryBookStudent");

            entity.Property(e => e.AcceptDateTime).HasColumnType("datetime");
            entity.Property(e => e.IssueDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.TbLibraryBookStudents)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Librar__BookI__2180FB33");

            entity.HasOne(d => d.Student).WithMany(p => p.TbLibraryBookStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Librar__Stude__22751F6C");
        });

        modelBuilder.Entity<TbLibraryFine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Libra__3214EC0745F8D5C6");

            entity.ToTable("tb_LibraryFine");

            entity.Property(e => e.FineAmount).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbLibraryFines)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Librar__FeeId__3FD07829");

            entity.HasOne(d => d.School).WithMany(p => p.TbLibraryFines)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Librar__Schoo__40C49C62");
        });

        modelBuilder.Entity<TbLogin>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tb_Login__1788CC4C72DFAEB7");

            entity.ToTable("tb_Login");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbLogins)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Login__School__06CD04F7");
        });

        modelBuilder.Entity<TbLoginAdmin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__tb_Login__719FE488BB48C408");

            entity.ToTable("tb_LoginAdmin");
        });

        modelBuilder.Entity<TbMenuList>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__tb_MenuL__C99ED230A39246A7");

            entity.ToTable("tb_MenuList");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__tb_Messa__C87C0C9CE9467C8E");

            entity.ToTable("tb_Message");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.TbMessages)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Messag__Stude__245D67DE");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TbMessages)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Messag__Teach__25518C17");
        });

        modelBuilder.Entity<TbOtpmessage>(entity =>
        {
            entity.HasKey(e => e.OtpId).HasName("PK__tb_OTPMe__3143C4A366B5B251");

            entity.ToTable("tb_OTPMessage");

            entity.Property(e => e.ExpTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.Otp).HasColumnName("OTP");
            entity.Property(e => e.Otptype).HasColumnName("OTPType");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.TbOtpmessages)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_OTPMes__Stude__68D28DBC");
        });

        modelBuilder.Entity<TbParent>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PK__tb_Paren__D339516FE0DE3C57");

            entity.ToTable("tb_Parent");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbParentMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__tb_Paren__C87C0C9CAB15191E");

            entity.ToTable("tb_ParentMessage");

            entity.Property(e => e.ReadStatus).HasDefaultValue(true, "IsTermAndConditionAccepted");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Sender).WithMany(p => p.TbParentMessages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Parent__Sende__2645B050");

            entity.HasOne(d => d.Student).WithMany(p => p.TbParentMessages)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Parent__Stude__2739D489");
        });

        modelBuilder.Entity<TbPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__tb_Payme__9B556A38EA744B88");

            entity.ToTable("tb_Payment");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ChequeDate).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.MaxAmount).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Paymen__Class__282DF8C2");

            entity.HasOne(d => d.Fee).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Paymen__FeeId__29221CFB");

            entity.HasOne(d => d.IssuedPersonNavigation).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.IssuedPerson)
                .HasConstraintName("FK__tb_Paymen__Issue__73DA2C14");

            entity.HasOne(d => d.School).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Paymen__Schoo__2A164134");

            entity.HasOne(d => d.Student).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Paymen__Stude__2B0A656D");
        });

        modelBuilder.Entity<TbPaymentBillNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Payme__3214EC0792F153A5");

            entity.ToTable("tb_PaymentBillNo");

            entity.HasOne(d => d.School).WithMany(p => p.TbPaymentBillNos)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Paymen__Schoo__2BFE89A6");
        });

        modelBuilder.Entity<TbPaymentTest>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__tb_Payme__9B556A387CEC2200");

            entity.ToTable("tb_PaymentTest");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.MaxAmount).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbPurchase>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__tb_Purch__2C83A9C20C95C2B3");

            entity.ToTable("tb_Purchase");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbPushDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tb_PushData");

            entity.HasOne(d => d.School).WithMany()
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK__tb_PushDa__Schoo__2CF2ADDF");
        });

        modelBuilder.Entity<TbReligion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Relig__3214EC07778F5CA5");

            entity.ToTable("tb_Religion");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbResetPassword>(entity =>
        {
            entity.HasKey(e => e.ResetPasswordId).HasName("PK__tb_Reset__805BA262505BA0B8");

            entity.ToTable("tb_ResetPassword");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbSalaryType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Salar__3214EC0753345BC5");

            entity.ToTable("tb_SalaryType");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbSalaryTypes)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Salary__Schoo__4CC05EF3");
        });

        modelBuilder.Entity<TbSalesStock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__tb_Sales__2C83A9C2ADA44875");

            entity.ToTable("tb_SalesStock");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbSchool>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PK__tb_Schoo__3DA4675BEECB7661");

            entity.ToTable("tb_School");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbSchoolModuleDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Schoo__3214EC071449B053");

            entity.ToTable("tb_SchoolModuleDetails");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Main).WithMany(p => p.TbSchoolModuleDetails)
                .HasForeignKey(d => d.MainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_School__MainI__22951AFD");

            entity.HasOne(d => d.SchoolModule).WithMany(p => p.TbSchoolModuleDetails)
                .HasForeignKey(d => d.SchoolModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_School__Schoo__247D636F");

            entity.HasOne(d => d.SchoolSubModule).WithMany(p => p.TbSchoolModuleDetails)
                .HasForeignKey(d => d.SchoolSubModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_School__Schoo__23893F36");
        });

        modelBuilder.Entity<TbSchoolModuleHome>(entity =>
        {
            entity.ToTable("tb_SchoolModuleHome");

            entity.Property(e => e.Timestamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbSchoolModuleMain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Schoo__3214EC07CB93837D");

            entity.ToTable("tb_SchoolModuleMain");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbSchoolModuleMains)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_School__Schoo__257187A8");
        });

        modelBuilder.Entity<TbSchoolSenderId>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Schoo__3214EC074B8DCA72");

            entity.ToTable("tb_SchoolSenderId");

            entity.HasOne(d => d.School).WithMany(p => p.TbSchoolSenderIds)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_School__Schoo__6BAEFA67");
        });

        modelBuilder.Entity<TbSchoolSubModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Schoo__3214EC07B3CB5D30");

            entity.ToTable("tb_SchoolSubModule");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Main).WithMany(p => p.TbSchoolSubModules)
                .HasForeignKey(d => d.MainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_School__MainI__1DD065E0");
        });

        modelBuilder.Entity<TbSetting>(entity =>
        {
            entity.HasKey(e => e.SettingsId).HasName("PK__tb_Setti__991B19FCFE597CAF");

            entity.ToTable("tb_Settings");

            entity.Property(e => e.FeeStartDate).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbSettings)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK__tb_Settin__Schoo__2DE6D218");
        });

        modelBuilder.Entity<TbSmsHead>(entity =>
        {
            entity.HasKey(e => e.HeadId).HasName("PK__tb_SmsHe__EB3F25107A78569D");

            entity.ToTable("tb_SmsHead");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbSmsHeads)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_SmsHea__Schoo__2EDAF651");
        });

        modelBuilder.Entity<TbSmsHistory>(entity =>
        {
            entity.ToTable("tb_SmsHistory");

            entity.Property(e => e.DelivaryStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_tb_SmsHistory_IsActive");
            entity.Property(e => e.MessageDate).HasColumnType("datetime");
            entity.Property(e => e.SendStatus).IsUnicode(false);

            entity.HasOne(d => d.Head).WithMany(p => p.TbSmsHistories)
                .HasForeignKey(d => d.HeadId)
                .HasConstraintName("FK__tb_SmsHis__HeadI__2FCF1A8A");

            entity.HasOne(d => d.Schol).WithMany(p => p.TbSmsHistories)
                .HasForeignKey(d => d.ScholId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_SmsHistory_tb_School");

            entity.HasOne(d => d.Stuent).WithMany(p => p.TbSmsHistories)
                .HasForeignKey(d => d.StuentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_SmsHistory_tb_Student");
        });

        modelBuilder.Entity<TbSmsPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__tb_SmsPa__322035CCF2A4AA86");

            entity.ToTable("tb_SmsPackage");

            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.SmsRate).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbSmsPackages)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_SmsPac__Schoo__62E4AA3C");
        });

        modelBuilder.Entity<TbSmtpdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_SMTPD__3214EC07E1AC2E4B");

            entity.ToTable("tb_SMTPDetail");

            entity.HasOne(d => d.School).WithMany(p => p.TbSmtpdetails)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_SMTPDe__Schoo__32AB8735");
        });

        modelBuilder.Entity<TbStaff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__tb_Staff__96D4AB1794DB8938");

            entity.ToTable("tb_Staff");

            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Doj)
                .HasColumnType("datetime")
                .HasColumnName("DOJ");
            entity.Property(e => e.Esipercentage)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ESIPercentage");
            entity.Property(e => e.Pfpercentage)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PFPercentage");
            entity.Property(e => e.SalaryAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.TbStaffs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Staff__UserId__339FAB6E");
        });

        modelBuilder.Entity<TbStaffFileCollection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Staff__3214EC07D4D12F72");

            entity.ToTable("tb_StaffFileCollection");

            entity.Property(e => e.ReceivingDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbStaffFileCollections)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StaffF__Schoo__48EFCE0F");

            entity.HasOne(d => d.Staff).WithMany(p => p.TbStaffFileCollections)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StaffF__Staff__49E3F248");
        });

        modelBuilder.Entity<TbStaffSmshistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Staff__3214EC07E107A389");

            entity.ToTable("tb_StaffSMSHistory");

            entity.Property(e => e.DelivaryStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MessageDate).HasColumnType("datetime");
            entity.Property(e => e.SendStatus).IsUnicode(false);

            entity.HasOne(d => d.Head).WithMany(p => p.TbStaffSmshistories)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StaffS__HeadI__3493CFA7");

            entity.HasOne(d => d.Schol).WithMany(p => p.TbStaffSmshistories)
                .HasForeignKey(d => d.ScholId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StaffS__Schol__3587F3E0");

            entity.HasOne(d => d.Staff).WithMany(p => p.TbStaffSmshistories)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StaffS__Staff__367C1819");
        });

        modelBuilder.Entity<TbState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__tb_State__C3BA3B3A690D7C4B");

            entity.ToTable("tb_State");
        });

        modelBuilder.Entity<TbStockAccountHead>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tb_Stock__349DA5A679A86D0E");

            entity.ToTable("tb_StockAccountHead");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStockBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stock__3214EC074D71CB50");

            entity.ToTable("tb_StockBalance");

            entity.Property(e => e.Closing).HasColumnType("money");
            entity.Property(e => e.CurrentDate).HasColumnType("datetime");
            entity.Property(e => e.Opening).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStockBankEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stock__3214EC071175EFB5");

            entity.ToTable("tb_StockBankEntry");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ChequeDate).HasColumnType("datetime");
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStockCashEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stock__3214EC07532374A5");

            entity.ToTable("tb_StockCashEntry");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStockIncome>(entity =>
        {
            entity.ToTable("tb_StockIncome");

            entity.Property(e => e.AccountHead).IsUnicode(false);
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())", "DF_tb_StockIncome_Date")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_tb_StockIncome_IsActive");
            entity.Property(e => e.Particular).IsUnicode(false);

            entity.HasOne(d => d.School).WithMany(p => p.TbStockIncomes)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_StockIncome_tb_School");
        });

        modelBuilder.Entity<TbStockPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__tb_Stock__9B556A3860E2E565");

            entity.ToTable("tb_StockPayment");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ChequeDate).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.MaxAmount).HasColumnType("money");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStockPaymentBillNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stock__3214EC079EB45CE1");

            entity.ToTable("tb_StockPaymentBillNo");
        });

        modelBuilder.Entity<TbStockStudentBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__tb_Stock__A760D5BE9F447E19");

            entity.ToTable("tb_StockStudentBalance");

            entity.Property(e => e.Amount).HasColumnType("money");
        });

        modelBuilder.Entity<TbStockStudentPaidAmount>(entity =>
        {
            entity.HasKey(e => e.PaidId).HasName("PK__tb_Stock__B6E768CDB81C23DE");

            entity.ToTable("tb_StockStudentPaidAmount");

            entity.Property(e => e.BalanceAmount).HasColumnType("money");
            entity.Property(e => e.PaidAmount).HasColumnType("money");
            entity.Property(e => e.PreviousBalance).HasColumnType("money");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStockStudentPaidAmounts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StockS__Stude__737017C0");
        });

        modelBuilder.Entity<TbStockSubLedgerDatum>(entity =>
        {
            entity.HasKey(e => e.LedgerId).HasName("PK__tb_Stock__AE70E0CF0DBEAFD4");

            entity.ToTable("tb_StockSubLedgerData");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.AccHead).WithMany(p => p.TbStockSubLedgerData)
                .HasForeignKey(d => d.AccHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StockS__AccHe__74643BF9");
        });

        modelBuilder.Entity<TbStockUpdate>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__tb_Stock__2C83A9C2C350FAD0");

            entity.ToTable("tb_StockUpdate");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TbStockUpdates)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StockU__Categ__753864A1");

            entity.HasOne(d => d.School).WithMany(p => p.TbStockUpdates)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StockU__Schoo__762C88DA");

            entity.HasOne(d => d.User).WithMany(p => p.TbStockUpdates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_StockU__UserI__7720AD13");
        });

        modelBuilder.Entity<TbStockVoucherNumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stock__3214EC0785807228");

            entity.ToTable("tb_StockVoucherNumber");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStudent>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__tb_Stude__32C52B9917E029DB");

            entity.ToTable("tb_Student");

            entity.Property(e => e.BloodGroup).HasMaxLength(50);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Doj)
                .HasColumnType("datetime")
                .HasColumnName("DOJ");
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.IsSamrtPhoneUser).HasDefaultValue(false);
            entity.Property(e => e.ReligionCast).HasColumnName("Religion_Cast");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Bus).WithMany(p => p.TbStudents)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__BusId__2F10007B");

            entity.HasOne(d => d.Class).WithMany(p => p.TbStudents)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Class__300424B4");

            entity.HasOne(d => d.Division).WithMany(p => p.TbStudents)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Divis__30F848ED");

            entity.HasOne(d => d.Parent).WithMany(p => p.TbStudents)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__tb_Studen__Paren__31EC6D26");

            entity.HasOne(d => d.School).WithMany(p => p.TbStudents)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Schoo__32E0915F");
        });

        modelBuilder.Entity<TbStudentBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__tb_Stude__A760D5BE5D14E175");

            entity.ToTable("tb_StudentBalance");

            entity.Property(e => e.Amount).HasColumnType("money");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentBalances)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Stude__3C34F16F");
        });

        modelBuilder.Entity<TbStudentCurrentBillActivated>(entity =>
        {
            entity.HasKey(e => e.BillActiveId);

            entity.ToTable("tb_StudentCurrentBillActivated");

            entity.Property(e => e.BillingForStudentDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbStudentCurrentBillActivateds)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_StudentCurrentBillActivated_tb_School");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentCurrentBillActivateds)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_StudentCurrentBillActivated_tb_Student");
        });

        modelBuilder.Entity<TbStudentCwsn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stude__3214EC07E73DC07B");

            entity.ToTable("tb_Student_CWSN");

            entity.Property(e => e.CwsnData).HasColumnName("CWSN_Data");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbStudentDetailedDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stude__3214EC078F5EA505");

            entity.ToTable("tb_StudentDetailedDetails");

            entity.Property(e => e.ContactLandLine).HasColumnName("Contact_LandLine");
            entity.Property(e => e.ContactOfficeLine).HasColumnName("Contact_OfficeLine");
            entity.Property(e => e.DobcertificateFilePath).HasColumnName("DOBCertificate_FilePath");
            entity.Property(e => e.GuardianAddress).HasColumnName("Guardian_Address");
            entity.Property(e => e.PlaceOfbirth).HasColumnName("PlaceOFBirth");
            entity.Property(e => e.PreviousDateOfAdmission)
                .HasColumnType("datetime")
                .HasColumnName("Previous_DateOfAdmission");
            entity.Property(e => e.PreviousDateOfLeaving)
                .HasColumnType("datetime")
                .HasColumnName("Previous_DateOfLeaving");
            entity.Property(e => e.PreviousSchoolName).HasColumnName("Previous_SchoolName");
            entity.Property(e => e.PreviousStandard).HasColumnName("Previous_Standard");
            entity.Property(e => e.RevenueDistrict).HasColumnName("Revenue_District");
            entity.Property(e => e.TcDate).HasColumnName("TC_Date");
            entity.Property(e => e.TcFilepath).HasColumnName("TC_Filepath");
            entity.Property(e => e.TcNumber).HasColumnName("TC_Number");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.VaccinatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TbStudentDetailedDetails)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_Studen__Categ__3118447E");

            entity.HasOne(d => d.Religion).WithMany(p => p.TbStudentDetailedDetails)
                .HasForeignKey(d => d.ReligionId)
                .HasConstraintName("FK__tb_Studen__Relig__30242045");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentDetailedDetails)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Stude__2F2FFC0C");
        });

        modelBuilder.Entity<TbStudentFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Stude__3214EC073F7D2926");

            entity.ToTable("tb_StudentFiles");

            entity.Property(e => e.ReceivingDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbStudentFiles)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Schoo__34E8D562");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentFiles)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Stude__33F4B129");
        });

        modelBuilder.Entity<TbStudentMark>(entity =>
        {
            entity.HasKey(e => e.MarkId).HasName("PK__tb_Stude__4E30D36616F530E7");

            entity.ToTable("tb_StudentMarks");

            entity.Property(e => e.ExternalMark).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InternalMark).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Exam).WithMany(p => p.TbStudentMarks)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__ExamI__3D2915A8");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentMarks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Stude__3E1D39E1");

            entity.HasOne(d => d.Subject).WithMany(p => p.TbStudentMarks)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Subje__3F115E1A");
        });

        modelBuilder.Entity<TbStudentPaidAmount>(entity =>
        {
            entity.HasKey(e => e.PaidId).HasName("PK__tb_Stude__B6E768CD28E605E9");

            entity.ToTable("tb_StudentPaidAmount");

            entity.Property(e => e.BalanceAmount).HasColumnType("money");
            entity.Property(e => e.PaidAmount).HasColumnType("money");
            entity.Property(e => e.PreviousBalance).HasColumnType("money");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentPaidAmounts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Stude__40058253");
        });

        modelBuilder.Entity<TbStudentPremotion>(entity =>
        {
            entity.HasKey(e => e.PremotionId).HasName("PK__tb_Stude__AA1632F032294C22");

            entity.ToTable("tb_StudentPremotion");

            entity.Property(e => e.LastUpdate).HasDefaultValue(false);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.FromDivisionNavigation).WithMany(p => p.TbStudentPremotionFromDivisionNavigations)
                .HasForeignKey(d => d.FromDivision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__FromD__681373AD");

            entity.HasOne(d => d.OldClassNavigation).WithMany(p => p.TbStudentPremotions)
                .HasForeignKey(d => d.OldClass)
                .HasConstraintName("FK__tb_Studen__OldCl__147C05D0");

            entity.HasOne(d => d.School).WithMany(p => p.TbStudentPremotions)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK__tb_Studen__Schoo__119F9925");

            entity.HasOne(d => d.Student).WithMany(p => p.TbStudentPremotions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__Stude__671F4F74");

            entity.HasOne(d => d.ToDivisionNavigation).WithMany(p => p.TbStudentPremotionToDivisionNavigations)
                .HasForeignKey(d => d.ToDivision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Studen__ToDiv__690797E6");
        });

        modelBuilder.Entity<TbSubLedgerDatum>(entity =>
        {
            entity.HasKey(e => e.LedgerId).HasName("PK__tb_SubLe__AE70E0CFDC086BC5");

            entity.ToTable("tb_SubLedgerData");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.AccHead).WithMany(p => p.TbSubLedgerData)
                .HasForeignKey(d => d.AccHeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_SubLed__AccHe__40F9A68C");
        });

        modelBuilder.Entity<TbSubject>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("PK__tb_Subje__4D9BB84A911F5528");

            entity.ToTable("tb_Subjects");

            entity.Property(e => e.TmeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.SchoolINavigation).WithMany(p => p.TbSubjects)
                .HasForeignKey(d => d.SchoolI)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Subjec__Schoo__318258D2");
        });

        modelBuilder.Entity<TbTeacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__tb_Teach__EDF25964C5CDEE63");

            entity.ToTable("tb_Teacher");

            entity.Property(e => e.Doj)
                .HasColumnType("datetime")
                .HasColumnName("DOJ");
            entity.Property(e => e.Esipercentage)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ESIPercentage");
            entity.Property(e => e.Pfpercentage)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PFPercentage");
            entity.Property(e => e.SalaryAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasDefaultValue(1L, "UserId");

            entity.HasOne(d => d.School).WithMany(p => p.TbTeachers)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Teache__Schoo__36B12243");

            entity.HasOne(d => d.User).WithMany(p => p.TbTeachers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_Login_UserID");
        });

        modelBuilder.Entity<TbTeacherClass>(entity =>
        {
            entity.HasKey(e => e.TeacherClassId).HasName("PK__tb_Teach__8FE4FE1245230ED3");

            entity.ToTable("tb_TeacherClass");

            entity.HasOne(d => d.Class).WithMany(p => p.TbTeacherClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Teache__Class__49C3F6B7");

            entity.HasOne(d => d.Division).WithMany(p => p.TbTeacherClasses)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Teache__Divis__44CA3770");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TbTeacherClasses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Teache__Teach__45BE5BA9");
        });

        modelBuilder.Entity<TbTeacherFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Teach__3214EC079449FAB4");

            entity.ToTable("tb_TeacherFiles");

            entity.Property(e => e.ReceivingDate).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbTeacherFiles)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Teache__Schoo__38B96646");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TbTeacherFiles)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Teache__Teach__37C5420D");
        });

        modelBuilder.Entity<TbTimeTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_TimeT__3214EC07AB5A57A8");

            entity.ToTable("tb_TimeTable");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.TbTimeTables)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_TimeTa__Class__373B3228");

            entity.HasOne(d => d.Division).WithMany(p => p.TbTimeTables)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_TimeTa__Divis__382F5661");

            entity.HasOne(d => d.School).WithMany(p => p.TbTimeTables)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_TimeTa__Schoo__39237A9A");

            entity.HasOne(d => d.Subject).WithMany(p => p.TbTimeTables)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_TimeTa__Subje__3A179ED3");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TbTimeTables)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_TimeTa__Teach__3B0BC30C");
        });

        modelBuilder.Entity<TbTravel>(entity =>
        {
            entity.HasKey(e => e.TravelId).HasName("PK__tb_Trave__E9315235A3FA0B5D");

            entity.ToTable("tb_Travel");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Trip).WithMany(p => p.TbTravels)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Travel__TripI__0C85DE4D");
        });

        modelBuilder.Entity<TbTrip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PK__tb_Trip__51DC713E0E59E4ED");

            entity.ToTable("tb_Trip");

            entity.Property(e => e.ReachTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.TripDate).HasColumnType("datetime");

            entity.HasOne(d => d.Bus).WithMany(p => p.TbTrips)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Trip__BusId__6E01572D");

            entity.HasOne(d => d.Driver).WithMany(p => p.TbTrips)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Trip__DriverI__34C8D9D1");

            entity.HasOne(d => d.School).WithMany(p => p.TbTrips)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Trip__SchoolI__35BCFE0A");
        });

        modelBuilder.Entity<TbUserAllotedMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_UserA__3214EC070278406F");

            entity.ToTable("tb_UserAllotedMenu");

            entity.HasOne(d => d.Menu).WithMany(p => p.TbUserAllotedMenus)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK__tb_UserAl__MenuI__5B78929E");

            entity.HasOne(d => d.User).WithMany(p => p.TbUserAllotedMenus)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tb_UserAl__UserI__5A846E65");
        });

        modelBuilder.Entity<TbUserModuleDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_UserM__3214EC07C528FF54");

            entity.ToTable("tb_UserModuleDetails");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Main).WithMany(p => p.TbUserModuleDetails)
                .HasForeignKey(d => d.MainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_UserModuleDetails_tb_SchoolModuleHome");

            entity.HasOne(d => d.SubModule).WithMany(p => p.TbUserModuleDetails)
                .HasForeignKey(d => d.SubModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_UserMo__UserM__35A7EF71");

            entity.HasOne(d => d.UserModule).WithMany(p => p.TbUserModuleDetails)
                .HasForeignKey(d => d.UserModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_UserMo__UserM__31D75E8D");
        });

        modelBuilder.Entity<TbUserModuleMain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_UserM__3214EC07138BD3F2");

            entity.ToTable("tb_UserModuleMain");

            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbUserModuleMains)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_UserMo__Schoo__32CB82C6");
        });

        modelBuilder.Entity<TbVoucherNumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Vouch__3214EC07B4EBF664");

            entity.ToTable("tb_VoucherNumber");

            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbVoucherNumbers)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Vouche__Schoo__473C8FC7");
        });

        modelBuilder.Entity<TbWagesShowsSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tb_Wages__3214EC07A83392AE");

            entity.ToTable("tb_WagesShowsSettings");

            entity.Property(e => e.TimeStap).HasColumnType("datetime");

            entity.HasOne(d => d.School).WithMany(p => p.TbWagesShowsSettings)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_WagesS__Schoo__4F9CCB9E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
