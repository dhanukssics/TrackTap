using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTap.ClassLibrary;
using TrackTap.ClassLibrary.Utility;

namespace TrackTap.Data
{
    public class School : BaseReference
    {
        private tb_School school;
        public School(tb_School obj) { school = obj; }
        public School(long id) { school = _Entities.tb_School.FirstOrDefault(z => z.SchoolId == id); }
        public long SchoolId { get { return school.SchoolId; } }
        public string SchoolName { get { return school.SchoolName; } }
        public string Address { get { return school.Address; } }
        public string City { get { return school.City; } }
        public string Website { get { return school.Website; } }
        public string Contact { get { return school.Contact; } }
        public System.DateTime TimeStamp { get { return school.TimeStamp; } }
        public System.Guid SchoolGuidId { get { return school.SchoolGuidId; } }
        public bool IsActive { get { return school.IsActive; } }
        public string FilePath { get { return school.FilePath; } }
        public string State { get { return school.State; } }
        public string Latitude { get { return school.Latitude; } }
        public string Longitude { get { return school.Longitude; } }
        public string FooterMessage { get { return school.BillingFooterMessage; } }
        public bool? PaymentOption { get { return school.PaymentOption == null ? false : school.PaymentOption; } }

       

        public List<Teacher> Teachers { get { return school.tb_Teacher.Where(z => z.IsActive).ToList().Select(z => new Teacher(z)).ToList(); } }

        public List<Class> Class { get { return school.tb_Class.Where(z => z.IsActive).ToList().Select(z => new Class(z)).ToList(); } }

        public List<Division> Divisions { get { return school.tb_Class.SelectMany(z => z.tb_Division).Where(z => z.IsActive && z.tb_Class.IsActive).ToList().Select(z => new Division(z)).ToList(); } }

        public Login Login { get { return school.tb_Login.Where(z => z.SchoolId == school.SchoolId && z.IsActive).ToList().Select(z => new Login(z)).FirstOrDefault(); } }


        public List<Student> GetStudentDetails()
        {
            return school.tb_Student.Where(z => z.IsActive).ToList().Select(q => new Student(q)).ToList();
        }
        public List<Student> GetPublishedClassStudentDetails()
        {
            return school.tb_Student.Where(z => z.IsActive && z.tb_Class.PublishStatus == true).ToList().Select(q => new Student(q)).ToList();
        }
        public List<Student> GetStudentDetailsByFeeDiscount(long feeId)
        {
            var list1 = school.tb_Student.Where(z => z.IsActive).ToList().Select(q => new Student(q)).ToList();
            var list2 = school.tb_Student.SelectMany(z => z.tb_FeeDiscount).Where(z => z.IsActive && z.FeeId == feeId).ToList().Select(q => new Student(q.tb_Student)).ToList();
            var list3 = list1.Except(list2).ToList();
            return list3;
        }
        public List<FeeDiscount> GetStudentDiscountList()
        {
            return school.tb_Student.SelectMany(z => z.tb_FeeDiscount).Where(z => z.IsActive).ToList().Select(q => new FeeDiscount(q)).ToList();
        }
        public List<FeeStudent> GetSpecialFeeStudentList(long feeId)
        {
            return school.tb_Student.SelectMany(z => z.tb_FeeStudent).Where(z => z.IsActive && z.FeeId == feeId).ToList().Select(q => new FeeStudent(q)).ToList();
        }
        public List<Fee> GetAllFees()
        {
            return school.tb_Fee.Where(z => z.IsActive).ToList().Select(z => new Fee(z)).ToList();
        }

        public List<SPGetDailyReports> GetReportDailyByDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            string EndDate = endDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";

