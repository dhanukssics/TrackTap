using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTap.ClassLibrary.Utility;
using TrackTap.Models;

namespace TrackTap.Data
{
    public class Student : BaseReference
    {
        private TbStudent student;
        public Student(TbStudent obj) { student = obj; }
        public Student(long id) { student = _Entities.tb_Student.FirstOrDefault(z => z.StudentId == id); }
        public long StudentId { get { return student.StudentId; } }
        public long SchoolId { get { return student.SchoolId; } }
        public string StudentSpecialId { get { return student.StudentSpecialId; } }
        public string StundentName { get { return student.StundentName; } }
        public string ParentEmail { get { return student.ParentEmail; } }
        public string Gender { get { return student.Gender; } }
        public string BloodGroup { get { return student.BloodGroup; } }
        public string ParentName { get { return student.ParentName; } }
        public string Address { get { return student.Address; } }
        public string City { get { return student.City; } }
        public string ContactNumber { get { return student.ContactNumber; } }
        public string ClasssNumber { get { return student.ClasssNumber; } }
        public long ClassId { get { return student.ClassId; } }
        public long DivisionId { get { return student.DivisionId; } }
        public long BusId { get { return student.BusId; } }
        public string TripNo { get { return student.TripNo; } }
        public string BioNumber { get { return student.BioNumber; } }
        public string FilePath { get { return student.FilePath; } }
        public System.DateTime TimeStamp { get { return student.TimeStamp; } }
        public System.Guid? StudentGuid { get { return student.StudentGuid; } }
        public bool IsActive { get { return student.IsActive; } }
        public Nullable<long> ParentId { get { return student.ParentId; } }
        public string State { get { return student.State; } }
        public string ClassName { get { return student.Class.Class; } }
        public Division Division { get { return new Data.Division(student.Division); } }
        public string DivisionName { get { return Division.DivisionName; } }
        public Bus Bus { get { return new Data.Bus(student.tb_Bus); } }
        public string BusSpecialId { get { return student.tb_Bus.BusSpecialId; } }
        public string SchoolName { get { return student.tb_School.SchoolName; } }
        public string SchoolAddress { get { return student.tb_School.Address; } }
        public string Latitude { get { return student.tb_School.Latitude; } }
        public string Longitude { get { return student.tb_School.Longitude; } }
        public int ClassOrder { get { return student.tb_Class.ClassOrder; } }
        public long AcademicYearId { get { return student.tb_Class.AcademicYearId; } }
        public Nullable<bool> IsSamrtPhoneUser { get { return student.IsSamrtPhoneUser; } }
        public DateTime? DOB { get { return student.DOB; } }
        public bool ReadStatus { get { return NewMessage(student); } }
        public School School { get { return new Data.School(student.tb_School); } }
        public string DOBString()
        {
            string dobString = " ";
            var studentDateB = "";
            if (student.DOB != null)
            {
                dobString = student.DOB.ToString();
                studentDateB = Convert.ToDateTime(dobString).ToShortDateString();
            }
            return studentDateB;
        }
        private bool NewMessage(tb_Student student)
        {
            if (student.ParentId != null)
            {
                var msg = _Entities.tb_ParentMessage.Where(x => x.SenderId == student.ParentId && x.IsActive && x.ReadStatus == true).Select(x => x.ReadStatus).FirstOrDefault();
                return msg;
            }
            else
                return false;
        }

