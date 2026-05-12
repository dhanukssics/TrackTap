using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TrackTap.PostModel;
using TrackTap.DataLibrary;
using TrackTap.Models;
using System.Net;
using System.IO;
using TrackTap.ClassLibrary.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Text;

namespace TrackTap.Controllers
{
    public class TeacherController : BaseController
    {
        // GET: Teacher
        public IActionResult Attendance()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.userId = _user.UserId;
            model.Selecteddate = CurrentTime;
            return View(model);
        }
        public IActionResult TeacherAttendance()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.userId = _user.UserId;
            model.Selecteddate = CurrentTime;
            return View(model);
        }

        public PartialViewResult AttendanceMarking(string id)
        {
            string[] splitData = id.Split('~');
            DateTime minDate = Convert.ToDateTime(splitData[0]);
            int shift = Convert.ToInt32(splitData[1]);
            long classId = Convert.ToInt32(splitData[2]);
            long divisionId = Convert.ToInt32(splitData[3]);

            string Maxdate = minDate.Date.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(Maxdate);


            var atndance = _Entities.tb_Attendance.Where(z => z.ClassId == classId && z.DivisionId == divisionId && z.AttendanceDate >= minDate && z.AttendanceDate <= maxDate && z.ShiftStatus == shift).OrderBy(x => x.tb_Student.StundentName).ToList();

            AttendanceModels model = new AttendanceModels();
            model.classId = classId;
            model.divisionId = divisionId;
            model.shift = shift;
            model.maxDate = maxDate;
            model.minDate = minDate;


            bool status = false;
            string msg = string.Empty;
            // var school = Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (atndance.Count <= 0)
            {
                status = true;
                return PartialView("~/Views/Teacher/_pv_AttendanceMark_Grid.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/Teacher/_pv_AttendanceView_Grid.cshtml", model);
            }
        }

        //Code added by Gayathri (16/01/2024)teacher attendance 
        public PartialViewResult AttendanceMarking_Teacher(string id)
        {
            string[] splitData = id.Split('~');
            DateTime minDate = Convert.ToDateTime(splitData[0]);
            int shift = Convert.ToInt32(splitData[1]);
            //long classId = Convert.ToInt32(splitData[2]);
            //long divisionId = Convert.ToInt32(splitData[3]);

            string Maxdate = minDate.Date.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(Maxdate);


            var atndance = _Entities.tb_AttendanceTeacher.Where(z => z.AttendanceDate >= minDate && z.AttendanceDate <= maxDate && z.ShiftStatus == shift).ToList();

            AttendanceModels model = new AttendanceModels();
            //model.classId = classId;
            //model.divisionId = divisionId;
            model.shift = shift;
            model.maxDate = maxDate;
            model.minDate = minDate;
            model.SchoolId = _user.SchoolId;

            bool status = false;
            string msg = string.Empty;
            // var school = Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (atndance.Count <= 0)
            {
                status = true;
                return PartialView("~/Views/Teacher/_pv_AttendanceMark_Grid_Teacher.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/Teacher/_pv_AttendanceView_Grid_Teacher.cshtml", model);
            }
        }

        [HttpPost]
        public object Attendance(SchoolModel model)
        {
            bool status = false;
            string msg = "Failed";
            long teacherId = Convert.ToInt64(model.teacherId);
            long classId = Convert.ToInt64(model.classId);
            long divisionId = Convert.ToInt64(model.divisionId);
            int shiftStatus = model.shiftStatus;
            DateTime attendancedate = Convert.ToDateTime(model.attendanceDate);
            var classDetails = new TrackTap.DataLibrary.Data.Division(divisionId);
            try
            {

                try
                {
                    _Entities.SP_DeleteOldAttendance(attendancedate, _user.UserId, classId, divisionId, shiftStatus);
                }
                catch
                {

                }
                long headId = 0;
                //if (model.studentList.Count(x => x.attendaneStatus == "False") > 0)
                //{//Archana 27-Mar-2018 The message head wants to insert the head table only when the list have atleast one absenties.

                //}
                foreach (var item in model.studentList)
                {
                    var attendance = _Entities.tb_Attendance.Create();
                    attendance.AttendanceGuid = Guid.NewGuid();
                    attendance.StaffId = _user.UserId;
                    attendance.ClassId = classId;
                    attendance.DivisionId = divisionId;
                    attendance.AttendanceDate = attendancedate;
                    //attendance.AttendanceData = attendanceStatus;
                    attendance.AttendanceData = item.attendaneStatus == "False" ? false : true;
                    attendance.IsActive = true;
                    attendance.TimeStamp = CurrentTime;
                    attendance.StudentId = Convert.ToInt64(item.studentId);
                    attendance.ShiftStatus = shiftStatus;
                    _Entities.tb_Attendance.Add(attendance);
                    status = _Entities.SaveChanges() > 0;
                    //var studentList = _Entities.tb_Student.Where(x => x.StudentId == attendance.StudentId && x.IsActive).FirstOrDefault();
                    //if (studentList.ContactNumber != null)
                    //{
                    //    if (studentList.ContactNumber != string.Empty)
                    //    {
                    //        if (attendance.AttendanceData != true)
                    //        {
                    //            //Thread Thread = new Thread(() => SendSMS(studentList.ContactNumber, studentList.StundentName, attendance.AttendanceData, attendance.AttendanceDate, studentList.StudentId, headId));
                    //            //Thread.Start();
                    //            //string message = "Dear Parent,  " + studentList.StundentName + " is absent today (" + attendance.AttendanceDate.ToString("dd/MM/yyyy") + ") - " + _user.Name;
                    //            ////----------------Send Email
                    //            //SendStudentEmail(message, studentList.ParentEmail, "Attendance Details", studentList.tb_School.SchoolName);// Send EMAIL for student attendance
                    //            //SendStudentPush(studentList, message);
                    //        }

                    //    }
                    //}
                }
                var student_list = model.studentList.Where(z => z.attendaneStatus == "False").ToList();
                if (student_list != null)
                {
                    Thread Thread = new Thread(() => SendSMSForAttendance(student_list, attendancedate, divisionId));
                    Thread.Start();
                }

                msg = status ? "Success" : "Failed";
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        //Added by Gayathri A(22/01/2024)
        [HttpPost]
        public object AttendanceTeacher(SchoolModel model)
        {
            bool status = false;
            string msg = "Failed";
            long teacherId = Convert.ToInt64(model.teacherId);
            //long classId = Convert.ToInt64(model.classId);
            //long divisionId = Convert.ToInt64(model.divisionId);
            int shiftStatus = model.shiftStatus;
            DateTime attendancedate = Convert.ToDateTime(model.attendanceDate);
            //var classDetails = new TrackTap.DataLibrary.Data.Division(divisionId);
            try
            {

                try
                {
                    _Entities.SP_DeleteOldAttendanceTeacher(attendancedate, _user.UserId,shiftStatus);
                }
                catch
                {

                }
                long headId = 0;
                
                foreach (var item in model.studentList)
                {
                    var attendance = _Entities.tb_AttendanceTeacher.Create();
                    attendance.AttendanceGuid = Guid.NewGuid();
                    //attendance.StaffId = _user.UserId;
                    //attendance.ClassId = classId;
                    //attendance.DivisionId = divisionId;
                    attendance.AttendanceDate = attendancedate;
                    //attendance.AttendanceData = attendanceStatus;
                    attendance.AttendanceData = item.attendaneStatus == "False" ? false : true;
                    attendance.IsActive = true;
                    attendance.TimeStamp = CurrentTime;
                    attendance.TeacherId = Convert.ToInt64(item.studentId);
                    attendance.ShiftStatus = shiftStatus;
                    _Entities.tb_AttendanceTeacher.Add(attendance);
                    status = _Entities.SaveChanges() > 0;
                  
                }
                var student_list = model.studentList.Where(z => z.attendaneStatus == "False").ToList();
                if (student_list != null)
                {
                    //Thread Thread = new Thread(() => SendSMSForAttendance(student_list, attendancedate));
                    //Thread.Start();
                }

                msg = status ? "Success" : "Failed";
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public bool SendSMSForAttendance(List<Student> list, DateTime attendanceDate, long divisionId)
        {
            try
            {
                var classDetails = new TrackTap.DataLibrary.Data.Division(divisionId);
                long headId = 0;
                bool status = false;
                string msg = "Failed";
                var smsHead = new tb_SmsHead();
                smsHead.Head = "Attendance " + CurrentTime.ToString("dd-MM-yyyy") + " of " + classDetails.Class.ClassName + " - " + classDetails.DivisionName;
                smsHead.SchoolId = _user.SchoolId;
                smsHead.TimeStamp = CurrentTime;
                smsHead.IsActive = true;
                smsHead.SenderType = 0;
                _Entities.tb_SmsHead.Add(smsHead);
                status = _Entities.SaveChanges() > 0;
                headId = smsHead.HeadId;
                foreach (var item in list)
                {
                    long studentId = Convert.ToInt64(item.studentId);
                    var studentList = _Entities.tb_Student.Where(x => x.StudentId == studentId && x.IsActive).FirstOrDefault();
                    if (studentList.ContactNumber != null)
                    {
                        if (studentList.ContactNumber != string.Empty)
                        {

                            SendSMS(studentList.ContactNumber, studentList.StundentName, false, attendanceDate, studentList.StudentId, headId);
                            string message = "Dear Parent,  " + studentList.StundentName + " is absent today (" + attendanceDate.ToString("dd/MM/yyyy") + ") - " + _user.Name;
                            //----------------Send Email
                            SendStudentEmail(message, studentList.ParentEmail, "Attendance Details", studentList.tb_School.SchoolName);// Send EMAIL for student attendance
                            SendStudentPush(studentList, message);

                        }
                    }
                }
            }
            catch
            {

            }
            return true;

        }
        private void SendStudentPush(tb_Student studentDetails, string message)
        {
            try
            {
                string schoolName = "Message from " + studentDetails.tb_School.SchoolName;
                var tokenData = _Entities.tb_DeviceToken.Where(x => x.UserId == studentDetails.ParentId && x.IsActive == true && x.LoginStatus == 1).OrderBy(x => x.TokenId).FirstOrDefault();
                if (tokenData != null)
                {
                    var applicationID = "";
                    var senderId = "";
                    var pushData = _Entities.tb_PushData.Where(x => x.SchoolId == studentDetails.SchoolId).FirstOrDefault();
                    if (pushData != null)
                    {
                        applicationID = pushData.LegacyNumber;
                        senderId = pushData.SenderId;
                    }
                    else
                    {
                        applicationID = "AIzaSyAGcW_XdoA-bwVtUQ4IcnncTM2Toso3sv4";
                        senderId = "47900857750";
                    }

                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        to = tokenData.Token,
                        notification = new
                        {
                            body = message,
                            title = schoolName
                        },
                        priority = "high",
                        data = new
                        {
                            Role = "Teacher",
                            Function = "AttendanceData"
                        },
                        from = "School"
                    };
                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(data);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);

                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SendStudentEmail(string message, string parentEmail, string subject, string schoolName)
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/BirthdayWishesEmail.html");
            var emailTemplate = System.IO.File.ReadAllText(filePath);
            var mBody = emailTemplate.Replace("{{resetLink}}", message).Replace("{{resetLink1}}", schoolName);
            bool sendMail = Send(subject, mBody, parentEmail);
        }
        private bool Send(string subject, string mailbody, string email)
        {
            try
            {
                //MailMessage msg = new MailMessage();
                //msg.Subject = subject;
                //msg.Body = mailbody;
                //msg.From = new MailAddress("info.schoolman@gmail.com");
                //msg.To.Add(new MailAddress(email));
                //msg.IsBodyHtml = true;
                //SmtpClient client = new SmtpClient();
                //client.Host = "k2smtp.gmail.com";
                //NetworkCredential basicauthenticationinfo = new NetworkCredential("info.schoolman@gmail.com", "Info@123");
                //client.Port = int.Parse("587");//25//465
                //client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                //client.Credentials = basicauthenticationinfo;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //        System.Security.Cryptography.X509Certificates.X509Chain chain,
                //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                //{
                //    return true;
                //};
                //try
                //{
                //    client.Send(msg);
                //    client.Dispose();
                //}
                //catch (Exception ex)
                //{
                //}
                try
                {
                    MailMessage msg = new MailMessage();
                    msg.Subject = subject;
                    msg.Body = mailbody;
                    msg.From = new MailAddress("schoolman@srishtis.com");
                    msg.To.Add(new MailAddress(email, "Dear"));
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = "k2smtpout.secureserver.net";
                    System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("schoolman@srishtis.com", "ca@12345");
                    client.Port = int.Parse("25");
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;
                    client.Credentials = basicauthenticationinfo;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(msg);
                }
                catch (Exception ex) { }
                return true;

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public bool SendSMS(string phone, string student, bool status, DateTime attendanceDate, long studentId, long headId)
        {
            var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (school.SmsActive)
            {
                var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
                if (package != null)
                {
                    if (package.ToDate >= CurrentTime)
                    {
                        //     ---------------------------------

                        string message = "";
                        if (status)
                        {
                            //  message = "Dear Parent,  " + student + " is present today (" + attendanceDate.ToString("dd/MM/yyyy") + ") - " + _user.Name;
                        }
                        else
                        {
                            message = "Dear Parent,  " + student + " is absent today (" + attendanceDate.ToString("dd/MM/yyyy") + ") - " + _user.Name;
                        }
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
                        var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + message + "&sender=" + senderName;

                        // var url = "http://bhashsms.com//api/sendmsg.php?user=srishtitrans&pass=123456&sender=MCHILD&phone=" + phone + "&text=" + message + "&priority=ndnd&stype=normal";
                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest request = this.GetRequest(url);
                        WebResponse webResponse = request.GetResponse();
                        var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                        var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                        alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);
                        if (responseText != null)
                        {
                            try
                            {
                                if (headId != 0)
                                {
                                    tb_SmsHistory sms = new tb_SmsHistory();
                                    sms.IsActive = true;
                                    sms.MessageContent = message;
                                    sms.SendStatus = Convert.ToString(respList.success);
                                    sms.MessageDate = CurrentTime;
                                    sms.ScholId = _user.SchoolId;
                                    var xx = message.Length;
                                    sms.IsActive = true;
                                    sms.MobileNumber = phone;
                                    decimal count = message.Length / 160; // 22-11-2018 Shinu
                                    if ((count % 1) > 0)
                                    {
                                        sms.SmsSentPerStudent = Convert.ToInt32(count) + 1;
                                    }
                                    else
                                    {
                                        sms.SmsSentPerStudent = Convert.ToInt32(count);
                                    }
                                    sms.StuentId = studentId;
                                    sms.HeadId = headId;
                                    if (respList.data != null)
                                    {
                                        sms.MessageReturnId = respList.data[0].messageId;
                                        sms.DelivaryStatus = "Pending";
                                    }
                                    _Entities.tb_SmsHistory.Add(sms);
                                    _Entities.SaveChanges();
                                }
                            }
                            catch
                            {

                            }
                            return true;
                        }
                        else
                            return false;


                        //       --------------------------------------

                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;



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
        public IActionResult MyTimeTable()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.userId = _user.UserId;
            return View(model);
        }


    }
}