            var g = _Entities.SP_GetDaily_Report(school.SchoolId, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)).ToList().Select(z => new SPGetDailyReports(z)).ToList().OrderBy(z => z.Date).ToList();
            return g;
        }
        public List<Student> GetStudentForParent(string admissionNo)
        {
            return school.tb_Student.Where(z => z.StudentSpecialId.ToUpper() == admissionNo.ToUpper()).ToList().Select(q => new Student(q)).ToList();
        }
        public List<Login> GetStaffDetails()
        {
            return school.tb_Login.Where(z => z.IsActive && z.RoleId == (int)UserRole.Staff).ToList().Select(q => new Login(q)).ToList();
        }
        public List<SmsHistory> GetAllSmsOnSchool()
        {
            var result = _Entities.tb_SmsHistory.Where(x => x.ScholId == school.SchoolId).ToList().Select(x => new SmsHistory(x)).ToList();
            return result;

        }
        public List<BookCategory> GetBookCategory()
        {
            var result = school.tb_BookCategory.Where(x => x.IsActive).ToList().Select(x => new BookCategory(x)).ToList();
            return result;

        }
        public List<LaboratoryCategory> GetLaboratoryCategory()
        {
            var result = school.tb_LaboratoryCategory.Where(x => x.IsActive).ToList().Select(x => new LaboratoryCategory(x)).ToList();
            return result;

        }
        public List<LibraryBook> GetAllBook()
        {
            var result = school.tb_BookCategory.Where(x => x.IsActive).ToList().SelectMany(z => z.tb_LibraryBook.Where(m => m.IsActive).ToList()).ToList().Select(x => new LibraryBook(x)).ToList();
            return result;

        }
        public List<Circular> AllCircularList()
        {
            return _Entities.tb_Circular.Where(z => z.SchoolId == school.SchoolId && z.IsActive).ToList().Select(x => new Circular(x)).ToList();
        }

        public List<CalendarEvent> GetCalendarUpcomingEvent()
        {
            string StartDate = CurrentTime.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);
            return school.tb_CalenderEvent.Where(z => z.EventDate >= minDate).ToList().Select(z => new CalendarEvent(z)).ToList();
        }
        public List<CalendarEvent> GetCalendarEventByDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);
            string EndDate = endDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(EndDate);
            return school.tb_CalenderEvent.Where(z => z.EventDate >= minDate && z.EventDate <= maxDate).ToList().Select(z => new CalendarEvent(z)).ToList();
        }

        public List<IncomeExp> GetTrialBalance()
        {
            List<IncomeExp> list1 = new List<IncomeExp>();
            var income = school.tb_Income.Where(x => x.IsActive).ToList();
            var groupedAccountHeadIncome = income.Select(x =>
               new
               {
                   head = x.AccountHead.ToUpper(),
                   amount = x.Amount
               }).GroupBy(s => new { s.head })
                         .Select(g =>
               new
               {
                   head = g.Key.head,
                   amount = g.Sum(x => (Convert.ToDecimal(x.amount))),
               }
              ).ToList();
            if (groupedAccountHeadIncome.Count > 0)
            {
                foreach (var item in groupedAccountHeadIncome)
                {
                    IncomeExp one = new IncomeExp();
                    one.head = item.head;
                    one.amount = item.amount;
                    one.accountType = 1;
                    list1.Add(one);
                }
            }

            List<IncomeExp> list2 = new List<IncomeExp>();
            var expense = school.tb_Expense.Where(x => x.IsActive).ToList();
            var groupedAccountHeadExp = expense.Select(x =>
               new
               {
                   head = x.AccountHead.ToUpper(),
                   amount = x.Amount
               }).GroupBy(s => new { s.head })
                         .Select(g =>
               new
               {
                   head = g.Key.head,
                   amount = g.Sum(x => (Convert.ToDecimal(x.amount))),
               }
              ).ToList();
            if (groupedAccountHeadExp.Count > 0)
            {
                foreach (var item in groupedAccountHeadExp)
                {
                    IncomeExp one = new IncomeExp();
                    one.head = item.head;
                    one.amount = item.amount;
                    one.accountType = 2;
                    list2.Add(one);
                }
            }
            var result = list1.Concat(list2).OrderBy(x => x.head).ToList();

            return result;
        }

        public List<BalanceSheetData> GetBalanceSheet()
        {
            List<IncomeExp> list = new List<IncomeExp>();
            List<BalanceSheetData> mainDataList = new List<BalanceSheetData>();
            var exp = school.tb_Expense.Where(x => x.IsActive && x.Particular == "Asset").ToList();
            var groupedAccountHead = exp.Select(x =>
               new
               {
                   head = x.AccountHead.ToUpper(),
                   amount = x.Amount
               }).GroupBy(s => new { s.head })
                         .Select(g =>
               new
               {
                   head = g.Key.head,
                   amount = g.Sum(x => (Convert.ToDecimal(x.amount))),
               }
              ).ToList();
            if (groupedAccountHead.Count > 0)
            {
                foreach (var item in groupedAccountHead)
                {
                    IncomeExp one = new IncomeExp();
                    one.head = item.head;
                    one.amount = item.amount;
                    one.accountType = 4;
                    list.Add(one);
                }
            }

            List<IncomeExp> list2 = new List<IncomeExp>();
            var income = school.tb_Income.Where(x => x.IsActive && x.Particular == "Liabilities").ToList();
            var groupedAccountHeadInc = income.Select(x =>
               new
               {
                   head = x.AccountHead.ToUpper(),
                   amount = x.Amount
               }).GroupBy(s => new { s.head })
                         .Select(g =>
               new
               {
                   head = g.Key.head,
                   amount = g.Sum(x => (Convert.ToDecimal(x.amount))),
               }
              ).ToList();
            if (groupedAccountHeadInc.Count > 0)
            {
                foreach (var item in groupedAccountHeadInc)
                {
                    IncomeExp one = new IncomeExp();
                    one.head = item.head;
                    one.amount = item.amount;
                    one.accountType = 3;
                    list2.Add(one);
                }
            }
            //---------------Archana -----------------------------------
            int count = 0;
            if (list.Count > list2.Count)
            {
                foreach (var item in list)
                {
                    BalanceSheetData one = new BalanceSheetData();
                    one.Liabilities = item.head;
                    one.LiabilitiesAmount = Convert.ToString(item.amount);
                    if (count + 1 < list2.Count)
                    {
                        one.Asset = list2[count].head;
                        one.AssetAmount = Convert.ToString(list2[count].amount);
                    }
                    else
                    {
                        one.Asset = "";
                        one.AssetAmount = "";
                    }
                    mainDataList.Add(one);
                    count = count + 1;
                }
            }
            else
            {
                foreach (var item in list2)
                {
                    BalanceSheetData one = new BalanceSheetData();
                    one.Asset = item.head;
                    one.AssetAmount = Convert.ToString(item.amount);
                    if (count + 1 < list.Count)
                    {
                        one.Liabilities = list[count].head;
                        one.LiabilitiesAmount = Convert.ToString(list[count].amount);
                    }
                    else
                    {
                        one.Liabilities = "";
                        one.LiabilitiesAmount = "";
                    }
                    mainDataList.Add(one);
                    count = count + 1;
                }
            }
            //  var result = list.Concat(list2).OrderBy(x => x.head).ToList();
            return mainDataList;
        }

        public List<IncomeAndExpenditureData> GetIncomeAndExpenditure()
        {
            List<IncomeAndExpenditureData> mainDataList = new List<IncomeAndExpenditureData>();
            List<IncomeExp> list = new List<IncomeExp>();
            var exp = school.tb_Expense.Where(x => x.IsActive && x.Particular == "Indirect Expense" || x.Particular == "Direct Expense").ToList();
            var groupedAccountHead = exp.Select(x =>
               new
               {
                   head = x.AccountHead.ToUpper(),
                   amount = x.Amount
               }).GroupBy(s => new { s.head })
                         .Select(g =>
               new
               {
                   head = g.Key.head,
                   amount = g.Sum(x => (Convert.ToDecimal(x.amount))),
               }
              ).ToList();
            if (groupedAccountHead.Count > 0)
            {
                foreach (var item in groupedAccountHead)
                {
                    IncomeExp one = new IncomeExp();
                    one.head = item.head;
                    one.amount = item.amount;
                    one.accountType = 5;
                    list.Add(one);
                }
            }

            List<IncomeExp> list2 = new List<IncomeExp>();
            var income = school.tb_Income.Where(x => x.IsActive && x.Particular == "Indirect Income" || x.Particular == "Direct Income").ToList();
            var groupedAccountHeadInc = income.Select(x =>
               new
               {
                   head = x.AccountHead.ToUpper(),
                   amount = x.Amount
               }).GroupBy(s => new { s.head })
                         .Select(g =>
               new
               {
                   head = g.Key.head,
                   amount = g.Sum(x => (Convert.ToDecimal(x.amount))),
               }
              ).ToList();
            if (groupedAccountHeadInc.Count > 0)
            {
                foreach (var item in groupedAccountHeadInc)
                {
                    IncomeExp one = new IncomeExp();
                    one.head = item.head;
                    one.amount = item.amount;
                    one.accountType = 6;
                    list2.Add(one);
                }
            }

            //-------------------------------------------------------------
            int count = 0;
            if (list.Count > list2.Count)
            {
                foreach (var item in list)
                {
                    IncomeAndExpenditureData one = new IncomeAndExpenditureData();
                    one.Expenditure = item.head;
                    one.ExpenditureAmount = Convert.ToString(item.amount);
                    if (count + 1 < list2.Count)
                    {
                        one.Income = list2[count].head;
                        one.IncomeAmount = Convert.ToString(list2[count].amount);
                    }
                    else
                    {
                        one.Income = "";
                        one.IncomeAmount = "";
                    }
                    mainDataList.Add(one);
                    count = count + 1;
                }
            }
            else
            {
                foreach (var item in list2)
                {
                    IncomeAndExpenditureData one = new IncomeAndExpenditureData();
                    one.Income = item.head;
                    one.IncomeAmount = Convert.ToString(item.amount);
                    if (count + 1 < list.Count)
                    {
                        one.Expenditure = list[count].head;
                        one.ExpenditureAmount = Convert.ToString(list[count].amount);
                    }
                    else
                    {
                        one.Expenditure = "";
                        one.ExpenditureAmount = "";
                    }
                    mainDataList.Add(one);
                    count = count + 1;
                }
            }
            // return list.Concat(list2).OrderBy(x => x.head).ToList(); 
            return mainDataList;
        }
        public List<Exams> AllExamList(string Classname)
        {
            List<Exams> data = new List<Exams>();
            if (Classname != string.Empty)
            {
                if (Classname != null)
                {
                    var classId = _Entities.tb_Class.Where(x => x.Class == Classname && x.IsActive && x.SchoolId == SchoolId).FirstOrDefault();
                    data = _Entities.tb_Exams.Where(x => x.SchoolId == school.SchoolId && x.IsActive && x.ClassId == classId.ClassId).ToList().Select(x => new Exams(x)).ToList();
                }
                else
                {
                    data = _Entities.tb_Exams.Where(x => x.SchoolId == school.SchoolId && x.IsActive).ToList().Select(x => new Exams(x)).ToList();
                }
            }
            else
            {
                data = _Entities.tb_Exams.Where(x => x.SchoolId == school.SchoolId && x.IsActive).ToList().Select(x => new Exams(x)).ToList();
            }
            return data;
        }
        public List<Trip> GetTripDetailById(DateTime selDate, long busId)
        {
            string StartDate = selDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);
            string EndDate = minDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(EndDate);
            return school.tb_Trip.Where(z => z.TripDate >= minDate && z.TripDate <= maxDate && z.BusId == busId).ToList().Select(z => new Trip(z)).ToList();
        }
        public tb_File GetSchoolDiaryLink()
        {
            return school.tb_File.Where(z => z.IsActive && z.FileModule == 1).FirstOrDefault();
        }
        //----------------- For Account Head Data------------- 06-Apr-2018
        public List<SubLedgerData> GetAccountDetails()
        {
            var data = school.tb_AccountHead.Where(x => x.IsActive).SelectMany(z => z.tb_SubLedgerData).Where(x => x.IsActive).ToList().Select(z => new SubLedgerData(z)).ToList();
            return data;

        }
        public List<SubLedgerData> GetAccountDetails_new(Int64 schoolid)
        {
            var data = school.tb_AccountHead.Where(x => x.IsActive && x.SchoolId== schoolid).SelectMany(z => z.tb_SubLedgerData).Where(x => x.IsActive).ToList().Select(z => new SubLedgerData(z)).ToList();
            return data;

        }
        public List<tb_AddCategory> GetLaboratoryCategory(Int64 schoolid)
        {

            var data = _Entities.tb_AddCategory.Where(x => x.SchoolId == schoolid).ToList();
            return data;

        }

        public List<tb_LaboratoryCategory> GetCategorynames()
        {

            var data = _Entities.tb_LaboratoryCategory.ToList();
            return data;

        }
        public string CurrentStatus()
        {
            decimal expense = school.tb_DayBookData.Where(x => x.IsActive && x.TypeId == 0).Sum(x => x.Amount);
            decimal income = school.tb_DayBookData.Where(x => x.IsActive && x.TypeId == 1).Sum(x => x.Amount);
            string amout = "";
            if (expense > income)
            {
                decimal diff = expense - income;
                amout = "Loss : " + diff + " /-";
            }
            else
            {
                decimal diff = income - expense;
                amout = "Profit : " + diff + " /-";
            }
            return amout;
        }
        //------------------For get cash book report 
        public List<SPCashBookReportData> GetCashBookDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.ToString("MM-dd-yyyy");
            string EndDate = endDate.ToString("MM-dd-yyyy");
            //string StartDate = startDate.ToString("dd-MM-yyyy");
            //string EndDate = endDate.ToString("dd-MM-yyyy");
            //var data = _Entities.sp_CashBookReportData(startDate, endDate,school.SchoolId).ToList().Select(x => new SPCashBookReportData(x)).ToList();
            var xx = Convert.ToDateTime(StartDate);
            var x2x = Convert.ToDateTime(EndDate);

            var data = _Entities.sp_CashBookReportData(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), school.SchoolId).ToList().Select(x => new SPCashBookReportData(x)).ToList();
            return data;
        }
        public List<sp_CashBookReportSummary_Result> GetCashBookDateSummary(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.ToString("MM-dd-yyyy");
            string EndDate = endDate.ToString("MM-dd-yyyy");
            //string StartDate = startDate.ToString("dd-MM-yyyy");
            //string EndDate = endDate.ToString("dd-MM-yyyy");
            //var data = _Entities.sp_CashBookReportData(startDate, endDate,school.SchoolId).ToList().Select(x => new SPCashBookReportData(x)).ToList();
            var xx = Convert.ToDateTime(StartDate);
            var x2x = Convert.ToDateTime(EndDate);

            var data = _Entities.sp_CashBookReportSummary(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), school.SchoolId).ToList();
            return data;
        }
        public List<sp_CashBookDailyReport_Result> GetCashBookDailyReportDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.ToString("MM-dd-yyyy");
            string EndDate = endDate.ToString("MM-dd-yyyy");
            //string StartDate = startDate.ToString("dd-MM-yyyy");
            //string EndDate = endDate.ToString("dd-MM-yyyy");
            //var data = _Entities.sp_CashBookReportData(startDate, endDate,school.SchoolId).ToList().Select(x => new SPCashBookReportData(x)).ToList();
            var xx = Convert.ToDateTime(StartDate);
            var x2x = Convert.ToDateTime(EndDate);

            var data = _Entities.sp_CashBookDailyReport(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), school.SchoolId).ToList();//.Select(x => new SpCashBookDailyReport(x)).ToList();
            var xyz = data.OrderBy(z => z.VoucherNo).ToList();//.GroupBy(z => z.EntryDate).ToList();
            return xyz;
        }
        public List<LedgerDataModel> GetLedgerData(DateTime startDate, DateTime endDate, long headId, int typeId)
        {
            string StartDate = startDate.ToString("MM-dd-yyyy");
            string EndDate = endDate.ToString("MM-dd-yyyy");

            var data = _Entities.SP_LEDGERDATAREPORT(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), school.SchoolId).ToList();
            if (headId != 0)
            {
                data = data.Where(x => x.HeadId == headId && x.FromData == typeId).ToList();
            }
            List<LedgerDataModel> dataList = new List<LedgerDataModel>();
            if (data.Count > 0)
            {
                var headList = data.Select(x => x.HeadId).Distinct().ToList();
                if (headList.Count > 0)
                {
                    foreach (var item in headList)
                    {
                        LedgerDataModel one = new LedgerDataModel();
                        one.HeadId = item;
                        var subList = data.Where(x => x.HeadId == item).ToList();
                        one.HeadName = subList[0].AccHeadName;
                        one.CreditTotal = subList.Sum(x => x.Credit) ?? 0;
                        one.DebitTotal = subList.Sum(x => x.Debit) ?? 0;
                        one.list = new List<SubLedgerDetails>();
                        foreach (var sub in subList)
                        {
                            SubLedgerDetails subOne = new SubLedgerDetails();
                            subOne.EntryDate = sub.EntryDate ?? CurrentTime;
                            subOne.VoucherNumber = sub.VoucherNo;
                            subOne.DebitAmount = sub.Debit ?? 0;
                            subOne.CreditAmount = sub.Credit ?? 0;
                            subOne.Narration = sub.Narration;
                            subOne.Symbol = subOne.DebitAmount == 0 ? "Cr" : "Dr";
                            one.list.Add(subOne);
                        }
                        dataList.Add(one);
                    }
                    LedgerDataModel oneTotal = new LedgerDataModel();
                    oneTotal.HeadId = 0;
                    oneTotal.HeadName = "";
                    oneTotal.list = new List<SubLedgerDetails>();
                    SubLedgerDetails subTotal = new SubLedgerDetails();
                    subTotal.Narration = "Total";
                    subTotal.DebitAmount = data.Sum(x => x.Debit ?? 0);
                    subTotal.CreditAmount = data.Sum(x => x.Credit ?? 0);
                    oneTotal.list.Add(subTotal);
                    dataList.Add(oneTotal);
                }
            }
            return dataList;
        }

        public List<SPDayBookStatus> GetCashBookStatus(DateTime startDate, DateTime endDate)
        {
            var data = _Entities.SP_DayBookStatus(startDate, endDate, school.SchoolId).ToList().Select(x => new SPDayBookStatus(x)).ToList();
            return data;
        }
        //-------------------For get Bank List
        public List<Banks> GetBanksDetails()
        {
            var data = school.tb_Banks.Where(x => x.IsActive).ToList().Select(z => new Banks(z)).ToList();
            return data;
        }
        public decimal GetCurrentBalance()
        {
            decimal amount;
            var deposit = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 0).Sum(x => x.Amount);
            var withdraw = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 1).Sum(x => x.Amount);
            //if (deposit > withdraw)
            amount = deposit - withdraw;
            //else
            //    amount = 0;
            return amount;
        }


        public List<sp_TrialBalance_Result> GetTrialBalanceList(DateTime StartDate, DateTime Today)
        {
            var data = _Entities.sp_TrialBalance(school.SchoolId, StartDate, Today).ToList();
            var balance = data.Sum(x => x.Income) - data.Sum(x => x.Expense);
            sp_TrialBalance_Result one = new sp_TrialBalance_Result();
            one.AccHeadName = "Total";
            one.Expense = data.Sum(x => x.Expense);
            one.Income = data.Sum(x => x.Income);
            data.Add(one);
            //sp_TrialBalance_Result one1 = new sp_TrialBalance_Result();
            //one1.AccHeadName = "Cash in hand";
            //one1.Expense = balance;
            //data.Add(one1);
            var cashBookdata = _Entities.SP_DayBookStatus(StartDate, Today, school.SchoolId).ToList().Select(x => new SPDayBookStatus(x)).ToList();
            sp_TrialBalance_Result one1 = new sp_TrialBalance_Result();
            one1.AccHeadName = "Cash in hand";
            if (cashBookdata[0].ClosingBalance > 0)
                one1.Expense = cashBookdata[0].ClosingBalance;
            else
                one1.Income = cashBookdata[0].ClosingBalance;
            data.Add(one1);
            return data;
        }
        public List<sp_BalanceSheet_Result> GetBalanceSheetList()
        {
            List<sp_BalanceSheet_Result> data = new List<sp_BalanceSheet_Result>();
            var dataFull = _Entities.sp_BalanceSheet(school.SchoolId).ToList();
            var assetsData = dataFull.Where(x => x.TypeId == 0).ToList();
            var liabilityData = dataFull.Where(x => x.TypeId == 1).ToList();
            int count = 0;
            if (assetsData.Count > liabilityData.Count)
            {
                foreach (var item in assetsData)
                {
                    sp_BalanceSheet_Result one = new sp_BalanceSheet_Result();
                    one.Head = item.Head;
                    one.Assets = item.Assets;
                    one.AssetsAmount = item.AssetsAmount;
                    if (count < liabilityData.Count)
                    {
                        one.Liability = liabilityData[count].Liability;
                        one.LiabilityAmount = liabilityData[count].LiabilityAmount;
                    }
                    else
                    {
                        one.Liability = item.Liability;
                        one.LiabilityAmount = item.LiabilityAmount;
                    }
                    data.Add(one);
                    count = count + 1;
                }
            }
            else
            {
                foreach (var item in liabilityData)
                {
                    sp_BalanceSheet_Result one = new sp_BalanceSheet_Result();
                    one.Head = item.Head;
                    if (count < assetsData.Count)
                    {
                        one.Assets = assetsData[count].Assets;
                        one.AssetsAmount = assetsData[count].AssetsAmount;
                    }
                    else
                    {
                        one.Assets = item.Assets;
                        one.AssetsAmount = item.AssetsAmount;
                    }
                    one.Liability = item.Liability;
                    one.LiabilityAmount = item.LiabilityAmount;
                    data.Add(one);
                    count = count + 1;
                }
            }
            var cashBookdata = _Entities.SP_DayBookStatus(CurrentTime, CurrentTime, school.SchoolId).ToList().Select(x => new SPDayBookStatus(x)).ToList();
            if (cashBookdata.Count > 0)
            {
                sp_BalanceSheet_Result cashBook = new sp_BalanceSheet_Result();
                if (cashBookdata[0].ClosingBalance > 0)
                {
                    cashBook.Assets = "Cash In Hand";
                    cashBook.AssetsAmount = cashBookdata[0].ClosingBalance;
                }
                else
                {
                    cashBook.Liability = "Cash In Hand";
                    cashBook.LiabilityAmount = cashBookdata[0].ClosingBalance;
                }
                cashBook.Head = "CashBook";
                data.Add(cashBook);
            }
            sp_BalanceSheet_Result balance = new sp_BalanceSheet_Result();
            balance.Assets = "Total";
            balance.AssetsAmount = data.Sum(x => x.AssetsAmount);
            balance.Liability = "Total";
            balance.LiabilityAmount = data.Sum(x => x.LiabilityAmount);
            balance.Head = "Total";
            data.Add(balance);
            return data;
        }
        public List<sp_BankStatement_Result> GetBankStatementList(DateTime startDate, DateTime endDate, long BankId)
        {
            var data = _Entities.sp_BankStatement(school.SchoolId, startDate, endDate, BankId).ToList();
            return data;
        }
        public decimal CurrentBankBalance(DateTime startDate, DateTime enddate)
        {
            string StartDate = startDate.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);
            string EndDate = enddate.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(EndDate);
            var Withdraw = school.tb_BankBookData.Where(x => x.EntryDate > minDate && x.EntryDate <= maxDate && x.IsActive && x.TypeId == 1).Sum(x => x.Amount);
            var Deposit = school.tb_BankBookData.Where(x => x.EntryDate > minDate && x.EntryDate <= maxDate && x.IsActive && x.TypeId == 0).Sum(x => x.Amount);
            var balance = Deposit - Withdraw;
            return balance;
        }
        public Tuple<decimal, int, int, decimal, decimal> Statements(DateTime startDate, DateTime enddate, long BankId)
        {
            //decimal OpeningBalance = Convert.ToDecimal(((List<SPDayBookStatus>)GetCashBookStatus(startDate, enddate))[0].OpeningBalance); 
            //decimal OpeningBalance = CurrentBankBalance(startDate, enddate);
            //decimal OpeningBalance = GetCurrentBalance();
            var OpeningBalance = _Entities.sp_BankYesterdayBalance(startDate, enddate, school.SchoolId, BankId).ToList().Select(x => new SPBankYesterdayBalances(x)).FirstOrDefault();
            decimal balance = Convert.ToDecimal(OpeningBalance.Balance);
            var data = GetBankStatementList(startDate, enddate, BankId);
            int CrCount = data.Count(x => x.Deposit != 0);
            int DrCount = data.Count(x => x.Withdraw != 0);
            decimal credits = data.Sum(x => x.Deposit ?? 0);
            decimal debits = data.Sum(x => x.Withdraw ?? 0);
            return new Tuple<decimal, int, int, decimal, decimal>(balance, DrCount, CrCount, debits, credits);
        }
        public List<ReceiptPaymentDataModel> GetReceiptPayment(DateTime startdate, DateTime enddate)
        {

            var receiptPayment = new TrackTap.Data.School(school.SchoolId).GetReceiptPaymentBankData();
            var openingBankBalance = new TrackTap.Data.School(school.SchoolId).GetReceiptPaymentBankDataByDate(startdate);
            var cashInHand = new TrackTap.Data.School(school.SchoolId).GetCashBookStatus(startdate, enddate);
            decimal cashInHandRP = cashInHand.FirstOrDefault().OpeningBalance ?? 0;
            decimal closingBalanceRP = cashInHand.FirstOrDefault().ClosingBalance ?? 0;

            List<ReceiptPaymentDataModel> list = new List<ReceiptPaymentDataModel>();
            //ReceiptPaymentDataModel openingBankBalanceRP = new ReceiptPaymentDataModel();
            //openingBankBalanceRP.Type = 0;
            //openingBankBalanceRP.Receipt = "Opening Balance";
            //openingBankBalanceRP.Payment = "";
            //openingBankBalanceRP.ReceiptNarration = "";
            //openingBankBalanceRP.PaymentNarration = "";
            //openingBankBalanceRP.ReceiptAmount = cashInHand.FirstOrDefault().OpeningBalance ?? 0;
            //openingBankBalanceRP.PaymentAmount = 0;
            //list.Add(openingBankBalanceRP);
            //foreach (var item in receiptPayment)
            //{
            //    ReceiptPaymentDataModel receiptPaymentRP = new ReceiptPaymentDataModel();
            //    receiptPaymentRP.Type = 0;
            //    receiptPaymentRP.Receipt = item.BankName;
            //    receiptPaymentRP.ReceiptAmount = item.amount;
            //    receiptPaymentRP.ReceiptNarration = "";
            //    receiptPaymentRP.Payment = "";
            //    receiptPaymentRP.PaymentAmount = 0;
            //    receiptPaymentRP.PaymentNarration = "";
            //    list.Add(receiptPaymentRP);
            //}




            var data = _Entities.sp_ReceiptAndPaymentData(startdate, enddate, school.SchoolId).ToList();
            var IncomeData = data.Where(x => x.TypeId == 1).GroupBy(x => new { x.TypeId, x.AccHeadName }).Select(y => new sp_ReceiptAndPaymentData_Result
            {
                TypeId = y.Key.TypeId,
                AccHeadName = y.Key.AccHeadName,
                Expense = y.Sum(x => x.Expense),
                Income = y.Sum(x => x.Income)

            }).ToList();
            var ExpenseData = data.Where(x => x.TypeId == 0).GroupBy(x => new { x.TypeId, x.AccHeadName }).Select(y => new sp_ReceiptAndPaymentData_Result
            {
                TypeId = y.Key.TypeId,
                AccHeadName = y.Key.AccHeadName,
                Expense = y.Sum(x => x.Expense),
                Income = y.Sum(x => x.Income)
            }).ToList();
            int count = 0;
            if (data.Count > 0)
            {
                if (IncomeData.Count >= ExpenseData.Count)
                {
                    foreach (var item in IncomeData)
                    {
                        ReceiptPaymentDataModel one = new ReceiptPaymentDataModel();
                        one.Type = item.TypeId;
                        one.Receipt = item.AccHeadName;
                        one.ReceiptAmount = item.Income ?? 0;
                        one.ReceiptNarration = item.Narration;
                        try
                        {
                            one.Payment = ExpenseData[count].AccHeadName;
                            one.PaymentAmount = ExpenseData[count].Expense ?? 0;
                            one.PaymentNarration = ExpenseData[count].Narration;
                        }
                        catch
                        {
                            one.Payment = "";
                            one.PaymentAmount = 0;
                            one.PaymentNarration = "";
                        }

                        list.Add(one);
                        count = count + 1;
                    }

                }
                else
                {
                    foreach (var item in ExpenseData)
                    {
                        ReceiptPaymentDataModel one = new ReceiptPaymentDataModel();
                        one.Type = item.TypeId;
                        one.Payment = item.AccHeadName;
                        one.PaymentAmount = item.Expense ?? 0;
                        one.PaymentNarration = item.Narration;
                        try
                        {
                            one.Receipt = IncomeData[count].AccHeadName;
                            one.ReceiptAmount = IncomeData[count].Income ?? 0;
                            one.ReceiptNarration = IncomeData[count].Narration;
                        }
                        catch
                        {
                            one.Receipt = "";
                            one.ReceiptAmount = 0;
                            one.ReceiptNarration = "";
                        }
                        list.Add(one);
                        count = count + 1;
                    }
                }
                ReceiptPaymentDataModel total = new ReceiptPaymentDataModel();
                total.Type = 0;
                total.Receipt = "Total";
                total.Payment = "Total";
                total.ReceiptNarration = "";
                total.PaymentNarration = "";
                total.ReceiptAmount = list.Sum(x => x.ReceiptAmount) + cashInHandRP;
                total.PaymentAmount = list.Sum(x => x.PaymentAmount);
                list.Add(total);
            }
            return list;
        }
        public List<BankBalanceAmountModel> GetReceiptPaymentBankData()
        {
            List<BankBalanceAmountModel> list = new List<BankBalanceAmountModel>();
            var data = school.tb_BankBookData.Where(x => x.IsActive).ToList().Select(x => new BankBookData(x)).ToList();
            var banks = school.tb_Banks.Where(x => x.IsActive).ToList();
            foreach (var item in banks)
            {
                BankBalanceAmountModel one = new BankBalanceAmountModel();
                one.BankName = item.BankName;
                var deposit = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 0 && x.BankId == item.BankId).Sum(x => x.Amount);
                var withdraw = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 1 && x.BankId == item.BankId).Sum(x => x.Amount);
                one.amount = deposit - withdraw;
                list.Add(one);
            }
            return list;
        }
        public List<BankBalanceAmountModel> GetReceiptPaymentBankDataByDate(DateTime eDate)
        {
            List<BankBalanceAmountModel> list = new List<BankBalanceAmountModel>();
            var data = school.tb_BankBookData.Where(x => x.IsActive).ToList().Select(x => new BankBookData(x)).ToList();
            var banks = school.tb_Banks.Where(x => x.IsActive).ToList();
            foreach (var item in banks)
            {
                BankBalanceAmountModel one = new BankBalanceAmountModel();
                one.BankName = item.BankName;
                var deposit = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 0 && x.BankId == item.BankId && x.EntryDate < eDate).Sum(x => x.Amount);
                var withdraw = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 1 && x.BankId == item.BankId && x.EntryDate < eDate).Sum(x => x.Amount);
                one.amount = deposit - withdraw;
                list.Add(one);
            }
            return list;
        }
        public decimal GetBankCurrentBalance(long BankId)
        {
            decimal amount;
            var deposit = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 0 && x.BankId == BankId).Sum(x => x.Amount);
            var withdraw = school.tb_BankBookData.Where(x => x.IsActive && x.TypeId == 1 && x.BankId == BankId).Sum(x => x.Amount);
            amount = deposit - withdraw;
            return amount;
        }
        public List<Subjects> GetAllSubjects()
        {
            var data = school.tb_Subjects.Where(x => x.IsActive).ToList().Select(z => new Subjects(z)).ToList();
            return data;
        }

        public List<TimetableListingModel> GetTimetable(long ClassId, long DivisionId)
        {
            var data = _Entities.tb_TimeTable.Where(x => x.ClassId == ClassId && x.DivisionId == DivisionId && x.IsActive).ToList().OrderBy(x => x.Periods).Select(x => new TimeTable(x)).ToList();
            List<TimetableListingModel> list = new List<TimetableListingModel>();
            for (int i = 0; i < 6; i++)
            {
                var newData = data.Where(x => x.DayId == i).ToList();
                TimetableListingModel one = new TimetableListingModel();
                if (newData.Count > 0)
                {
                    one.DayName = newData[0].DayName;
                    one.One = newData.Where(x => x.Periods == 1).Select(x => x.Subject).FirstOrDefault();
                    one.two = newData.Where(x => x.Periods == 2).Select(x => x.Subject).FirstOrDefault();
                    one.three = newData.Where(x => x.Periods == 3).Select(x => x.Subject).FirstOrDefault();
                    one.four = newData.Where(x => x.Periods == 4).Select(x => x.Subject).FirstOrDefault();
                    one.five = newData.Where(x => x.Periods == 5).Select(x => x.Subject).FirstOrDefault();
                    one.six = newData.Where(x => x.Periods == 6).Select(x => x.Subject).FirstOrDefault();
                    one.seven = newData.Where(x => x.Periods == 7).Select(x => x.Subject).FirstOrDefault();
                    one.eight = newData.Where(x => x.Periods == 8).Select(x => x.Subject).FirstOrDefault();
                }
                else
                {
                    if (i == 0)
                        one.DayName = "Monday";
                    else if (i == 1)
                        one.DayName = "Tuesday";
                    else if (i == 2)
                        one.DayName = "Wednesday";
                    else if (i == 3)
                        one.DayName = "Thursday";
                    else if (i == 4)
                        one.DayName = "Friday";
                    if (i == 5)
                        one.DayName = "Saturday";
                    one.One = "";
                    one.two = "";
                    one.three = "";
                    one.four = "";
                    one.five = "";
                    one.six = "";
                    one.seven = "";
                    one.eight = "";
                }
                list.Add(one);
            }
            return list;
        }
        public List<Payment> GetDetailedCollectionReportDate(DateTime StartDate, DateTime endDate)
        {
            var cccc = _Entities.sp_DetailedCollectionReport(school.SchoolId, StartDate, endDate).ToList();
            List<Payment> data = _Entities.sp_DetailedCollectionReport(school.SchoolId, StartDate, endDate).ToList().Select(x => new Payment(x.PaymentId)).ToList();
            return data;
        }
        public List<Student> GetAllStudentdata(long classId, long divisionId)
        {
            if (divisionId == 0)
            {
                return school.tb_Student.Where(x => x.ClassId == classId && x.IsActive).ToList().OrderBy(x => x.ClassId).ThenBy(x => x.DivisionId).ThenBy(x => x.StundentName).Select(x => new Student(x)).ToList();
            }
            else
            {
                return school.tb_Student.Where(x => x.ClassId == classId && x.DivisionId == divisionId && x.IsActive).ToList().OrderBy(x => x.ClassId).ThenBy(x => x.DivisionId).ThenBy(x => x.StundentName).Select(x => new Student(x)).ToList();
            }
        }






        public List<SP_GetLibraryDueBook_Result> GetBookDueList()
        {
            string currentDate = CurrentTime.ToString("MM/dd/yyyy");
            DateTime today = Convert.ToDateTime(currentDate);
            var data = _Entities.SP_GetLibraryDueBook(school.SchoolId, today).ToList();
            return data;
        }
        public List<SP_GetDailyFeeCollection_Home_Result> GetDailyFeeCollectionHome()
        {
            string StartDate = CurrentTime.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);
            string EndDate = CurrentTime.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(EndDate);
            var data = _Entities.SP_GetDailyFeeCollection_Home(school.SchoolId, minDate, maxDate).ToList();
            return data;
        }
        public List<SP_GetLibraryBookCount_Home_Result> GetLibraryBookountHome()
        {
            var data = _Entities.SP_GetLibraryBookCount_Home(school.SchoolId).ToList();
            return data;
        }
        public List<SPMonthlyAttendance> GetMonthlyAttendancereportReportDate(DateTime StartDate, long ClassId, long DivionId, int ShiftId)
        {
            var data = _Entities.sp_MonthlyAttendance(StartDate, school.SchoolId, Convert.ToBoolean(ShiftId), ClassId, DivionId).ToList().Select(x => new SPMonthlyAttendance(x)).ToList();
            return data;

        }
        //Created by Gayathri A for 24/01/2024 created teacher attendance report
        public List<SPMonthlyAttendanceTeacher> GetMonthlyAttendancereportReportDateTeacher(DateTime StartDate,int ShiftId)
        {
            var data = _Entities.sp_MonthlyAttendanceTeacher(StartDate, school.SchoolId, Convert.ToBoolean(ShiftId)).ToList().Select(x => new SPMonthlyAttendanceTeacher(x)).ToList();
            return data;

        }
        public List<Fee> GetSpecialFeeList()
        {
            return school.tb_Fee.Where(z => z.IsActive && z.FeeType == (int)FeeType.SpecialFee).OrderBy(z => z.FeesName).ToList().Select(z => new Fee(z)).ToList();
        }

        public List<TimetableListingModel> GetMyTimetable(long TeacheruserId)
        {
            long TeacherId = school.tb_Teacher.Where(x => x.UserId == TeacheruserId && x.IsActive).Select(x => x.TeacherId).FirstOrDefault();
            var data = _Entities.tb_TimeTable.Where(x => x.TeacherId == TeacherId && x.IsActive).ToList().OrderBy(x => x.Periods).Select(x => new TimeTable(x)).ToList();
            List<TimetableListingModel> list = new List<TimetableListingModel>();
            for (int i = 0; i < 6; i++)
            {
                var newData = data.Where(x => x.DayId == i).ToList().Select(x => new TimeTable(x.Id)).ToList();
                TimetableListingModel one = new TimetableListingModel();
                if (newData.Count > 0)
                {
                    one.DayName = newData[0].DayName;
                    one.One = newData.Where(x => x.Periods == 1).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 1).Select(x => x.DivisionName).FirstOrDefault();
                    one.two = newData.Where(x => x.Periods == 2).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 2).Select(x => x.DivisionName).FirstOrDefault();
                    one.three = newData.Where(x => x.Periods == 3).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 3).Select(x => x.DivisionName).FirstOrDefault();
                    one.four = newData.Where(x => x.Periods == 4).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 4).Select(x => x.DivisionName).FirstOrDefault();
                    one.five = newData.Where(x => x.Periods == 5).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 5).Select(x => x.DivisionName).FirstOrDefault();
                    one.six = newData.Where(x => x.Periods == 6).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 6).Select(x => x.DivisionName).FirstOrDefault();
                    one.seven = newData.Where(x => x.Periods == 7).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 7).Select(x => x.DivisionName).FirstOrDefault();
                    one.eight = newData.Where(x => x.Periods == 8).Select(x => x.ClassName).FirstOrDefault() + "-" + newData.Where(x => x.Periods == 8).Select(x => x.DivisionName).FirstOrDefault();
                }
                else
                {
                    if (i == 0)
                        one.DayName = "Monday";
                    else if (i == 1)
                        one.DayName = "Tuesday";
                    else if (i == 2)
                        one.DayName = "Wednesday";
                    else if (i == 3)
                        one.DayName = "Thursday";
                    else if (i == 4)
                        one.DayName = "Friday";
                    if (i == 5)
                        one.DayName = "Saturday";
                    one.One = "";
                    one.two = "";
                    one.three = "";
                    one.four = "";
                    one.five = "";
                    one.six = "";
                    one.seven = "";
                    one.eight = "";
                }
                list.Add(one);
            }
            return list;
        }



        public List<OutstandingReportMainModel> GetOutStandingData(long ClassId, long DivisionId, long FeeId, DateTime fromDate, DateTime toDate)
        {
            List<OutstandingReportMainModel> model = new List<OutstandingReportMainModel>();
            List<OutstandingReportMainModel> newModel = new List<OutstandingReportMainModel>();
            var data =  _Entities.SP_OutstandingReportNew(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportNew(x)).ToList();

            //var data = await Task.Run(() => _Entities.SP_OutstandingReportNew(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportNew(x)).ToList());


            var data1 = _Entities.SP_OutstandingReportDivisionWise(school.SchoolId, ClassId, DivisionId, FeeId).ToList();//.Select(x => new SPOutstandingReportDivisionWise(x)).ToList();

            //var data1 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise(x)).ToList());


            if (fromDate != null && fromDate.Year != 1 && toDate != null && toDate.Year != 1)
            {
                data = data.Where(x => x.DueDate >= fromDate && x.DueDate <= toDate).ToList();
                data1.Where(x => x.DueDate >= fromDate && x.DueDate <= toDate).ToList();
            }
            if (data.Count > data1.Count)
            {
                var students = data.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                foreach (var item in students)
                {
                    decimal totalAmount = 00;
                    var discountFee = item.Discount.ToList();
                    var list = data.Where(x => x.StudentId == item.StudentId).ToList();
                    OutstandingReportMainModel one = new OutstandingReportMainModel();
                    one.SubList = new List<SubList>();
                    one.StudentId = item.StudentId;
                    one.StudentName = item.StundentName;
                    one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                    one.ContactNumber = item.ContactNumber;
                    one.DivisionId = item.DivisionId;
                    one.ClassOrder = item.ClassOrder;
                    foreach (var sub in list)
                    {
                        SubList subOne = new SubList();
                        var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                        if (disc != null)
                        {
                            decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                            subOne.Amount = amount;
                        }
                        else
                        {
                            subOne.Amount = sub.Amount ?? 0;
                        }
                        var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                        if (monthDis != null)
                        {
                            subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                        }
                        subOne.FeeId = sub.FeeId;
                        subOne.FeeName = sub.Feename;
                        one.SubList.Add(subOne);
                        totalAmount = totalAmount + subOne.Amount;
                    }
                    var list2 = data1.Where(x => x.StudentId == item.StudentId).ToList();
                    foreach (var sub in list2)
                    {
                        SubList subOne = new SubList();
                        var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                        if (disc != null)
                        {
                            decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                            subOne.Amount = amount;
                        }
                        else
                        {
                            subOne.Amount = sub.Amount ?? 0;
                        }
                        var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                        if (monthDis != null)
                        {
                            subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                        }
                        subOne.FeeId = sub.FeeId;
                        subOne.FeeName = sub.Feename;
                        one.SubList.Add(subOne);
                        totalAmount = totalAmount + subOne.Amount;
                    }
                    one.Total = totalAmount;
                    model.Add(one);
                }
            }
            else
            {
                var students = data1.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                foreach (var item in students)
                {
                    decimal totalAmount = 00;
                    var discountFee = item.Discount.ToList();
                    var list = data1.Where(x => x.StudentId == item.StudentId).ToList();
                    OutstandingReportMainModel one = new OutstandingReportMainModel();
                    one.SubList = new List<SubList>();
                    one.StudentId = item.StudentId;
                    one.StudentName = item.StundentName;
                    one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                    one.ContactNumber = item.ContactNumber;
                    one.DivisionId = item.DivisionId;
                    one.ClassOrder = item.ClassOrder;
                    foreach (var sub in list)
                    {
                        SubList subOne = new SubList();
                        var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                        if (disc != null)
                        {
                            decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                            subOne.Amount = amount;
                        }
                        else
                        {
                            subOne.Amount = sub.Amount ?? 0;
                        }
                        var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                        if (monthDis != null)
                        {
                            subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                        }
                        subOne.FeeId = sub.FeeId;
                        subOne.FeeName = sub.Feename;
                        one.SubList.Add(subOne);
                        totalAmount = totalAmount + subOne.Amount;
                    }
                    var list2 = data.Where(x => x.StudentId == item.StudentId).ToList();
                    foreach (var sub in list2)
                    {
                        SubList subOne = new SubList();
                        var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                        if (disc != null)
                        {
                            decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                            subOne.Amount = amount;
                        }
                        else
                        {
                            subOne.Amount = sub.Amount ?? 0;
                        }
                        var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                        if (monthDis != null)
                        {
                            subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                        }
                        subOne.FeeId = sub.FeeId;
                        subOne.FeeName = sub.Feename;
                        one.SubList.Add(subOne);
                        totalAmount = totalAmount + subOne.Amount;
                    }
                    one.Total = totalAmount;
                    model.Add(one);
                }
            }
            newModel = model.OrderBy(x => x.ClassOrder).ThenBy(x => x.DivisionId).ThenBy(x => x.StudentName).ToList();
            return newModel;
        }




        public async Task<List<OutstandingReportMainModel>> GetOutStandingData_new(long ClassId, long DivisionId, long FeeId,DateTime fromDate,DateTime toDate)
        {
            List<OutstandingReportMainModel> model = new List<OutstandingReportMainModel>();
            List<OutstandingReportMainModel> newModel = new List<OutstandingReportMainModel>();
            //var data =  _Entities.SP_OutstandingReportNew(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportNew(x)).ToList();

            if (FeeId != 0)
            {
                var data = await Task.Run(() => _Entities.SP_OutstandingReportNew(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportNew(x)).ToList());


                //var data1 = _Entities.SP_OutstandingReportDivisionWise(school.SchoolId, ClassId, DivisionId, FeeId).ToList();//.Select(x => new SPOutstandingReportDivisionWise(x)).ToList();

                var data1 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise(x)).ToList());


                if (fromDate != null && fromDate.Year != 1 && toDate != null && toDate.Year != 1)
                {
                    data = data.Where(x => x.DueDate >= fromDate && x.DueDate <= toDate).ToList();
                    data1.Where(x => x.DueDate >= fromDate && x.DueDate <= toDate).ToList();
                }
                if (data.Count > data1.Count)
                {
                    var students = data.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                    foreach (var item in students)
                    {
                        decimal totalAmount = 00;
                        var discountFee = item.Discount.ToList();
                        var list = data.Where(x => x.StudentId == item.StudentId).ToList();
                        OutstandingReportMainModel one = new OutstandingReportMainModel();
                        one.SubList = new List<SubList>();
                        one.StudentId = item.StudentId;
                        one.StudentName = item.StundentName;
                        one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                        one.ContactNumber = item.ContactNumber;
                        one.DivisionId = item.DivisionId;
                        one.ClassOrder = item.ClassOrder;
                        foreach (var sub in list)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount ?? 0;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        var list2 = data1.Where(x => x.StudentId == item.StudentId).ToList();
                        foreach (var sub in list2)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount ?? 0;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        one.Total = totalAmount;
                        model.Add(one);
                    }
                }
                else
                {
                    var students = data1.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                    foreach (var item in students)
                    {
                        decimal totalAmount = 00;
                        var discountFee = item.Discount.ToList();
                        var list = data1.Where(x => x.StudentId == item.StudentId).ToList();
                        OutstandingReportMainModel one = new OutstandingReportMainModel();
                        one.SubList = new List<SubList>();
                        one.StudentId = item.StudentId;
                        one.StudentName = item.StundentName;
                        one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                        one.ContactNumber = item.ContactNumber;
                        one.DivisionId = item.DivisionId;
                        one.ClassOrder = item.ClassOrder;
                        foreach (var sub in list)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount ?? 0;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        var list2 = data.Where(x => x.StudentId == item.StudentId).ToList();
                        foreach (var sub in list2)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount ?? 0;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        one.Total = totalAmount;
                        model.Add(one);
                    }
                }
                newModel = model.OrderBy(x => x.ClassOrder).ThenBy(x => x.DivisionId).ThenBy(x => x.StudentName).ToList();


            }
            else
            {
                var data = await Task.Run(() => _Entities.SP_OutstandingReportNew(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportNew(x)).ToList());


                //var data1 = _Entities.SP_OutstandingReportDivisionWise(school.SchoolId, ClassId, DivisionId, FeeId).ToList();//.Select(x => new SPOutstandingReportDivisionWise(x)).ToList();

                //var data1 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_one(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_one(x)).ToList());


                var data2 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_two(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_two(x)).ToList());
                var data3 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_three(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_three(x)).ToList());
                var data4 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_four(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_four(x)).ToList());
                var data5 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_five(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_five(x)).ToList());
                var data6 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_six(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_six(x)).ToList());
                var data7 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_seven(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_seven(x)).ToList());
                var data8 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_eight(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_eight(x)).ToList());
                var data9 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_nine(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_nine(x)).ToList());
                var data10 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_ten(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_ten(x)).ToList());
                var data11 = await Task.Run(() => _Entities.SP_OutstandingReportDivisionWise_eleven(school.SchoolId, ClassId, DivisionId, FeeId).ToList().Select(x => new SPOutstandingReportDivisionWise_eleven(x)).ToList());

               List<SP_OutstandingReportDivisionWise_one_Result> data1 = new List<SP_OutstandingReportDivisionWise_one_Result>();

                foreach(var a1 in data2)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data3)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data4)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount =Convert.ToDecimal( a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data5)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data6)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data7)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data8)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data9)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }
                foreach (var a1 in data10)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }

                foreach (var a1 in data11)
                {
                    SP_OutstandingReportDivisionWise_one_Result mo = new SP_OutstandingReportDivisionWise_one_Result();

                    mo.StudentId = a1.StudentId;
                    mo.FeeId = a1.FeeId;
                    mo.Amount = Convert.ToDecimal(a1.Amount);
                    //mo.FeeGuid = a1.FeeGuid;
                    mo.Feename = a1.Feename;
                    mo.DueDate = a1.DueDate;
                    mo.DiscountAllowed = a1.DiscountAllowed;
                    mo.StudentspecialFee = a1.StudentspecialFee;

                    data1.Add(mo);

                }




                if (fromDate != null && fromDate.Year != 1 && toDate != null && toDate.Year != 1)
                {
                    data = data.Where(x => x.DueDate >= fromDate && x.DueDate <= toDate).ToList();
                    data1.Where(x => x.DueDate >= fromDate && x.DueDate <= toDate).ToList();
                }
                if (data.Count >data1.Count)
                {
                    var students = data.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                    foreach (var item in students)
                    {
                        decimal totalAmount = 00;
                        var discountFee = item.Discount.ToList();
                        var list = data.Where(x => x.StudentId == item.StudentId).ToList();
                        OutstandingReportMainModel one = new OutstandingReportMainModel();
                        one.SubList = new List<SubList>();
                        one.StudentId = item.StudentId;
                        one.StudentName = item.StundentName;
                        one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                        one.ContactNumber = item.ContactNumber;
                        one.DivisionId = item.DivisionId;
                        one.ClassOrder = item.ClassOrder;
                        foreach (var sub in list)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount ?? 0;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        var list2 = data1.Where(x => x.StudentId == item.StudentId).ToList();
                        foreach (var sub in list2)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount =sub.Amount - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        one.Total = totalAmount;
                        model.Add(one);
                    }
                }
                else
                {
                    var students = data1.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                    foreach (var item in students)
                    {
                        decimal totalAmount = 00;
                        var discountFee = item.Discount.ToList();
                        var list = data1.Where(x => x.StudentId == item.StudentId).ToList();
                        OutstandingReportMainModel one = new OutstandingReportMainModel();
                        one.SubList = new List<SubList>();
                        one.StudentId = item.StudentId;
                        one.StudentName = item.StundentName;
                        one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                        one.ContactNumber = item.ContactNumber;
                        one.DivisionId = item.DivisionId;
                        one.ClassOrder = item.ClassOrder;
                        foreach (var sub in list)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = sub.Amount - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        var list2 = data.Where(x => x.StudentId == item.StudentId).ToList();
                        foreach (var sub in list2)
                        {
                            SubList subOne = new SubList();
                            var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                            if (disc != null)
                            {
                                decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                                subOne.Amount = amount;
                            }
                            else
                            {
                                subOne.Amount = sub.Amount ?? 0;
                            }
                            var monthDis = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == one.StudentId && x.FeeId == sub.FeeId && x.IsActive && x.DiscountAllowFeeDate.Month == sub.DueDate.Month && x.DiscountAllowFeeDate.Year == sub.DueDate.Year).FirstOrDefault();
                            if (monthDis != null)
                            {
                                subOne.Amount = subOne.Amount - (monthDis.DiscountAmount ?? 0);
                            }
                            subOne.FeeId = sub.FeeId;
                            subOne.FeeName = sub.Feename;
                            one.SubList.Add(subOne);
                            totalAmount = totalAmount + subOne.Amount;
                        }
                        one.Total = totalAmount;
                        model.Add(one);
                    }
                }
                newModel = model.OrderBy(x => x.ClassOrder).ThenBy(x => x.DivisionId).ThenBy(x => x.StudentName).ToList();


            }





            return newModel;
        }

        public List<OutstandingReportMainModel> GetBilledData(DateTime startDate, DateTime endDate)
        {
            List<OutstandingReportMainModel> model = new List<OutstandingReportMainModel>();
            var data = _Entities.Sp_BilledReport(school.SchoolId, startDate, endDate).ToList();//.Select(x => new SPOutstandingReportNew(x)).ToList();
            if (data.Count > 0)
            {
                var students = data.Select(o => o.StudentId).Distinct().ToList().Select(o => new TrackTap.Data.Student(o)).ToList().OrderBy(x => x.StundentName);
                foreach (var item in students)
                {
                    decimal totalAmount = 00;
                    var discountFee = item.Discount.ToList();
                    var list = data.Where(x => x.StudentId == item.StudentId).ToList();
                    OutstandingReportMainModel one = new OutstandingReportMainModel();
                    one.SubList = new List<SubList>();
                    one.StudentId = item.StudentId;
                    one.StudentName = item.StundentName;
                    one.ClassDetails = item.ClassName + " / " + item.DivisionName;
                    one.ContactNumber = item.ContactNumber;
                    one.DivisionId = item.DivisionId;
                    one.ClassOrder = item.ClassOrder;
                    foreach (var sub in list)
                    {
                        SubList subOne = new SubList();
                        var disc = discountFee.Where(x => x.feeId == sub.FeeId && sub.DiscountAllowed == 0).FirstOrDefault();
                        if (disc != null)
                        {
                            decimal amount = (sub.Amount ?? 0) - disc.discountAmount;
                            subOne.Amount = amount;
                        }
                        else
                        {
                            subOne.Amount = sub.Amount ?? 0;
                        }
                        subOne.FeeId = sub.FeeId;
                        subOne.FeeName = sub.Feename;
                        one.SubList.Add(subOne);
                        totalAmount = totalAmount + subOne.Amount;
                    }
                    one.Total = totalAmount;
                    model.Add(one);
                }
                List<OutstandingReportMainModel> newModel = new List<OutstandingReportMainModel>();
                newModel = model.OrderBy(x => x.ClassOrder).ThenBy(x => x.DivisionId).ThenBy(x => x.StudentName).ToList();
                return newModel;
            }
            else
            {
                return model;
            }

        }


        public SchoolSenderId GetSenderDetails()
        {
            var data = school.tb_SchoolSenderId.Where(x => x.IsActive == true).ToList().Select(x => new SchoolSenderId(x)).FirstOrDefault();
            return data;
        }
        public List<Exams> GetProgressData(long studentId, long classId, long divisionId)
        {
            var data = _Entities.tb_Exams.Where(x => x.ClassId == classId && x.DivisionId == divisionId && x.IsActive).ToList().OrderBy(x => x.ExamDate).Select(x => new Exams(x)).ToList();
            return data;
        }
        public List<StockUpdate> GetCurrentStock()
        {
            var data = school.tb_StockUpdate.Where(x => x.IsActive).ToList().Select(x => new StockUpdate(x)).OrderByDescending(x => x.TimeStamp).ToList();
            return data;
        }
        public List<StockUpdate> GetCurrentStockReport(long catId)
        {
            if (catId != 0)
            {
                var data = school.tb_StockUpdate.Where(x => x.IsActive && x.CategoryId == catId).ToList().Select(x => new StockUpdate(x)).OrderByDescending(x => x.TimeStamp).ToList();
                return data;
            }
            else
            {
                var data = school.tb_StockUpdate.Where(x => x.IsActive).ToList().Select(x => new StockUpdate(x)).OrderByDescending(x => x.TimeStamp).ToList();
                return data;
            }
        }
        public AccountHead GetFeeIncomeHead()//**
        {
            var data = school.tb_AccountHead.Where(x => x.IsActive == true && x.ForBill == true).ToList().Select(x => new AccountHead(x)).FirstOrDefault();
            return data;
        }
        public string GetOpeningBalance(DateTime fromDate, long bankId, int sourceId)// sourceId:0=Cash ,1=Bank
        {
            try
            {
                if (bankId == 0 && sourceId == 2)
                {
                    decimal close = 0;
                    long bId = 0;
                    var data = _Entities.sp_OpeningBalanceAmountInCashBook(school.SchoolId, fromDate, bankId, sourceId).ToList();
                    foreach (var item in data)
                    {
                        if (bId != item.BankId)
                        {
                            close = close + item.Opening ?? 0;
                        }
                        bId = item.BankId ?? 0;
                    }
                    return Convert.ToString(close);
                }
                else
                {
                    var data = _Entities.sp_OpeningBalanceAmountInCashBook(school.SchoolId, fromDate, bankId, sourceId).FirstOrDefault();
                    return Convert.ToString(data.Opening);
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public List<SPCashEntryReport> GetCashBookReport(DateTime fromdate, DateTime toDate, long schoolId)
        {
            var data = _Entities.sp_CashEntryReport(SchoolId, fromdate, toDate).ToList().Select(x => new SPCashEntryReport(x)).ToList();
            return data;
        }
        public string GetClosingBalance(DateTime fromDate, long bankId, int sourceId)// sourceId:0=Cash ,1=Bank
        {
            try
            {
                if (bankId == 0 && sourceId == 2)
                {
                    decimal close = 0;
                    long bId = 0;
                    var data = _Entities.sp_ClosingBalanceAmount(school.SchoolId, fromDate, bankId, sourceId).ToList();
                    foreach (var item in data)
                    {
                        if (bId != item.BankId)
                        {
                            close = close + item.Opening ?? 0;
                        }
                        bId = item.BankId ?? 0;
                    }
                    return Convert.ToString(close);
                }
                else
                {
                    var data = _Entities.sp_ClosingBalanceAmount(school.SchoolId, fromDate, bankId, sourceId).FirstOrDefault();
                    return Convert.ToString(data.Opening);
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public List<SPBankEntryReport> GetBankBookReport(DateTime fromdate, DateTime toDate, long schoolId, long bankId)
        {
            var data = _Entities.sp_BankEntryReport(SchoolId, fromdate, toDate, bankId).ToList().Select(x => new SPBankEntryReport(x)).ToList();
            return data;
        }
        public List<ReceiptPaymentNewModel> GetRPReport(DateTime fromdate, DateTime toDate, long schoolId, int Iscontra)
        {
            #region Without contra
            if (Iscontra == 0)   //Added by basheer to include contra vouchers in receipt and payment
            {
                var bankList = school.tb_Banks.Where(x => x.IsActive).ToList();
                List<ReceiptPaymentNewModel> list = new List<ReceiptPaymentNewModel>();
                var data = _Entities.sp_ReceiptPayment(SchoolId, fromdate, toDate).OrderBy(x => x.FromData).ToList();
                decimal fdInterestAmount = data
                                            .Where(x => x.AccHeadName != null &&
                                                        x.AccHeadName.Trim().ToUpper().Contains("FIXED DEPOSIT - BANK INTEREST"))
                                            .Sum(x => x.Receipt ?? 0);
                if (data != null && data.Count > 0)
                {
                    var receiptList = data.Where(x => x.Payment == 0 && x.Receipt != 0).OrderBy(x => x.FromData).OrderBy(x => x.AccHeadName).ToList();
                    var paymentList = data.Where(x => x.Payment != 0 && x.Receipt == 0).OrderBy(x => x.FromData).OrderBy(x => x.AccHeadName).ToList();
                    foreach (var item in receiptList)
                    {
                        var head = "";
                        if (item.FromData == 3 || item.FromData == 4)
                        {
                            if (Convert.ToInt32(item.BillNo) == 0)
                                head = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            else
                                head = item.AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                        }
                        else
                        {
                            head = item.AccHeadName;
                        }
                        receiptList.Where(w => w == item).ToList().ForEach(s => s.AccHeadName = head);
                    }
                    receiptList = receiptList.GroupBy(x => x.AccHeadName).Select(m => new sp_ReceiptPayment_Result
                    {
                        AccHeadName = m.First().AccHeadName,
                        FromData = m.First().FromData,
                        Id = 0,
                        BillNo = 0,
                        Receipt = m.Sum(p => p.Receipt),
                        Payment = m.Sum(p => p.Payment)
                    }).ToList();
                    //-------------------------------------------------------------
                    foreach (var item in paymentList)
                    {
                        var head = "";
                        if (item.FromData == 3 || item.FromData == 4)
                        {
                            if (Convert.ToInt32(item.BillNo) == 0)
                                head = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            else
                                head = item.AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();




                        }
                        else
                        {
                            head = item.AccHeadName;
                        }
                        paymentList.Where(w => w == item).ToList().ForEach(s => s.AccHeadName = head);
                    }
                    paymentList = paymentList.GroupBy(x => x.AccHeadName).Select(m => new sp_ReceiptPayment_Result
                    {
                        AccHeadName = m.First().AccHeadName,
                        FromData = m.First().FromData,
                        Id = 0,
                        BillNo = 0,
                        Receipt = m.Sum(p => p.Receipt),
                        Payment = m.Sum(p => p.Payment)
                    }).ToList();
                    paymentList = paymentList
                                .Where(x => x.AccHeadName == null ||
                                           !x.AccHeadName.Trim().ToUpper().Contains("FIXED DEPOSIT - BANK INTEREST"))
                                .ToList();
                    //-------------------------------------------------------------
                    int i = 0;
                    #region Data Model Creating
                    if (receiptList.Count > paymentList.Count)
                    {
                        foreach (var item in receiptList)
                        {
                            ReceiptPaymentNewModel one = new ReceiptPaymentNewModel();
                            //if (item.FromData == 3 || item.FromData == 4)
                            //{
                            //    if (Convert.ToInt32(item.BillNo) == 0)
                            //        one.AccountHeadReceipt = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            //    else
                            //        one.AccountHeadReceipt = item.AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                            //}
                            //else
                            //{
                            one.AccountHeadReceipt = item.AccHeadName;
                            //}
                            one.receipt = item.Receipt ?? 0;
                            try
                            {
                                long subId = paymentList[i].Id;
                                //if (paymentList[i].FromData == 3 || paymentList[i].FromData == 4)
                                //{
                                //    if (Convert.ToInt32(paymentList[i].BillNo) == 0)
                                //        one.AccountHeadPayment = paymentList[i].AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == subId).Select(x => x.SubLedgerName).FirstOrDefault();
                                //    else
                                //        one.AccountHeadPayment = paymentList[i].AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                //}
                                //{
                                one.AccountHeadPayment = paymentList[i].AccHeadName;
                                //}
                                one.payment = paymentList[i].Payment ?? 0;
                            }
                            catch
                            {
                                one.AccountHeadPayment = "";
                                one.payment = 0;
                            }
                            one.SLNo = 2;
                            list.Add(one);
                            i = i + 1;
                        }
                        //}
                    }
                    else
                    {
                        foreach (var item in paymentList)
                        {
                            ReceiptPaymentNewModel one = new ReceiptPaymentNewModel();
                            //if (item.FromData == 3 || item.FromData == 4)
                            //{
                            //    if (Convert.ToInt32(item.BillNo) == 0)
                            //        one.AccountHeadPayment = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            //    else
                            //        one.AccountHeadPayment = item.AccHeadName + " " + _Entities.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                            //}
                            //else
                            //{
                            one.AccountHeadPayment = item.AccHeadName;
                            //}
                            one.payment = item.Payment ?? 0;
                            try
                            {
                                long subId = receiptList[i].Id;
                                //if (receiptList[i].FromData == 3 || receiptList[i].FromData == 4)
                                //{
                                //    if (Convert.ToInt32(receiptList[i].BillNo) == 0)
                                //        one.AccountHeadReceipt = receiptList[i].AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == subId).Select(x => x.SubLedgerName).FirstOrDefault();
                                //    else
                                //        one.AccountHeadReceipt = receiptList[i].AccHeadName + " " + _Entities.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                //}
                                //{
                                one.AccountHeadReceipt = receiptList[i].AccHeadName;
                                //}
                                one.receipt = receiptList[i].Receipt ?? 0;
                            }
                            catch (Exception ex)
                            {
                                one.AccountHeadReceipt = "";
                                one.receipt = 0;
                            }
                            one.SLNo = 2;
                            list.Add(one);
                            i = i + 1;
                        }
                    }
                    #endregion Data Model Creating
                }

                //------------------------------Receipt---------------------------
                #region Open Cash 
                ReceiptPaymentNewModel open = new ReceiptPaymentNewModel();
                open.receipt = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.BankId == 0 && x.SourceId == 0 && x.CurrentDate.Date < fromdate && x.IsActive).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                open.AccountHeadReceipt = "CASH IN HAND";
                open.payment = 0;
                open.AccountHeadPayment = "";
                open.Id = 0;
                open.FromData = 0;
                open.SLNo = 0;
                list.Add(open);
                #endregion Open Cash
                #region Bank Balance
                var balance = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.SourceId == 2 && x.BankId != 0 && x.IsActive && x.CurrentDate.Date < fromdate).OrderByDescending(x => x.CurrentDate).ToList();
                foreach (var x in balance)
                {
                    var xxxx = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                    if (list.Any(m => m.AccountHeadReceipt == xxxx))
                    {

                    }
                    else
                    {
                        ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                        b.receipt = x.Closing;
                        b.AccountHeadReceipt = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                        b.payment = 0;
                        b.AccountHeadPayment = "";
                        b.Id = 0;
                        b.FromData = 0;
                        b.SLNo = 1;
                        list.Add(b);
                    }
                }
                var baalanceBank = bankList.Where(x => !list.Any(h => h.AccountHeadReceipt == x.BankName.ToUpper())).ToList();
                foreach (var bb in baalanceBank)
                {
                    ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                    b.receipt = 0;
                    b.AccountHeadReceipt = bb.BankName.ToUpper();
                    b.payment = 0;
                    b.AccountHeadPayment = "";
                    b.Id = 0;
                    b.FromData = 0;
                    b.SLNo = 1;
                    list.Add(b);
                }
                #endregion Bank Balance
                //---------------------------------------------------
                //------------------------------Payment---------------------------
                #region Closing Cash 
                ReceiptPaymentNewModel close = new ReceiptPaymentNewModel();
                close.payment = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.BankId == 0 && x.SourceId == 0 && x.CurrentDate.Date <= toDate && x.IsActive).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                close.AccountHeadPayment = "CASH IN HAND";
                close.receipt = 0;
                close.AccountHeadReceipt = "";
                close.Id = 0;
                close.FromData = 0;
                close.SLNo = 3;
                list.Add(close);
                #endregion close Cash
                #region Bank Balance
                //var balanceClose = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.SourceId == 2 && x.BankId != 0 && x.IsActive && x.CurrentDate.Date <= toDate).OrderByDescending(x => x.CurrentDate).ToList();
                //foreach (var x in balanceClose)
                //{
                //    var xxxxx = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                //    if (list.Any(m => m.AccountHeadPayment == xxxxx))
                //    {

                //    }
                //    else
                //    {
                //        ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                //        b.payment = x.Closing;
                //        b.AccountHeadPayment = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                //        b.receipt = 0;
                //        b.AccountHeadReceipt = "";
                //        b.Id = 0;
                //        b.FromData = 0;
                //        b.SLNo = 4;
                //        list.Add(b);
                //    }
                //}
                var balanceClose = school.tb_Balance
                                    .Where(x => x.SchoolId == school.SchoolId &&
                                                x.SourceId == 2 &&
                                                x.BankId != 0 &&
                                                x.IsActive &&
                                                x.CurrentDate.Date <= toDate)
                                    .OrderByDescending(x => x.CurrentDate)
                                    .ToList();

                foreach (var x in balanceClose)
                {
                    var bankName = school.tb_Banks
                        .Where(y => y.BankId == x.BankId)
                        .Select(y => y.BankName)
                        .FirstOrDefault()?.ToUpper();

                    if (!list.Any(m => m.AccountHeadPayment == bankName))
                    {
                        ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();

                        decimal closingAmount = x.Closing;

                        
                        b.payment = closingAmount;
                        b.AccountHeadPayment = bankName;
                        b.receipt = 0;
                        b.AccountHeadReceipt = "";
                        b.Id = 0;
                        b.FromData = 0;
                        b.SLNo = 4;
                        list.Add(b);
                    }
                }
                var baalanceBankb = bankList.Where(x => !list.Any(h => h.AccountHeadPayment == x.BankName.ToUpper())).ToList();
                foreach (var bb in baalanceBankb)
                {
                    ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                    b.receipt = 0;
                    b.AccountHeadPayment = bb.BankName.ToUpper();
                    b.payment = 0;
                    b.AccountHeadReceipt = "";
                    b.Id = 0;
                    b.FromData = 0;
                    b.SLNo = 4;
                    list.Add(b);
                }
                #endregion Bank Balance

                //---------------------------------------------------
                return list.OrderBy(x => x.SLNo).ToList();
            }

            #endregion

            #region withcontra
            else
            {
                var bankList = school.tb_Banks.Where(x => x.IsActive).ToList();
                List<ReceiptPaymentNewModel> list = new List<ReceiptPaymentNewModel>();
                var data = _Entities.sp_ReceiptPaymentWithContra(SchoolId, fromdate, toDate).OrderBy(x => x.FromData).ToList();
                decimal fdInterestAmount = data
                                            .Where(x => x.AccHeadName != null &&
                                                        x.AccHeadName.Trim().ToUpper().Contains("FIXED DEPOSIT - BANK INTEREST"))
                                            .Sum(x => x.Receipt ?? 0);
                if (data != null && data.Count > 0)
                {
                    var receiptList = data.Where(x => x.Payment == 0 && x.Receipt != 0).OrderBy(x => x.FromData).OrderBy(x => x.AccHeadName).ToList();
                    var paymentList = data.Where(x => x.Payment != 0 && x.Receipt == 0).OrderBy(x => x.FromData).OrderBy(x => x.AccHeadName).ToList();
                    foreach (var item in receiptList)
                    {
                        var head = "";
                        if (item.FromData == 3 || item.FromData == 4)
                        {
                            if (Convert.ToInt32(item.BillNo) == 0)
                                head = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            else
                                head = item.AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                        }
                        else
                        {
                            head = item.AccHeadName;
                        }
                        receiptList.Where(w => w == item).ToList().ForEach(s => s.AccHeadName = head);
                    }
                    receiptList = receiptList.GroupBy(x => x.AccHeadName).Select(m => new sp_ReceiptPaymentWithContra_Result
                    {
                        AccHeadName = m.First().AccHeadName,
                        FromData = m.First().FromData,
                        Id = 0,
                        BillNo = 0,
                        Receipt = m.Sum(p => p.Receipt),
                        Payment = m.Sum(p => p.Payment)
                    }).ToList();
                    //-------------------------------------------------------------
                    foreach (var item in paymentList)
                    {
                        var head = "";
                        if (item.FromData == 3 || item.FromData == 4)
                        {
                            if (Convert.ToInt32(item.BillNo) == 0)
                                head = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            else
                                head = item.AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                        }
                        else
                        {
                            head = item.AccHeadName;
                        }
                        paymentList.Where(w => w == item).ToList().ForEach(s => s.AccHeadName = head);
                    }
                    paymentList = paymentList.GroupBy(x => x.AccHeadName).Select(m => new sp_ReceiptPaymentWithContra_Result
                    {
                        AccHeadName = m.First().AccHeadName,
                        FromData = m.First().FromData,
                        Id = 0,
                        BillNo = 0,
                        Receipt = m.Sum(p => p.Receipt),
                        Payment = m.Sum(p => p.Payment)
                    }).ToList();
                    paymentList = paymentList
                                    .Where(x => x.AccHeadName == null ||
                                               !x.AccHeadName.Trim().ToUpper().Contains("FIXED DEPOSIT - BANK INTEREST"))
                                    .ToList();
                    //-------------------------------------------------------------
                    int i = 0;
                    #region Data Model Creating
                    if (receiptList.Count > paymentList.Count)
                    {
                        foreach (var item in receiptList)
                        {
                            ReceiptPaymentNewModel one = new ReceiptPaymentNewModel();
                            //if (item.FromData == 3 || item.FromData == 4)
                            //{
                            //    if (Convert.ToInt32(item.BillNo) == 0)
                            //        one.AccountHeadReceipt = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            //    else
                            //        one.AccountHeadReceipt = item.AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                            //}
                            //else
                            //{
                            one.AccountHeadReceipt = item.AccHeadName;
                            //}
                            one.receipt = item.Receipt ?? 0;
                            try
                            {
                                long subId = paymentList[i].Id;
                                //if (paymentList[i].FromData == 3 || paymentList[i].FromData == 4)
                                //{
                                //    if (Convert.ToInt32(paymentList[i].BillNo) == 0)
                                //        one.AccountHeadPayment = paymentList[i].AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == subId).Select(x => x.SubLedgerName).FirstOrDefault();
                                //    else
                                //        one.AccountHeadPayment = paymentList[i].AccHeadName + " " + school.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                //}
                                //{
                                one.AccountHeadPayment = paymentList[i].AccHeadName;
                                //}
                                one.payment = paymentList[i].Payment ?? 0;
                            }
                            catch
                            {
                                one.AccountHeadPayment = "";
                                one.payment = 0;
                            }
                            one.SLNo = 2;
                            list.Add(one);
                            i = i + 1;
                        }
                        //}
                    }
                    else
                    {
                        foreach (var item in paymentList)
                        {
                            ReceiptPaymentNewModel one = new ReceiptPaymentNewModel();
                            //if (item.FromData == 3 || item.FromData == 4)
                            //{
                            //    if (Convert.ToInt32(item.BillNo) == 0)
                            //        one.AccountHeadPayment = item.AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.Id).Select(x => x.SubLedgerName).FirstOrDefault();
                            //    else
                            //        one.AccountHeadPayment = item.AccHeadName + " " + _Entities.tb_Fee.Where(x => x.FeeId == item.Id).Select(x => x.FeesName).FirstOrDefault();
                            //}
                            //else
                            //{
                            one.AccountHeadPayment = item.AccHeadName;
                            //}
                            one.payment = item.Payment ?? 0;
                            try
                            {
                                long subId = receiptList[i].Id;
                                //if (receiptList[i].FromData == 3 || receiptList[i].FromData == 4)
                                //{
                                //    if (Convert.ToInt32(receiptList[i].BillNo) == 0)
                                //        one.AccountHeadReceipt = receiptList[i].AccHeadName + " " + _Entities.tb_SubLedgerData.Where(x => x.LedgerId == subId).Select(x => x.SubLedgerName).FirstOrDefault();
                                //    else
                                //        one.AccountHeadReceipt = receiptList[i].AccHeadName + " " + _Entities.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                //}
                                //{
                                one.AccountHeadReceipt = receiptList[i].AccHeadName;
                                //}
                                one.receipt = receiptList[i].Receipt ?? 0;
                            }
                            catch (Exception ex)
                            {
                                one.AccountHeadReceipt = "";
                                one.receipt = 0;
                            }
                            one.SLNo = 2;
                            list.Add(one);
                            i = i + 1;
                        }
                    }
                    #endregion Data Model Creating
                }

                //------------------------------Receipt---------------------------
                #region Open Cash 
                ReceiptPaymentNewModel open = new ReceiptPaymentNewModel();
                open.receipt = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.BankId == 0 && x.SourceId == 0 && x.CurrentDate.Date < fromdate && x.IsActive).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                open.AccountHeadReceipt = "CASH IN HAND";
                open.payment = 0;
                open.AccountHeadPayment = "";
                open.Id = 0;
                open.FromData = 0;
                open.SLNo = 0;
                list.Add(open);
                #endregion Open Cash
                #region Bank Balance
                var balance = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.SourceId == 2 && x.BankId != 0 && x.IsActive && x.CurrentDate.Date < fromdate).OrderByDescending(x => x.CurrentDate).ToList();
                foreach (var x in balance)
                {
                    var xxxx = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                    if (list.Any(m => m.AccountHeadReceipt == xxxx))
                    {

                    }
                    else
                    {
                        ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                        b.receipt = x.Closing;
                        b.AccountHeadReceipt = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                        b.payment = 0;
                        b.AccountHeadPayment = "";
                        b.Id = 0;
                        b.FromData = 0;
                        b.SLNo = 1;
                        list.Add(b);
                    }
                }
                var baalanceBank = bankList.Where(x => !list.Any(h => h.AccountHeadReceipt == x.BankName.ToUpper())).ToList();
                foreach (var bb in baalanceBank)
                {
                    ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                    b.receipt = 0;
                    b.AccountHeadReceipt = bb.BankName.ToUpper();
                    b.payment = 0;
                    b.AccountHeadPayment = "";
                    b.Id = 0;
                    b.FromData = 0;
                    b.SLNo = 1;
                    list.Add(b);
                }
                #endregion Bank Balance
                //---------------------------------------------------
                //------------------------------Payment---------------------------
                #region Closing Cash 
                ReceiptPaymentNewModel close = new ReceiptPaymentNewModel();
                close.payment = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.BankId == 0 && x.SourceId == 0 && x.CurrentDate.Date <= toDate && x.IsActive).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                close.AccountHeadPayment = "CASH IN HAND";
                close.receipt = 0;
                close.AccountHeadReceipt = "";
                close.Id = 0;
                close.FromData = 0;
                close.SLNo = 3;
                list.Add(close);
                #endregion close Cash
                #region Bank Balance
                //var balanceClose = school.tb_Balance.Where(x => x.SchoolId == school.SchoolId && x.SourceId == 2 && x.BankId != 0 && x.IsActive && x.CurrentDate.Date <= toDate).OrderByDescending(x => x.CurrentDate).ToList();
                //foreach (var x in balanceClose)
                //{
                //    var xxxxx = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                //    if (list.Any(m => m.AccountHeadPayment == xxxxx))
                //    {

                //    }
                //    else
                //    {
                //        ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                //        b.payment = x.Closing;
                //        b.AccountHeadPayment = school.tb_Banks.Where(y => y.BankId == x.BankId).Select(y => y.BankName).FirstOrDefault().ToUpper();
                //        b.receipt = 0;
                //        b.AccountHeadReceipt = "";
                //        b.Id = 0;
                //        b.FromData = 0;
                //        b.SLNo = 4;
                //        list.Add(b);
                //    }
                //}
                var balanceClose = school.tb_Balance
                                    .Where(x => x.SchoolId == school.SchoolId &&
                                                x.SourceId == 2 &&
                                                x.BankId != 0 &&
                                                x.IsActive &&
                                                x.CurrentDate.Date <= toDate)
                                    .OrderByDescending(x => x.CurrentDate)
                                    .ToList();

                foreach (var x in balanceClose)
                {
                    var bankName = school.tb_Banks
                        .Where(y => y.BankId == x.BankId)
                        .Select(y => y.BankName)
                        .FirstOrDefault()?.ToUpper();

                    if (!list.Any(m => m.AccountHeadPayment == bankName))
                    {
                        ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();

                        decimal closingAmount = x.Closing;

                        b.payment = closingAmount;
                        b.AccountHeadPayment = bankName;
                        b.receipt = 0;
                        b.AccountHeadReceipt = "";
                        b.Id = 0;
                        b.FromData = 0;
                        b.SLNo = 4;
                        list.Add(b);
                    }
                }
                var baalanceBankb = bankList.Where(x => !list.Any(h => h.AccountHeadPayment == x.BankName.ToUpper())).ToList();
                foreach (var bb in baalanceBankb)
                {
                    ReceiptPaymentNewModel b = new ReceiptPaymentNewModel();
                    b.receipt = 0;
                    b.AccountHeadPayment = bb.BankName.ToUpper();
                    b.payment = 0;
                    b.AccountHeadReceipt = "";
                    b.Id = 0;
                    b.FromData = 0;
                    b.SLNo = 4;
                    list.Add(b);
                }
                #endregion Bank Balance

                //---------------------------------------------------
                return list.OrderBy(x => x.SLNo).ToList();
            }

            #endregion


        }
        public decimal CashAmount(DateTime startDate, DateTime endDate, int type)//type=0 = Opening 1 = Closing 
        {
            if (type == 0)
            {
                var data = school.tb_Balance.Where(x => x.SourceId == 0 && x.BankId == 0 && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) <= endDate.Date).OrderByDescending(x => x.CurrentDate).FirstOrDefault();
                if (data != null)
                    return data.Closing;
                else
                    return 0;
            }
            else
            {
                var data = school.tb_Balance.Where(x => x.SourceId == 0 && x.BankId == 0 && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) >= startDate.Date).OrderByDescending(x => x.CurrentDate).FirstOrDefault();
                if (data != null)
                    return data.Closing;
                else
                    return 0;
            }
        }
        public decimal PaymentClosingBank(DateTime startDate, DateTime endDate, int type)//type=0 = Receipt 1 = Payment
        {
            var data = school.tb_Balance.Where(x => x.SourceId == 2 && x.BankId != 0 && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) <= endDate.Date).OrderByDescending(x => x.CurrentDate).FirstOrDefault();
            if (data != null)
                return data.Closing;
            else
                return 0;
        }
        public List<SPDayBookReport> GetDayBookReport(DateTime fromDate, DateTime toDate, long schoolId) // Not use this Stored Procedure in live 
        {
            var data = _Entities.sp_DayBookReport(schoolId, fromDate, toDate).ToList().Select(x => new SPDayBookReport(x)).ToList();
            return data;
        }
        public List<DayBookReportModel> DayBookReportDetailsCollection(DateTime fromDate, DateTime toDate)
        {
            var banks = school.tb_Banks.ToList();
            List<DayBookReportModel> data = new List<DayBookReportModel>();
            var balanceDetails = school.tb_Balance.Where(x => (x.CurrentDate.Date) <= toDate.Date && (x.CurrentDate.Date) >= fromDate.Date && x.IsActive).OrderBy(x => x.CurrentDate).ToList();
            var bankDetails = school.tb_BankEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date).OrderBy(x=>Convert.ToInt32(x.VoucherNumber)).ToList();
            var cashDetails = school.tb_CashEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date).OrderBy(x =>Convert.ToInt32(x.VoucherNumber)).ToList();
            if (balanceDetails != null && balanceDetails.Count > 0)
            {
                var dateList = balanceDetails.GroupBy(x => x.CurrentDate.Date).ToList();
                foreach (var item in dateList)
                {
                    DayBookReportModel one = new DayBookReportModel();
                    one.EnterDate = item.Key;
                    //one.Opening = school.tb_Balance.Where(x => (x.CurrentDate.Date) < item.Key).Sum(x => x.Closing);
                    decimal cc = school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    decimal bnk = 0;
                    foreach (var bk in banks)
                    {
                        bnk = bnk + school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 2 && x.BankId == bk.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    }
                    one.Opening = bnk + cc;

                    decimal close = one.Opening;
                    one._list = new List<DayBookReportDetails>();
                  
                    if (cashDetails != null && cashDetails.Count > 0)
                    {
                        var cashList = cashDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var cash in cashList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();

                            subOne.Commen_Id =Convert.ToInt64(cash.VoucherNumber);
                       
                            if (cash.BillNo != null && cash.BillNo != "")
                                //subOne.VoucherNo = cash.VoucherNumber + " / " + cash.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = cash.VoucherNumber + " - " + cash.BillNo + " / " + one.EnterDate.Year;
                           // subOne.VoucherNo = cash.BillNo + " / " + one.EnterDate.Year;



                            else
                                subOne.VoucherNo = cash.VoucherNumber;
                            

                            subOne.TransactionType = cash.TransactionType;
                            subOne.AccountHeadName = cash.tb_AccountHead.AccHeadName;
                            if (cash.BillNo == null || cash.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == cash.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == cash.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (cash.TransactionType == "R")
                            {
                                subOne.IncomeAmount = cash.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = cash.Amount ?? 0;
                            }
                            subOne.Narration = cash.Narration;
                            subOne.FromStatus = "C";
                            one._list.Add(subOne);

                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    if (bankDetails != null && bankDetails.Count > 0)
                    {
                        var bankList = bankDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var bak in bankList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();


                            subOne.Commen_Id = Convert.ToInt64(bak.VoucherNumber);
                            if (bak.BillNo != null && bak.BillNo != "")
                                //subOne.VoucherNo = bak.VoucherNumber + " / " + bak.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = bak.VoucherNumber + " - " + bak.BillNo + " / " + one.EnterDate.Year;
                               // subOne.VoucherNo = bak.BillNo + " / " + one.EnterDate.Year;



                            else
                                subOne.VoucherNo = bak.VoucherNumber;
                          
                            subOne.TransactionType = bak.TransactionType;
                            subOne.AccountHeadName = bak.tb_AccountHead.AccHeadName;
                            if (bak.BillNo == null || bak.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == bak.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == bak.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (bak.TransactionType == "R")
                            {
                                subOne.IncomeAmount = bak.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = bak.Amount ?? 0;
                            }
                            subOne.Narration = bak.Narration;
                            subOne.FromStatus = "B";
                            one._list.Add(subOne);
                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                  
                   
                    one.Closing = close;
                    data.Add(one);
                    
                    
                }
              
            }
            //Code added by gayathri:8/11/2022 display the voucherno with ascendingorder
            //var result = data.OrderBy(i => i._list.FirstOrDefault().VoucherNumber_one).ToList();

            //var result = data.OrderBy(i => i._list.OrderBy(d => new { d.VoucherNo })).ToList();
            var result = data.OrderBy(i => i.EnterDate).ToList();
            //return data.OrderBy(x => x.EnterDate).ToList();
            return result;

        }

        public List<LedgerReportDataModel> GetLedgerReport(DateTime startDate, DateTime toDate, long schoolId, long headId, long subId)
        {
            List<LedgerReportDataModel> list = new List<LedgerReportDataModel>();
            #region HeadId=0: Means wants to show only the all account head, avoid the subledgers
            if (headId == 0)
            {
                var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date).ToList();
                var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date).ToList();
                var accountHeadIdCash = cashData.Select(x =>x.HeadId).Distinct().ToList();
                var accountHeadIdBank = bankData.Select(x => x.HeadId).Distinct().ToList();
              
                var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
               
                if (accountHead.Count > 0 && accountHead != null)
                {
                    foreach (var item in accountHead)
                    {
                        LedgerReportDataModel one = new LedgerReportDataModel();
                        var headdata = school.tb_AccountHead.Where(x => x.AccountId == item).FirstOrDefault();
                        one.AccountHead = headdata.AccHeadName; 
                    
                        one._LedgerDetailsList = new List<LedgerDetailsList>();
                      
                        var cashDate = cashData.Where(x => x.HeadId == item).Select(x => x.EnterDate.Date).Distinct().ToList();
                        var bankDate = bankData.Where(x => x.HeadId == item).Select(x => x.EnterDate.Date).Distinct().ToList();
                        var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                        if (headdata.ForBill == true)
                        {
                            var datesCancel = school.tb_BillCancelAccounts.Where(x => x.IsActive == true && !allDates.Any(y => y.Date == x.CancelDate.Date) && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date).Select(x => x.CancelDate.Date).ToList();
                            allDates = allDates.Union(datesCancel).OrderBy(x => x.Date).ToList();
                        }

                        foreach (var date in allDates)
                        {
                            LedgerDetailsList subOne = new LedgerDetailsList();
                            subOne.AccountDate = date;
                            subOne.SubLedger = "";
                            
                            var incomeCash = cashData.Where(x => x.HeadId == item && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                            var incomeBank = bankData.Where(x => x.HeadId == item && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                            var expenseCash = cashData.Where(x => x.HeadId == item && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                            var expenseBank = bankData.Where(x => x.HeadId == item && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                            var income = incomeCash + incomeBank;
                            var expense = expenseCash + expenseBank;
                            if (headdata.ForBill == true)
                            {
                                var cancelExpense = school.tb_BillCancelAccounts.Where(x => x.CancelDate.Date == date.Date).Sum(x => x.Amount);
                                expense = cancelExpense;
                            }
                            subOne.Income = income ?? 0;
                            subOne.Expence = expense ?? 0;
                            if (income > expense)
                            {
                                //subOne.Income = (income - expense) ?? 0;
                                //subOne.Expence = 0;
                                subOne.Status = "Cr";
                            }
                            else if (income == expense)
                            {
                                //subOne.Expence = 0;
                                //subOne.Income = 0;
                                subOne.Status = "";
                            }
                            else
                            {
                                //subOne.Expence = (expense - income) ?? 0;
                                //subOne.Income = 0;
                                subOne.Status = "Dt";
                            }
                            one._LedgerDetailsList.Add(subOne);
                        }
                        list.Add(one);
                    }
                }
            }
            #endregion Means wants to show only the all account head, avoid the subledgers
            #region SubId=0 and Head!=0: Means wants to show only the particular head with no sub
            else if (subId == 0)
            {
                var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                var accountHeadIdCash = cashData.Select(x => x.HeadId).Distinct().ToList();
                var accountHeadIdBank = bankData.Select(x => x.HeadId).Distinct().ToList();
                var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
                if (accountHead.Count > 0 && accountHead != null)
                {
                    LedgerReportDataModel one = new LedgerReportDataModel();
                    var headHead = school.tb_AccountHead.Where(x => x.AccountId == headId).FirstOrDefault();
                    one.AccountHead = headHead.AccHeadName;
                    one._LedgerDetailsList = new List<LedgerDetailsList>();
                    var cashDate = cashData.Where(x => x.HeadId == headId).Select(x => x.EnterDate.Date).Distinct().ToList();
                    var bankDate = bankData.Where(x => x.HeadId == headId).Select(x => x.EnterDate.Date).Distinct().ToList();
                    var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                    if (headHead.ForBill == true)
                    {
                        var datesCancel = school.tb_BillCancelAccounts.Where(x => x.IsActive == true && !allDates.Any(y => y.Date == x.CancelDate.Date) && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date).Select(x => x.CancelDate.Date).ToList();
                        allDates = allDates.Union(datesCancel).OrderBy(x => x.Date).ToList();
                    }
                    foreach (var date in allDates)
                    {
                        LedgerDetailsList subOne = new LedgerDetailsList();
                        subOne.AccountDate = date;
                        subOne.SubLedger = "";
                        var incomeCash = cashData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                        var incomeBank = bankData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                        var expenseCash = cashData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                        var expenseBank = bankData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                        var income = incomeCash + incomeBank;
                        var expense = expenseCash + expenseBank;
                        if (headHead.ForBill == true)
                        {
                            var cancelExpense = school.tb_BillCancelAccounts.Where(x => x.CancelDate.Date == date.Date).Sum(x => x.Amount);
                            expense = cancelExpense;
                        }
                        subOne.Income = income ?? 0;
                        subOne.Expence = expense ?? 0;
                        if (income > expense)
                        {
                            subOne.Status = "Cr";
                        }
                        else if (income == expense)
                        {
                            subOne.Status = "";
                        }
                        else
                        {
                            subOne.Status = "Dt";
                        }
                        one._LedgerDetailsList.Add(subOne);
                    }
                    list.Add(one);
                }
            }
            #endregion SubId=0 and Head!=0: Means wants to show only the particular head with no sub

            //****************** Subledger select the all condition ***********************
            #region Select all subledger data
            else if (subId == 1)//Subledger ALL
            {
                var checkFee = school.tb_AccountHead.Where(x => x.AccountId == headId && x.ForBill == true).FirstOrDefault();
                #region Select all subledger data from the fees
                if (checkFee != null)
                {
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                    LedgerReportDataModel one = new LedgerReportDataModel();
                    one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                    one._LedgerDetailsList = new List<LedgerDetailsList>();
                    var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().OrderBy(x => x.Date).ToList();
                    var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().OrderBy(x => x.Date).ToList();
                    var allDates = cashDate.Union(bankDate).ToList().OrderBy(x => x.Date).ToList();
                    var cancellCashBillsNotIncludes = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date && !allDates.Any(y => y.Date == x.CancelDate.Date)).Select(x => x.CancelDate.Date).ToList();
                    allDates = allDates.Union(cancellCashBillsNotIncludes).ToList().OrderBy(x => x.Date).ToList();
                    if (allDates != null && allDates.Count > 0)
                    {
                        foreach (var day in allDates)
                        {
                            var cashSub = cashData.Where(x => x.EnterDate.Date == day.Date).Select(x => x.SubId).Distinct().ToList();
                            var bankSub = bankData.Where(x => x.EnterDate.Date == day.Date).Select(x => x.SubId).Distinct().ToList();
                            var allSub = cashSub.Union(bankSub).OrderBy(x => x).ToList();
                            var cancelSub = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date && x.CancelDate.Date == day.Date && !allSub.Any(y => y == x.ItemId)).Select(x => x.ItemId).ToList();
                            var sub = allSub.Union(cancelSub).ToList();
                            foreach (var item in sub)
                            {
                                LedgerDetailsList oneSub = new LedgerDetailsList();
                                oneSub.AccountDate = day;
                                oneSub.SubLedger = school.tb_Fee.Where(x => x.FeeId == item && x.IsActive).Select(x => x.FeesName).FirstOrDefault();
                                if (oneSub.SubLedger != null)
                                {
                                    var cashIncome = cashData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "R").Sum(x => x.Amount);
                                    var bankIncome = bankData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "R").Sum(x => x.Amount);

                                    var cashExpense = cashData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "P").Sum(x => x.Amount);
                                    var bankExpense = bankData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "P").Sum(x => x.Amount);

                                    var cancelExpense = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.CancelDate.Date == day.Date && x.ItemId == item).Sum(x => x.Amount);
                                    var income = cashIncome + bankIncome;
                                    var expense = bankExpense + cashExpense + cancelExpense;

                                    oneSub.Income = income ?? 0;
                                    oneSub.Expence = expense ?? 0;
                                    if (income > expense)
                                        oneSub.Status = "Cr";
                                    else if (income == expense)
                                        oneSub.Status = "";
                                    else
                                        oneSub.Status = "Dt";

                                    one._LedgerDetailsList.Add(oneSub);
                                }
                            }
                        }
                        list.Add(one);
                    }
                }
                #endregion Select all subledger data from the fees
                else
                {
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();

                    LedgerReportDataModel one = new LedgerReportDataModel();
                    one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                    one._LedgerDetailsList = new List<LedgerDetailsList>();
                    var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().ToList();
                    var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().ToList();
                    var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                    if (allDates != null && allDates.Count > 0)
                    {
                        foreach (var day in allDates)
                        {
                            var cashSub = cashData.Where(x => x.EnterDate.Date == day.Date).Select(x => x.SubId).Distinct().ToList();
                            var bankSub = bankData.Where(x => x.EnterDate.Date == day.Date).Select(x => x.SubId).Distinct().ToList();
                            var allSub = cashSub.Union(bankSub).OrderBy(x => x).ToList();
                            foreach (var item in allSub)
                            {
                                LedgerDetailsList oneSub = new LedgerDetailsList();
                                oneSub.AccountDate = day;
                                oneSub.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item && x.IsActive).Select(x => x.SubLedgerName).FirstOrDefault();
                                if (oneSub.SubLedger != null)
                                {
                                    var cashIncome = cashData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "R").Sum(x => x.Amount);
                                    var bankIncome = bankData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "R").Sum(x => x.Amount);
                                    var cashExpense = cashData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "P").Sum(x => x.Amount);
                                    var bankExpense = bankData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "P").Sum(x => x.Amount);
                                    var income = cashIncome + bankIncome;
                                    var expense = bankExpense + cashExpense;

                                    oneSub.Income = income ?? 0;
                                    oneSub.Expence = expense ?? 0;
                                    if (income > expense)
                                        oneSub.Status = "Cr";
                                    else if (income == expense)
                                        oneSub.Status = "";
                                    else
                                        oneSub.Status = "Dt";

                                    one._LedgerDetailsList.Add(oneSub);
                                }
                            }
                            list.Add(one);
                        }
                    }

                }
            }
            #endregion Select all subledger data
            //****************** Subledger select the all condition ***********************
            #region SubId!=0 and Head!=0: Means wants to show only the particular head with particular sub
            else
            {
                var checkFee = school.tb_AccountHead.Where(x => x.AccountId == headId && x.ForBill == true).FirstOrDefault();
                #region Means the filter head is From the fees
                if (checkFee != null)
                {
                    var cancelSubLedgerId = _Entities.tb_SubLedgerData.Where(x => x.IsActive && x.AccHeadId == checkFee.AccountId).OrderByDescending(x => x.LedgerId).FirstOrDefault();
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var accountHeadIdCash = cashData.Select(x => x.HeadId).Distinct().ToList();
                    var accountHeadIdBank = bankData.Select(x => x.HeadId).Distinct().ToList();
                    var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
                    if (accountHead.Count > 0 && accountHead != null)
                    {
                        LedgerReportDataModel one = new LedgerReportDataModel();
                        one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                        one._LedgerDetailsList = new List<LedgerDetailsList>();
                        var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().ToList();
                        var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().ToList();
                        var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                        if (allDates.Count > 0 && allDates != null)
                        {
                            foreach (var date in allDates)
                            {
                                LedgerDetailsList subOne = new LedgerDetailsList();
                                subOne.AccountDate = date;
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                var incomeCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                                var incomeBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                                var expenseCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                                var expenseBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                                if (cancelSubLedgerId != null && cancelSubLedgerId.SubLedgerName == "Bill Cancel")
                                {
                                    var cancelCash = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date == date && x.HeadId == headId && x.SubId == cancelSubLedgerId.LedgerId).ToList();
                                    var cancelBank = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date == date && x.HeadId == headId && x.SubId == cancelSubLedgerId.LedgerId).ToList();
                                    if (cancelCash.Count > 0 && cancelCash != null)
                                    {
                                        foreach (var xx in cancelCash)
                                        {
                                            var cancelAmount = school.tb_BillCancelAccounts.Where(x => x.CashBankType == false && x.CashBankId == xx.Id && x.ItemId == subId).ToList();
                                            if (cancelAmount != null && cancelAmount.Count > 0)
                                            {
                                                expenseCash = expenseCash + cancelAmount.Sum(x => x.Amount);
                                            }
                                        }
                                    }
                                    if (cancelBank.Count > 0 && cancelBank != null)
                                    {
                                        foreach (var xx in cancelBank)
                                        {
                                            var cancelAmount = school.tb_BillCancelAccounts.Where(x => x.CashBankType == true && x.CashBankId == xx.Id && x.ItemId == subId).ToList();
                                            if (cancelAmount != null && cancelAmount.Count > 0)
                                            {
                                                expenseBank = expenseBank + cancelAmount.Sum(x => x.Amount);
                                            }
                                        }
                                    }
                                }
                                var income = incomeCash + incomeBank;
                                var expense = expenseCash + expenseBank;
                                subOne.Income = income ?? 0;
                                subOne.Expence = expense ?? 0;
                                if (income > expense)
                                {
                                    //subOne.Income = (income - expense) ?? 0;
                                    //subOne.Expence = 0;
                                    subOne.Status = "Cr";
                                }
                                else if (income == expense)
                                {
                                    //subOne.Expence = 0;
                                    //subOne.Income = 0;
                                    subOne.Status = "";
                                }
                                else
                                {
                                    //subOne.Expence = (expense - income) ?? 0;
                                    //subOne.Income = 0;
                                    subOne.Status = "Dt";
                                }
                                one._LedgerDetailsList.Add(subOne);
                            }

                            #region Checking that the dates of cancelling while it we not taken
                            var cancellCashBillsNotIncludes = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.ItemId == subId && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date && !allDates.Any(y => y.Date == x.CancelDate.Date)).ToList();
                            if (cancellCashBillsNotIncludes != null && cancellCashBillsNotIncludes.Count > 0)
                            {
                                var cancelDates = cancellCashBillsNotIncludes.Select(x => x.CancelDate).Distinct().OrderBy(x => x.Date).ToList();
                                foreach (var yy in cancelDates)
                                {
                                    LedgerDetailsList subOne = new LedgerDetailsList();
                                    subOne.AccountDate = yy;
                                    subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                    subOne.Income = 0;
                                    subOne.Expence = cancellCashBillsNotIncludes.Where(x => x.CancelDate.Date == yy.Date).Sum(x => x.Amount);
                                    subOne.Status = "Dt";
                                    one._LedgerDetailsList.Add(subOne);
                                }
                            }
                            #endregion Checking that the dates of cancelling while it we not taken
                        }
                        list.Add(one);

                    }
                }
                #endregion Means the filter head is From the fees
                #region Means the filter head is general account section 
                else
                {
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var accountHeadIdCash = cashData.Select(x => x.HeadId).Distinct().ToList();
                    var accountHeadIdBank = bankData.Select(x => x.HeadId).Distinct().ToList();
                    var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
                    if (accountHead.Count > 0 && accountHead != null)
                    {
                        LedgerReportDataModel one = new LedgerReportDataModel();
                        one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                        one._LedgerDetailsList = new List<LedgerDetailsList>();
                        var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().ToList();
                        var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().ToList();
                        var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                        if (allDates.Count > 0 && allDates != null)
                        {
                            foreach (var date in allDates)
                            {
                                LedgerDetailsList subOne = new LedgerDetailsList();
                                subOne.AccountDate = date;
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == subId).Select(x => x.SubLedgerName).FirstOrDefault();
                                var incomeCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                                var incomeBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R").Sum(x => x.Amount);
                                var expenseCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                                var expenseBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P").Sum(x => x.Amount);
                                var income = incomeCash + incomeBank;
                                var expense = expenseCash + expenseBank;
                                subOne.Income = income ?? 0;
                                subOne.Expence = expense ?? 0;
                                if (income > expense)
                                {
                                    //subOne.Income = (income - expense) ?? 0;
                                    //subOne.Expence = 0;
                                    subOne.Status = "Cr";
                                }
                                else if (income == expense)
                                {
                                    //subOne.Expence = 0;
                                    //subOne.Income = 0;
                                    subOne.Status = "";
                                }
                                else
                                {
                                    //subOne.Expence = (expense - income) ?? 0;
                                    //subOne.Income = 0;
                                    subOne.Status = "Dt";
                                }
                                one._LedgerDetailsList.Add(subOne);
                            }
                        }
                        list.Add(one);
                    }
                }
                #endregion Means the filter head is general account section 
            }
            #endregion SubId!=0 and Head!=0: Means wants to show only the particular head with particular sub
            return list.OrderBy(x => x.AccountHead).ToList();
            //return list.ToList();
        }

        public tb_Staff Staff(long userId)
        {
            var data = _Entities.tb_Staff.Where(x => x.tb_Login.SchoolId == school.SchoolId && x.IsActive && x.tb_Login.IsActive && x.tb_Login.UserId == userId).FirstOrDefault();
            return data;
        }
        public int SalaryTypeStatus()
        {
            var data = school.tb_SalaryType.Where(x => x.IsActive).FirstOrDefault();
            if (data != null)
                return data.TypeId;
            else
                return 0;
        }
        public bool GetCurrentStatusOfWagesEmployees()
        {
            var data = school.tb_WagesShowsSettings.Where(x => x.IsActive).FirstOrDefault();
            if (data != null)
                return data.IsWagesShows;
            else
                return true;
        }


        //Basheer on 30-09-2019 for role module 
        public Nullable<long> UserType { get { return school.SchoolType ?? 0; } }

        public School(long id, int status) { school = _Entities.tb_School.Where(z => z.SchoolId == id).FirstOrDefault(); }
        public List<UserModule> GetTearhersModules()
        {
            List<UserModule> list = new List<UserModule>();
            if (school.SchoolType != null)
            {
                var data = _Entities.tb_SchoolModuleDetails.Where(c => c.SchoolModuleId == school.SchoolType && c.IsActive).ToList();
                foreach (var item in data)
                {
                    UserModule one = new UserModule();
                    one.MainId = item.MainId;
                    one.SubId = item.SchoolSubModuleId;
                    list.Add(one);
                }
            }
            return list;
        }

        //Baasheer on 21-11-2019 for accounts hide

        public bool GetAccountHide()
        {
            var data = _Entities.tb_AccountsHide.Where(x => x.IsActive && x.SchoolId == school.SchoolId).FirstOrDefault();
            if (data != null)
                return data.IsAccountHide;
            else
                return false;
        }

      

           

        //Basheer on 25-11-2019 to show details in accounts hiding section

        public List<DayBookReportModel> DayBookReportDetailsCollectionReciept(DateTime fromDate, DateTime toDate)
        {
           

            var banks = school.tb_Banks.ToList();
            List<DayBookReportModel> data = new List<DayBookReportModel>();
            var balanceDetails = school.tb_Balance.Where(x => (x.CurrentDate.Date) <= toDate.Date && (x.CurrentDate.Date) >= fromDate.Date && x.IsActive).OrderBy(x => x.CurrentDate).ToList();
            var bankDetails = school.tb_BankEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "RV").ToList();
            var cashDetails = school.tb_CashEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "RV").ToList();
            



            if (balanceDetails != null && balanceDetails.Count > 0)
            {
                var dateList = balanceDetails.GroupBy(x => x.CurrentDate.Date).ToList();
                foreach (var item in dateList)
                {
                    DayBookReportModel one = new DayBookReportModel();
                    one.EnterDate = item.Key;

                    //Used to store existing details from accounthidedetails table
                    var list = _Entities.tb_AccountsHideDetails.Where(z => z.Schoolid == school.SchoolId && z.VoucherType == "R" && (z.EnterDate >= fromDate && z.EnterDate <= toDate)).ToList();
                    if (list != null)
                        one.voucheridgroup = String.Join("~", from items in list select items.VoucherNo);

                    //end

                    //one.Opening = school.tb_Balance.Where(x => (x.CurrentDate.Date) < item.Key).Sum(x => x.Closing);
                    decimal cc = school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    decimal bnk = 0;
                    foreach (var bk in banks)
                    {
                        bnk = bnk + school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 2 && x.BankId == bk.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    }
                    one.Opening = bnk + cc;

                    decimal close = one.Opening;
                    one._list = new List<DayBookReportDetails>();
                    if (cashDetails != null && cashDetails.Count > 0)
                    {
                        var cashList = cashDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var cash in cashList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();
                            if (cash.BillNo != null && cash.BillNo != "")
                                //subOne.VoucherNo = cash.VoucherNumber + " / " + cash.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = cash.VoucherNumber + " - " + cash.BillNo + " / " + one.EnterDate.Year;
                            else
                                subOne.VoucherNo = cash.VoucherNumber;

                            subOne.TransactionType = cash.TransactionType;
                            subOne.AccountHeadName = cash.tb_AccountHead.AccHeadName;
                            if (cash.BillNo == null || cash.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == cash.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == cash.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (cash.TransactionType == "R")
                            {
                                subOne.IncomeAmount = cash.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = cash.Amount ?? 0;
                            }
                            subOne.Narration = cash.Narration;
                            subOne.FromStatus = "C";
                            one._list.Add(subOne);

                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    if (bankDetails != null && bankDetails.Count > 0)
                    {
                        var bankList = bankDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var bak in bankList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();
                            if (bak.BillNo != null && bak.BillNo != "")
                                //subOne.VoucherNo = bak.VoucherNumber + " / " + bak.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = bak.VoucherNumber + " - " + bak.BillNo + " / " + one.EnterDate.Year;
                            else
                                subOne.VoucherNo = bak.VoucherNumber;
                            subOne.TransactionType = bak.TransactionType;
                            subOne.AccountHeadName = bak.tb_AccountHead.AccHeadName;
                            if (bak.BillNo == null || bak.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == bak.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == bak.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (bak.TransactionType == "R")
                            {
                                subOne.IncomeAmount = bak.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = bak.Amount ?? 0;
                            }
                            subOne.Narration = bak.Narration;
                            subOne.FromStatus = "B";
                            one._list.Add(subOne);
                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    one.Closing = close;
                    data.Add(one);
                }
            }
            return data.OrderBy(x => x.EnterDate).ToList();
        }
        public List<DayBookReportModel> DayBookReportDetailsCollectionPayment(DateTime fromDate, DateTime toDate)
        {
            var banks = school.tb_Banks.ToList();
            List<DayBookReportModel> data = new List<DayBookReportModel>();
            var balanceDetails = school.tb_Balance.Where(x => (x.CurrentDate.Date) <= toDate.Date && (x.CurrentDate.Date) >= fromDate.Date && x.IsActive).OrderBy(x => x.CurrentDate).ToList();
            var bankDetails = school.tb_BankEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "PV").ToList();
            var cashDetails = school.tb_CashEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "PV").ToList();
            if (balanceDetails != null && balanceDetails.Count > 0)
            {
                var dateList = balanceDetails.GroupBy(x => x.CurrentDate.Date).ToList();
                foreach (var item in dateList)
                {
                    DayBookReportModel one = new DayBookReportModel();
                    one.EnterDate = item.Key;
                    //one.Opening = school.tb_Balance.Where(x => (x.CurrentDate.Date) < item.Key).Sum(x => x.Closing);
                    decimal cc = school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    decimal bnk = 0;
                    foreach (var bk in banks)
                    {
                        bnk = bnk + school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 2 && x.BankId == bk.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    }
                    one.Opening = bnk + cc;

                    decimal close = one.Opening;
                    one._list = new List<DayBookReportDetails>();
                    if (cashDetails != null && cashDetails.Count > 0)
                    {
                        var cashList = cashDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var cash in cashList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();
                            if (cash.BillNo != null && cash.BillNo != "")
                                //subOne.VoucherNo = cash.VoucherNumber + " / " + cash.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = cash.VoucherNumber + " - " + cash.BillNo + " / " + one.EnterDate.Year;
                            else
                                subOne.VoucherNo = cash.VoucherNumber;

                            subOne.TransactionType = cash.TransactionType;
                            subOne.AccountHeadName = cash.tb_AccountHead.AccHeadName;
                            if (cash.BillNo == null || cash.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == cash.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == cash.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (cash.TransactionType == "R")
                            {
                                subOne.IncomeAmount = cash.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = cash.Amount ?? 0;
                            }
                            subOne.Narration = cash.Narration;
                            subOne.FromStatus = "C";
                            one._list.Add(subOne);

                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    if (bankDetails != null && bankDetails.Count > 0)
                    {
                        var bankList = bankDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var bak in bankList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();
                            if (bak.BillNo != null && bak.BillNo != "")
                                //subOne.VoucherNo = bak.VoucherNumber + " / " + bak.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = bak.VoucherNumber + " - " + bak.BillNo + " / " + one.EnterDate.Year;
                            else
                                subOne.VoucherNo = bak.VoucherNumber;
                            subOne.TransactionType = bak.TransactionType;
                            subOne.AccountHeadName = bak.tb_AccountHead.AccHeadName;
                            if (bak.BillNo == null || bak.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == bak.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == bak.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (bak.TransactionType == "R")
                            {
                                subOne.IncomeAmount = bak.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = bak.Amount ?? 0;
                            }
                            subOne.Narration = bak.Narration;
                            subOne.FromStatus = "B";
                            one._list.Add(subOne);
                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    one.Closing = close;
                    data.Add(one);
                }
            }
            return data.OrderBy(x => x.EnterDate).ToList();
        }


        public List<DayBookReportModel> DayBookReportDetailsCollectionContra(DateTime fromDate, DateTime toDate)
        {
            var banks = school.tb_Banks.ToList();
            List<DayBookReportModel> data = new List<DayBookReportModel>();
            var balanceDetails = school.tb_Balance.Where(x => (x.CurrentDate.Date) <= toDate.Date && (x.CurrentDate.Date) >= fromDate.Date && x.IsActive).OrderBy(x => x.CurrentDate).ToList();
            var bankDetails = school.tb_BankEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "CV").ToList();
            var cashDetails = school.tb_CashEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "CV").ToList();
            if (balanceDetails != null && balanceDetails.Count > 0)
            {
                var dateList = balanceDetails.GroupBy(x => x.CurrentDate.Date).ToList();
                foreach (var item in dateList)
                {
                    DayBookReportModel one = new DayBookReportModel();
                    one.EnterDate = item.Key;
                    //one.Opening = school.tb_Balance.Where(x => (x.CurrentDate.Date) < item.Key).Sum(x => x.Closing);
                    decimal cc = school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    decimal bnk = 0;
                    foreach (var bk in banks)
                    {
                        bnk = bnk + school.tb_Balance.Where(x => x.CurrentDate.Date < item.Key && x.SourceId == 2 && x.BankId == bk.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
                    }
                    one.Opening = bnk + cc;

                    decimal close = one.Opening;
                    one._list = new List<DayBookReportDetails>();
                    if (cashDetails != null && cashDetails.Count > 0)
                    {
                        var cashList = cashDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var cash in cashList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();
                            if (cash.BillNo != null && cash.BillNo != "")
                                //subOne.VoucherNo = cash.VoucherNumber + " / " + cash.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = cash.VoucherNumber + " - " + cash.BillNo + " / " + one.EnterDate.Year;
                            else
                                subOne.VoucherNo = cash.VoucherNumber;

                            subOne.TransactionType = cash.TransactionType;
                            subOne.AccountHeadName = cash.tb_AccountHead.AccHeadName;
                            if (cash.BillNo == null || cash.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == cash.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == cash.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (cash.TransactionType == "R")
                            {
                                subOne.IncomeAmount = cash.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = cash.Amount ?? 0;
                            }
                            subOne.Narration = cash.Narration;
                            subOne.FromStatus = "C";
                            one._list.Add(subOne);

                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    if (bankDetails != null && bankDetails.Count > 0)
                    {
                        var bankList = bankDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                        foreach (var bak in bankList)
                        {
                            DayBookReportDetails subOne = new DayBookReportDetails();
                            if (bak.BillNo != null && bak.BillNo != "")
                                //subOne.VoucherNo = bak.VoucherNumber + " / " + bak.BillNo;//BILLNO WITH YEAR FOR MONISHA 
                                subOne.VoucherNo = bak.VoucherNumber + " - " + bak.BillNo + " / " + one.EnterDate.Year;
                            else
                                subOne.VoucherNo = bak.VoucherNumber;
                            subOne.TransactionType = bak.TransactionType;
                            subOne.AccountHeadName = bak.tb_AccountHead.AccHeadName;
                            if (bak.BillNo == null || bak.BillNo == string.Empty)
                                subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == bak.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
                            else
                                subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == bak.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
                            if (bak.TransactionType == "R")
                            {
                                subOne.IncomeAmount = bak.Amount ?? 0;
                                subOne.ExpenseAmount = 0;
                            }
                            else
                            {
                                subOne.IncomeAmount = 0;
                                subOne.ExpenseAmount = bak.Amount ?? 0;
                            }
                            subOne.Narration = bak.Narration;
                            subOne.FromStatus = "B";
                            one._list.Add(subOne);
                            close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
                        }
                    }
                    one.Closing = close;
                    data.Add(one);
                }
            }
            return data.OrderBy(x => x.EnterDate).ToList();
        }

        //Day book report after hiding account details ..it will not show opening an closing balance

        public List<DayBookReportModelAccountHide> DayBookReportDetailsCollectionAfterHide(DateTime fromDate, DateTime toDate)
        {

            List<DayBookReportModelAccountHide> data = new List<DayBookReportModelAccountHide>();

            //var bankDetails = school.tb_BankEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "CV").ToList();
            //var cashDetails = school.tb_CashEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate.Date) <= toDate.Date && (x.EnterDate.Date) >= fromDate.Date && x.VoucherType == "CV").ToList();
            var AccountsHideDetails = _Entities.tb_AccountsHideDetails.Where(x => x.Schoolid == school.SchoolId && (x.EnterDate) <= toDate && (x.EnterDate) >= fromDate).ToList();

            var dateList = AccountsHideDetails.GroupBy(x => x.EnterDate).ToList();
            foreach (var item in dateList)
            {
                DayBookReportModelAccountHide one = new DayBookReportModelAccountHide();
                one.EnterDate = item.Key;
                //one.Opening = school.tb_Balance.Where(x => (x.CurrentDate.Date) < item.Key).Sum(x => x.Closing);

                one._list = new List<DayBookReportDetailsAccountHide>();
                if (AccountsHideDetails != null && AccountsHideDetails.Count > 0)
                {
                    var cashList = AccountsHideDetails.Where(x => x.EnterDate.Date == item.Key).ToList();
                    foreach (var cash in cashList)
                    {
                        DayBookReportDetailsAccountHide subOne = new DayBookReportDetailsAccountHide();
                        subOne.VoucherNo = cash.VoucherNo;
                        subOne.TransactionType = cash.TransactionType;
                        subOne.AccountHeadName = cash.AccountHeadName;
                        subOne.SubLedger = cash.SubLedger;
                        subOne.IncomeAmount = cash.IncomeAmount;
                        subOne.ExpenseAmount = cash.ExpenseAmount;
                        subOne.Narration = cash.Naration;
                        subOne.FromStatus = cash.FromStatus;
                        subOne.VoucherType = cash.VoucherType;
                        one._list.Add(subOne);
                    }
                }
                data.Add(one);
            }

            return data.OrderBy(x => x.EnterDate).ToList();
        }


        //jibin 9/17/2020
        public List<StockUpdate> GetPurchaseByDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            string EndDate = endDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            var data = school.tb_StockUpdate.Where(x => x.IsActive && x.TimeStamp <= endDate && x.TimeStamp >= startDate).ToList().Select(x => new StockUpdate(x)).OrderByDescending(x => x.TimeStamp).ToList();
            return data;

        }


        public List<StockUpdate> GetSalesByDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            string EndDate = endDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            var data = school.tb_StockUpdate.Where(x => x.IsActive && x.TimeStamp <= endDate && x.TimeStamp >= startDate).ToList().Select(x => new StockUpdate(x)).OrderByDescending(x => x.TimeStamp).ToList();
            return data;

        }

         public List<PurchaseUpdate> GetCurrentPurchaseReport(long catId)
        {
            if (catId != 0)
            {

              //  var d = _Entities.tb_Purchase.Where(x => x.IsActive ).ToList().OrderByDescending(x => x.TimeStamp).ToList();
                var data =_Entities.tb_Purchase.Where(x => x.IsActive && x.CategoryId == catId).ToList().Select(z => new PurchaseUpdate(z)).OrderByDescending(z => z.TimeStamp).ToList();
                return data;
            }
            else
            {
                var data =_Entities.tb_Purchase.Where(x => x.IsActive).ToList().Select(z => new PurchaseUpdate(z)).OrderByDescending(z => z.TimeStamp).ToList();
                return data;
            }
        }


        public List<Student> GetStudentDetailsByAdmissionNo(string adno)
        {
            return _Entities.tb_Student.Where(z => z.StudentSpecialId == adno).ToList().Select(q => new Student(q)).ToList();
        }





        public List<StockSales> GetCollectionByDate(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            string EndDate = endDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            var data = _Entities.tb_SalesStock.Where(x => x.IsActive && x.TimeStamp <= endDate && x.TimeStamp >= startDate).ToList().Select(x => new StockSales(x)).OrderByDescending(x => x.TimeStamp).ToList();
            return data;

        }



        public List<StockSales> GetCurrentCollectionReport(long catId)
        {
            if (catId != 0)
            {
                var data = _Entities.tb_SalesStock.Where(x => x.IsActive && x.CategoryId == catId).ToList().Select(x => new StockSales(x)).OrderByDescending(x => x.TimeStamp).ToList();
                return data;
            }
            else
            {
                var data = _Entities.tb_SalesStock.Where(x => x.IsActive).ToList().Select(x => new StockSales(x)).OrderByDescending(x => x.TimeStamp).ToList();
                return data;
            }
        }




        public List<SpCashEntryReportStock> GetCashBookReportStock(DateTime fromdate, DateTime toDate, long schoolId)
        {
            var data = _Entities.sp_StockCashEntryReport(SchoolId, fromdate, toDate).ToList().Select(x => new SpCashEntryReportStock(x)).ToList();
            return data;
        }


        public List<SPBankEntryReportStock> GetBankBookReportStock(DateTime fromdate, DateTime toDate, long schoolId, long bankId)
        {
            var data = _Entities.sp_StockBankEntryReport(SchoolId, fromdate, toDate, bankId).ToList().Select(x => new SPBankEntryReportStock(x)).ToList();
            return data;
        }




        public List<SPStockDayBookReport> GetDayBookReportStock(DateTime fromDate, DateTime toDate, long schoolId) 
        {
            var data = _Entities.sp_StockDayBookReport(schoolId, fromDate, toDate).ToList().Select(x => new SPStockDayBookReport(x)).ToList();
            return data;
        }

        public string GetOpeningBalanceStock(DateTime fromDate, long bankId, int sourceId)// sourceId:0=Cash ,1=Bank
        {
            try
            {
                if (bankId == 0 && sourceId == 2)
                {
                    decimal close = 0;
                    long bId = 0;
                    var data = _Entities.sp_StockOpeningBalanceAmountInCashBook(school.SchoolId, fromDate, bankId, sourceId).ToList();
                    foreach (var item in data)
                    {
                        if (bId != item.BankId)
                        {
                            close = close + item.Opening ?? 0;
                        }
                        bId = item.BankId ?? 0;
                    }
                    return Convert.ToString(close);
                }
                else
                {
                    var data = _Entities.sp_StockOpeningBalanceAmountInCashBook(school.SchoolId, fromDate, bankId, sourceId).FirstOrDefault();
                    return Convert.ToString(data.Opening);
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string GetClosingBalanceStock(DateTime fromDate, long bankId, int sourceId)// sourceId:0=Cash ,1=Bank
        {
            try
            {
                if (bankId == 0 && sourceId == 2)
                {
                    decimal close = 0;
                    long bId = 0;
                    var data = _Entities.sp_StockClosingBalanceAmount(school.SchoolId, fromDate, bankId, sourceId).ToList();
                    foreach (var item in data)
                    {
                        if (bId != item.BankId)
                        {
                            close = close + item.Opening ?? 0;
                        }
                        bId = item.BankId ?? 0;
                    }
                    return Convert.ToString(close);
                }
                else
                {
                    var data = _Entities.sp_StockClosingBalanceAmount(school.SchoolId, fromDate, bankId, sourceId).FirstOrDefault();
                    return Convert.ToString(data.Opening);
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public List<StockSales> GetSalesByDateStock(DateTime startDate, DateTime endDate)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            string EndDate = endDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            var data = _Entities.tb_SalesStock.Where(x => x.IsActive && x.TimeStamp <= endDate && x.TimeStamp >= startDate).ToList().Select(x => new StockSales(x)).OrderByDescending(x => x.TimeStamp).ToList();
            return data;

        }



        public List<StockSales> GetCurrentStockReportStock(long catId)
        {
            if (catId != 0)
            {
                var data = _Entities.tb_SalesStock.Where(x => x.IsActive && x.CategoryId == catId).ToList().Select(x => new StockSales(x)).OrderByDescending(x => x.TimeStamp).ToList();
                return data;
            }
            else
            {
                var data = _Entities.tb_SalesStock.Where(x => x.IsActive).ToList().Select(x => new StockSales(x)).OrderByDescending(x => x.TimeStamp).ToList();
                return data;
            }
        }


        //public List<DayBookReportModel> DayBookReportDetailsCollections(DateTime fromDate, DateTime toDate)
        //{
        //    var banks = school.tb_Banks.ToList();
        //    string FromtDate = fromDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
        //    string ToDate = toDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
        //    List<DayBookReportModel> data = new List<DayBookReportModel>();
        //    var balanceDetails = school.tb_StockBalance.Where(x => (x.CurrentDate.Date) <= toDate.Date && (x.CurrentDate.Date) >= fromDate.Date && x.IsActive).OrderBy(x => x.CurrentDate).ToList();
        //    var balanceDetails = _Entities.tb_StockBalance.Where(x => x.CurrentDate <= toDate && x.CurrentDate >= fromDate && x.IsActive).OrderBy(x => x.CurrentDate).ToList();
        //    var bankDetails = _Entities.tb_StockBankEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate) <= toDate && (x.EnterDate) >= fromDate).ToList();
        //    var cashDetails = _Entities.tb_StockCashEntry.Where(x => x.CancelStatus == false && x.IsActive == true && (x.EnterDate) <= toDate && (x.EnterDate) >= fromDate).ToList();
        //    if (balanceDetails != null && balanceDetails.Count > 0)
        //    {
        //        var dateList = balanceDetails.GroupBy(x => x.CurrentDate.Date).ToList();
        //        foreach (var item in dateList)
        //        {
        //            DayBookReportModel one = new DayBookReportModel();
        //            one.EnterDate = item.Key.Date;
        //            one.Opening = school.tb_Balance.Where(x => (x.CurrentDate.Date) < item.Key).Sum(x => x.Closing);
        //            decimal cc = _Entities.tb_StockBalance.Where(x => (x.CurrentDate) < item.Key && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
        //            decimal bnk = 0;
        //            foreach (var bk in banks)
        //            {
        //                bnk = bnk + _Entities.tb_StockBalance.Where(x => (x.CurrentDate) < item.Key && x.SourceId == 2 && x.BankId == bk.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.Closing).FirstOrDefault();
        //            }
        //            one.Opening = bnk + 0;

        //            decimal close = one.Opening;
        //            one._list = new List<DayBookReportDetails>();
        //            if (cashDetails != null && cashDetails.Count > 0)
        //            {
        //                var cashList = cashDetails.Where(x => (x.EnterDate) == item.Key).ToList();
        //                foreach (var cash in cashList)
        //                {
        //                    DayBookReportDetails subOne = new DayBookReportDetails();
        //                    if (cash.BillNo != null && cash.BillNo != "")
        //                        subOne.VoucherNo = cash.VoucherNumber + " / " + cash.BillNo;//BILLNO WITH YEAR FOR MONISHA 
        //                    subOne.VoucherNo = cash.VoucherNumber + " - " + cash.BillNo + " / " + one.EnterDate.Year;
        //                    else
        //                        subOne.VoucherNo = cash.VoucherNumber;

        //                    subOne.TransactionType = cash.TransactionType;
        //                    subOne.AccountHeadName = cash.tb_AccountHead.AccHeadName;
        //                    if (cash.BillNo == null || cash.BillNo == string.Empty)
        //                        subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == cash.StockId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
        //                    else
        //                        subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == cash.StockId).Select(x => x.FeesName).FirstOrDefault() ?? "";
        //                    if (cash.TransactionType == "R")
        //                    {
        //                        subOne.IncomeAmount = cash.Amount ?? 0;
        //                        subOne.ExpenseAmount = 0;
        //                    }
        //                    else
        //                    {
        //                        subOne.IncomeAmount = 0;
        //                        subOne.ExpenseAmount = cash.Amount ?? 0;
        //                    }
        //                    subOne.Narration = cash.Narration;
        //                    subOne.FromStatus = "C";
        //                    one._list.Add(subOne);

        //                    close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
        //                }
        //            }
        //            if (bankDetails != null && bankDetails.Count > 0)
        //            {
        //                var bankList = bankDetails.Where(x => x.EnterDate == item.Key).ToList();
        //                foreach (var bak in bankList)
        //                {
        //                    DayBookReportDetails subOne = new DayBookReportDetails();
        //                    if (bak.BillNo != null && bak.BillNo != "")
        //                        subOne.VoucherNo = bak.VoucherNumber + " / " + bak.BillNo;//BILLNO WITH YEAR FOR MONISHA 
        //                    subOne.VoucherNo = bak.VoucherNumber + " - " + bak.BillNo + " / " + one.EnterDate.Year;
        //                    else
        //                        subOne.VoucherNo = bak.VoucherNumber;
        //                    subOne.TransactionType = bak.TransactionType;
        //                    subOne.AccountHeadName = bak.tb_AccountHead.AccHeadName;
        //                    if (bak.BillNo == null || bak.BillNo == string.Empty)
        //                        subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == bak.SubId).Select(x => x.SubLedgerName).FirstOrDefault() ?? "";
        //                    else
        //                        subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == bak.SubId).Select(x => x.FeesName).FirstOrDefault() ?? "";
        //                    if (bak.TransactionType == "R")
        //                    {
        //                        subOne.IncomeAmount = bak.Amount ?? 0;
        //                        subOne.ExpenseAmount = 0;
        //                    }
        //                    else
        //                    {
        //                        subOne.IncomeAmount = 0;
        //                        subOne.ExpenseAmount = bak.Amount ?? 0;
        //                    }
        //                    subOne.Narration = bak.Narration;
        //                    subOne.FromStatus = "B";
        //                    one._list.Add(subOne);
        //                    close = close + subOne.IncomeAmount - subOne.ExpenseAmount;
        //                }
        //            }
        //            one.Closing = close;
        //            data.Add(one);
        //        }
        //    }
        //    return data.OrderBy(x => x.EnterDate).ToList();
        //}


        //jibin 9/17/2020

        //jibin 11/26/2020
        //jibin 11/26/2020
        //jibin 11/26/2020
        public List<LedgerReportDataModel> Report_GetLedgerReport(DateTime startDate, DateTime toDate, long schoolId, long headId, long subId)
        {
            List<LedgerReportDataModel> list = new List<LedgerReportDataModel>();
            #region HeadId=0: Means wants to show only the all account head, avoid the subledgers
            if (headId == 0)
            {
                var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date).ToList();
                var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date).ToList();
                var accountHeadIdCash = cashData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                var accountHeadIdBank = bankData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();

                var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();

                if (accountHead.Count > 0 && accountHead != null)
                {
                    foreach (var item in accountHead)
                    {
                        LedgerReportDataModel one = new LedgerReportDataModel();
                        var headdata = school.tb_AccountHead.Where(x => x.AccountId == item.HeadId).FirstOrDefault();
                        one.AccountHead = headdata.AccHeadName;


                        one.Narration = item.Narration;

                        one._LedgerDetailsList = new List<LedgerDetailsList>();

                        var cashDate = cashData.Where(x => x.HeadId == item.HeadId).Select(x => x.EnterDate.Date).Distinct().ToList();
                        var bankDate = bankData.Where(x => x.HeadId == item.HeadId).Select(x => x.EnterDate.Date).Distinct().ToList();
                        var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                        if (headdata.ForBill == true)
                        {
                            var datesCancel = school.tb_BillCancelAccounts.Where(x => x.IsActive == true && !allDates.Any(y => y.Date == x.CancelDate.Date) && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date).Select(x => x.CancelDate.Date).ToList();
                            allDates = allDates.Union(datesCancel).OrderBy(x => x.Date).ToList();
                        }

                        foreach (var date in allDates)
                        {
                            LedgerDetailsList subOne = new LedgerDetailsList();
                            subOne.AccountDate = date;
                            subOne.SubLedger = "";

                            var incomeCash = cashData.Where(x => x.HeadId == item.HeadId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            var incomeBank = bankData.Where(x => x.HeadId == item.HeadId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            var expenseCash = cashData.Where(x => x.HeadId == item.HeadId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            var expenseBank = bankData.Where(x => x.HeadId == item.HeadId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            if (incomeCash == null && incomeBank == null && expenseBank == null && expenseCash == null)
                            {
                                continue;
                            }
                            if (incomeCash == null)
                            {
                                incomeCash = 0;
                            }
                            if (incomeBank == null)
                            {
                                incomeBank = 0;
                            }
                            if (expenseCash == null)
                            {
                                expenseCash = 0;
                            }
                            if (expenseBank == null)
                            {
                                expenseBank = 0;
                            }

                            var income = incomeCash + incomeBank;
                            var expense = expenseCash + expenseBank;
                            if (headdata.ForBill == true)
                            {
                                var cancelExpense = school.tb_BillCancelAccounts.Where(x => x.CancelDate.Date == date.Date).Sum(x => x.Amount);
                                expense = cancelExpense;
                            }
                            one.Income = income ?? 0;
                            one.Expence = expense ?? 0;
                            subOne.Income = income ?? 0;
                            subOne.Expence = expense ?? 0;
                            if (income > expense)
                            {
                                //subOne.Income = (income - expense) ?? 0;
                                //subOne.Expence = 0;
                                subOne.Status = "Cr";
                            }
                            else if (income == expense)
                            {
                                //subOne.Expence = 0;
                                //subOne.Income = 0;
                                subOne.Status = "";
                            }
                            else
                            {
                                //subOne.Expence = (expense - income) ?? 0;
                                //subOne.Income = 0;
                                subOne.Status = "Dt";
                            }
                            one._LedgerDetailsList.Add(subOne);
                        }
                        list.Add(one);
                    }
                }
            }
            #endregion Means wants to show only the all account head, avoid the subledgers
            #region SubId=0 and Head!=0: Means wants to show only the particular head with no sub
            else if (subId == 0)
            {
                var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                var accountHeadIdCash = cashData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                var accountHeadIdBank = bankData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
                if (accountHead.Count > 0 && accountHead != null)
                {
                    foreach (var item in accountHead)
                    {
                        LedgerReportDataModel one = new LedgerReportDataModel();
                        var headHead = school.tb_AccountHead.Where(x => x.AccountId == headId).FirstOrDefault();
                        one.AccountHead = headHead.AccHeadName;
                        one.Narration = item.Narration;

                        one._LedgerDetailsList = new List<LedgerDetailsList>();
                        var cashDate = cashData.Where(x => x.HeadId == headId).Select(x => x.EnterDate.Date).Distinct().ToList();
                        var bankDate = bankData.Where(x => x.HeadId == headId).Select(x => x.EnterDate.Date).Distinct().ToList();
                        var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                        if (headHead.ForBill == true)
                        {
                            var datesCancel = school.tb_BillCancelAccounts.Where(x => x.IsActive == true && !allDates.Any(y => y.Date == x.CancelDate.Date) && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date).Select(x => x.CancelDate.Date).ToList();
                            allDates = allDates.Union(datesCancel).OrderBy(x => x.Date).ToList();
                        }
                        foreach (var date in allDates)
                        {
                            LedgerDetailsList subOne = new LedgerDetailsList();
                            subOne.AccountDate = date;
                            subOne.SubLedger = "";
                            var incomeCash = cashData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            var incomeBank = bankData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            var expenseCash = cashData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            var expenseBank = bankData.Where(x => x.HeadId == headId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                            if (incomeCash == null && incomeBank == null && expenseBank == null && expenseCash == null)
                            {
                                continue;
                            }
                            if (incomeCash == null)
                            {
                                incomeCash = 0;
                            }
                            if (incomeBank == null)
                            {
                                incomeBank = 0;
                            }
                            if (expenseCash == null)
                            {
                                expenseCash = 0;
                            }
                            if (expenseBank == null)
                            {
                                expenseBank = 0;
                            }
                            var income = incomeCash + incomeBank;
                            var expense = expenseCash + expenseBank;
                            if (headHead.ForBill == true)
                            {
                                var cancelExpense = school.tb_BillCancelAccounts.Where(x => x.CancelDate.Date == date.Date).Sum(x => x.Amount);
                                expense = cancelExpense;
                            }
                            one.Income = income ?? 0;
                            one.Expence = expense ?? 0;
                            subOne.Income = income ?? 0;
                            subOne.Expence = expense ?? 0;
                            if (income > expense)
                            {
                                subOne.Status = "Cr";
                            }
                            else if (income == expense)
                            {
                                subOne.Status = "";
                            }
                            else
                            {
                                subOne.Status = "Dt";
                            }
                            one._LedgerDetailsList.Add(subOne);
                        }
                        list.Add(one);
                    }
                }
            }
            #endregion SubId=0 and Head!=0: Means wants to show only the particular head with no sub

            //****************** Subledger select the all condition ***********************
            #region Select all subledger data
            else if (subId == 1)//Subledger ALL
            {
                var checkFee = school.tb_AccountHead.Where(x => x.AccountId == headId && x.ForBill == true).FirstOrDefault();
                #region Select all subledger data from the fees
                if (checkFee != null)
                {
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                    LedgerReportDataModel one = new LedgerReportDataModel();
                    one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                    one._LedgerDetailsList = new List<LedgerDetailsList>();
                    var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().OrderBy(x => x.Date).ToList();
                    var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().OrderBy(x => x.Date).ToList();
                    var allDates = cashDate.Union(bankDate).ToList().OrderBy(x => x.Date).ToList();
                    var cancellCashBillsNotIncludes = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date && !allDates.Any(y => y.Date == x.CancelDate.Date)).Select(x => x.CancelDate.Date).ToList();
                    allDates = allDates.Union(cancellCashBillsNotIncludes).ToList().OrderBy(x => x.Date).ToList();
                    if (allDates != null && allDates.Count > 0)
                    {
                        foreach (var day in allDates)
                        {
                            var cashSub = cashData.Where(x => x.EnterDate.Date == day.Date).Select(x => x.SubId).Distinct().ToList();
                            var bankSub = bankData.Where(x => x.EnterDate.Date == day.Date).Select(x => x.SubId).Distinct().ToList();
                            var allSub = cashSub.Union(bankSub).OrderBy(x => x).ToList();
                            var cancelSub = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date && x.CancelDate.Date == day.Date && !allSub.Any(y => y == x.ItemId)).Select(x => x.ItemId).ToList();
                            var sub = allSub.Union(cancelSub).ToList();
                            foreach (var item in sub)
                            {
                                LedgerDetailsList oneSub = new LedgerDetailsList();

                                oneSub.AccountDate = day;
                                oneSub.SubLedger = school.tb_Fee.Where(x => x.FeeId == item && x.IsActive).Select(x => x.FeesName).FirstOrDefault();
                                if (oneSub.SubLedger != null)
                                {
                                    var cashIncome = cashData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "R").Select(x => x.Amount).FirstOrDefault();
                                    var bankIncome = bankData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "R").Select(x => x.Amount).FirstOrDefault();

                                    var cashExpense = cashData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "P").Select(x => x.Amount).FirstOrDefault();
                                    var bankExpense = bankData.Where(x => x.HeadId == headId && x.SubId == item && x.EnterDate.Date == day.Date && x.TransactionType == "P").Select(x => x.Amount).FirstOrDefault();
                                    if (cashIncome == null && bankIncome == null && cashExpense == null && bankExpense == null)
                                    {
                                        continue;
                                    }
                                    if (cashIncome == null)
                                    {
                                        cashIncome = 0;
                                    }
                                    if (bankIncome == null)
                                    {
                                        bankIncome = 0;
                                    }
                                    if (cashExpense == null)
                                    {
                                        cashExpense = 0;
                                    }
                                    if (bankExpense == null)
                                    {
                                        bankExpense = 0;
                                    }

                                    var cancelExpense = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.CancelDate.Date == day.Date && x.ItemId == item).Sum(x => x.Amount);
                                    var income = cashIncome + bankIncome;
                                    var expense = bankExpense + cashExpense + cancelExpense;
                                    one.Income = income ?? 0;
                                    one.Expence = expense ?? 0;
                                    oneSub.Income = income ?? 0;
                                    oneSub.Expence = expense ?? 0;
                                    if (income > expense)
                                        oneSub.Status = "Cr";
                                    else if (income == expense)
                                        oneSub.Status = "";
                                    else
                                        oneSub.Status = "Dt";

                                    one._LedgerDetailsList.Add(oneSub);
                                }
                            }
                        }
                        list.Add(one);
                    }
                }
                #endregion Select all subledger data from the fees
                else
                {
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId).ToList();

                    LedgerReportDataModel one = new LedgerReportDataModel();
                    one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                    one._LedgerDetailsList = new List<LedgerDetailsList>();
                    var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().ToList();
                    var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().ToList();
                    var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                    if (allDates != null && allDates.Count > 0)
                    {
                        foreach (var day in allDates)
                        {
                            var cashSub = cashData.Where(x => x.EnterDate.Date == day.Date).Select(x => new { x.SubId, x.VoucherNumber, x.Narration }).Distinct().ToList();
                            var bankSub = bankData.Where(x => x.EnterDate.Date == day.Date).Select(x => new { x.SubId, x.VoucherNumber, x.Narration }).Distinct().ToList();
                            var allSub = cashSub.Union(bankSub).OrderBy(x => x).ToList();
                            foreach (var item in allSub)
                            {
                                LedgerDetailsList oneSub = new LedgerDetailsList();
                                one.Narration = item.Narration;
                                oneSub.AccountDate = day;
                                oneSub.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == item.SubId && x.IsActive).Select(x => x.SubLedgerName).FirstOrDefault();
                                if (oneSub.SubLedger != null)
                                {
                                    var cashIncome = cashData.Where(x => x.HeadId == headId && x.SubId == item.SubId && x.EnterDate.Date == day.Date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var bankIncome = bankData.Where(x => x.HeadId == headId && x.SubId == item.SubId && x.EnterDate.Date == day.Date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var cashExpense = cashData.Where(x => x.HeadId == headId && x.SubId == item.SubId && x.EnterDate.Date == day.Date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var bankExpense = bankData.Where(x => x.HeadId == headId && x.SubId == item.SubId && x.EnterDate.Date == day.Date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    if (cashIncome == null && bankIncome == null && cashExpense == null && bankExpense == null)
                                    {
                                        continue;
                                    }
                                    if (cashIncome == null)
                                    {
                                        cashIncome = 0;
                                    }
                                    if (bankIncome == null)
                                    {
                                        bankIncome = 0;
                                    }
                                    if (cashExpense == null)
                                    {
                                        cashExpense = 0;
                                    }
                                    if (bankExpense == null)
                                    {
                                        bankExpense = 0;
                                    }

                                    var income = cashIncome + bankIncome;
                                    var expense = bankExpense + cashExpense;
                                    one.Income = income ?? 0;
                                    one.Expence = expense ?? 0;
                                    oneSub.Income = income ?? 0;
                                    oneSub.Expence = expense ?? 0;
                                    if (income > expense)
                                        oneSub.Status = "Cr";
                                    else if (income == expense)
                                        oneSub.Status = "";
                                    else
                                        oneSub.Status = "Dt";

                                    one._LedgerDetailsList.Add(oneSub);
                                }
                            }
                            list.Add(one);
                        }
                    }

                }
            }
            #endregion Select all subledger data
            //****************** Subledger select the all condition ***********************
            #region SubId!=0 and Head!=0: Means wants to show only the particular head with particular sub
            else
            {
                var checkFee = school.tb_AccountHead.Where(x => x.AccountId == headId && x.ForBill == true).FirstOrDefault();
                #region Means the filter head is From the fees
                if (checkFee != null)
                {
                    var cancelSubLedgerId = _Entities.tb_SubLedgerData.Where(x => x.IsActive && x.AccHeadId == checkFee.AccountId).OrderByDescending(x => x.LedgerId).FirstOrDefault();
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var accountHeadIdCash = cashData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                    var accountHeadIdBank = bankData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                    var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
                    if (accountHead.Count > 0 && accountHead != null)
                    {
                        foreach (var item in accountHead)
                        {
                            LedgerReportDataModel one = new LedgerReportDataModel();
                            one.Narration = item.Narration;
                            one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                            one._LedgerDetailsList = new List<LedgerDetailsList>();
                            var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().ToList();
                            var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().ToList();
                            var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                            if (allDates.Count > 0 && allDates != null)
                            {
                                foreach (var date in allDates)
                                {
                                    LedgerDetailsList subOne = new LedgerDetailsList();
                                    subOne.AccountDate = date;
                                    subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                    var incomeCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var incomeBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var expenseCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var expenseBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();

                                    if (incomeCash == null && incomeBank == null && expenseCash == null && expenseBank == null)
                                    {
                                        continue;
                                    }
                                    if (incomeCash == null)
                                    {
                                        incomeCash = 0;
                                    }
                                    if (incomeBank == null)
                                    {
                                        incomeBank = 0;
                                    }
                                    if (expenseCash == null)
                                    {
                                        expenseCash = 0;
                                    }
                                    if (expenseBank == null)
                                    {
                                        expenseBank = 0;
                                    }
                                    if (cancelSubLedgerId != null && cancelSubLedgerId.SubLedgerName == "Bill Cancel")
                                    {
                                        var cancelCash = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date == date && x.HeadId == headId && x.SubId == cancelSubLedgerId.LedgerId).ToList();
                                        var cancelBank = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date == date && x.HeadId == headId && x.SubId == cancelSubLedgerId.LedgerId).ToList();
                                        if (cancelCash.Count > 0 && cancelCash != null)
                                        {
                                            foreach (var xx in cancelCash)
                                            {
                                                var cancelAmount = school.tb_BillCancelAccounts.Where(x => x.CashBankType == false && x.CashBankId == xx.Id && x.ItemId == subId).ToList();
                                                if (cancelAmount != null && cancelAmount.Count > 0)
                                                {
                                                    expenseCash = expenseCash + cancelAmount.Sum(x => x.Amount);
                                                }
                                            }
                                        }
                                        if (cancelBank.Count > 0 && cancelBank != null)
                                        {
                                            foreach (var xx in cancelBank)
                                            {
                                                var cancelAmount = school.tb_BillCancelAccounts.Where(x => x.CashBankType == true && x.CashBankId == xx.Id && x.ItemId == subId).ToList();
                                                if (cancelAmount != null && cancelAmount.Count > 0)
                                                {
                                                    expenseBank = expenseBank + cancelAmount.Sum(x => x.Amount);
                                                }
                                            }
                                        }
                                    }
                                    var income = incomeCash + incomeBank;
                                    var expense = expenseCash + expenseBank;
                                    one.Income = income ?? 0;
                                    one.Expence = expense ?? 0;
                                    subOne.Income = income ?? 0;
                                    subOne.Expence = expense ?? 0;
                                    if (income > expense)
                                    {
                                        //subOne.Income = (income - expense) ?? 0;
                                        //subOne.Expence = 0;
                                        subOne.Status = "Cr";
                                    }
                                    else if (income == expense)
                                    {
                                        //subOne.Expence = 0;
                                        //subOne.Income = 0;
                                        subOne.Status = "";
                                    }
                                    else
                                    {
                                        //subOne.Expence = (expense - income) ?? 0;
                                        //subOne.Income = 0;
                                        subOne.Status = "Dt";
                                    }
                                    one._LedgerDetailsList.Add(subOne);
                                }

                                #region Checking that the dates of cancelling while it we not taken
                                var cancellCashBillsNotIncludes = school.tb_BillCancelAccounts.Where(x => x.IsActive && x.ItemId == subId && x.CancelDate.Date >= startDate.Date && x.CancelDate.Date <= toDate.Date && !allDates.Any(y => y.Date == x.CancelDate.Date)).ToList();
                                if (cancellCashBillsNotIncludes != null && cancellCashBillsNotIncludes.Count > 0)
                                {
                                    var cancelDates = cancellCashBillsNotIncludes.Select(x => x.CancelDate).Distinct().OrderBy(x => x.Date).ToList();
                                    foreach (var yy in cancelDates)
                                    {
                                        LedgerDetailsList subOne = new LedgerDetailsList();
                                        subOne.AccountDate = yy;
                                        subOne.SubLedger = _Entities.tb_Fee.Where(x => x.FeeId == subId).Select(x => x.FeesName).FirstOrDefault();
                                        subOne.Income = 0;
                                        subOne.Expence = cancellCashBillsNotIncludes.Where(x => x.CancelDate.Date == yy.Date).Sum(x => x.Amount);
                                        subOne.Status = "Dt";
                                        one._LedgerDetailsList.Add(subOne);
                                    }
                                }
                                #endregion Checking that the dates of cancelling while it we not taken
                            }
                            list.Add(one);
                        }

                    }
                }
                #endregion Means the filter head is From the fees
                #region Means the filter head is general account section 
                else
                {
                    var cashData = school.tb_CashEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var bankData = school.tb_BankEntry.Where(x => x.IsActive == true && x.CancelStatus == false && x.EnterDate.Date >= startDate.Date && x.EnterDate.Date <= toDate.Date && x.HeadId == headId && x.SubId == subId).ToList();
                    var accountHeadIdCash = cashData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                    var accountHeadIdBank = bankData.Select(x => new { x.HeadId, x.Narration, x.VoucherNumber }).Distinct().ToList();
                    var accountHead = accountHeadIdCash.Union(accountHeadIdBank).ToList();
                    if (accountHead.Count > 0 && accountHead != null)
                    {
                        foreach (var item in accountHead)
                        {
                            LedgerReportDataModel one = new LedgerReportDataModel();
                            one.AccountHead = school.tb_AccountHead.Where(x => x.AccountId == headId).Select(x => x.AccHeadName).FirstOrDefault();
                            one.Narration = item.Narration;
                            one._LedgerDetailsList = new List<LedgerDetailsList>();
                            var cashDate = cashData.Select(x => x.EnterDate.Date).Distinct().ToList();
                            var bankDate = bankData.Select(x => x.EnterDate.Date).Distinct().ToList();
                            var allDates = cashDate.Union(bankDate).OrderBy(x => x.Date).ToList();
                            if (allDates.Count > 0 && allDates != null)
                            {
                                foreach (var date in allDates)
                                {
                                    LedgerDetailsList subOne = new LedgerDetailsList();
                                    subOne.AccountDate = date;
                                    subOne.SubLedger = _Entities.tb_SubLedgerData.Where(x => x.LedgerId == subId).Select(x => x.SubLedgerName).FirstOrDefault();
                                    var incomeCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var incomeBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "R" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var expenseCash = cashData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();
                                    var expenseBank = bankData.Where(x => x.HeadId == headId && x.SubId == subId && x.EnterDate.Date == date && x.TransactionType == "P" && x.VoucherNumber == item.VoucherNumber).Select(x => x.Amount).FirstOrDefault();

                                    if (incomeCash == null && incomeBank == null && expenseCash == null && expenseBank == null)
                                    {
                                        continue;
                                    }
                                    if (incomeCash == null)
                                    {
                                        incomeCash = 0;
                                    }
                                    if (incomeBank == null)
                                    {
                                        incomeBank = 0;
                                    }
                                    if (expenseCash == null)
                                    {
                                        expenseCash = 0;
                                    }
                                    if (expenseBank == null)
                                    {
                                        expenseBank = 0;
                                    }
                                    var income = incomeCash + incomeBank;
                                    var expense = expenseCash + expenseBank;
                                    one.Income = income ?? 0;
                                    one.Expence = expense ?? 0;
                                    subOne.Income = income ?? 0;
                                    subOne.Expence = expense ?? 0;
                                    if (income > expense)
                                    {
                                        //subOne.Income = (income - expense) ?? 0;
                                        //subOne.Expence = 0;
                                        subOne.Status = "Cr";
                                    }
                                    else if (income == expense)
                                    {
                                        //subOne.Expence = 0;
                                        //subOne.Income = 0;
                                        subOne.Status = "";
                                    }
                                    else
                                    {
                                        //subOne.Expence = (expense - income) ?? 0;
                                        //subOne.Income = 0;
                                        subOne.Status = "Dt";
                                    }
                                    one._LedgerDetailsList.Add(subOne);
                                }
                            }
                            list.Add(one);
                        }
                    }
                }
                #endregion Means the filter head is general account section 
            }
            #endregion SubId!=0 and Head!=0: Means wants to show only the particular head with particular sub
            //return list.OrderBy(x =>new { x.AccountHead,x.Narration}).ToList();
            return list.ToList();
        }

        //jibin 11/26


        //jibin 11/26
        //jibin 11/26

    }




}
