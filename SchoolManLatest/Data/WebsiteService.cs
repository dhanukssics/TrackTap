using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTap.ClassLibrary.Utility;

namespace TrackTap.Data
{
    public class WebsiteService : BaseReference
    {
        public List<Class> getAllClassByFeeId(long schoolId, long feeId)
        {
            var x = new List<Class>();
            if (feeId != Convert.ToInt64(0) && feeId != null)
            {
                var list = _Entities.tb_FeeClass.Where(z => z.tb_Fee.SchoolId == schoolId && z.FeeId == feeId && z.IsActive).ToList().Select(z => z.ClassId).ToList();
                x = _Entities.tb_Class.Where(z => z.SchoolId == schoolId && !list.Contains(z.ClassId) && z.IsActive).ToList().Select(z => new Class(z)).ToList();
                return x;
            }
            else
            {
                return x;
            }
        }

        public List<FeeClass> getAllFeeClassByFeeId(long schoolId, long feeId)
        {
            return _Entities.tb_FeeClass.Where(z => z.FeeId == feeId && z.IsActive && z.tb_Fee.SchoolId == schoolId && z.tb_Class.PublishStatus == true).ToList().
                Select(z => new FeeClass(z)).ToList();
        }
        public List<string> gtGetAllFeeHead(long schoolId)
        {
            var school = _Entities.tb_Fee.Where(z => z.IsActive && z.SchoolId == schoolId).Select(x => x.FeesName).ToList();
            return school;
        }
        public List<string> gtAllIncomeAccountHead(long schoolId)
        {
            return _Entities.tb_Income.Where(x => x.IsActive && x.SchoolId == schoolId).ToList().Select(z => new Income(z)).ToList().Select(x => x.AccountHead).Distinct().ToList();
        }
        public List<string> gtAllExpenceAccountHead(long schoolId)
        {

            return _Entities.tb_Expense.Where(x => x.IsActive && x.SchoolId == schoolId).ToList().Select(z => new Expence(z)).ToList().Select(x => x.AccountHead).Distinct().ToList();
        }
        public List<string> GetIncomeAccountHeads(string str)
        {
            return _Entities.tb_Income.Where(x => x.IsActive && (x.AccountHead.StartsWith(str))).ToList().Select(z => new Income(z)).ToList().Select(x => x.AccountHead).Distinct().ToList();
        }
        public List<Income> IncomeList()
        {
            return _Entities.tb_Income.Where(x => x.IsActive).ToList().Select(z => new Income(z)).ToList();
        }
        public List<Expence> ExpenceList()
        {
            return _Entities.tb_Expense.Where(x => x.IsActive).ToList().Select(z => new Expence(z)).ToList();
        }
        public List<Income> IncomelistOnDate(DateTime cdate, long SchoolId)
        {
            return _Entities.tb_Income.Where(x => (x.IsActive && x.Date.Year == cdate.Year && x.Date.Month == cdate.Month && x.Date.Day == cdate.Day) && x.SchoolId == SchoolId).ToList().Select(z => new Income(z)).ToList();

        }
        public List<Expence> ExpencelistOnDate(DateTime cdate, long SchoolId)
        {
            return _Entities.tb_Expense.Where(x => (x.IsActive && x.Date.Year == cdate.Year && x.Date.Month == cdate.Month && x.Date.Day == cdate.Day) && x.SchoolId == SchoolId).ToList().Select(z => new Expence(z)).ToList();

        }
        public double BalancebdDate(DateTime cdate, long SchoolId)
        {
            string endDate = cdate.ToString("MM-dd-yyyy");
            DateTime date = Convert.ToDateTime(endDate);
            var balance = _Entities.Sp_BalanceBDIncoExp(date, SchoolId, null, null).FirstOrDefault();
            return Convert.ToDouble(balance);

        }
        public Tuple<string, string, List<SmsHistory>> GetAllSmsOnDate(DateTime cdate, long schoolId)
        {
            var result = _Entities.tb_SmsHistory.Where(x => (x.MessageDate.Value.Year == cdate.Year && x.MessageDate.Value.Month == cdate.Month && x.MessageDate.Value.Day == cdate.Day) && x.ScholId == schoolId).ToList().Select(z => new SmsHistory(z)).ToList();
            var sum = result.Sum(x => x.SmsPerStudent).ToString();
            var totalsum = _Entities.tb_SmsHistory.Where(x => x.ScholId == schoolId).ToList().Sum(z => z.SmsSentPerStudent).ToString();
            return new Tuple<string, string, List<SmsHistory>>(sum, totalsum, result);

        }
        public Tuple<string, string, List<SPSmsDataOnDatecs>> GetAllSmsOnTwoDate(string cdate, string edate, long SchoolId)
        {
            // long SchoolId = 19;
            // return _Entities.tb_SmsHistory.ToList().Select(z => new SmsHistory(z)).ToList();

            // var result = _Entities.tb_SmsHistory.Where(x => x.MessageDate.Value.Year == cdate && x.MessageDat1e.Value.Month == cdate.Month && x.MessageDate.Value.Day == cdate.Day).ToList().Select(z => new SmsHistory(z)).ToList();
            var ScId = Convert.ToString(SchoolId);
            //var sdate = Convert.ToDateTime(cdate);
            //var esdate = Convert.ToDateTime(edate);
            var result = _Entities.SP_GetAllSmsOnTwoDates(cdate, edate, ScId).ToList().Select(x => new SPSmsDataOnDatecs(x)).ToList();
            var sum = result.Sum(x => x.SmsSentPerStudent).ToString();
            var totalsum = _Entities.tb_SmsHistory.Where(x => x.ScholId == SchoolId).ToList().Sum(z => z.SmsSentPerStudent).ToString();

            return new Tuple<string, string, List<SPSmsDataOnDatecs>>(sum, totalsum, result);

        }
        public Tuple<string, string, List<SmsHead>> GetAllSmsHeadByDate(string cdate, string edate, long SchoolId)
        {

            var ScId = Convert.ToString(SchoolId);
            var smsHist = _Entities.tb_SmsHistory.Where(x => x.ScholId == SchoolId && x.SendStatus == "1" && x.IsActive == true).ToList();
            var totalsum = smsHist.Sum(z => z.SmsSentPerStudent).ToString();

            var smsStaffHist = _Entities.tb_StaffSMSHistory.Where(x => x.ScholId == SchoolId && x.SendStatus == "1" && x.IsActive == true).ToList();
            var totalsum2 = smsStaffHist.Sum(z => z.SmsSentPerStudent).ToString();

            DateTime cddate = Convert.ToDateTime(cdate);
            string StartDate = cddate.Date.ToShortDateString() + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);