        public Teacher Teacher { get { return FetchTeacher(); } }
        public Teacher FetchTeacher()
        {
            var row = student.tb_Class.tb_TeacherClass.FirstOrDefault(z => z.DivisionId == DivisionId);
            if (row != null)
            {
                return new Teacher(row.tb_Teacher);
            }
            else
            {
                return null;
            }
        }
        public List<FeeDiscount> Discount { get { return student.tb_FeeDiscount.Where(z => z.IsActive).ToList().Select(z => new FeeDiscount(z)).ToList(); } }
        public List<SPFullFee> GetStudentPaymentFees()
        {
            var g = _Entities.SP_FullFee(student.ClassId, student.StudentId,0).ToList().Select(z => new SPFullFee(z)).ToList();
            return g;
        }
        public List<SPAdvanceFee> GetStudentAdvancePaymentFees()
        {
            var g = _Entities.SP_AdvanceFee(student.ClassId, student.StudentId).ToList().Select(z => new SPAdvanceFee(z)).ToList();
            return g;
        }
        public Driver Driver
        {
            get
            {
                var currentDateTime = DateTime.UtcNow.Date;
                //return _Entities.tb_Trip.Where(z => EntityFunctions.TruncateTime(z.StartTime) == currentDateTime && z.TravellingStatus == 1 && z.IsActive && z.TripNo == student.TripNo && z.BusId == student.BusId).ToList().Select(z => new Driver(z.DriverId)).FirstOrDefault();// old Original Code
                try
                {
                    var daat = _Entities.tb_Trip.Where(z => EntityFunctions.TruncateTime(z.StartTime) == currentDateTime && z.TravellingStatus == 1 && z.IsActive && z.TripNo == student.TripNo && z.BusId == student.BusId).ToList().Select(z => new Driver(z.DriverId)).FirstOrDefault();
                    return daat;
                }
                catch
                {
                    return null;
                }

            }
        }

        public List<SPEditFeeBilling> GetStudentFeeList()
        {

            var g = _Entities.SP_EditFullFee(student.ClassId, student.StudentId).ToList().Select(z => new SPEditFeeBilling(z)).ToList();

            return g;
        }

