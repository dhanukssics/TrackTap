using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap
{
    public class Enum
    {

    }
    public enum UserRole
    {
        School = 1,
        Staff = 2,
        Teacher = 3,
        Parent = 4,
    }

    public enum FeeType
    {
        CommonFee = 1, //Fees for all student
        SpecialFee = 2
    }
    public enum MessageType
    {
        ForStudent = 1,
        ForClass = 2,
        ForSchool = 3
    }
    public enum DataFromStatus
    {
        Cash = 0,
        Bill = 1,
        Bank = 2
    }
    public enum TravellingStatus
    {
        Start = 0,
        Ongoing = 1,
        Stop = 2,
        NotUpdated = 4
    }
    public enum SMSSendType
    {
        Student = 0,
        Staff = 1
    }
    public enum AccountType
    {
        Expense = 0,
        Income = 1,
        Reverse = 2
    }
    public enum BankType
    {
        Deposit = 0, // Expense // Debit
        Withdraw = 1// Income // Credit
    }
    public enum AssetsLiabilityType
    {
        Assets = 0,
        Liability = 1
    }
    public enum PaymentBillType
    {
        NormalBill = 0,
        BillDue = 1,
        FineBill = 2
    }
    public enum Days
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5
    }
    public enum Periods
    {
        One = 0,
        two = 1,
        three = 2,
        four = 3,
        five = 4,
        six = 5,
        seven = 6,
        eight = 7
    }
    public enum AttendanceShift
    {
        Morning = 0,
        Evening = 1
    }
    public enum AttendanceStatus
    {
        NotTaken = 0,
        Present = 1,
        Absent = 2
    }
    public enum SchoolMsgFromApp
    {
        FullSchool = 0,
        ClassWise = 1
    }
    public enum StockStatus
    {
        Active = 0,
        InActive = 1,
        Broken = 2
    }
    public enum PaymentMode
    {// in Billing section, the payment mode is hard coded , the data is starting from 1 . But the can't be start with 1 , becouse here we must have the 0th data.
        // so here we take the value from the enum with add 2. ie.,
        //Cash = 0,//=1
        Cheque = 0,//=2
        Other = 1//=3
    }
    public enum Nationality
    {
        Indian = 0
    }
    public enum EconomicalStatus
    {
        APL = 0,
        BPL = 1
    }
    public enum PanchayathirajSystem
    {
        GramaPanchayath = 0,
        Municipaliy = 1,
        Corporation = 2
    }
    public enum InstructionMedium
    {
        Malayalam = 0,
        English = 1,
        Tamil = 2,
        Kannada = 3
    }
    public enum FirstLanguagePaper1
    {
        Malayalam = 0,
        [Display(Name = "Addl Eng")]
        AddlEng = 1,
        [Display(Name = "Sanskrit (A)")]
        SanskritA = 2,
        Arabic = 3,
        Urdu = 4,
        Tamil = 5,
        Kannada = 6,
        Gujarati = 7,
        [Display(Name = "Sanskrit (O)")]
        SanskritO = 8,
    }
    public enum FirstLanguagePaper2
    {
        Malayalam = 0,
        [Display(Name = "Spl Eng")]
        SplEng = 1,
        [Display(Name = "F Science")]
        FScience = 2,
        [Display(Name = "Spl Sanskrit")]
        SplSanskrit = 3,
        [Display(Name = "Arabic (O)")]
        ArabicO = 4,
        Urdu = 5,
        Tamil = 6,
        Kannada = 7,
        Gujarati = 8,
        [Display(Name = "Sanskrit (O)")]
        SanskritO = 9,
    }
    public enum ThirdLanguage
    {
        [Display(Name = "General Knowledge")]
        GeneralKnowledge = 0,
        Hindi = 1
    }
    public enum LearningDisability
    {
        NoDisability=0,
        [Display(Name = "LD (Dyslexic)")]
        Dyslexic = 1,
        [Display(Name = "LD (Dysgraphia)")]
        Dysgraphia = 2,
        [Display(Name = "LD (Dyscalculia)")]
        Dyscalculia = 3
    }
    public enum StudentFileType
    {
        Memo=0,
        Achievement = 1
    }
    public enum SalaryType
    {
        Basic =0,
        Gross=1
    }
    public enum UserTypeModule
    {
        Staff = 0,
        Teacher = 1
    }
}
