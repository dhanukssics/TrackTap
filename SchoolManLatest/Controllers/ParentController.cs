using CCA.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Web;
using TrackTap.Helper;
using TrackTap.Models;
using TrackTap.Repository;
using TrackTap.Utility;
namespace TrackTap.Controllers
{
    public class ParentController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ParentController(SchoolDbContext Entities, SchoolRepository schoolRepository, ParentRepository parentRepository, TeacherRepository teacherRepository,HttpClient httpClient, IConfiguration configuration) : base(Entities, schoolRepository, parentRepository, teacherRepository)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        //
        // GET: /Parent/

        public IActionResult Home()
        {
            ParentRegisterModel model = new ParentRegisterModel();
            model.parentId = _parentUser.ParentId;
            return View(model);
        }
        public IActionResult AddChildView()
        {
            return PartialView(
                "~/Views/Parent/_pv_AddChildModel.cshtml");
        }
        public IActionResult SearchAdmission(string id)
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
        public IActionResult AttendancePartial(string id)
        {
            string[] splitData = id.Split('~');
            int monthInDigit = DateTime.ParseExact(splitData[1], "MMMM", CultureInfo.InvariantCulture).Month;
            AttendanceModels model = new AttendanceModels();
            model.studentId= Convert.ToInt64(splitData[0]);
            model.month = Convert.ToInt16(monthInDigit);
            model.year = Convert.ToInt16(splitData[2]); 
            return PartialView("~/Views/Parent/_pv_Attendance_Grid.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> OtpSubmit(string id)
        {
            bool status = false;

            string message = "Wrong OTP";

            try
            {
                string[] splitData =
                    id.Split('~');

                long studentId =
                    Convert.ToInt64(
                        splitData[0]);

                string otp =
                    splitData[1];

                var otpDetail = await _Entities
                    .TbOtpmessages
                    .Where(x =>
                        x.StudentId ==
                            studentId

                        && x.ExpTimeStamp >=
                            CurrentTime

                        && x.Otp == otp

                        && x.IsActive)
                    .OrderByDescending(x =>
                        x.OtpId)
                    .FirstOrDefaultAsync();

                if (otpDetail != null)
                {
                    var student = await _Entities
                        .TbStudents
                        .FirstOrDefaultAsync(x =>
                            x.StudentId ==
                                studentId);

                    if (student != null)
                    {
                        student.ParentId =
                            _parentUser.ParentId;

                        otpDetail.IsActive =
                            false;

                        status =
                            await _Entities
                                .SaveChangesAsync() > 0;

                        message = status
                            ? "Student Added"
                            : "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                status = status,
                msg = message
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddChildToParent(long id)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                var student = await _Entities
                    .TbStudents
                    .FirstOrDefaultAsync(x =>
                        x.StudentId == id);

                if (student == null)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "Student not found"
                    });
                }

                student.ParentId =
                    _parentUser.ParentId;

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                message = status
                    ? "Student Added"
                    : "Failed";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                status = status,
                msg = message
            });
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

        public async Task<IActionResult> OtpModelView(long id)
        {
            var model =
                new StudentModel
                {
                    studentId = id
                };

            // Generate 6 digit OTP
            string otpCode =
                Random.Shared
                    .Next(0, 999999)
                    .ToString("D6");

            DateTime maxTimeOtp =
                CurrentTime.AddMinutes(5);

            var student = await _Entities
                .TbStudents
                .FirstOrDefaultAsync(x =>
                    x.StudentId == id);

            if (student != null)
            {
                string senderName = "MYSCHO";

                var senderData = await _Entities
                    .TbSchoolSenderIds
                    .FirstOrDefaultAsync(x =>
                        x.SchoolId ==
                            student.SchoolId

                        && x.IsActive==true);

                if (senderData != null)
                {
                    senderName =
                        senderData.SenderId;
                }

                string message =
                    $"OTP for SchoolMan - {otpCode}";

                string url =
                    "http://alvosms.in/api/v1/send"
                    + "?token=ivku4o2r6gjdq98bm3aesl50pyz7h1"
                    + $"&numbers={student.ContactNumber}"
                    + "&route=2"
                    + $"&message={Uri.EscapeDataString(message)}"
                    + $"&sender={senderName}";

                try
                {
                    string responseText =
                        await _httpClient
                            .GetStringAsync(url);

                    var respList =
                        JsonSerializer.Deserialize<alvosmsResp>(
                            responseText);
                }
                catch
                {
                    // SMS sending failed
                }
            }

            var otp =
                new TbOtpmessage
                {
                    StudentId =
                        model.studentId,

                    Otp =
                        otpCode,

                    Otptype = 1,

                    IsActive = true,

                    ExpTimeStamp =
                        maxTimeOtp,

                    TimeStamp =
                        CurrentTime
                };

            await _Entities
                .TbOtpmessages
                .AddAsync(otp);

            await _Entities
                .SaveChangesAsync();

            return PartialView(
                "~/Views/Parent/_pv_AddChild_OTP.cshtml",
                model);
        }
        private async Task<string> GetRequest(string url)
        {
            _httpClient.DefaultRequestHeaders
                .UserAgent
                .ParseAdd(
                    "Mozilla/5.0");

            _httpClient.Timeout =
                TimeSpan.FromMinutes(5);

            var response =
                await _httpClient
                    .GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadAsStringAsync();
        }
        public IActionResult StudentHistoryBillPartialView(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id);
            return PartialView("~/Views/Parent/_pv_History_Billing_StudentFee_Model.cshtml", model);
        }
        public IActionResult LoadTableForBilling(string id)
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
        public async Task<IActionResult> StudentMainBillPay(FeeModel model)
        {
            using var transaction =
                await _Entities.Database
                    .BeginTransactionAsync();

            try
            {
                decimal sumAmt = 0;

                bool status = false;

                List<string> feeDetails =
                    model.FeeDetails
                        .Split(',')
                        .ToList();

                long schoolId = model.SchoolId;

                long classId = model.ClassId;

                long studentId = model.StudentId;

                DateTime billDate = CurrentTime;

                decimal totalAmountPaid =
                    model.PaidAmount;

                Guid paymentGuid =
                    Guid.NewGuid();

                long billNo = 1;

                var billNoData = await _Entities
                    .TbPaymentBillNos
                    .FirstOrDefaultAsync(x =>
                        x.SchoolId == schoolId);

                if (billNoData != null)
                {
                    billNo =
                        billNoData.BillNo + 1;

                    billNoData.BillNo =
                        billNo;
                }
                else
                {
                    var slNoTable =
                        new TbPaymentBillNo
                        {
                            SchoolId = schoolId,
                            BillNo = 1
                        };

                    await _Entities
                        .TbPaymentBillNos
                        .AddAsync(slNoTable);

                    billNo = 1;
                }

                foreach (var fee in feeDetails)
                {
                    string[] splitData =
                        fee.Split('^');

                    decimal paymentAmount =
                        Convert.ToDecimal(
                            splitData[0]);

                    long feeId =
                        Convert.ToInt64(
                            splitData[1]);

                    Guid feeGuid =
                        new Guid(splitData[2]);

                    decimal maxAmount =
                        Convert.ToDecimal(
                            splitData[3]);

                    decimal discount =
                        Convert.ToDecimal(
                            splitData[4]);

                    decimal finalAmount =
                        maxAmount - discount;

                    sumAmt += paymentAmount;

                    var payment =
                        new TbPayment
                        {
                            FeeId = feeId,

                            FeeGuid = feeGuid,

                            MaxAmount = maxAmount,

                            Discount = discount,

                            Amount = paymentAmount,

                            BillNo = billNo,

                            IsPaid = false,

                            PaymentType = 2,

                            PaymentGuid = paymentGuid,

                            StudentId = studentId,

                            ClassId = classId,

                            SchoolId = schoolId,

                            TimeStamp = billDate,

                            IsActive = true
                        };

                    await _Entities
                        .TbPayments
                        .AddAsync(payment);

                    int isAmountEdit =
                        Convert.ToInt32(
                            splitData[5]);

                    if (isAmountEdit != 0
                        && finalAmount != paymentAmount)
                    {
                        var due =
                            new TbFeeDue
                            {
                                FeeId = feeId,

                                Amount =
                                    finalAmount
                                    - paymentAmount,

                                FeeDuesGuid =
                                    Guid.NewGuid(),

                                StudentId =
                                    studentId,

                                IsActive = true,

                                DueDate =
                                    billDate,

                                TimeStamp =
                                    billDate
                            };

                        await _Entities
                            .TbFeeDues
                            .AddAsync(due);
                    }
                }

                await _Entities
                    .SaveChangesAsync();

                await transaction
                    .CommitAsync();

                return Json(new
                {
                    status = true,
                    serialNo = billNo,
                    payment = true,
                    msg = "Bill Paid Successfully"
                });
            }
            catch (Exception ex)
            {
                await transaction
                    .RollbackAsync();

                return Json(new
                {
                    status = false,
                    msg = ex.Message
                });
            }
        }
        public IActionResult PrintAccountBillData(string id)
        {
            string[] splitData = id.Split('~');
            var model = new PrintBill();
            model.studentId = Convert.ToInt64(splitData[0]);
            model.billNumber = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/Parent/_pv_PrintAccountBillData.cshtml", model);
        }
        #region PaymentGateway
        public async Task<IActionResult> CoursePayment(int id)
        {
            bool status = true;

            decimal amount = 0;

            string course = "";

            switch (id)
            {
                case 1:
                    amount = 24000;
                    course = "Core Php with Responsive Web";
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
                    course = "Ionic";
                    break;

                default:
                    status = false;
                    break;
            }

            if (!status)
            {
                return Json(new
                {
                    status = false,
                    msg = "Invalid course"
                });
            }

            Guid guid =
                Guid.NewGuid();

            string reference =
                guid.ToString()
                    .Replace("-", "")
                    .Substring(0, 10)
                    .ToUpper();

            HttpContext.Session.SetString(
                "REFERENCE",
                reference);

            var student = await _Entities
                .TbParents
                .FirstOrDefaultAsync(x =>
                    x.ParentId ==
                        _parentUser.ParentId);

            if (student == null)
            {
                return Json(new
                {
                    status = false,
                    msg = "Parent not found"
                });
            }

            string baseUrl =
                $"{Request.Scheme}://{Request.Host}";

            var pay =
                new PaymentModels
                {
                    ReferenceNo =
                        reference,

                    Description =
                        "Payment",

                    ReturnUrl =
                        $"{baseUrl}/Parent/ccavResponseHandler",

                    Name =
                        student.ParentName,

                    Address =
                        student.Address,

                    City =
                        student.City,

                    State =
                        student.State,

                    PostalCode =
                        student.PostalCode,

                    PhoneNo =
                        student.ContactNumber,

                    Email =
                        student.Email,

                    Amount =
                        Convert.ToDouble(amount),

                    CourseName =
                        course,

                    BillNo = 11,

                    SchoolId = 1,

                    StudentId = 1
                };

            HttpContext.Session.SetString(
                "PaymentReference",
                pay.ReferenceNo);

            return Json(new
            {
                status = true,
                payment = pay
            });
        }
        public IActionResult PaymentPost()
        {
            var model =
                new PaymentModels();

            string? reference =
                HttpContext.Session
                    .GetString("REFERENCE");

            if (!string.IsNullOrEmpty(reference))
            {
                model.ReferenceNo =
                    reference;
            }

            string? paymentData =
                HttpContext.Session
                    .GetString("PaymentPostData");

            if (string.IsNullOrEmpty(paymentData))
            {
                return RedirectToAction(
                    "CoursePayment");
            }

            var pay =
                JsonSerializer.Deserialize<PaymentModels>(
                    paymentData);

            if (pay == null)
            {
                return RedirectToAction(
                    "CoursePayment");
            }

            model.Description =
                "Payment";

            model.ReturnUrl =
                pay.ReturnUrl;

            model.Name =
                pay.Name;

            model.Address =
                pay.Address;

            model.City =
                pay.City;

            model.State =
                pay.State;

            model.PostalCode =
                pay.PostalCode;

            model.PhoneNo =
                pay.PhoneNo;

            model.Email =
                pay.Email;

            model.Amount =
                pay.Amount;

            model.CourseName =
                pay.CourseName;

            model.BillNo =
                pay.BillNo;

            model.SchoolId =
                pay.SchoolId;

            model.StudentId =
                pay.StudentId;

            return View(model);
        }
        [HttpPost]
        public IActionResult CCAVRequestHandler()
        {
            var pay =
                new PaymentModels();

            var ccaCrypto =
                new CCACrypto();

            string workingKey =
                _configuration["CCAvenue:WorkingKey"];

            string accessCode =
                _configuration["CCAvenue:AccessCode"];

            string ccaRequest = "";

            string strEncRequest = "";

            string iframeSrc = "";

            var sortedDict =
                Request.HasFormContentType
                    ? Request.Form
                        .OrderBy(x => x.Key)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value.ToString())
                    : Request.Query
                        .OrderBy(x => x.Key)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value.ToString());

            foreach (var item in sortedDict)
            {
                if (!string.IsNullOrEmpty(item.Key)
                    && !item.Key.StartsWith("_"))
                {
                    ccaRequest +=
                        $"{item.Key}={item.Value}&";
                }
            }

            strEncRequest =
                ccaCrypto.Encrypt(
                    ccaRequest,
                    workingKey);

            iframeSrc =
                "https://secure.ccavenue.com/transaction/transaction.do"
                + "?command=initiateTransaction"
                + $"&encRequest={strEncRequest}"
                + $"&access_code={accessCode}";

            pay.iframeSrc =
                iframeSrc;

            return View(pay);
        }
        [HttpPost]
        public async Task<IActionResult> ccavResponseHandler()
        {
            bool status = false;

            var model =
                new PaymentModels();

            string? paymentData =
                HttpContext.Session
                    .GetString("PaymentPostData");

            if (string.IsNullOrEmpty(paymentData))
            {
                return RedirectToAction(
                    "CoursePayment");
            }

            var pay =
                JsonSerializer.Deserialize<PaymentModels>(
                    paymentData);

            if (pay == null)
            {
                return RedirectToAction(
                    "CoursePayment");
            }

            string workingKey =
                _configuration["CCAvenue:WorkingKey"];

            var ccaCrypto =
                new CCACrypto();

            string encResponse =
                Request.Form["encResp"];

            string decryptedResponse =
                ccaCrypto.Decrypt(
                    encResponse,
                    workingKey);

            var responseData =
                decryptedResponse
                    .Split('&',
                        StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split('='))
                    .Where(x => x.Length == 2)
                    .ToDictionary(
                        x => x[0].Trim(),
                        x => x[1].Trim());

            var ccavenueTable =
                new TbCcavenueCourseResponse();

            bool isSuccess = false;

            decimal amount = 0;

            if (responseData.ContainsKey("order_id"))
            {
                ccavenueTable.OrderId =
                    responseData["order_id"];
            }

            if (responseData.ContainsKey("order_status"))
            {
                string orderStatus =
                    responseData["order_status"];

                ccavenueTable.OrderStatus =
                    orderStatus != "Failure";

                model.PaymentStatus =
                    orderStatus;

                isSuccess =
                    orderStatus != "Failure";
            }

            if (responseData.ContainsKey("payment_mode"))
            {
                ccavenueTable.PaymentMode =
                    responseData["payment_mode"];
            }

            if (responseData.ContainsKey("tracking_id"))
            {
                ccavenueTable.TrackingId =
                    responseData["tracking_id"];
            }

            if (responseData.ContainsKey("amount"))
            {
                decimal.TryParse(
                    responseData["amount"],
                    out amount);

                ccavenueTable.Amount =
                    amount;
            }

            ccavenueTable.ParentId =
                _parentUser.ParentId;

            ccavenueTable.Course =
                pay.CourseName;

            ccavenueTable.BillNo =
                pay.BillNo;

            ccavenueTable.SchoolId =
                pay.SchoolId;

            await _Entities
                .TbCcavenueCourseResponses
                .AddAsync(ccavenueTable);

            using var transaction =
                await _Entities.Database
                    .BeginTransactionAsync();

            try
            {
                if (isSuccess)
                {
                    var paymentList =
                        await _Entities
                            .TbPayments
                            .Include(x => x.Student)
                            .Where(x =>
                                x.StudentId ==
                                    pay.StudentId

                                && x.BillNo ==
                                    pay.BillNo)
                            .ToListAsync();

                    foreach (var item in paymentList)
                    {
                        item.IsPaid = true;
                    }

                    await _Entities
                        .SaveChangesAsync();

                    try
                    {
                        var student =
                            paymentList
                                .FirstOrDefault()
                                ?.Student;

                        if (student != null)
                        {
                            string senderName =
                                "MYSCHO";

                            var senderData =
                                await _Entities
                                    .TbSchoolSenderIds
                                    .FirstOrDefaultAsync(x =>
                                        x.SchoolId ==
                                            pay.SchoolId
                                        && x.IsActive==true);

                            if (senderData != null)
                            {
                                senderName =
                                    senderData.SenderId;
                            }

                            string message =
                                $"Dear Parent of {student.StundentName}, you have paid Rs.{amount:0.00} on {CurrentTime}";

                            string url =
                                "http://alvosms.in/api/v1/send"
                                + "?token="
                                + _configuration["SmsSettings:Token"]
                                + $"&numbers={student.ContactNumber}"
                                + "&route=2"
                                + $"&message={Uri.EscapeDataString(message)}"
                                + $"&sender={senderName}";

                            string responseText =
                                await _httpClient
                                    .GetStringAsync(url);

                            var respList =
                                JsonSerializer.Deserialize<alvosmsResp>(
                                    responseText);

                            var smsHead =
                                new TbSmsHead
                                {
                                    Head =
                                        $"BillDate Payment {student.StundentName}",

                                    SchoolId =
                                        _user.SchoolId,

                                    TimeStamp =
                                        CurrentTime,

                                    IsActive = true,

                                    SenderType =
                                        (int)SMSSendType.Student
                                };

                            await _Entities
                                .TbSmsHeads
                                .AddAsync(smsHead);

                            await _Entities
                                .SaveChangesAsync();

                            var sms =
                                new TbSmsHistory
                                {
                                    IsActive = true,

                                    MessageContent =
                                        message,

                                    MessageDate =
                                        CurrentTime,

                                    ScholId =
                                        _user.SchoolId,

                                    StuentId =
                                        pay.StudentId,

                                    MobileNumber =
                                        student.ContactNumber,

                                    HeadId =
                                        smsHead.HeadId,

                                    SendStatus =
                                        Convert.ToString(
                                            respList?.success),

                                    SmsSentPerStudent = 1
                                };

                            if (respList?.data != null
                                && respList.data.Count > 0)
                            {
                                sms.MessageReturnId =
                                    respList.data[0]
                                        .messageId;

                                sms.DelivaryStatus =
                                    "Pending";
                            }

                            await _Entities
                                .TbSmsHistories
                                .AddAsync(sms);

                            await _Entities
                                .SaveChangesAsync();
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    var studentPaidAmount =
                        await _Entities
                            .TbStudentPaidAmounts
                            .FirstOrDefaultAsync(x =>
                                x.StudentId ==
                                    pay.StudentId

                                && x.BillNo ==
                                    pay.BillNo);

                    if (studentPaidAmount != null)
                    {
                        studentPaidAmount.IsActive =
                            false;

                        studentPaidAmount.AddAccountStatus =
                            false;

                        var studentBalance =
                            await _Entities
                                .TbStudentBalances
                                .FirstOrDefaultAsync(x =>
                                    x.StudentId ==
                                        pay.StudentId);

                        if (studentBalance != null)
                        {
                            studentBalance.Amount =
                                studentPaidAmount
                                    .PreviousBalance ?? 0;
                        }

                        await _Entities
                            .SaveChangesAsync();
                    }
                }

                await transaction
                    .CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction
                    .RollbackAsync();

                model.PaymentStatus =
                    "Error";
            }

            if (amount != Convert.ToDecimal(pay.Amount))
            {
                model.PaymentStatus =
                    "Fraud";
            }

            model.Amount =
                Convert.ToDouble(amount);

            model.CourseName =
                pay.CourseName;

            model.StudentId =
                pay.StudentId;

            model.BillNo =
                pay.BillNo;

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
        public IActionResult MessageSectionView(string id)
        {
           StudentModel model = new StudentModel();
            model.studentId = Convert.ToInt64(id); 
            return PartialView("~/Views/Parent/_pv_Message_View.cshtml", model);
        }

        public IActionResult MessageSectionInnerView(string id)
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
