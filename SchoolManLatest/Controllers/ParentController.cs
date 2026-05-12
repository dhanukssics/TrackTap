using CCA.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web.Script.Serialization;
using TrackTap.ClassLibrary;
using TrackTap.ClassLibrary.Utility;
using TrackTap.DataLibrary;
using TrackTap.Helper;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class ParentController : BaseController
    {
        //
        // GET: /Parent/

        public IActionResult Home()
        {
            ParentRegisterModel model = new ParentRegisterModel();
            model.parentId = _parentUser.ParentId;
            return View(model);
        }
        public PartialViewResult AddChildView()
        {
            return PartialView("~/Views/Parent/_pv_AddChildModel.cshtml");
        }
        public PartialViewResult SearchAdmission(string id)
        {
            StudentModel model = new StudentModel();
            string[] splitData = id.Split('~');
            model.admissionNo = splitData[0];
            model.schoolId = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/Parent/_pv_StudentDetailsParent.cshtml", model);
        }


        public IActionResult Attendance(string id)
        {
            AttendanceModels model = new AttendanceModels();
            model.studentId = Convert.ToInt64(id);
            return View(model);
        }
        public PartialViewResult AttendancePartial(string id)
        {
            string[] splitData = id.Split('~');
            int monthInDigit = DateTime.ParseExact(splitData[1], "MMMM", CultureInfo.InvariantCulture).Month;
            AttendanceModels model = new AttendanceModels();
            model.studentId= Convert.ToInt64(splitData[0]);
            model.month = Convert.ToInt16(monthInDigit);
            model.year = Convert.ToInt16(splitData[2]); 
            return PartialView("~/Views/Parent/_pv_Attendance_Grid.cshtml", model);
        }

        public object OtpSubmit(string id)
        {
            bool status = false;
            string message = "Wrong OTP";
            string[] splitData = id.Split('~');
            long studentId = Convert.ToInt64(splitData[0]);
            string otp = splitData[1];






            var otpDetail = _Entities.tb_OTPMessage.Where(z => z.StudentId == studentId && z.ExpTimeStamp >= CurrentTime && z.OTP == otp).OrderByDescending(z => z.OtpId).FirstOrDefault();
            if (otpDetail != null)
            {
                var student = _Entities.tb_Student.Where(z => z.StudentId == studentId).FirstOrDefault();
                if (student != null)
                {
                    student.ParentId = _parentUser.ParentId;
                    status = _Entities.SaveChanges() > 0;
                    message = status ? "Student Added" : "failed";
                    if (status)
                    {
                        otpDetail.IsActive = false;
                        _Entities.SaveChanges();
                    }
                }

            }
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public object AddChildToParent(string id)
        {

            long studentId = Convert.ToInt64(id);
            bool status = false;
            string message = "Failed";
            var student = _Entities.tb_Student.Where(z => z.StudentId == studentId).FirstOrDefault();
            if (student != null)
                student.ParentId = _parentUser.ParentId;
            status = _Entities.SaveChanges() > 0;
            message = status ? "Student Added" : "failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult BillingDetails(string id)
        {
            string[] splitData = id.Split('~');
            long studentId = Convert.ToInt64(splitData[0]);
            string admissionNo = splitData[1];

            var student = new TrackTap.Data.Student(studentId);
            FeeModel model = new FeeModel();
            if (student.StudentSpecialId == admissionNo)
            {
                model.SchoolModel = new SchoolModel();
                model.SchoolModel.studentName = student.StundentName;
                model.SchoolModel.classNumber = student.ClasssNumber; //Archana
                model.SchoolModel.className = student.ClassName;
                model.SchoolModel.division = student.DivisionName;
                model.SchoolModel.classInCharge = student.Teacher == null ? "Not Assigned" : student.Teacher.TeacherName;
                model.SchoolModel.classId = student.ClassId;
                model.SchoolModel.studentId = studentId;
                model.AdmissionNo = student.StudentSpecialId;
            }


            return View(model);
        }

        public PartialViewResult OtpModelView(string id)
        {
            StudentModel model = new StudentModel();
            model.studentId = Convert.ToInt64(id);
            Random generator = new Random();
            String r = generator.Next(0, 999999).ToString("D6");

            DateTime maxTimeOtp = CurrentTime;
            double minuts = 5;
            maxTimeOtp = maxTimeOtp.AddMinutes(minuts);
            var student = _Entities.tb_Student.Where(z => z.StudentId == model.studentId).FirstOrDefault();
            if (student != null)
            {

                var senderName = "MYSCHO";
                //if (student.SchoolId == 10116)
                //{
                //    senderName = "PARDSE";
                //}
                //else if (student.SchoolId == 10117)
                //{
                //    senderName = "HOLYIN";
                //}
                var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == student.SchoolId && x.IsActive == true).FirstOrDefault();
                if (senderData != null)
                    senderName = senderData.SenderId;
                var message = "OTP for SchoolMan - " + r;
                var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + student.ContactNumber + "&route=2&message=" + message + "&sender=" + senderName;
                //  var url = "http://bhashsms.com//api/sendmsg.php?user=srishtitrans&pass=123456&sender=MCHILD&phone=" + phone + "&text=" + item.Description + "&priority=ndnd&stype=normal";

                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest request = this.GetRequest(url);
                WebResponse webResponse = request.GetResponse();
                var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);


            }



            var otp = _Entities.tb_OTPMessage.Create();
            otp.StudentId = model.studentId;
            otp.OTP = r;
            otp.OTPType = 1;
            otp.IsActive = true;
            otp.ExpTimeStamp = maxTimeOtp;
            otp.TimeStamp = CurrentTime;
            _Entities.tb_OTPMessage.Add(otp);
            _Entities.SaveChanges();
            return PartialView("~/Views/Parent/_pv_AddChild_OTP.cshtml", model);
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
        public PartialViewResult StudentHistoryBillPartialView(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id);
            return PartialView("~/Views/Parent/_pv_History_Billing_StudentFee_Model.cshtml", model);
        }
        public PartialViewResult LoadTableForBilling(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            string[] splitData = id.Split('~');
            model.SchoolModel.studentId = Convert.ToInt64(splitData[1]);
            model.BillNumber = Convert.ToInt64(splitData[0]);
            return PartialView("~/Views/Parent/_pv_History_PopupGrid.cshtml", model);
        }
        public IActionResult ChildAcademyPayment()
        {
            return View();
        }

        public IActionResult CoursesOffered()
        {
            return View();
        }

        [HttpPost]
        public object StudentMainBillPay(FeeModel model)
        {
            decimal sumAmt = 0;
            bool status = false;
            string message = "Failed";
            List<string> feeDetails = model.FeeDetails.Split(',').ToList();
            long SchoolId = model.SchoolId;
            long ClassId = model.ClassId;
            long StudentId = model.StudentId;
            DateTime BillDate = CurrentTime;
            decimal TotalAmountPaid = 0;
            if (model.PaidAmount != 0)
            {
                TotalAmountPaid = Convert.ToDecimal(model.PaidAmount);

            }
            Guid PaymentGuid = Guid.NewGuid(); // To find same bill elements
            long BillNo = 1;
            var payment = new tb_Payment();
            var billNo = _Entities.tb_PaymentBillNo.Where(z => z.SchoolId == SchoolId).FirstOrDefault();
            if (billNo != null)
            {
                BillNo = billNo.BillNo + 1;
            }
            else
            {
                var slNoTable = new tb_PaymentBillNo();
                slNoTable.SchoolId = SchoolId;
                slNoTable.BillNo = 1;
                _Entities.tb_PaymentBillNo.Add(slNoTable);
                status = _Entities.SaveChanges() > 0 ? true : false;

            }
            foreach (var fee in feeDetails)
            {
                string[] splitData = fee.Split('^');
                decimal paymentAmount = Convert.ToDecimal(splitData[0]);
                long feeId = Convert.ToInt32(splitData[1]);
                payment.FeeId = feeId;
                payment.FeeGuid = new Guid(splitData[2]);
                decimal maxAmount = Convert.ToDecimal(splitData[3]);
                decimal discount = Convert.ToDecimal(splitData[4]);
                payment.MaxAmount = maxAmount;
                payment.Discount = discount;
                int isAmountEdit = Convert.ToInt16(splitData[5]);
                if (isAmountEdit != 0)
                {
                    var paymentList = new TrackTap.Data.Student(StudentId).GetStudentPaymentFees().OrderBy(z => z.DueDate).ToList();
                    var dueFee = paymentList.Where(z => z.FeeGuid == payment.FeeGuid).FirstOrDefault();
                    if (dueFee != null)
                    {
                        if (dueFee.Amount != paymentAmount)
                        {
                            var due = new tb_FeeDues();
                            due.FeeId = payment.FeeId;
                            decimal amtAfterDiscount = maxAmount - discount;
                            due.Amount = amtAfterDiscount - paymentAmount;
                            due.FeeDuesGuid = Guid.NewGuid();
                            due.StudentId = StudentId;
                            due.IsActive = true;
                            due.DueDate = dueFee.DueDate;
                            due.TimeStamp = BillDate;
                            _Entities.tb_FeeDues.Add(due);
                            // status = _Entities.SaveChanges() > 0 ? true : false;
                        }
                    }
                }

                payment.Amount = paymentAmount;
                sumAmt = sumAmt + payment.Amount;

                payment.BillNo = BillNo;
                payment.IsPaid = false;
                payment.PaymentType = 2;
                payment.PaymentGuid = PaymentGuid;
                payment.StudentId = StudentId;
                payment.ClassId = ClassId;
                payment.SchoolId = SchoolId;
                payment.TimeStamp = BillDate;
                payment.IsActive = true;
                //payment.IssuedPerson = _user.UserId;
                _Entities.tb_Payment.Add(payment);
                status = _Entities.SaveChanges() > 0 ? true : false;

            }
            var billNo1 = _Entities.tb_PaymentBillNo.Where(z => z.SchoolId == SchoolId).FirstOrDefault();
            if (billNo1 != null)
            {
                billNo.BillNo = BillNo;
                status = _Entities.SaveChanges() > 0 ? true : false;

            }
            bool ispayable = false;
            decimal payableAmount = 0;
            decimal prevBal = 0;
            try
            {
                decimal bal = 0;
                //bool balAndCash = false;

                decimal tempSumTotal = 0;
                tempSumTotal = sumAmt;
                if (sumAmt == 0)
                {
                    sumAmt = payment.Amount;
                }
                var balance = _Entities.tb_StudentBalance.Where(z => z.StudentId == StudentId && z.IsActive).FirstOrDefault();
                if (balance != null)
                {
                    prevBal = balance.Amount;
                    bal = balance.Amount;
                    //sumAmt = (bal - sumAmt);
                    if ((prevBal < tempSumTotal) && (prevBal != 0))
                    {
                        ispayable = true;
                        payableAmount = tempSumTotal - prevBal;

                    }
                    //if (sumAmt > tempSumTotal)//check negetive or not 
                    //{
                    //    sumAmt = tempSumTotal;
                    //}
                    //else
                    //{
                    //    if (!ispayable)
                    //    {

                    //    balAndCash = true;
                    //    }
                    //    sumAmt = Math.Abs(sumAmt);
                    //}
                    if (TotalAmountPaid != 0)
                    {
                        var tempBal = TotalAmountPaid - sumAmt;
                        bal = tempBal + prevBal;
                    }
                    else
                    {
                        if (ispayable)
                        {
                            bal = 0;
                        }
                        else
                        {
                            bal = balance.Amount - sumAmt;
                        }
                    }
                    if (bal < 0) //if no balance available (balance.Amount - sumAmt) gets -ve
                    {
                        bal = 0;
                    }
                }
                else
                {
                    if (TotalAmountPaid != 0)
                    {
                        bal = TotalAmountPaid - sumAmt;
                    }
                    else
                    {

                        //bal = Math.Abs(bal - sumAmt);
                        bal = bal - sumAmt;

                    }
                    if (bal < 0)
                    {
                        bal = 0;
                    }
                }
                if (balance != null)
                {
                    try
                    {
                        balance.Amount = bal;
                        _Entities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var messageex = ex.Message;

                    }
                }
                else
                {
                    if (bal != 0)
                    {
                        var studentBalance = new tb_StudentBalance();
                        studentBalance.StudentId = StudentId;
                        studentBalance.Amount = bal;
                        studentBalance.IsActive = true;
                        _Entities.tb_StudentBalance.Add(studentBalance);
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                }
                if ((TotalAmountPaid != 0) || (bal != 0))
                {



                    var studentPaidsAmount = new tb_StudentPaidAmount();
                    studentPaidsAmount.StudentId = StudentId;
                    studentPaidsAmount.PaidAmount = TotalAmountPaid;
                    studentPaidsAmount.PreviousBalance = prevBal;
                    studentPaidsAmount.BalanceAmount = bal;
                    studentPaidsAmount.BillNo = BillNo;
                    studentPaidsAmount.IsActive = true;
                    studentPaidsAmount.AddAccountStatus = false;
                    _Entities.tb_StudentPaidAmount.Add(studentPaidsAmount);
                    status = _Entities.SaveChanges() > 0 ? true : false;



                }
                //else if (bal != 0)
                //{
                //    var studentPaidsAmount = new tb_StudentPaidAmount();
                //    studentPaidsAmount.StudentId = StudentId;
                //    studentPaidsAmount.PaidAmount = TotalAmountPaid;
                //    studentPaidsAmount.PreviousBalance = prevBal;
                //    studentPaidsAmount.BalanceAmount = bal;
                //    studentPaidsAmount.BillNo = BillNo;
                //    studentPaidsAmount.IsActive = true;
                //    _Entities.tb_StudentPaidAmount.Add(studentPaidsAmount);
                //    status = _Entities.SaveChanges() > 0 ? true : false;
                //}
                if (ispayable)
                {
                    var studentPaidsAmount = new tb_StudentPaidAmount();
                    studentPaidsAmount.StudentId = StudentId;
                    studentPaidsAmount.PaidAmount = payableAmount;
                    studentPaidsAmount.PreviousBalance = prevBal;
                    studentPaidsAmount.BalanceAmount = bal;
                    studentPaidsAmount.BillNo = BillNo;
                    studentPaidsAmount.IsActive = true;
                    studentPaidsAmount.AddAccountStatus = false;
                    _Entities.tb_StudentPaidAmount.Add(studentPaidsAmount);
                    status = _Entities.SaveChanges() > 0 ? true : false;
                }

            }
            catch (Exception ex)
            {


            }

            var studDetails = _Entities.tb_Student.Where(z => z.StudentId == StudentId && z.IsActive == true).FirstOrDefault();
            var dateTime = BillDate.ToString("dd-MMM-yyyy");

            var description = "failed";
            bool isGateway = false;
            if (ispayable || (prevBal == 0))
            {
                var amountTopay = payableAmount == 0 ? sumAmt : payableAmount;
                isGateway = true;
                Guid guid = Guid.NewGuid();
                string reference = guid.ToString().Replace("-", string.Empty).Substring(0, 10).ToUpper();
                Session["REFERENCE"] = reference;
                //var payment = Entities.tb_Payment.Where(x => x.UserId == _user.UserId && x.PaymentType == 1).FirstOrDefault();
                PaymentModels pay = new PaymentModels();
                pay.ReferenceNo = Session["REFERENCE"].ToString();
                var student = _Entities.tb_Parent.Where(z => z.ParentId == _parentUser.ParentId).FirstOrDefault();
                pay.Description = "Payment";
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                pay.ReturnUrl = baseUrl + "/Parent/ccavResponseHandler";
                //pay.ReturnUrl = "http://localhost:16138/Parent/ccavResponseHandler";
                pay.Name = student.ParentName;
                pay.Address = student.Address;
                pay.City = student.City;
                pay.State = student.State;
                pay.PostalCode = student.PostalCode;
                pay.PhoneNo = student.ContactNumber;
                pay.Email = student.Email;
                pay.Amount = Convert.ToDouble(amountTopay);
                pay.CourseName = "Bill";
                pay.BillNo = BillNo;
                pay.SchoolId = SchoolId;
                pay.StudentId = StudentId;
                Session["PaymentPostData"] = pay;
            }
            //try
            //{
            //    var smtpDetails = _Entities.tb_SMTPDetail.Where(z => z.SchoolId == SchoolId).FirstOrDefault();

            //    var paidAmount = Convert.ToInt32(payment.Amount);
            //    var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/email/FeePayment.html");
            //    var emailTemplate = System.IO.File.ReadAllText(filePath);
            //    var mailBody = emailTemplate.Replace("{{schoolname}}", studDetails.tb_School.SchoolName)
            //       .Replace("{{parent}}", studDetails.ParentName)
            //    .Replace("{{student}}", studDetails.StundentName)
            //    .Replace("{{amount}}", string.Format("{0:0.00}", sumAmt))
            //    .Replace("{{date}}", dateTime);
            //    Mail.Send("School Fee Payment", mailBody, studDetails.ParentName, smtpDetails.EmailId, smtpDetails.Password, new System.Collections.ArrayList { studDetails.ParentEmail });


            //    description = "success";

            //}
            //catch
            //{

            //    description = "Something went wrong";
            //}
            return Json(new { status = status, serialNo = BillNo, payment = isGateway, msg = status ? "Bill Paid Sucessfully" : "Failed To Pay Bill" }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PrintAccountBillData(string id)
        {
            string[] splitData = id.Split('~');
            var model = new PrintBill();
            model.studentId = Convert.ToInt64(splitData[0]);
            model.billNumber = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/Parent/_pv_PrintAccountBillData.cshtml", model);
        }
        #region PaymentGateway
        public IActionResult CoursePayment(string id)
        {

            bool status = true;
            int caseSwitch = Convert.ToInt16(id);

            float amount = 1;
            string course = "";
            switch (caseSwitch)
            {
                case 1:
                    amount = 24000;
                    course = "Core Php with responsive web";
                    break;
                case 2:
                    amount = 22000;
                    course = "Dotnet";
                    break;
                case 3:
                    amount = 10000;
                    course = "Java";
                    break;
                case 4:
                    amount = 24000;
                    course = "Android";
                    break;
                case 5:
                    amount = 23000;
                    course = "Ios";
                    break;
                case 6:
                    amount = 230;
                    course = "ionic";
                    break;
                default:
                    status = false;
                    break;
            }
            Guid guid = Guid.NewGuid();
            string reference = guid.ToString().Replace("-", string.Empty).Substring(0, 10).ToUpper();
            Session["REFERENCE"] = reference;
            //var payment = Entities.tb_Payment.Where(x => x.UserId == _user.UserId && x.PaymentType == 1).FirstOrDefault();
            PaymentModels pay = new PaymentModels();
            pay.ReferenceNo = Session["REFERENCE"].ToString();
            var student = _Entities.tb_Parent.Where(z => z.ParentId == _parentUser.ParentId).FirstOrDefault();
            pay.Description = "Payment";
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            pay.ReturnUrl = baseUrl + "/Parent/ccavResponseHandler";
            //pay.ReturnUrl = "http://localhost:16138/Parent/ccavResponseHandler";
            pay.Name = student.ParentName;
            pay.Address = student.Address;
            pay.City = student.City;
            pay.State = student.State;
            pay.PostalCode = student.PostalCode;
            pay.PhoneNo = student.ContactNumber;
            pay.Email = student.Email;
            pay.Amount = amount;
            pay.CourseName = course;
            pay.BillNo = 11;
            pay.SchoolId = 1;
            pay.StudentId = 1;
            Session["PaymentPostData"] = pay;
            //return View("CCAVRequestHandler",pay);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult PaymentPost()
        {
            PaymentModels model = new PaymentModels();
            PaymentModels pay = new PaymentModels();
            if (Session["REFERENCE"] != null)
            {
                model.ReferenceNo = Session["REFERENCE"].ToString();
            }
            model.Amount = pay.Amount;
            model.Description = "Payment";
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            //model.ReturnUrl = "http://localhost:16138/Student/PaymentResponse";
            pay = (PaymentModels)Session["PaymentPostData"];
            model.ReturnUrl = pay.ReturnUrl; //http://localhost:16138/Parent/PaymentResponse";//"http://localhost:16138/Parent/ccavResponseHandler";
            model.Name = pay.Name;
            model.Address = pay.Address;
            model.City = pay.City;
            model.State = pay.State;
            model.PostalCode = pay.PostalCode;
            model.PhoneNo = pay.PhoneNo;
            model.Email = pay.Email;
            model.Amount = pay.Amount;
            return View(model);
        }
        public IActionResult CCAVRequestHandler()
        {

            PaymentModels pay = new PaymentModels();
            CCACrypto ccaCrypto = new CCACrypto();
            string workingKey = "3891AA5249F6E3DBA928422EB4BA18DD";//put in the 32bit alpha numeric key in the quotes provided here 	
            string ccaRequest = "";
            string strEncRequest = "";
            string iframeSrc = "";
            string strAccessCode = "AVTM75FA75BW58MTWB";// put the access key in the quotes provided here.


            NameValueCollection nameValue = (Request.Form.Count > 0) ? Request.Form : Request.QueryString;
            SortedDictionary<string, string> sortedDict = NameValueCreator.SortNameValueCollection(nameValue);

            foreach (KeyValuePair<string, string> p in sortedDict)
            {
                if (p.Key != null)
                {
                    if (!p.Key.StartsWith("_"))
                    {
                        ccaRequest = ccaRequest + p.Key + "=" + p.Value + "&";
                        /* Response.Write(name + "=" + Request.Form[name]);
                          Response.Write("</br>");*/
                    }
                }
            }
            strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
            iframeSrc = "https://secure.ccavenue.com/transaction/transaction.do?command=initiateTransaction&encRequest=" + strEncRequest + "&access_code=" + strAccessCode;
            pay.iframeSrc = iframeSrc;
            //pay.strEncRequest = strEncRequest;
            //pay.strAccessCode = strAccessCode;
            return View(pay);
        }
        public IActionResult ccavResponseHandler()
        {
            bool status = false;
            PaymentModels model = new PaymentModels();
            PaymentModels pay = new PaymentModels();
            pay = (PaymentModels)Session["PaymentPostData"];

            string workingKey = "3891AA5249F6E3DBA928422EB4BA18DD";//put in the 32bit alpha numeric key in the quotes provided here
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(Request.Form["encResp"], workingKey);
            NameValueCollection Params = new NameValueCollection();
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    Params.Add(Key, Value);
                }
            }
            var ccavenueTable = new tb_CcavenueCourseResponse();
            var amt = "";
            bool isSuccess = false;
            for (int i = 0; i < Params.Count; i++)
            {
                if (Params.Keys[i] == "order_id")
                {
                    ccavenueTable.OrderId = Params[i];
                }
                else if (Params.Keys[i] == "order_status")
                {
                    ccavenueTable.OrderStatus = Params[i] == "Failure" ? false : true;
                    model.PaymentStatus = Params[i];
                    isSuccess = Params[i] == "Failure" ? false : true;
                }
                else if (Params.Keys[i] == "payment_mode")
                {
                    ccavenueTable.PaymentMode = Params[i];
                }
                else if (Params.Keys[i] == "tracking_id ")
                {
                    ccavenueTable.TrackingId = Params[i];
                }
                else if (Params.Keys[i] == "amount")
                {
                    ccavenueTable.Amount = Convert.ToDecimal(Params[i]);
                    amt = Params[i];
                }
                // Response.Write(Params.Keys[i] + " = " + Params[i] + "<br>");
            }
            ccavenueTable.ParentId = _parentUser.ParentId;
            ccavenueTable.Course = pay.CourseName;
            ccavenueTable.BillNo = pay.BillNo;
            ccavenueTable.SchoolId = pay.SchoolId;

            // ccavenueTable.Amount = Convert.ToDecimal(pay.Amount);
            _Entities.tb_CcavenueCourseResponse.Add(ccavenueTable);
            _Entities.SaveChanges();
            if (isSuccess)
            {

                var paymentList = _Entities.tb_Payment.Where(z => z.StudentId == pay.StudentId && z.BillNo == pay.BillNo).ToList();
                foreach (var item in paymentList)
                {
                    item.IsPaid = true;
                    _Entities.SaveChanges();
                }

                try
                {
                    var history = new tb_SmsHistory();
                    var numbers = new List<string>();
                    var MsgId = new List<string>();

                    var numb = "";
                    string messagepre = "";


                    var senderName = "MYSCHO";

                    var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == pay.SchoolId && x.IsActive == true).FirstOrDefault();
                    if (senderData != null)
                        senderName = senderData.SenderId;
                    status = true;

                    var smsHead = new tb_SmsHead();
                    smsHead.Head = "BillDate Payment " + paymentList.FirstOrDefault().tb_Student.StundentName;
                    smsHead.SchoolId = _user.SchoolId;
                    smsHead.TimeStamp = CurrentTime;
                    smsHead.IsActive = true;
                    smsHead.SenderType = (int)SMSSendType.Student;
                    _Entities.tb_SmsHead.Add(smsHead);
                    status = _Entities.SaveChanges() > 0;


                    messagepre = "Dear Parent of " + paymentList.FirstOrDefault().tb_Student.StundentName + ", you have paid Rs." + string.Format("{0:0.00}", amt) + " on " + CurrentTime;

                    var phone = paymentList.FirstOrDefault().tb_Student.ContactNumber.ToString();
                    int length = messagepre.Length;
                    int que = length / 160;
                    int rem = length % 160;
                    if (rem > 0)
                        que++;
                    int smsCount = que;
                    var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + messagepre + "&sender=" + senderName;
                    //  var url = "http://bhashsms.com//api/sendmsg.php?user=srishtitrans&pass=123456&sender=MCHILD&phone=" + phone + "&text=" + item.Description + "&priority=ndnd&stype=normal";

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
                        sms.StuentId = pay.StudentId;
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
                catch (Exception ex)
                {
                    var x = ex.InnerException;
                }

            }
            else
            {
                var studentPaidAmount = _Entities.tb_StudentPaidAmount.Where(z => z.StudentId == pay.StudentId && z.BillNo == pay.BillNo).FirstOrDefault();
                if (studentPaidAmount != null)
                {
                    studentPaidAmount.IsActive = false;
                    studentPaidAmount.AddAccountStatus = false;
                    var studentBalance = _Entities.tb_StudentBalance.Where(z => z.StudentId == pay.StudentId).FirstOrDefault();
                    if (studentBalance != null)
                    {
                        studentBalance.Amount = studentPaidAmount.PreviousBalance ?? 0;
                    }
                    status = _Entities.SaveChanges() > 0 ? true : false;

                }

            }
            double amnt = Convert.ToDouble(amt);
            if (amnt != pay.Amount)
            {
                model.PaymentStatus = "Fraud";
            }

            model.Amount = Convert.ToDouble(amt);
            model.CourseName = pay.CourseName;
            model.StudentId = pay.StudentId;
            model.BillNo = pay.BillNo;
            return View(model);
        }


        #endregion



        #region Message
        public IActionResult Messages()
        {
            ParentRegisterModel model = new ParentRegisterModel();
            model.parentId = _parentUser.ParentId;
            return View(model);
        }
        public PartialViewResult MessageSectionView(string id)
        {
           StudentModel model = new StudentModel();
            model.studentId = Convert.ToInt64(id); 
            return PartialView("~/Views/Parent/_pv_Message_View.cshtml", model);
        }

        public PartialViewResult MessageSectionInnerView(string id)
        {
            TrackTap.MapModel.ParentTeacherConversationMapModel model = new TrackTap.MapModel.ParentTeacherConversationMapModel();
            string[] splitData = id.Split('~');
            model.length = Convert.ToInt32(splitData[0]);
            model.StudentId = splitData[1];
            model.start = 0;
            return PartialView("~/Views/Parent/_pv_Message_Inner_View.cshtml", model);
        }




        #endregion
        public IActionResult Dummy()
        {
            return View();
        }
    }
}
