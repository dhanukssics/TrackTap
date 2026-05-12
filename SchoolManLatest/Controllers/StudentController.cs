using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web.Script.Serialization;
using TrackTap.ClassLibrary;
using TrackTap.ClassLibrary.Utility;
using TrackTap.DataLibrary;
using TrackTap.Data;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class StudentController : BaseController
    {
        // GET: Student
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Subjects()
        {
            SubjectsModel model = new SubjectsModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object SubmitAddSubject(SubjectsModel model)
        {
            bool status = false;
            string message = "Failed";
            try
            {
                var sub = new tb_Subjects();
                sub.SchoolI = _user.SchoolId;
                sub.SubjectName = model.SubjectName;
                sub.IsActive = true;
                sub.TmeStamp = CurrentTime;
                _Entities.tb_Subjects.Add(sub);
                status = _Entities.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

            }
            message = status ? " Successful" : "Failed !";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public object DeleteSubject(string id)
        {
            bool status = false;
            string msg = "False";
            long subId = Convert.ToInt64(id);
            var sub = _Entities.tb_Subjects.FirstOrDefault(x => x.SubId == subId && x.IsActive);
            if (sub != null)
            {
                sub.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetSubjectDataList()
        {
            var model = new SubjectsModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Student/_pv_SubjectList.cshtml", model);
        }
        public IActionResult TimeTable()
        {
            TimetableModel model = new TimetableModel();
            model.SchoolId = _user.SchoolId;
            model.Period = ClassLibrary.Periods.One;
            model.SchoolName = _user.tb_School.SchoolName;
            return View(model);
        }
        public PartialViewResult GetTimeTableList(string id)
        {
            string[] splitdata = id.Split('~');
            var model = new TimetableModel();
            model.SchoolId = _user.SchoolId;
            model.ClassId = Convert.ToInt64(splitdata[0]);
            model.DivisonId = Convert.ToInt64(splitdata[1]);
            return PartialView("~/Views/Student/_TimetableList.cshtml", model);
        }
        public object SubmitTimetable(TimetableModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                int day = Convert.ToInt32((Days)model.DayId);
                int period = (int)model.Period + 1;
                if (_Entities.tb_TimeTable.Any(x => x.SchoolId == _user.SchoolId && x.ClassId == model.ClassId && x.DivisionId == model.DivisonId && x.Periods == ((int)model.Period + 1) && x.DayId == (int)model.DayId && x.IsActive))
                {
                    msg = "Already assigned this period, please edit the data!";
                }

                else if (_Entities.tb_TimeTable.Any(x => x.SchoolId == _user.SchoolId && x.TeacherId == model.TeacherId && x.Periods == period && x.DayId == day && x.IsActive))
                {
                    msg = " Already assigned this teacher to an another class !";
                }
                else
                {
                    var data = new tb_TimeTable();
                    data.SchoolId = _user.SchoolId;
                    data.ClassId = model.ClassId;
                    data.DivisionId = model.DivisonId;
                    data.TeacherId = model.TeacherId;
                    data.SubjectId = model.SubjectId;
                    data.DayId = (int)model.DayId;
                    data.Periods = (int)model.Period + 1;
                    data.IsActive = true;
                    data.TimeStamp = CurrentTime;
                    _Entities.tb_TimeTable.Add(data);
                    status = _Entities.SaveChanges() > 0;
                    msg = " Timetable added !";
                }
            }
            catch (Exception ex)
            {
                msg = "Please select all data";
            }
            //msg = status ? " Timetable added" : "Failed!";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditTimeTable(string id)
        {
            string[] splitdata = id.Split('~');
            var model = new TimetableModel();
            model.SchoolId = _user.SchoolId;
            model.ClassId = Convert.ToInt64(splitdata[2]);
            model.DivisonId = Convert.ToInt64(splitdata[3]);
            var dayData = splitdata[1].ToString();
            int period = Convert.ToInt32(splitdata[0]);
            int day = 0;
            if (dayData == "Monday")
                day = 0;
            else if (dayData == "Tuesday")
                day = 1;
            else if (dayData == "Wednesday")
                day = 2;
            else if (dayData == "Thursday")
                day = 3;
            else if (dayData == "Friday")
                day = 4;
            else if (dayData == "Saturday")
                day = 5;
            var data = _Entities.tb_TimeTable.Where(x => x.SchoolId == _user.SchoolId && x.ClassId == model.ClassId && x.DivisionId == model.DivisonId && x.Periods == period && x.DayId == day && x.IsActive).FirstOrDefault();
            model.TableId = data.Id;
            model.TeacherId = data.TeacherId;
            model.SubjectId = data.SubjectId;
            model.Period = (Periods)data.Periods - 1;
            model.DayId = (Days)data.DayId;
            return PartialView("~/Views/Student/_pv_EditTimeTable.cshtml", model);
        }

        public object EditTimeTableData(TimetableModel model)
        {
            bool status = false;
            string msg = "Success";
            try
            {
                var data = _Entities.tb_TimeTable.Where(x => x.SchoolId == _user.SchoolId && x.Id == model.TableId && x.IsActive).FirstOrDefault();
                if (data != null)
                {
                    if (data.TeacherId != model.TeacherId)
                    {
                        if (_Entities.tb_TimeTable.Any(x => x.SchoolId == _user.SchoolId && x.TeacherId == model.TeacherId && x.Periods == data.Periods && x.DayId == data.DayId && x.IsActive))
                        {
                            msg = " Already assigned this teacher to an another class !";
                        }
                        else
                        {
                            data.TeacherId = model.TeacherId;
                            data.SubjectId = model.SubjectId;
                            _Entities.SaveChanges();
                            status = true;
                            msg = "Successful";
                        }
                    }
                    else
                    {
                        data.TeacherId = model.TeacherId;
                        data.SubjectId = model.SubjectId;
                        _Entities.SaveChanges();
                        status = true;
                        msg = "Successful";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult InstallationInstruction()
        {
            var model = new SchoolModelForId();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }

        [HttpPost]
        public object SendInstallationInstruction(SchoolModelForId model)
        {

            HttpClient client = new HttpClient();
            var history = new tb_SmsHistory();
            var numbers = new List<string>();
            var MsgId = new List<string>();
            var numb = "";
            string message = "Failed";
            var status = false;
            string messagepre = "";
            long studentId = 0;



            var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (school.SmsActive)
            {
                var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
                if (package != null)
                {
                    if (package.ToDate >= CurrentTime)
                    {
                        //     ---------------------------------


                        var appdet = _Entities.tb_PushData.Where(x => x.SchoolId == _user.SchoolId).Select(x => x.PlayStore).FirstOrDefault();
                        if (appdet != null)
                        {
                            List<SendMessage> Userdata = JsonConvert.DeserializeObject<List<SendMessage>>(model.Data).ToList();
                            if (Userdata.Count > 0)
                            {
                                var senderName = "MYSCHO";
                                //if (_user.SchoolId == 10116)
                                //{
                                //    senderName = "PARDSE";
                                //}
                                //else if (_user.SchoolId == 10117)
                                //{
                                //    senderName = "HOLYIN";
                                //}
                                var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true).FirstOrDefault();
                                if (senderData != null)
                                    senderName = senderData.SenderId;
                                message = "success";
                                status = true;
                                var smsHead = new tb_SmsHead();
                                smsHead.Head = "Installation Instruction ";
                                smsHead.SchoolId = _user.SchoolId;
                                smsHead.TimeStamp = CurrentTime;
                                smsHead.IsActive = true;
                                smsHead.SenderType = (int)SMSSendType.Student;
                                _Entities.tb_SmsHead.Add(smsHead);
                                status = _Entities.SaveChanges() > 0;

                                if (Userdata[0].list.Count > 0 && Userdata[0].list != null)
                                {
                                    foreach (var ms in Userdata[0].list)
                                    {
                                        studentId = Convert.ToInt64(ms.StudentId);
                                        var studentDetails = _Entities.tb_Student.Where(z => z.StudentId == studentId).FirstOrDefault();
                                        messagepre = "Dear  parent of " + studentDetails.StundentName + " ( Admission No : " + studentDetails.StudentSpecialId + " ), Kindly follow the below link to install the school APP,  to get in touch with your child’s school and to know about the attendance and marks. " + appdet;
                                        var phone = ms.Number.ToString();
                                        int length = messagepre.Length;
                                        int que = length / 160;
                                        int rem = length % 160;
                                        if (rem > 0)
                                            que++;
                                        int smsCount = que;
                                        var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + messagepre + "&sender=" + senderName;
                                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                                        HttpWebRequest request = this.GetRequest(url);
                                        WebResponse webResponse = request.GetResponse();
                                        var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                                        var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                                        alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);
                                        if (status)
                                        {
                                            tb_SmsHistory sms = new tb_SmsHistory();
                                            sms.IsActive = true;
                                            sms.MessageContent = messagepre;
                                            sms.MessageDate = CurrentTime;
                                            sms.ScholId = _user.SchoolId;
                                            sms.StuentId = studentId;
                                            sms.MobileNumber = phone;
                                            sms.HeadId = smsHead.HeadId;
                                            sms.SendStatus = Convert.ToString(respList.success);
                                            if (respList.data != null)
                                            {
                                                sms.MessageReturnId = respList.data[0].messageId;
                                                sms.DelivaryStatus = "Pending";
                                            }
                                            sms.SmsSentPerStudent = smsCount;
                                            _Entities.tb_SmsHistory.Add(sms);
                                            _Entities.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            message = "Sorry , you don't have an app !";
                        }
                    }
                }
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

            request.Timeout = Convert.ToInt32(new TimeSpan(0, 5, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
        }


        public PartialViewResult ProgressCardView(string id)
        {
            string[] splitdata = id.Split('~');
            StudentModel model = new StudentModel();
            model.studentId = Convert.ToInt64(splitdata[0]);
            model.classId = Convert.ToInt64(splitdata[1]);
            model.divisionId = Convert.ToInt64(splitdata[2]);
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/Student/_p_v_ProgressCard.cshtml", model);
        }

        public PartialViewResult ProgressCardButton(string id)
        {
            string[] splitdata = id.Split('~');
            StudentModel model = new StudentModel();
            model.studentId = Convert.ToInt64(splitdata[0]);
            model.Data = Convert.ToString(splitdata[1]);
            return PartialView("~/Views/Student/_pv_ProgressCardButton.cshtml", model);
        }
        public IActionResult ProgressCardGeneration(string id)
        {
            string[] splitdata = id.Split('~');
            long studnetId = Convert.ToInt64(splitdata[0]);
            string[] exams = splitdata[1].Split(',');
            int examcount = exams.Length; //basheer on 28/01/2019 to get the count of exams
            var student = _Entities.tb_Student.Where(x => x.StudentId == studnetId && x.IsActive).FirstOrDefault();
            ProgressCardReportModel model = new ProgressCardReportModel();
            model.SchoolName = student.tb_School.SchoolName;
            model.SchoolLogo = student.tb_School.FilePath;
            model.SchoolAddress = student.tb_School.Address;
            model.ClassName = student.tb_Class.Class + " " + student.tb_Division.Division;
            model.AccademicSession = student.tb_Class.tb_AcademicYear.AcademicYear;
            model.StudentName = student.StundentName;
            model.StudentAddress = student.Address;
            model.Parent = student.ParentName;
            model.DateOfBirth = student.DOB.ToString();
            model.AdmissionNumber = student.StudentSpecialId;
            model.ClassNumber = student.ClasssNumber;
            if (examcount > 1)
            {
                long examOne = Convert.ToInt64(exams[0]);
                long examTwo = Convert.ToInt64(exams[1]);
                var one = _Entities.tb_Exams.Where(x => x.ExamId == examOne && x.IsActive).ToList().Select(x => new Exams(x)).FirstOrDefault();
                var two = _Entities.tb_Exams.Where(x => x.ExamId == examTwo && x.IsActive).ToList().Select(x => new Exams(x)).FirstOrDefault();
                model.ExamOne = one.ExamName;
                model.ExamTwo = two.ExamName;
                List<ExamSubjects> oneSub = one.ExamSubjectsList.Union(two.ExamSubjectsList).ToList();
                var subjects = oneSub.Select(x => x.SubjectId).Distinct().ToList().Select(x => new Subjects(x)).ToList();
                List<StudentMarks> markOne = student.tb_StudentMarks.Where(x => x.ExamId == one.ExamId && x.IsActive).ToList().Select(x => new StudentMarks(x)).ToList();
                List<StudentMarks> markTwo = student.tb_StudentMarks.Where(x => x.ExamId == two.ExamId && x.IsActive).ToList().Select(x => new StudentMarks(x)).ToList();

                model.Marks = new List<StudentProgressCardMarks>();
                foreach (var item in subjects)
                {
                    StudentProgressCardMarks progress = new StudentProgressCardMarks();
                    progress.Subject = item.SubjectName;
                    var x1 = one.ExamSubjectsList.Where(x => x.SubjectId == item.SubId && x.IsActive).FirstOrDefault();
                    var x2 = two.ExamSubjectsList.Where(x => x.SubjectId == item.SubId && x.IsActive).FirstOrDefault();
                    if (x1 != null)
                    {
                        progress.InternalOne = markOne.Where(x => x.SubjectId == x1.SubId).Select(x => x.InternalMark).FirstOrDefault() ?? 0;
                        progress.ExternalOne = markOne.Where(x => x.SubjectId == x1.SubId).Select(x => x.ExternalMark).FirstOrDefault() ?? 0;
                        progress.TotalOne = progress.InternalOne + progress.ExternalOne;
                        progress.InternalTotalOne = x1.InternalMarks;
                        progress.ExternalTotalOne = x1.ExternalMark;
                        progress.GrandTotalOne = x1.InternalMarks + x1.ExternalMark;
                    }
                    else
                    {
                        progress.InternalOne = 0;
                        progress.ExternalOne = 0;
                        progress.TotalOne = progress.InternalOne + progress.ExternalOne;
                        progress.InternalTotalOne = 0;
                        progress.ExternalTotalOne = 0;
                        progress.GrandTotalOne = 0;
                    }

                    if (x2 != null)
                    {
                        progress.InternalTwo = markTwo.Where(x => x.SubjectId == x2.SubId).Select(x => x.InternalMark).FirstOrDefault() ?? 0;
                        progress.ExternalTwo = markTwo.Where(x => x.SubjectId == x2.SubId).Select(x => x.ExternalMark).FirstOrDefault() ?? 0;
                        progress.TotalTwo = progress.InternalTwo + progress.ExternalTwo;
                        progress.InternalTotalTwo = x2.InternalMarks;
                        progress.ExternalTotalTwo = x2.ExternalMark;
                        progress.GrandTotalTwo = x2.InternalMarks + x2.ExternalMark;
                    }
                    else
                    {
                        progress.InternalTwo = 0;
                        progress.ExternalTwo = 0;
                        progress.TotalTwo = progress.InternalTwo + progress.ExternalTwo;
                        progress.InternalTotalTwo = 0;
                        progress.ExternalTotalTwo = 0;
                        progress.GrandTotalTwo = 0;
                    }

                    progress.GrandTotal = progress.TotalOne + progress.TotalTwo;
                    progress.GrandGrandTotal = progress.GrandTotalOne + progress.GrandTotalTwo;

                    var totalOutOff = oneSub.Where(x => x.ExamId == one.ExamId && x.SubjectId == item.SubId).Select(x => x.InternalMarks).FirstOrDefault() + oneSub.Where(x => x.ExamId == one.ExamId && x.SubjectId == item.SubId).Select(x => x.ExternalMark).FirstOrDefault();
                    totalOutOff = totalOutOff + oneSub.Where(x => x.ExamId == two.ExamId && x.SubjectId == item.SubId).Select(x => x.InternalMarks).FirstOrDefault() + oneSub.Where(x => x.ExamId == two.ExamId && x.SubjectId == item.SubId).Select(x => x.ExternalMark).FirstOrDefault();
                    if (totalOutOff == 0)
                        totalOutOff = 1;
                    var percentage = Math.Round((progress.GrandTotal / totalOutOff) * 100, 2);
                    if (percentage > 90)
                        progress.Grade = "A1";
                    else if (percentage > 80)
                        progress.Grade = "A2";
                    else if (percentage > 70)
                        progress.Grade = "B1";
                    else if (percentage > 60)
                        progress.Grade = "B2";
                    else if (percentage > 50)
                        progress.Grade = "C1";
                    else if (percentage > 40)
                        progress.Grade = "C2";
                    else if (percentage > 32)
                        progress.Grade = "D";
                    else
                        progress.Grade = "E";
                    progress.Rank = "1";
                    model.Marks.Add(progress);
                }
                var OutOff = oneSub.Where(x => x.ExamId == one.ExamId && x.IsActive).Sum(x => x.InternalMarks) + oneSub.Where(x => x.ExamId == one.ExamId && x.IsActive).Sum(x => x.ExternalMark);
                OutOff = OutOff + oneSub.Where(x => x.ExamId == two.ExamId && x.IsActive).Sum(x => x.InternalMarks) + oneSub.Where(x => x.ExamId == one.ExamId && x.IsActive).Sum(x => x.ExternalMark);
                if (OutOff == 0)
                    OutOff = 1;
                var studentTotal = model.Marks.Sum(x => x.GrandTotal);
                model.Overall = studentTotal + "/" + OutOff;
                model.Percentage = (studentTotal / OutOff) * 100;
                if (model.Percentage > 90)
                    model.Grade = "A1";
                else if (model.Percentage > 80)
                    model.Grade = "A2";
                else if (model.Percentage > 70)
                    model.Grade = "B1";
                else if (model.Percentage > 60)
                    model.Grade = "B2";
                else if (model.Percentage > 50)
                    model.Grade = "C1";
                else if (model.Percentage > 40)
                    model.Grade = "C2";
                else if (model.Percentage > 32)
                    model.Grade = "D";
                else
                    model.Grade = "E";
                model.Rank = "1";
                if (model.Percentage < 32)
                {
                    model.Status = "";
                }
                else
                {
                    model.Status = "**Congratulations!";
                }
            }
            else
            {
                long examOne = Convert.ToInt64(exams[0]);
                // long examTwo = 0;
                var one = _Entities.tb_Exams.Where(x => x.ExamId == examOne && x.IsActive).ToList().Select(x => new Exams(x)).FirstOrDefault();
                //var two = _Entities.tb_Exams.Where(x => x.ExamId == examTwo && x.IsActive).ToList().Select(x => new Exams(x)).FirstOrDefault();
                model.ExamOne = one.ExamName;
                //model.ExamTwo = two.ExamName;
                List<ExamSubjects> oneSub = one.ExamSubjectsList.ToList();
                var subjects = oneSub.Select(x => x.SubjectId).Distinct().ToList().Select(x => new Subjects(x)).ToList();
                List<StudentMarks> markOne = student.tb_StudentMarks.Where(x => x.ExamId == one.ExamId && x.IsActive).ToList().Select(x => new StudentMarks(x)).ToList();
                //List<StudentMarks> markTwo = student.tb_StudentMarks.Where(x => x.ExamId == two.ExamId && x.IsActive).ToList().Select(x => new StudentMarks(x)).ToList();

                model.Marks = new List<StudentProgressCardMarks>();
                foreach (var item in subjects)
                {
                    StudentProgressCardMarks progress = new StudentProgressCardMarks();
                    progress.Subject = item.SubjectName;
                    var x1 = one.ExamSubjectsList.Where(x => x.SubjectId == item.SubId && x.IsActive).FirstOrDefault();
                    //var x2 = two.ExamSubjectsList.Where(x => x.SubjectId == item.SubId && x.IsActive).FirstOrDefault();
                    if (x1 != null)
                    {
                        progress.InternalOne = markOne.Where(x => x.SubjectId == x1.SubId).Select(x => x.InternalMark).FirstOrDefault() ?? 0;
                        progress.ExternalOne = markOne.Where(x => x.SubjectId == x1.SubId).Select(x => x.ExternalMark).FirstOrDefault() ?? 0;
                        progress.TotalOne = progress.InternalOne + progress.ExternalOne;
                        progress.InternalTotalOne = x1.InternalMarks;
                        progress.ExternalTotalOne = x1.ExternalMark;
                        progress.GrandTotalOne = x1.InternalMarks + x1.ExternalMark;
                    }
                    else
                    {
                        progress.InternalOne = 0;
                        progress.ExternalOne = 0;
                        progress.TotalOne = progress.InternalOne + progress.ExternalOne;
                        progress.InternalTotalOne = 0;
                        progress.ExternalTotalOne = 0;
                        progress.GrandTotalOne = 0;
                    }

                    //if (x2 != null)
                    //{
                    //    progress.InternalTwo = markTwo.Where(x => x.SubjectId == x2.SubId).Select(x => x.InternalMark).FirstOrDefault() ?? 0;
                    //    progress.ExternalTwo = markTwo.Where(x => x.SubjectId == x2.SubId).Select(x => x.ExternalMark).FirstOrDefault() ?? 0;
                    //    progress.TotalTwo = progress.InternalTwo + progress.ExternalTwo;
                    //    progress.InternalTotalTwo = x2.InternalMarks;
                    //    progress.ExternalTotalTwo = x2.ExternalMark;
                    //    progress.GrandTotalTwo = x2.InternalMarks + x2.ExternalMark;
                    //}
                    //else
                    //{
                    //    progress.InternalTwo = 0;
                    //    progress.ExternalTwo = 0;
                    //    progress.TotalTwo = progress.InternalTwo + progress.ExternalTwo;
                    //    progress.InternalTotalTwo = 0;
                    //    progress.ExternalTotalTwo = 0;
                    //    progress.GrandTotalTwo = 0;
                    //}

                    progress.GrandTotal = progress.TotalOne;
                    progress.GrandGrandTotal = progress.GrandTotalOne;

                    var totalOutOff = oneSub.Where(x => x.ExamId == one.ExamId && x.SubjectId == item.SubId).Select(x => x.InternalMarks).FirstOrDefault() + oneSub.Where(x => x.ExamId == one.ExamId && x.SubjectId == item.SubId).Select(x => x.ExternalMark).FirstOrDefault();
                    //totalOutOff = totalOutOff + oneSub.Where(x => x.ExamId == two.ExamId && x.SubjectId == item.SubId).Select(x => x.InternalMarks).FirstOrDefault() + oneSub.Where(x => x.ExamId == two.ExamId && x.SubjectId == item.SubId).Select(x => x.ExternalMark).FirstOrDefault();
                    if (totalOutOff == 0)
                        totalOutOff = 1;
                    var percentage = Math.Round((progress.GrandTotal / totalOutOff) * 100, 1); //changed 2 into 1,because only one test 
                    if (percentage > 90)
                        progress.Grade = "A1";
                    else if (percentage > 80)
                        progress.Grade = "A2";
                    else if (percentage > 70)
                        progress.Grade = "B1";
                    else if (percentage > 60)
                        progress.Grade = "B2";
                    else if (percentage > 50)
                        progress.Grade = "C1";
                    else if (percentage > 40)
                        progress.Grade = "C2";
                    else if (percentage > 32)
                        progress.Grade = "D";
                    else
                        progress.Grade = "E";
                    progress.Rank = "1";
                    model.Marks.Add(progress);
                }
                var OutOff = oneSub.Where(x => x.ExamId == one.ExamId && x.IsActive).Sum(x => x.InternalMarks) + oneSub.Where(x => x.ExamId == one.ExamId && x.IsActive).Sum(x => x.ExternalMark);
                //OutOff = OutOff + oneSub.Where(x => x.ExamId == two.ExamId && x.IsActive).Sum(x => x.InternalMarks) + oneSub.Where(x => x.ExamId == one.ExamId && x.IsActive).Sum(x => x.ExternalMark);
                if (OutOff == 0)
                    OutOff = 1;
                var studentTotal = model.Marks.Sum(x => x.GrandTotal);
                model.Overall = studentTotal + "/" + OutOff;
                model.Percentage = (studentTotal / OutOff) * 100;
                if (model.Percentage > 90)
                    model.Grade = "A1";
                else if (model.Percentage > 80)
                    model.Grade = "A2";
                else if (model.Percentage > 70)
                    model.Grade = "B1";
                else if (model.Percentage > 60)
                    model.Grade = "B2";
                else if (model.Percentage > 50)
                    model.Grade = "C1";
                else if (model.Percentage > 40)
                    model.Grade = "C2";
                else if (model.Percentage > 32)
                    model.Grade = "D";
                else
                    model.Grade = "E";
                model.Rank = "1";
                if (model.Percentage < 32)
                {
                    model.Status = "";
                }
                else
                {
                    model.Status = "**Congratulations!";
                }

            }

            var present = _Entities.tb_Attendance.Where(x => x.StudentId == student.StudentId && x.IsActive && x.AttendanceDate.Year == CurrentTime.Year && x.AttendanceData == true).Count();
            var total = _Entities.tb_Attendance.Where(x => x.StudentId == student.StudentId && x.IsActive && x.AttendanceDate.Year == CurrentTime.Year).Count();
            model.Attendance = present.ToString() + " : " + total.ToString();
            model.CurrentDate = CurrentTime.ToShortDateString();
            return View(model);
        }
        public PartialViewResult GetTeacherTimeTableList()
        {
            var model = new TimetableModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Student/_pv_TeacherTimeList.cshtml", model);
        }
        public PartialViewResult GetTeacherTimeTableListReport(string id)
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.teacherId = Convert.ToInt64(id);
            var data = _Entities.tb_Teacher.Where(x => x.TeacherId == model.teacherId && x.IsActive).FirstOrDefault();
            model.userId = data.UserId;
            model.TeacherName = data.TeacherName;
            return PartialView("~/Views/Student/_pv_TeacherTimeTable.cshtml", model);
        }

        public IActionResult AddStudentDetailedDetails(string id)
        {
            ParentDetailsView model = new ParentDetailsView();
            model.StudentId = Convert.ToInt64(id);
            var studentData = _Entities.tb_Student.Where(x => x.StudentId == model.StudentId && x.IsActive).FirstOrDefault();
            if (studentData != null)
            {
                model.Address = studentData.Address;
                model.City = studentData.City;
                model.Email = studentData.ParentEmail;
                model.ContactNumber = studentData.ContactNumber;
                model.State = studentData.State;
                model.StudentName = studentData.StundentName.ToUpper();
                model.StudentClassDivision = studentData.tb_Class.Class + " " + studentData.tb_Division.Division;
                model.SchoolId = studentData.SchoolId;
                model.ParentName = studentData.ParentName;
            }
            return View(model);
        }
        public PartialViewResult SelectSiblings(string id)
        {
            SchoolModel model = new SchoolModel();
            model.CurrentAddedStudent = Convert.ToInt64(id);
            return PartialView("~/Views/Student/_pv_SelectSiblings.cshtml", model);
        }
        public PartialViewResult SearchAdmission(string id)
        {
            StudentModel model = new StudentModel();
            string[] splitData = id.Split('~');
            model.admissionNo = splitData[0];
            model.schoolId = Convert.ToInt64(splitData[1]);
            model.CurrentStudentId = Convert.ToInt64(splitData[2]);
            return PartialView("~/Views/Student/_pv_StudentDetailsInParent.cshtml", model);
        }
        public object AddParentToKid(string id)
        {
            bool status = false;
            string msg = "Failed";
            string[] splitData = id.Split('~');
            long siblings = Convert.ToInt64(splitData[0]);
            long currentStudent = Convert.ToInt64(splitData[1]);
            var parent = _Entities.tb_Student.Where(x => x.StudentId == siblings && x.IsActive).FirstOrDefault();
            if (parent != null)
            {
                if (parent.ParentId == null)
                {
                    msg = "This students information is not available !";
                }
                else
                {
                    var newStudent = _Entities.tb_Student.Where(x => x.StudentId == currentStudent && x.IsActive).FirstOrDefault();
                    newStudent.ParentId = parent.ParentId;
                    newStudent.ParentName = parent.ParentName;
                    //newStudent.MotherName = parent.MotherName;
                    status = _Entities.SaveChanges() > 0;
                }
            }
            if (status)
                msg = "Successful";
            return Json(new { status = status, msg = msg, studentId = currentStudent }, JsonRequestBehavior.AllowGet);
        }

        public object AddParentDetails(ParentDetailsView model)
        {
            string msg = "Failed";
            bool status = false;
            string studentId = model.StudentId.ToString();
            if (model.ParentName != null && model.ParentName != string.Empty && model.StudentId != null)
            {
                var parent = _Entities.tb_Parent.Create();
                parent.ParentName = model.ParentName;
                parent.Address = model.Address;
                parent.City = model.City;
                parent.Email = model.Email;
                parent.ContactNumber = model.ContactNumber;
                parent.Password = model.Password;
                parent.TimeStamp = CurrentTime;
                parent.ParentGuid = new Guid();
                parent.IsActive = true;
                parent.FilePath = null;
                parent.PostalCode = model.PostelCode;
                parent.MotherName = model.MotherName;
                _Entities.tb_Parent.Add(parent);
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    var student = _Entities.tb_Student.Where(x => x.StudentId == model.StudentId && x.IsActive).FirstOrDefault();
                    if (student != null)
                    {
                        student.ParentId = parent.ParentId;
                        _Entities.SaveChanges();
                    }
                    msg = "Successful";
                }
            }
            return Json(new { status = status, msg = msg, studentId = studentId }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult StudentAdvancedDetails(string id)
        {
            StudentDetailedView model = new StudentDetailedView();
            model.StudentId = Convert.ToInt64(id);
            var student = _Entities.tb_Student.Where(x => x.StudentId == model.StudentId && x.IsActive).FirstOrDefault();
            if (student != null)
            {
                model.StudentName = student.StundentName;
                model.StudentClassDivision = student.tb_Class.Class + " " + student.tb_Division.Division;
            }
            model.IsVaccinated = false;
            return PartialView("~/Views/Student/_pv_StudentAdvancedDetailsAdd.cshtml", model);
        }
        public IActionResult ViewStudentDetailedDetails(string id)
        {
            ParentDetailsView model = new ParentDetailsView();
            model.StudentId = Convert.ToInt64(id);
            var studentData = _Entities.tb_Student.Where(x => x.StudentId == model.StudentId && x.IsActive).FirstOrDefault();
            if (studentData != null)
            {
                model.Address = studentData.Address;
                model.City = studentData.City;
                model.Email = studentData.ParentEmail;
                model.ContactNumber = studentData.ContactNumber;
                model.State = studentData.State;
                model.StudentName = studentData.StundentName.ToUpper();
                model.StudentClassDivision = studentData.tb_Class.Class + " " + studentData.tb_Division.Division;
                model.SchoolId = studentData.SchoolId;
                model.ParentName = studentData.ParentName;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult DocumentUpload()
        {
            bool status = false;
            string msg = "Failed";
            string form = string.Empty, filepath = string.Empty;
            string userId = Convert.ToString(_user.UserId);
            string schoolId = Convert.ToString(_user.SchoolId);
            if (!string.IsNullOrEmpty(userId))
                form = userId;
            long formId = Convert.ToInt64(form);
            if (Request.Files.Count > 0)
            {
                var httpPostedFile = Request.Files[0];
                string randomFolder = Guid.NewGuid().ToString();
                string folderPath = Server.MapPath("~/Media/" + schoolId + "/StudentAdvanced/");
                string OrginalName = randomFolder + "" + httpPostedFile.FileName.ToString();
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var fileSavePath = Path.Combine(folderPath, OrginalName);
                httpPostedFile.SaveAs(fileSavePath);
                filepath = Path.Combine("/Media/" + schoolId + "/StudentAdvanced/", OrginalName);
                msg = "Success";
                status = true;
            }
            return Json(new { Status = status, Message = msg, UserData = filepath }, JsonRequestBehavior.AllowGet);
        }
        public object AddStudentAdvancedDetails(StudentDetailedView model)
        {
            bool status = false;
            string msg = "Failed";
            var oldData = _Entities.tb_StudentDetailedDetails.Where(x => x.StudentId == model.StudentId && x.IsActive).FirstOrDefault();
            if (oldData == null)
            {
                if (model.GuardianName == string.Empty)
                {
                    msg = "Please enter the Guardian Name";
                }
                else
                {
                    var data = _Entities.tb_StudentDetailedDetails.Create();
                    data.IsActive = true;
                    data.TimeStamp = CurrentTime;
                    data.GuardianName = model.GuardianName;
                    data.RelationShip = model.RelationShip;
                    data.Occupation = model.Occupation;
                    data.Guardian_Address = model.Guardian_Address;
                    data.Contact_LandLine = model.Contact_LandLine;
                    data.Contact_OfficeLine = model.Contact_OfficeLine;
                    data.Previous_SchoolName = model.Previous_SchoolName;
                    data.Previous_Standard = model.Previous_Standard;
                    data.TC_Filepath = model.TC_Filepath;
                    data.TC_Number = model.TC_Number;
                    data.PlaceOFBirth = model.PlaceOFBirth;
                    data.DOBCertificate_FilePath = model.DOBCertificate_FilePath;
                    data.ReligionId = model.ReligionId;
                    data.CategoryId = model.CategoryId;
                    data.Caste = model.Caste;
                    data.NationalityId = Convert.ToInt32(model.NationalityId);
                    data.MotherTongue = model.MotherTongue;
                    data.PermanentBodyMark1 = model.PermanentBodyMark1;
                    data.PermanentBodyMark2 = model.PermanentBodyMark2;
                    data.BoardingPoint = model.BoardingPoint;
                    data.KnownLanguage1 = model.KnownLanguage1;
                    data.KnownLanguage2 = model.KnownLanguage2;
                    data.Taluk = model.Taluk;
                    data.Revenue_District = model.Revenue_District;
                    data.PanchayatiRajSystemId = Convert.ToInt32(model.PanchayatiRajSystemId);
                    data.DistrictPanchayath = model.DistrictPanchayath;
                    data.BlockPanchayath = model.BlockPanchayath;
                    data.InstructionMediumId = Convert.ToInt32(model.InstructionMediumId);
                    data.FirstLanguagePaper1 = Convert.ToInt32(model.FirstLanguagePaper1);
                    data.FirstLanguagePaper2 = Convert.ToInt32(model.FirstLanguagePaper2);
                    data.ThirdLanguage = Convert.ToInt32(model.ThirdLanguage);
                    data.IsVaccinated = model.IsVaccinated;
                    data.LearingDisabilityId = Convert.ToInt32(model.LearingDisabilityId);
                    data.EconomicalStatus = Convert.ToInt32(model.EconomicalStatus);
                    data.StudentId = model.StudentId;
                    try
                    {
                        if (model.Previous_DateOfAdmissionString != string.Empty && model.Previous_DateOfAdmissionString != null)
                        {
                            string[] splitData = model.Previous_DateOfAdmissionString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var dob = mm + '-' + dd + '-' + yyyy;
                            data.Previous_DateOfAdmission = Convert.ToDateTime(dob);
                        }
                        if (model.Previous_DateOfLeavingString != string.Empty && model.Previous_DateOfLeavingString != null)
                        {
                            string[] splitData = model.Previous_DateOfLeavingString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var dob = mm + '-' + dd + '-' + yyyy;
                            data.Previous_DateOfLeaving = Convert.ToDateTime(dob);
                        }
                        if (model.TC_Date != string.Empty && model.TC_Date != null)
                        {
                            string[] splitData = model.TC_Date.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var dob = mm + '-' + dd + '-' + yyyy;
                            data.TC_Date = Convert.ToDateTime(dob).ToString();
                        }
                        if (model.VaccinatedDateString != string.Empty && model.VaccinatedDateString != null)
                        {
                            string[] splitData = model.VaccinatedDateString.Split('-');
                            var dd = splitData[0];
                            var mm = splitData[1];
                            var yyyy = splitData[2];
                            var dob = mm + '-' + dd + '-' + yyyy;
                            data.VaccinatedDate = Convert.ToDateTime(dob);
                        }
                    }
                    catch
                    {
                        msg = "Incorrect date format ";
                    }
                    _Entities.tb_StudentDetailedDetails.Add(data);
                    status = _Entities.SaveChanges() > 0;
                }
                if (status)
                {
                    msg = "Successful";
                }
            }
            else
            {
                msg = "This child's information is already exists!";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult StudentFileUploadHome(string id)
        {
            StudentFileUploadModel model = new StudentFileUploadModel();
            model.StudentId = Convert.ToInt64(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult MemoAchievementUpload()
        {
            bool status = false;
            string msg = "Failed";
            string form = string.Empty, filepath = string.Empty;
            string userId = Convert.ToString(_user.UserId);
            string schoolId = Convert.ToString(_user.SchoolId);
            if (!string.IsNullOrEmpty(userId))
                form = userId;
            long formId = Convert.ToInt64(form);
            if (Request.Files.Count > 0)
            {
                var httpPostedFile = Request.Files[0];
                string randomFolder = Guid.NewGuid().ToString();
                string folderPath = Server.MapPath("~/Media/" + schoolId + "/Achievement/");
                //string folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Media/" + schoolId + "/Achievement/");
                string OrginalName = randomFolder + "" + httpPostedFile.FileName.ToString();
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var fileSavePath = Path.Combine(folderPath, OrginalName);
                httpPostedFile.SaveAs(fileSavePath);
                filepath = Path.Combine("/Media/" + schoolId + "/Achievement/", OrginalName);
                msg = "Success";
                status = true;
            }
            return Json(new { Status = status, Message = msg, UserData = filepath }, JsonRequestBehavior.AllowGet);
        }

        public object AddStudentFiles(StudentFileUploadModel model)
        {
            bool status = false;
            string msg = "Failed";
            string studnetId = model.StudentId.ToString();
            if (model.FilePath == null)
            {
                msg = "Please select any file !";
            }
            else
            {
                try
                {
                    var data = _Entities.tb_StudentFiles.Create();
                    data.StudentId = model.StudentId;
                    data.SchoolId = _user.SchoolId;
                    data.FilePath = model.FilePath;
                    data.Description = model.Description;
                    data.IsActive = true;
                    data.TimeStamp = CurrentTime;
                    if (model.ReceivedDateString != string.Empty && model.ReceivedDateString != null)
                    {
                        string[] splitData = model.ReceivedDateString.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var dob = mm + '-' + dd + '-' + yyyy;
                        data.ReceivingDate = Convert.ToDateTime(dob);
                    }
                    data.FileType = Convert.ToInt32(model.TypeId);
                    _Entities.tb_StudentFiles.Add(data);
                    status = _Entities.SaveChanges() > 0;
                    if (status)
                        msg = "Successful";
                }
                catch
                {
                    msg = "Failed";
                }
            }
            return Json(new { status = status, msg = msg, studnetId = studnetId }, JsonRequestBehavior.AllowGet);
        }

        public object DeleteTimeTable(string id)
        {
            bool status = false;
            string message = "Failed";
            long tableId = Convert.ToInt64(id);
            var timeTable = _Entities.tb_TimeTable.FirstOrDefault(z => z.Id == tableId);
            if (timeTable != null)
            {
                timeTable.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? "Deleted" : "failed";
            return Json(new { status = status, msg = message, division = timeTable.DivisionId == null ?0 : timeTable.DivisionId,classid=timeTable.ClassId }, JsonRequestBehavior.AllowGet);
        }

















    }
}

