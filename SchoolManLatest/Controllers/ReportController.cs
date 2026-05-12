using System;
using System.Collections.Generic;
using TrackTap.Data;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.Models;
using TrackTap.ClassLibrary;
using System.Threading.Tasks;

namespace TrackTap.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OutstandingReport()
        {
            OutstandingReportModel model = new OutstandingReportModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        //Created by Gayathri:08/09/2023  list out the number of students who paid fees
        public IActionResult FeeDetailed_Report()
        {
            OutstandingReportModel model = new OutstandingReportModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public async Task<PartialViewResult> DatatableOutStandingReport(string id)
        {
           
            OutstandingReportModel model = new OutstandingReportModel();
            string[] splitData = id.Split('~');
            model.SchoolId = _user.SchoolId;
            model.ClassId = Convert.ToInt64(splitData[0]);
            if (model.ClassId == 0)
                model.DivisionId = 0;
            else
                model.DivisionId = Convert.ToInt64(splitData[1]);
            try
            {
                model.FeeId = Convert.ToInt64(splitData[2]);
            }
            catch
            {
                model.FeeId = 0;
            }
            int summary = Convert.ToInt32(splitData[3]);
            try
            {
                //model.FromDate = Convert.ToDateTime(splitData[4]);
                //model.ToDate = Convert.ToDateTime(splitData[5]);
            }
            catch
            {

            }
            if (summary == 1)
            {
                return PartialView("~/Views/Report/_OutstandingReportSummaryList.cshtml", model); // Avoid the detailed views for KTCT requirement by Monisha
            }
            else
            {
                
                // var a11 = aa.

                var data = await new TrackTap.DataLibrary.Data.School(model.SchoolId).GetOutStandingData_new(model.ClassId, model.DivisionId, model.FeeId, model.FromDate, model.ToDate);
                Session["datalists"] =  data;

                return PartialView("~/Views/Report/_OutstandingReportMultipleList.cshtml", model);
            }
        }
        //Created by Gayathri A (09/09/2023) feedetailed report
        public async Task<PartialViewResult> DatatablefeeReport(string id)
        {

            OutstandingReportModel model = new OutstandingReportModel();
            string[] splitData = id.Split('~');
            model.SchoolId = _user.SchoolId;
            model.ClassId = Convert.ToInt64(splitData[0]);
            model.AcademicYearId = Convert.ToInt64(splitData[2]);
            Int64 paidstatus = Convert.ToInt64(splitData[3]);
            model.ReportList_one = new List<ReportDateList_one>();
            try
            {
                model.FeeId = Convert.ToInt64(splitData[1]);
            }
            catch
            {
                model.FeeId = 0;
            }

                var Academicdate = _Entities.tb_AcademicYear.Where(x => x.YearId == model.AcademicYearId).FirstOrDefault();
                string[] splitAcademic = Academicdate.AcademicYear.Split('-');
                string Fromyear = Convert.ToString(splitAcademic[0]);
                string toyear = Convert.ToString(splitAcademic[1]);
            if (paidstatus == 0)
            {
                var feelist = _Entities.SP_FeeDetailedreport(model.SchoolId, model.ClassId, model.DivisionId, model.FeeId, Fromyear, toyear).OrderBy(x => x.StudentName).ToList();

                foreach (var Fee in feelist)
                {
                    ReportDateList_one li = new ReportDateList_one();
                    li.StudentSpecialId = Fee.SpecialId;
                    li.StudentName = Fee.StudentName;
                    li.feename = Fee.FeesName;
                    li.ClassName = Fee.Class;
                    li.Amount = Fee.Amount;
                    li.paidstatus = "PAID";
                    li.PaymentDate = Fee.TimeStamp;
                    model.ReportList_one.Add(li);
                }
            }
               
            
           else
            {
               
                    if(Academicdate.CurrentYear==true)
                    {
                        var studentlist = _Entities.tb_Student.Where(z => z.ClassId == model.ClassId && z.SchoolId==model.SchoolId).ToList();
                        var paymentlist = _Entities.tb_Payment.Where(x => x.ClassId == model.ClassId && x.tb_Fee.IsActive && x.SchoolId==model.SchoolId).ToList();
                        var result = paymentlist.GroupBy(test => test.StudentId)
                    .Select(grp => grp.First())
                    .ToList();
                    var feedetail = _Entities.tb_Fee.Where(z => z.SchoolId == model.SchoolId && z.IsActive == true).ToList();
                    var diff = (from e in studentlist
                                from f in feedetail
                                    where !result.Any(n => n.StudentId == e.StudentId) && !result.Any(n=>n.FeeId ==f.FeeId)
                                select new { e.StudentId,e.StudentSpecialId,e.StundentName,f.FeeId,f.FeesName }).ToList();

                    if (model.FeeId == 0)
                    {
                        foreach (var pay in diff)
                        {
                            ReportDateList_one li = new ReportDateList_one();
                            li.StudentSpecialId = pay.StudentSpecialId;
                            li.StudentName = pay.StundentName;
                            li.feename = pay.FeesName;
                            li.ClassName = _Entities.tb_Class.Where(x => x.ClassId == model.ClassId).Select(x => x.Class).FirstOrDefault();
                            li.Amount = 0;
                            li.paidstatus = "PENDING";
                            li.PaymentDate = new DateTime(0001, 01, 01);
                            model.ReportList_one.Add(li);
                        }
                    }
                    else
                    {
                        var feedetailed = diff.Where(x => x.FeeId == model.FeeId);
                        foreach (var pay in feedetailed)
                        {
                            ReportDateList_one li = new ReportDateList_one();
                            li.StudentSpecialId = pay.StudentSpecialId;
                            li.StudentName = pay.StundentName;
                            li.feename = pay.FeesName;
                            li.ClassName = _Entities.tb_Class.Where(x => x.ClassId == model.ClassId).Select(x => x.Class).FirstOrDefault();
                            li.Amount = 0;
                            li.paidstatus = "PENDING";
                            li.PaymentDate = new DateTime(0001, 01, 01);
                            model.ReportList_one.Add(li);
                        }

                    }
                    

                    }
                   
                else
                {
                    var studentpromo = _Entities.tb_StudentPremotion.Where(z => z.OldClass == model.ClassId && z.tb_Student.StudentId == z.StudentId).
                        Select(z => new
                        {
                            z.StudentId,
                            z.tb_Student.StundentName,
                            z.tb_Student.StudentSpecialId
                        }).ToList();


                    var paymentlist = _Entities.tb_Payment.Where(x => x.ClassId == model.ClassId && x.tb_Fee.IsActive).ToList();
                    var feedetail = _Entities.tb_Fee.Where(z => z.SchoolId == model.SchoolId && z.IsActive == true).ToList();
                    var result = paymentlist.GroupBy(test => test.StudentId)
                .Select(grp => grp.First())
                .ToList();
                    var diff = (from e in studentpromo
                                from f in feedetail
                                where !result.Any(n => n.StudentId == e.StudentId) && !result.Any(n => n.FeeId == f.FeeId)
                                select new { e.StudentId, e.StudentSpecialId, e.StundentName, f.FeeId, f.FeesName }).ToList();
                   if(model.FeeId==0)
                    {
                        foreach (var pay in diff)
                        {
                            ReportDateList_one li = new ReportDateList_one();
                            li.StudentSpecialId = pay.StudentSpecialId;
                            li.StudentName = pay.StundentName;
                            li.feename = pay.FeesName;
                            li.ClassName = _Entities.tb_Class.Where(x => x.ClassId == model.ClassId).Select(x => x.Class).FirstOrDefault();
                            li.Amount = 0;
                            li.paidstatus = "PENDING";
                            li.PaymentDate = new DateTime(0001, 01, 01);
                            model.ReportList_one.Add(li);
                        }
                    }
                   else
                    {
                        var feedetailed = diff.Where(x => x.FeeId == model.FeeId);
                        foreach (var pay in feedetailed)
                        {
                            ReportDateList_one li = new ReportDateList_one();
                            li.StudentSpecialId = pay.StudentSpecialId;
                            li.StudentName = pay.StundentName;
                            li.feename = pay.FeesName;
                            li.ClassName = _Entities.tb_Class.Where(x => x.ClassId == model.ClassId).Select(x => x.Class).FirstOrDefault();
                            li.Amount = 0;
                            li.paidstatus = "PENDING";
                            li.PaymentDate = new DateTime(0001, 01, 01);
                            model.ReportList_one.Add(li);
                        }
                    }
                   
                }

                
               
            }

            return PartialView("~/Views/Report/pv_Fee_report.cshtml", model);
            
        }



        public IActionResult DetailedCollectionReport()
        {
            FeeModel model = new FeeModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = CurrentTime;
            model.EndDate = CurrentTime;
            return View(model);
        }
        public object SearchDetailedData(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            FeeModel model = new FeeModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = start;
            model.EndDate = end;

            ////List<Payment> li_Pay = new List<Payment>();
            //List<DetailedCollectionReportModel> Li = new List<DetailedCollectionReportModel>();

            ////var data = _Entities.sp_DetailedCollectionReport(_user.SchoolId, start, end).ToList().Select(x =>(x.PaymentId)).ToList();
            //var data = _Entities.sp_DetailedCollectionReport(_user.SchoolId, start, end).ToList();

            //foreach (var a1 in data)
            //{
            //    DetailedCollectionReportModel DCR = new DetailedCollectionReportModel();
            //    DCR.BillNo = a1.BillNo;
            //    DCR.TimeStamp = a1.TimeStamp;
            //    var student = _Entities.tb_Student.Where(z => z.StudentId == a1.StudentId).FirstOrDefault();                
            //    DCR.StudentName = student.StundentName;

            //    var ClassName = _Entities.tb_Class.Where(z => z.SchoolId == _user.SchoolId && z.ClassId == student.ClassId).FirstOrDefault();
            //    DCR.ClassName = ClassName.Class;

            //    var Divitionn = _Entities.tb_Division.Where(z => z.DivisionId == student.DivisionId).FirstOrDefault();
            //    DCR.DivisionName = Divitionn.Division;

            //    var particulers = _Entities.tb_Fee.Where(z => z.FeeId == a1.FeeId).FirstOrDefault();
            //    DCR.Particulars = particulers.FeesName;

            //    DCR.Amount = a1.Amount;


            //}



            //var results =  TrackTap.DataLibrary.Data.School(schoolId).GetDetailedCollectionReportDate(startDate, endDate).OrderBy(z => z.BillNo);
            return PartialView("~/Views/Report/_pv_DetailedCollectionReportList.cshtml", model);
        }


        public IActionResult MonthlyAttendanceReport()
        {
            MonthlyAttandanceModel model = new MonthlyAttandanceModel();
            model.SchoolId = _user.SchoolId;
            model.AttandanceDate = CurrentTime;
            model.ShiftId = 0;
            return View(model);
        }
        //Created by Gayathri A for teacher attendance report
        public IActionResult MonthlyAttendanceReportTeach()
        {
            MonthlyAttandanceModel model = new MonthlyAttandanceModel();
            model.SchoolId = _user.SchoolId;
            model.AttandanceDate = CurrentTime;
            model.ShiftId = 0;
            return View(model);
        }
        public PartialViewResult DatatableMonthlyAttendanceReport(string id)
        {
            MonthlyAttandanceModel model = new MonthlyAttandanceModel();
            model.SchoolId = _user.SchoolId;
            string[] splitData = id.Split('~');
            try
            {
                model.ClassId = Convert.ToInt64(splitData[0]);
                model.DivisionId = Convert.ToInt64(splitData[1]);
                if (splitData[2] == "0")
                    model.ShiftId = AttendanceShift.Morning;
                else
                    model.ShiftId = AttendanceShift.Evening;
                model.AttandanceDate = Convert.ToDateTime(splitData[3]);
                model.shiftData = Convert.ToInt32(splitData[2]);
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Views/Report/_pv_MonthlyAttendanceList.cshtml", model);
        }
        //Gayathri A 24/01/2024 for teacher attendance report
        public PartialViewResult DatatableMonthlyAttendanceReportTeacher(string id)
        {
            MonthlyAttandanceModel model = new MonthlyAttandanceModel();
            model.SchoolId = _user.SchoolId;
            string[] splitData = id.Split('~');
            try
            {
                //model.ClassId = Convert.ToInt64(splitData[0]);
                //model.DivisionId = Convert.ToInt64(splitData[1]);
                if (splitData[0] == "0")
                    model.ShiftId = AttendanceShift.Morning;
                else
                    model.ShiftId = AttendanceShift.Evening;
                model.AttandanceDate = Convert.ToDateTime(splitData[1]);
                model.shiftData = Convert.ToInt32(splitData[0]);
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Views/Report/_pv_MonthlyAttendanceListteacher.cshtml", model);
        }
        public IActionResult StudentList()
        {
            StudentModel model = new StudentModel();
            model.schoolId = _user.SchoolId;
            model.SchoolName = _user.tb_School.SchoolName;
            return View(model);
        }

        public PartialViewResult DatatableStudentsListReport(string id)
        {
            StudentModel model = new StudentModel();
            model.schoolId = _user.SchoolId;
            string[] splitData = id.Split('~');
            //long schoolId = _user.SchoolId;
            model.classId = Convert.ToInt64(splitData[0]);
            model.divisionId = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/Report/_pv_StudentList.cshtml", model);
        }


        public IActionResult BilledReport()
        {
            FeeModel model = new FeeModel();
            model.StartDate = CurrentTime;
            return View(model);
        }
        public PartialViewResult BilledReportByDate(string id)
        {
            FeeModel model = new FeeModel();

            string[] splitData = id.Split('~');


            DateTime FDate = Convert.ToDateTime(splitData[0]);
            DateTime LDate = Convert.ToDateTime(splitData[1]);

            //string[] splitFDate = FDate.Split('-');
            //string startDate = splitFDate[1] + '/' + splitFDate[0] + '/' + splitFDate[2];

            //string[] splitLDate = LDate.Split('-');
            //string endDate = splitLDate[1] + '/' + splitLDate[0] + '/' + splitLDate[2] + ' ' + "11:59:00 PM";

            string endDate = LDate.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            model.StartDate = Convert.ToDateTime(FDate);
            model.EndDate = Convert.ToDateTime(endDate);
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Report/_BilledReport_Grid.cshtml", model);
        }

        public IActionResult Salaryreport()
        {
            SalaryReportModel model = new SalaryReportModel();
            model.SchoolId = _user.SchoolId;
            model.StartDate = CurrentTime;
            return View(model);

        }

        public PartialViewResult SalaryReport_staff_Teacher(int number = 0)
        {
            SalaryReportModel SalaryReportModel = new SalaryReportModel();
            List<SalaryReportModel> model = new List<SalaryReportModel>();
            var wagesShowOrHide = _Entities.tb_WagesShowsSettings.Where(z => z.SchoolId == _user.SchoolId && z.IsActive == true).FirstOrDefault();
            var loginResults = _Entities.tb_Login.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true).ToList();
            foreach (var a1 in loginResults)
            {
                SalaryReportModel m1 = new SalaryReportModel();
                if (a1.RoleId == (int)UserRole.Staff)
                {

                    var R1 = _Entities.tb_Staff.Where(x => x.UserId == a1.UserId && x.IsActive == true).FirstOrDefault();

                    if (wagesShowOrHide.IsWagesShows == false)
                    {

                        R1 = _Entities.tb_Staff.Where(x => x.UserId == a1.UserId && x.IsActive == true && x.IsPermanent == true).FirstOrDefault();
                    }


                    if (R1 != null)
                    {
                        m1.UserId = a1.UserId;
                        m1.StaffName = R1.StaffName;
                        m1.Contact = R1.Contact;
                        m1.Address = R1.Address;
                        m1.DOB = R1.DOB;
                        m1.TimeStamp = R1.TimeStamp;
                        m1.SalaryAmount = R1.SalaryAmount;
                        m1.PFPercentage = R1.PFPercentage;
                        m1.ESIPercentage = R1.ESIPercentage;
                        m1.IsPermanent = R1.IsPermanent;
                        m1.RoleId = (int)UserRole.Staff;
                        model.Add(m1);
                    }
                }
                else if (a1.RoleId == (int)UserRole.Teacher)
                {
                    var R2 = _Entities.tb_Teacher.Where(x => x.UserId == a1.UserId && x.IsActive == true).FirstOrDefault();
                    if (wagesShowOrHide.IsWagesShows == false)
                    {

                        R2 = _Entities.tb_Teacher.Where(x => x.UserId == a1.UserId && x.IsActive == true && x.IsPermanent == true).FirstOrDefault();
                    }

                    if (R2 != null)
                    {
                        m1.UserId = a1.UserId;
                        m1.TeacherSpecialId = R2.TeacherSpecialId;
                        m1.TeacherName = R2.TeacherName;
                        m1.ContactNumber = R2.ContactNumber;
                        m1.Email = R2.Email;
                        m1.TimeStamp = R2.TimeStamp;
                        m1.SalaryAmount = R2.SalaryAmount;
                        m1.PFPercentage = R2.PFPercentage;
                        m1.ESIPercentage = R2.ESIPercentage;
                        m1.IsPermanent = R2.IsPermanent;
                        m1.RoleId = (int)UserRole.Teacher;
                        model.Add(m1);
                    }
                }
            }
            SalaryReportModel.SalaryReportModelList = model;
            if (number == 2)
            {
                SalaryReportModel.SalaryReportModelList.Where(w => w.RoleId == 3).ToList().ForEach(s => s.RoleId = 0);
            }
            else if (number == 3)
            {
                SalaryReportModel.SalaryReportModelList.Where(w => w.RoleId == 2).ToList().ForEach(s => s.RoleId = 0);
            }


            //var Teacher_Results =_Entities.tb_Teacher 

            return PartialView("~/Views/Report/_Pv_SalaryReport_staff_Teacher.cshtml", SalaryReportModel);
        }

        public IActionResult BusReport()
        {
            BusReportModel model = new BusReportModel();
            model.SchoolId = _user.SchoolId;
            List<BusModel> BM_Li = new List<BusModel>();

            var var_gerBusname = _Entities.tb_Bus.Where(z => z.SchoolId == model.SchoolId && z.IsActive == true).ToList();
            foreach (var a1 in var_gerBusname)
            {
                BusModel BM = new BusModel();
                BM.BusName = a1.BusName;
                BM_Li.Add(BM);
            }
            model.BusNames_Lists = BM_Li.Distinct().ToList();
            return View(model);
        }
        public JsonResult TripLists(string name)
        {
            BusReportModel model = new BusReportModel();
            model.SchoolId = _user.SchoolId;

            List<BusModel> BM_Li = new List<BusModel>();
            //&& z.IsActive == false             
            var var_getTrip = _Entities.tb_Bus.Where(z => z.SchoolId == model.SchoolId && z.BusName == name && z.IsActive == true).ToList();
            foreach (var a1 in var_getTrip)
            {
                BusModel BM = new BusModel();
                BM.TripNumber = a1.TripNumber;
                BM_Li.Add(BM);
            }
            model.Trip_Lists = BM_Li;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult BusReport_students(BusReportModel model)
        {
            List<StudentsModel> ST_Lists = new List<StudentsModel>();
            BusModel BusModel = new BusModel();
            TripModel TripModel = new TripModel();
            DriverModel DriverModel = new DriverModel();
            try
            {
                model.SchoolId = _user.SchoolId;
                var var_bus = _Entities.tb_Bus.Where(x =>
                              x.SchoolId == model.SchoolId &&
                              x.BusName == model.BusName &&
                              x.TripNumber == model.TripNumber &&
                              x.IsActive == true
                              ).FirstOrDefault();

                BusModel.BusSpecialId = var_bus.BusSpecialId;
                BusModel.LocationStart = var_bus.LocationStart;
                BusModel.LocationEnd = var_bus.LocationEnd;
                model.BusModel = BusModel;

                var var_TripId = _Entities.tb_Trip.Where(x =>
                                x.SchoolId == model.SchoolId &&
                                x.BusId == var_bus.BusId && x.IsActive == true).FirstOrDefault();

                TripModel.TripDate = var_TripId.TripDate;
                TripModel.StartTime = var_TripId.StartTime;
                TripModel.ReachTime = var_TripId.ReachTime;
                model.TripModel = TripModel;

                var var_driverName = _Entities.tb_Driver.Where(x => x.SchoolId == model.SchoolId && x.DriverId == var_TripId.DriverId && x.IsActive == true).FirstOrDefault();
                DriverModel.DriverName = var_driverName.DriverName;
                DriverModel.DriverId = var_driverName.DriverId;
                model.DriverModel = DriverModel;

                var var_student = _Entities.tb_Student.Where(x => x.SchoolId == model.SchoolId && x.BusId == var_bus.BusId && x.IsActive == true).ToList();
                foreach (var a2 in var_student)
                {
                    StudentsModel sm = new StudentsModel();
                    sm.StundentName = a2.StundentName;
                    sm.ContactNumber = a2.ContactNumber;

                    var var_Class = _Entities.tb_Class.Where(x =>
                                    x.SchoolId == model.SchoolId &&
                                    x.ClassId == a2.ClassId && x.IsActive == true).FirstOrDefault();
                    sm.Class = var_Class.Class;

                    var var_Divition = _Entities.tb_Division.Where(x =>
                                       x.DivisionId == a2.DivisionId).FirstOrDefault();
                    sm.Division = var_Divition.Division;
                    ST_Lists.Add(sm);
                }

                model.StudentsModel_Lists = ST_Lists;

            }
            catch (Exception ex)
            {
                //return PartialView("~/Views/Report/_Pv_BusReport_students.cshtml", model);
            }



            return PartialView("~/Views/Report/_Pv_BusReport_students.cshtml", model);
        }


        //jibin 11/26/2020

        public IActionResult Report_LedgerHome()
        {
            LedgerReportModel model = new LedgerReportModel();
            model.SchoolId = _user.SchoolId;
            model.FromDate = CurrentTime;
            model.ToDate = CurrentTime;
            var input = _Entities.tb_AccountHead.Where(x => x.IsActive && x.SchoolId == _user.SchoolId).OrderBy(x => x.AccHeadName).ToList();
            ViewBag.store = input.Select(x => new SelectListItem { Text = x.AccHeadName, Value = x.AccountId.ToString() }).ToList();
            return View(model);
        }


        public PartialViewResult Report_LedgerReport(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            long headId = Convert.ToInt64(splitDate[2]);
            long subId = Convert.ToInt64(splitDate[3]);
            LedgerReportModel model = new LedgerReportModel();
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
            model.ToDate = end;
            model.SchoolName = _user.tb_School.SchoolName;
            model.Heading = Convert.ToString(splitDate[4]);
            try
            {
                model.HeadId = headId;
                model.SubId = subId;
            }
            catch
            {
                model.HeadId = 0;
                model.SubId = 0;
            }
            return PartialView("~/Views/Report/_pv_Report_LedgerReport.cshtml", model);
        }



        public object Report_LoadSubLedgerListWithFee(long id)
        {
            var head = _Entities.tb_AccountHead.Where(x => x.IsActive == true && x.AccountId == id).FirstOrDefault();
            if (head.ForBill == true)
            {
                var result = _Entities.tb_Fee.Where(x => x.IsActive == true && x.SchoolId == _user.SchoolId).ToList().Select(x =>
                new { x.FeeId, x.FeesName }
                ).ToList().OrderBy(x => x.FeesName);

                var data = result.Select(x => new SelectListItem { Text = x.FeesName, Value = x.FeeId.ToString() }).ToList();
                data.Add(new SelectListItem { Text = "Select", Value = "0" });
                data.Add(new SelectListItem { Text = "All", Value = "1" });
                return Json(new { status = data.Count > 0, list = data.OrderBy(x => x.Value).ToList() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = TrackTap.DataLibrary.Data.DropdownData.GetSubLedgerList(id);
                result.Add(new SelectListItem { Text = "All", Value = "0" });
                return Json(new { status = result.Count > 0, list = result.OrderBy(x => x.Value).ToList() }, JsonRequestBehavior.AllowGet);
            }
        }


        //jibin 11/26/2020


    }
}