        public List<SPStudentBillPayment> GetStudentPaidFees() //Archana 
        {
            var g = _Entities.SP_StudentBillPayment(student.StudentId).ToList().Select(z => new SPStudentBillPayment(z)).ToList();
            return g;
        }
        public List<Payment> GetStudentPaidFeesByBillNo(long billNo)
        {
            var data = _Entities.tb_Payment.Where(z => z.StudentId == student.StudentId && z.BillNo == billNo && z.ClassId == student.ClassId && z.IsActive).ToList().Select(z => new Payment(z)).ToList();
            if (data != null && data.Count > 0)
            {
                return data;
            }
            else
            {
                var list = student.tb_StudentPremotion.Where(x => x.IsActive == 1).ToList();
                foreach (var item in list)
                {
                    var a = _Entities.tb_Payment.Where(z => z.StudentId == student.StudentId && z.BillNo == billNo && z.ClassId == item.OldClass && z.IsActive).ToList().Select(z => new Payment(z)).ToList();
                    data.AddRange(a);
                }
                return data;
            }

        }
        public Student GetStudentData()
        {
            var data = _Entities.tb_Student.Where(x => x.StudentId == student.StudentId && x.IsActive).ToList().Select(x => new Student(x)).FirstOrDefault();
            return data;
        }
        public List<BillFeeDateHistory> GetHistoryGroupByStudentId()
        {
            return _Entities.Database.SqlQuery<BillFeeDateHistory>(string.Format("select StudentId,ClassId,Sum(Amount)as Amount ,Convert(date,TimeStamp) as [Date] from [tb_Payment] where StudentId={0} group by StudentId ,Convert(date,TimeStamp),tb_Payment.ClassId", student.StudentId)).ToList();
        }
        public List<billNumberList> GetBillHistory()
        {
            //var s = _Entities.tb_Payment.Where(z => z.StudentId == student.StudentId && z.ClassId == student.ClassId && z.BillNo != null).ToList();
            ////var sp = s.Select(z => z.BillNo).Distinct().ToList().Select(z=> new List<billNumberList>()).ToList();
            //var sp = s.Select(z => z.BillNo).Distinct().Select(z=> new billNumberList()).ToList();
            //return sp;

            //var s = _Entities.tb_Payment.Where(z => z.StudentId == student.StudentId && z.ClassId == student.ClassId && z.BillNo != null).ToList();
            //var sp = s.Select(z => z.BillNo).Distinct().ToList();
            //List<billNumberList> list = new List<billNumberList>();
            //foreach (var item in sp)
            //{
            //    billNumberList one = new billNumberList();
            //    one.BillNo=item.b
            //}
            var s = _Entities.tb_Payment.Where(z => z.StudentId == student.StudentId && z.ClassId == student.ClassId && z.BillNo != null && z.IsActive && z.IsPaid).ToList().Select(xz => new Payment(xz)).ToList();
            List<billNumberList> list = new List<billNumberList>();
            foreach (var item in s)
            {
                if (item.BillNo != null)
                {
                    billNumberList one = new billNumberList();
                    one.BillNo = item.BillNo ?? 00;
                    one.TimeStamp = item.TimeStamp;
                    one.FeeGuid = item.FeeGuid;
                    list.Add(one);
                }
            }
            var previousHistory = student.tb_StudentPremotion.Where(x => x.IsActive == 1).ToList();
            foreach (var st in previousHistory)
            {
                var o = _Entities.tb_Payment.Where(z => z.StudentId == st.StudentId && z.ClassId == st.OldClass && z.BillNo != null && z.IsActive && z.IsPaid).ToList().Select(xz => new Payment(xz)).ToList();
                foreach (var item in o)
                {
                    if (item.BillNo != null)
                    {
                        billNumberList one = new billNumberList();
                        one.BillNo = item.BillNo ?? 00;
                        one.TimeStamp = item.TimeStamp;
                        one.FeeGuid = item.FeeGuid;
                        list.Add(one);
                    }
                }
            }
            List<billNumberList> newList = new List<billNumberList>();
            newList = list.GroupBy(x => x.BillNo).Select(y => y.First()).ToList();
            return newList;
        }
        public List<Payment> GetHistoryTableByDate(DateTime TimeStamp)
        {
            string Maxdate = TimeStamp.Date.ToString("dd-MM-yyyy") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(Maxdate);
            return _Entities.tb_Payment.Where(z => z.TimeStamp >= TimeStamp && z.TimeStamp <= maxDate && z.IsActive == true && z.IsPaid == true).ToList().Select(x => new Payment(x)).ToList();
        }
        public List<Payment> GetHistoryTableByBillNo(long BillNo)
        {
            return _Entities.tb_Payment.Where(z => z.IsActive == true && z.IsPaid == true && z.StudentId == student.StudentId && z.BillNo == BillNo).ToList().Select(x => new Payment(x)).ToList();
        }
        public bool IsDueBill(Guid feeGuid)
        {
            return _Entities.tb_FeeDues.Where(z => z.IsActive == true && z.FeeDuesGuid == feeGuid).FirstOrDefault() == null ? false : true;
        }
        public List<Student> StudentList(long ClassId, long DivisionId)
        {
            List<Student> listData = new List<Student>();
            listData = _Entities.tb_Student.Where(x => x.ClassId == ClassId && x.DivisionId == DivisionId && x.IsActive).ToList().Select(x => new Student(x)).ToList();
            return listData;
        }
        public decimal StudentBalance()
        {
            decimal bal = 0;
            var balance = student.tb_StudentBalance.Where(z => z.IsActive == true).FirstOrDefault();
            if (balance != null)
            {
                bal = balance.Amount;
            }
            return bal;
        }
        public tb_StudentPaidAmount StudentPaidAmountByBillNo(long BillNo)
        {
            return student.tb_StudentPaidAmount.Where(z => z.BillNo == BillNo).FirstOrDefault();

        }

        //public decimal GetCurrentAmountParial(long feeId)
        //{
        //    decimal partialPayment = 0;

        //}
        public List<SPFullFee> GetStudentPaymentFeesWithAcademicYear(long academicyear)
        {
            if (student.tb_Class.AcademicYearId == academicyear)
            {
                var g = _Entities.SP_FullFee(student.ClassId, student.StudentId,0).ToList().Select(z => new SPFullFee(z)).ToList();
                return g;
            }
            else
            {
                var oldClassData = _Entities.tb_StudentPremotion.Where(x => x.StudentId == student.StudentId && x.OldClass != null && x.tb_Class.AcademicYearId == academicyear && x.IsActive == 1).FirstOrDefault();
                if (oldClassData != null)
                {
                    var HistoryDatas = GetBillHistory();


                    var g = _Entities.SP_FullFee(oldClassData.OldClass, student.StudentId,oldClassData.FromDivision).ToList().Select(z => new SPFullFee(z)).ToList();
                    return g;
                }
                else
                {
                    var g = _Entities.SP_FullFee(0, 0,0).ToList().Select(z => new SPFullFee(z)).ToList();
                    return g;
                }
            }
        }
        public List<tb_StudentFiles> StudentFiles()
        {
            var data = student.tb_StudentFiles.Where(x => x.IsActive).ToList();
            return data;
        }
        public List<SPEditCommonFeeBilling> GetStudentCommonFeeList()
        {
            var g = _Entities.sp_CommonFeeList(student.ClassId, student.StudentId).ToList().Select(z => new SPEditCommonFeeBilling(z)).ToList();
            return g;
        }
        public List<SPStudentDicount> GetStudentCommonFeeDiscount()
        {
            var discount = _Entities.sp_StudentDicount(student.StudentId).ToList().Select(x => new SPStudentDicount(x)).ToList();
            return discount;
        }