            DateTime eddate = Convert.ToDateTime(edate);
            string EndDate = eddate.Date.ToShortDateString() + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(EndDate);

            var result = _Entities.tb_SmsHead.Where(z => z.TimeStamp >= minDate && z.TimeStamp <= maxDate && z.SchoolId == SchoolId && z.IsActive).ToList().Select(z => new SmsHead(z)).ToList();
            var xcx = smsHist.Where(z => z.MessageDate >= minDate && z.MessageDate <= maxDate && z.ScholId == SchoolId).ToList();

            var xcxStaff = smsStaffHist.Where(z => z.MessageDate >= minDate && z.MessageDate <= maxDate && z.ScholId == SchoolId).ToList();
            var sum = xcx.Sum(x => x.SmsSentPerStudent).ToString();
            //var sum = xcx.Count();
            var sum2 = xcxStaff.Sum(x => x.SmsSentPerStudent).ToString();
            //var sum2 = xcxStaff.Count();

            var total = Convert.ToString(Convert.ToInt32(totalsum) + Convert.ToInt32(totalsum2));
            //var total = Convert.ToString(smsHist.Count() + smsStaffHist.Count());
            var daySum = Convert.ToString(Convert.ToInt32(sum) + Convert.ToInt32(sum2));
            //return new Tuple<string, string, List<SmsHead>>(sum, totalsum, result);
            return new Tuple<string, string, List<SmsHead>>(daySum, total, result);

        }
        public string GetLatestExpParticular(string AccountHead, long SchoolId)
        {
            var result = _Entities.tb_Expense.Where(x => x.AccountHead == AccountHead && x.SchoolId == SchoolId).OrderByDescending(x => x.Id).Select(x => x.Particular).FirstOrDefault();
            return result;
        }
        public string GetLatestIncParticular(string AccountHead, long SchoolId)
        {
            var result = _Entities.tb_Income.Where(x => x.AccountHead == AccountHead && x.SchoolId == SchoolId).OrderByDescending(x => x.Id).Select(x => x.Particular).FirstOrDefault();
            return result;
        }
        public List<SmsHistory> GetAllSms(long SchoolId)
        {
            return _Entities.tb_SmsHistory.Where(x => x.IsActive == true && x.ScholId == SchoolId).ToList().Select(z => new SmsHistory(z)).ToList();

        }
        public List<Exams> GetFullMarkData(long classId, long divisionId)
        {
            var exam = _Entities.tb_Exams.Where(z => z.ClassId == classId && z.DivisionId == divisionId && z.IsActive).ToList().OrderByDescending(x => x.TimeStamp).Select(z => new Exams(z)).ToList();
            return exam;
        }
        public List<StudentMarkListData> GetStudentMarks(long ExamId, long StudentId)
        {
            List<StudentMarkListData> list = new List<StudentMarkListData>();
            var data = _Entities.tb_StudentMarks.Where(x => x.StudentId == StudentId && x.ExamId == ExamId).ToList();
            foreach (var item in data)
            {
                StudentMarkListData one = new StudentMarkListData();
                one.SubjectName = item.tb_ExamSubjects.Subject;
                one.Internal = Convert.ToString(item.InternalMark + " / " + item.tb_ExamSubjects.InternalMarks);
                one.External = Convert.ToString(item.ExternalMark + " / " + item.tb_ExamSubjects.ExternalMark);
                one.Total = Convert.ToString(item.Mark + " / " + item.tb_ExamSubjects.Mark);
                one.SubjectId = item.tb_ExamSubjects.SubId;
                list.Add(one);
            }
            return list;
        }
        public List<Sp_BusTripHistoryHead_Result> GetTripByDate(DateTime startDate, long schoolId)
        {
            string StartDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "12:00:00 AM";
            DateTime minDate = Convert.ToDateTime(StartDate);

            string EndDate = startDate.Date.ToString("yyyy-MM-dd") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(EndDate);

            return _Entities.Sp_BusTripHistoryHead(schoolId, minDate, maxDate).ToList().OrderBy(z => z.BusName).ToList();
        }
        public List<string> GetAllSupplierList(long schoolId)
        {
            return _Entities.tb_StockUpdate.Where(x => x.IsActive && x.SchoolId == schoolId).ToList().Select(z => new StockUpdate(z)).ToList().Select(x => x.SupplierName).Distinct().ToList();
        }
        public List<FeeClassList> getAllClassDivisionByFeeId(long schoolId, long feeId)
        {
            var x = new List<Division>();
            var ab = new List<FeeClassList>();
            if (feeId != Convert.ToInt64(0) && feeId != null)
            {
                var list = _Entities.tb_FeeClass.Where(z => z.tb_Fee.SchoolId == schoolId && z.FeeId == feeId && z.IsActive && z.DivisionId == null).ToList().Select(z => z.ClassId).Distinct().ToList();
                var a = _Entities.tb_Class.Where(z => z.SchoolId == schoolId && z.PublishStatus == true && !list.Contains(z.ClassId) && z.IsActive).ToList().Select(z => new Class(z)).OrderBy(z => z.ClassOrder).ToList();
                foreach (var item in a)
                {
                    x.AddRange(item.Division);
                }
                var list2 = _Entities.tb_FeeClass.Where(z => z.tb_Fee.SchoolId == schoolId && z.FeeId == feeId && z.IsActive && z.DivisionId != null && !list.Contains(z.ClassId)).ToList().Select(z => z.DivisionId).ToList();
                if (list2 != null && list2.Count > 0)
                {
                    //var b = _Entities.tb_Division.Where(z => z.IsActive == true && !list2.Contains(z.DivisionId) && z.tb_Class.SchoolId==schoolId && z.tb_Class.PublishStatus==true && z.tb_Class.IsActive==true && !list.Any(y=>y==z.ClassId)).ToList().Select(z => new Division(z)).OrderBy(z => z.ClassOrder).ToList();
                    //if (b != null)
                    {
                        //x.AddRange(b);
                        x.RemoveAll(z => list2.Contains(z.DivisionId));
                    }
                }
                x = x.OrderBy(z => z.ClassOrder).OrderBy(z => z.DivisionName).Distinct().ToList();
                var classList = x.Select(z => z.ClassId).Distinct();
                foreach (var item in classList)
                {
                    var newList = x.Where(z => z.ClassId == item).ToList();
                    FeeClassList one = new FeeClassList();
                    one.ClassId = item;
                    one.ClassName = newList.Select(z => z.ClassName).FirstOrDefault();
                    one.list = new List<FeeDivisionList>();
                    foreach (var item1 in newList)
                    {
                        FeeDivisionList two = new FeeDivisionList();
                        two.DivisionId = item1.DivisionId;
                        two.DivisionName = item1.DivisionName;
                        one.list.Add(two);
                    }
                    ab.Add(one);
                }
                return ab;
            }
            else
            {
                return ab;
            }
        }
    }
}