        public int BankFee(long schoolId)
        {
            int bal = 0;
            var balance = _Entities.tb_BankPercentage.Where(x => x.SchoolId == schoolId && x.IsActive == true).FirstOrDefault();
            if (balance != null)
            {
                bal = balance.Amount;
            }
            return bal;
        }


        // jibin 9/24/2020


        public List<Student> GetStudentDetailsByAdmissionNo(string adno)
        {
            return _Entities.tb_Student.Where(z => z.StudentSpecialId==adno).ToList().Select(q => new Student(q)).ToList();
        }


        public List<StockSales> GetStudentStock(long uid,string Sid)
        {
            return _Entities.tb_SalesStock.Where(z => z.UserId == uid && z.SalesId==Sid ).ToList().Select(z => new StockSales(z)).ToList();
        }

       


        public List<billNumberList> GetBillHistoryStock()
        {
          
            var s = _Entities.tb_StockPayment.Where(z => z.StudentId == student.StudentId && z.ClassId == student.ClassId && z.BillNo != null && z.IsActive && z.IsPaid).ToList().Select(xz => new StockPayment(xz)).ToList();
            List<billNumberList> list = new List<billNumberList>();
            foreach (var item in s)
            {
                if (item.BillNo != null)
                {
                    billNumberList one = new billNumberList();
                    one.BillNo = item.BillNo ?? 00;
                    one.TimeStamp = item.TimeStamp;
                    list.Add(one);
                }
            }
            var previousHistory = student.tb_StudentPremotion.Where(x => x.IsActive == 1).ToList();
            foreach (var st in previousHistory)
            {
                var o = _Entities.tb_StockPayment.Where(z => z.StudentId == st.StudentId && z.ClassId == st.OldClass && z.BillNo != null && z.IsActive && z.IsPaid).ToList().Select(xz => new StockPayment(xz)).ToList();
                foreach (var item in o)
                {
                    if (item.BillNo != null)
                    {
                        billNumberList one = new billNumberList();
                        one.BillNo = item.BillNo ?? 00;
                        one.TimeStamp = item.TimeStamp;
                        list.Add(one);
                    }
                }
            }
            List<billNumberList> newList = new List<billNumberList>();
            newList = list.GroupBy(x => x.BillNo).Select(y => y.First()).ToList();
            return newList;
        }



        public List<StockPayment> GetStudentPaidStockByBillNo(long billNo)
        {
            var data = _Entities.tb_StockPayment.Where(z => z.StudentId == student.StudentId && z.BillNo == billNo && z.ClassId == student.ClassId && z.IsActive).ToList().Select(z => new StockPayment(z)).ToList();
            if (data != null && data.Count > 0)
            {
                return data;
            }
            else
            {
                var list = student.tb_StudentPremotion.Where(x => x.IsActive == 1).ToList();
                foreach (var item in list)
                {
                    var a = _Entities.tb_StockPayment.Where(z => z.StudentId == student.StudentId && z.BillNo == billNo && z.ClassId == item.OldClass && z.IsActive).ToList().Select(z => new StockPayment(z)).ToList();
                    data.AddRange(a);
                }
                return data;
            }

        }

        public Student GetStudentDataStock()
        {
            var data = _Entities.tb_Student.Where(x => x.StudentId == student.StudentId && x.IsActive).ToList().Select(x => new Student(x)).FirstOrDefault();
            return data;
        }

        // jibin 9/24/2020

    }
}
