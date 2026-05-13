using Azure.Core;
using CCA.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Specialized;

using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.Json;

using System.Xml.Linq;
using TrackTap.ClassLibrary;
using TrackTap.ClassLibrary.Utility;
using TrackTap.Data;
using TrackTap.Helper;
using TrackTap.Models;

using TrackTap.PostModel;
using TrackTap.Repository;


//using CCA.Util;
//using System.Web.Helpers;

namespace TrackTap.Controllers
{
    public class AccountController : PreLoginController
    {
        //
        // GET: /Account/
        public DateTime currentTime = DateTime.UtcNow;
        private readonly SchoolDbContext _Entities;
        private readonly SchoolRepository _schoolRepository;
        private readonly IWebHostEnvironment _environment;

        public AccountController(SchoolDbContext Entities, IWebHostEnvironment environment, SchoolRepository schoolRepository)
        {
            _Entities = Entities;
            _environment = environment;
            _schoolRepository = schoolRepository;
        }
        public IActionResult LoginPage()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult SchoolLogin()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Disclaimer()
        {
            return View();
        }
        public IActionResult Privacypolicy()
        {
            return View();
        }
        public IActionResult TermsConditions()
        {
            return View("Terms&Conditions");
        }
        public IActionResult SuperAdminLogin()
        {
            return View();
        }
        public IActionResult LoginParent()
        {
            return View();
        }
        public IActionResult ParentLogin()
        {
            return View();
        }
        public IActionResult ParentRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChkSuperAdminLogin(LoginModel model)
        {
            try
            {
                var user = await _Entities.TbLoginAdmins
                    .FirstOrDefaultAsync(x =>
                        x.UserName.ToLower() == model.Email.ToLower() &&
                        x.Password == model.Password &&
                        x.IsActive);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AdminId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "SuperAdmin")
            };

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    HttpContext.Session.SetString(
                        "User",
                        user.AdminId.ToString());

                    return Json(new
                    {
                        status = true,
                        msg = "Success"
                    });
                }

                return Json(new
                {
                    status = false,
                    msg = "Username/Password incorrect"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    msg = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ParentRegister(ParentRegisterModel model)
        {
            bool status = false;
            string message = "Failed";

            try
            {
                bool emailExists = await _Entities.TbParents
                    .AnyAsync(x =>
                        x.Email.ToLower() == model.email.ToLower() &&
                        x.IsActive);

                if (emailExists)
                {
                    return Json(new
                    {
                        Status = false,
                        Message = "Email Already Exists"
                    });
                }

                var newModel = new ParentRegistrationPostModel
                {
                    ParentName = model.parentName,
                    Address = model.address,
                    City = model.city,
                    Email = model.email,
                    ContactNumber = model.contactNo,
                    Password = model.password,
                    FilePath = model.FilePath,
                    image = model.image,
                    State = model.state
                };

                Tuple<bool, string, TbParent> data =
                    await _parentRepository.AddParentAsync(newModel);

                status = data.Item1;
                message = data.Item2;

                if (status)
                {
                    var parent = data.Item3;

                    var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.NameIdentifier,
                                        parent.ParentId.ToString()),

                                    new Claim(ClaimTypes.Name,
                                        parent.ParentName),

                                    new Claim(ClaimTypes.Role,
                                        UserRole.Parent.ToString())
                                };

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    HttpContext.Session.SetString(
                        "UserType",
                        ((int)UserRole.Parent).ToString());

                    HttpContext.Session.SetString(
                        "ParentId",
                        parent.ParentId.ToString());

                    return Json(new
                    {
                        Status = true,
                        Message = message
                    });
                }

                return Json(new
                {
                    Status = false,
                    Message = message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
        public IActionResult SchoolRegistration()
        {
            return View();
        }
        public IActionResult StudentDetails()
        {
            return View();
        }
        public IActionResult StaffLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckLogin(LoginModel model)
        {
            try
            {
                if (model.userType == (int)UserRole.School ||
                    model.userType == (int)UserRole.Staff ||
                    model.userType == (int)UserRole.Teacher)
                {
                    var user = await _Entities.TbLogins
                        .FirstOrDefaultAsync(x =>
                            x.Username.ToLower() == model.Email.ToLower() &&
                            x.Password == model.Password &&
                            x.IsActive &&
                            x.RoleId == model.userType);

                    if (user == null)
                    {
                        return Json(new
                        {
                            status = false,
                            msg = "Username/Password incorrect"
                        });
                    }

                    bool isAdmin = false;

                    if (user.RoleId == (int)UserRole.School)
                    {
                        isAdmin = true;
                    }
                    else if (user.RoleId == (int)UserRole.Teacher)
                    {
                        var teacher = await _Entities.TbTeachers
                            .FirstOrDefaultAsync(x => x.UserId == user.UserId);

                        if (teacher?.UserType != null)
                        {
                            isAdmin = await _Entities.TbUserModuleMains
                                .Where(x =>
                                    x.Id == teacher.UserType &&
                                    x.IsActive)
                                .Select(x => x.IsAdmin ?? false)
                                .FirstOrDefaultAsync();
                        }
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,
                            user.UserId.ToString()),

                        new Claim(ClaimTypes.Name,
                            user.Username),

                        new Claim(ClaimTypes.Role,
                            user.RoleId.ToString())
                    };

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    HttpContext.Session.SetString(
                        "UserId",
                        user.UserId.ToString());

                    HttpContext.Session.SetString(
                        "UserType",
                        user.RoleId.ToString());

                    HttpContext.Session.SetString(
                        "IsAdmin",
                        isAdmin.ToString());

                    return Json(new
                    {
                        status = true,
                        msg = "Success",
                        userType = user.RoleId
                    });
                }

                else if (model.userType == (int)UserRole.Parent)
                {
                    var parent = await _Entities.TbParents
                        .FirstOrDefaultAsync(x =>
                            x.Email.ToLower() == model.Email.ToLower()
                            && (x.Password == model.Password
                            || x.ContactNumber == model.Password)
                            && x.IsActive);

                    if (parent == null)
                    {
                        return Json(new
                        {
                            status = false,
                            msg = "Username/Password incorrect"
                        });
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,
                            parent.ParentId.ToString()),

                        new Claim(ClaimTypes.Name,
                            parent.ParentName),

                        new Claim(ClaimTypes.Role,
                            UserRole.Parent.ToString())
                    };

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    HttpContext.Session.SetString(
                        "ParentId",
                        parent.ParentId.ToString());

                    HttpContext.Session.SetString(
                        "UserType",
                        ((int)UserRole.Parent).ToString());

                    HttpContext.Session.SetString(
                        "IsAdmin",
                        "false");

                    return Json(new
                    {
                        status = true,
                        msg = "Success",
                        userType = (int)UserRole.Parent
                    });
                }

                return Json(new
                {
                    status = false,
                    msg = "Invalid User Type"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    msg = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SchoolRegistrationSubmit(SchoolRegisterModel model)
        {
            bool status = false;
            string message = "Failed";

            long schoolId = 0;

            string latData = "";
            string longData = "";

            try
            {
                bool emailExists = await _Entities.TbLogins
                    .AnyAsync(x =>
                        x.Username.ToLower() ==
                        model.emailaddress.ToLower()
                        && x.IsActive);

                if (emailExists)
                {
                    return Json(new
                    {
                        Status = false,
                        Message = "Email Already Exists"
                    });
                }

                try
                {
                    using var client = new HttpClient();

                    var requestUri =
                        $"https://maps.googleapis.com/maps/api/geocode/xml?address={Uri.EscapeDataString(model.address)}";

                    var response = await client.GetStringAsync(requestUri);

                    var xdoc = XDocument.Parse(response);

                    var result = xdoc.Element("GeocodeResponse")
                                     ?.Element("result");

                    var locationElement = result?
                        .Element("geometry")
                        ?.Element("location");

                    latData = locationElement?
                        .Element("lat")?.Value ?? "";

                    longData = locationElement?
                        .Element("lng")?.Value ?? "";
                }
                catch
                {
                    latData = "9.387137";
                    longData = "76.547018";
                }

                if (string.IsNullOrEmpty(latData))
                {
                    return Json(new
                    {
                        Status = false,
                        Message = "Invalid Address"
                    });
                }

                var school = new TbSchool
                {
                    SchoolGuidId = Guid.NewGuid(),
                    SchoolName = model.schoolName,
                    Address = model.address,
                    Contact = model.contactNumber,
                    IsActive = true,
                    City = model.city,
                    State = model.state,
                    TimeStamp = DateTime.UtcNow,
                    FilePath = model.FilePath,
                    Website = model.website,
                    Latitude = latData,
                    Longitude = longData
                };

                await _Entities.TbSchools.AddAsync(school);

                status = await _Entities.SaveChangesAsync() > 0;

                if (!status)
                {
                    return Json(new
                    {
                        Status = false,
                        Message = "Failed To Create School"
                    });
                }

                var login = new TbLogin
                {
                    SchoolId = school.SchoolId,
                    RoleId = (int)UserRole.School,
                    Name = school.SchoolName,
                    Username = model.emailaddress,
                    Password = model.password,
                    IsActive = true,
                    TimeStamp = DateTime.UtcNow,
                    DisableStatus = false,
                    LoginGuid = Guid.NewGuid()
                };

                await _Entities.TbLogins.AddAsync(login);

                status = await _Entities.SaveChangesAsync() > 0;

                if (!status)
                {
                    return Json(new
                    {
                        Status = false,
                        Message = "Failed To Create Login"
                    });
                }

                schoolId = school.SchoolId;

                var claims = new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        login.UserId.ToString()),

                    new Claim(
                        ClaimTypes.Name,
                        login.Username),

                    new Claim(
                        ClaimTypes.Role,
                        UserRole.School.ToString())
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                HttpContext.Session.SetString(
                    "UserId",
                    login.UserId.ToString());

                HttpContext.Session.SetString(
                    "UserType",
                    ((int)UserRole.School).ToString());

                HttpContext.Session.SetString(
                    "IsAdmin",
                    "true");

                message = "Success";

                return Json(new
                {
                    Status = true,
                    Message = message
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactUsModel model)
        {
            bool status = false;

            string message = "Failed";

            string description = "failed";

            try
            {
                var filePath = Path.Combine(
                    _environment.WebRootPath,
                    "Content",
                    "email",
                    "ContactAdmin.html");

                var emailTemplate =
                    await System.IO.File.ReadAllTextAsync(filePath);

                var mailBody = emailTemplate
                    .Replace("{{user}}", model.name)
                    .Replace("{{messgae}}", model.message)
                    .Replace("{{email}}", model.email)
                    .Replace("{{contactNo}}", model.contactNo)
                    .Replace("{{schoolName}}", model.schoolName);

                await SendMailAsync("School Man - ContactUs",mailBody,"info.schoolman@gmail.com");

                status = true;

                description = "success";
            }
            catch (Exception ex)
            {
                status = false;

                description = ex.Message;
            }

            message = status
                ? description
                : "Something went wrong!";

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
        private async Task<bool> SendMailAsync(string subject,string body,string toEmail)
        {
            try
            {
                using var message = new MailMessage();

                message.Subject = subject;

                message.Body = body;

                message.IsBodyHtml = true;

                message.To.Add(toEmail);

                message.From = new MailAddress(
                    "info@gmail.com");

                using var smtp = new SmtpClient(
                    "smtp.gmail.com",
                    587);

                smtp.Credentials =
                    new NetworkCredential(
                        "info@gmail.com",
                        "password");

                smtp.EnableSsl = true;

                await smtp.SendMailAsync(message);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public IActionResult SearchAdmission(string id)
        {
            var model = new StudentModel();

            string[] splitData = id.Split('~');

            model.admissionNo = splitData[0];

            model.schoolId = Convert.ToInt64(splitData[1]);

            return PartialView(
                "_pv_StudentDetailsNonLoginParent",
                model);
        }
        public async Task<IActionResult> BillingDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            string[] splitData = id.Split('~');

            if (splitData.Length < 2)
            {
                return BadRequest();
            }

            long studentId = Convert.ToInt64(splitData[0]);

            string admissionNo = splitData[1];

            var student = await _Entities.TbStudents
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var model = new FeeModel();

            if (student.StudentSpecialId == admissionNo)
            {
                model.SchoolModel = new SchoolModel
                {
                    studentName = student.StundentName,
                    classNumber = student.ClasssNumber,
                    className = student.Class.Class,
                    division = student.Division.Division,
                    //classInCharge = student.Teacher == null
                    //    ? "Not Assigned"
                    //    : student.Teacher.TeacherName,

                    classId = student.ClassId,

                    studentId = studentId
                };

                model.AdmissionNo = student.StudentSpecialId;
            }

            return View(model);
        }

        public IActionResult StudentHistoryBillPartialView(string id)
        {
            var model = new FeeModel
            {
                SchoolModel = new SchoolModel
                {
                    studentId = Convert.ToInt64(id)
                }
            };

            return PartialView(
                "_pv_History_Billing_StudentFee_Model",
                model);
        }
        public IActionResult LoadTableForBilling(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            string[] splitData = id.Split('~');

            if (splitData.Length < 2)
            {
                return BadRequest();
            }

            var model = new FeeModel
            {
                SchoolModel = new SchoolModel
                {
                    studentId = Convert.ToInt64(splitData[1])
                },

                BillNumber = Convert.ToInt64(splitData[0])
            };

            return PartialView(
                "_pv_History_PopupGrid",
                model);
        }


   
    [HttpPost]
    public async Task<IActionResult> StudentMainBillPay(FeeModel model)
    {
        decimal sumAmt = 0;

        bool status = false;

        string message = "Failed";

        List<string> feeDetails = model.FeeDetails.Split(',').ToList();

        long SchoolId = model.SchoolId;

        long ClassId = model.ClassId;

        long StudentId = model.StudentId;

        DateTime BillDate = DateTime.UtcNow;

        decimal TotalAmountPaid = 0;

        if (model.PaidAmount != 0)
        {
            TotalAmountPaid = Convert.ToDecimal(model.PaidAmount);
        }

        Guid PaymentGuid = Guid.NewGuid();

        long BillNo = 1;

        var billNo = await _Entities.TbPaymentBillNos
            .FirstOrDefaultAsync(z => z.SchoolId == SchoolId);

        if (billNo != null)
        {
            BillNo = billNo.BillNo + 1;
        }
        else
        {
            var slNoTable = new TbPaymentBillNo
            {
                SchoolId = SchoolId,
                BillNo = 1
            };

            await _Entities.TbPaymentBillNos.AddAsync(slNoTable);

            status = await _Entities.SaveChangesAsync() > 0;
        }

        foreach (var fee in feeDetails)
        {
            string[] splitData = fee.Split('^');

            decimal paymentAmount =
                Convert.ToDecimal(splitData[0]);

            long feeId =
                Convert.ToInt64(splitData[1]);

            var payment = new TbPayment();

            payment.FeeId = feeId;

            payment.FeeGuid = new Guid(splitData[2]);

            decimal maxAmount =
                Convert.ToDecimal(splitData[3]);

            decimal discount =
                Convert.ToDecimal(splitData[4]);

            payment.MaxAmount = maxAmount;

            payment.Discount = discount;

            int isAmountEdit =
                Convert.ToInt32(splitData[5]);

            if (isAmountEdit != 0)
            {
                var paymentList =
                    new TrackTap.Data.Student(StudentId)
                    .GetStudentPaymentFees()
                    .OrderBy(z => z.DueDate)
                    .ToList();

                var dueFee = paymentList
                    .FirstOrDefault(z =>
                        z.FeeGuid == payment.FeeGuid);

                if (dueFee != null)
                {
                    if (dueFee.Amount != paymentAmount)
                    {
                        var due = new TbFeeDue();

                        due.FeeId = payment.FeeId;

                        decimal amtAfterDiscount =
                            maxAmount - discount;

                        due.Amount =
                            amtAfterDiscount - paymentAmount;

                        due.FeeDuesGuid = Guid.NewGuid();

                        due.StudentId = StudentId;

                        due.IsActive = true;

                        due.DueDate = dueFee.DueDate;

                        due.TimeStamp = BillDate;

                        await _Entities.TbFeeDues
                            .AddAsync(due);
                    }
                }
            }

            payment.Amount = paymentAmount;

            sumAmt += payment.Amount;

            payment.BillNo = BillNo;

            payment.IsPaid = false;

            payment.PaymentType = 2;

            payment.PaymentGuid = PaymentGuid;

            payment.StudentId = StudentId;

            payment.ClassId = ClassId;

            payment.SchoolId = SchoolId;

            payment.TimeStamp = BillDate;

            payment.IsActive = true;

            if (_user.UserId != 0)
            {
                payment.IssuedPerson = _user.UserId;
            }

            await _Entities.TbPayments.AddAsync(payment);

            status = await _Entities.SaveChangesAsync() > 0;
        }

        var billNo1 = await _Entities.TbPaymentBillNos
            .FirstOrDefaultAsync(z =>
                z.SchoolId == SchoolId);

        if (billNo1 != null)
        {
            billNo1.BillNo = BillNo;

            status = await _Entities.SaveChangesAsync() > 0;
        }

        bool ispayable = false;

        decimal payableAmount = 0;

        decimal prevBal = 0;

        try
        {
            decimal bal = 0;

            decimal tempSumTotal = sumAmt;

            var balance = await _Entities.TbStudentBalances
                .FirstOrDefaultAsync(z =>
                    z.StudentId == StudentId &&
                    z.IsActive);

            if (balance != null)
            {
                prevBal = balance.Amount;

                bal = balance.Amount;

                if ((prevBal < tempSumTotal)
                    && (prevBal != 0))
                {
                    ispayable = true;

                    payableAmount =
                        tempSumTotal - prevBal;
                }

                if (TotalAmountPaid != 0)
                {
                    var tempBal =
                        TotalAmountPaid - sumAmt;

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

                if (bal < 0)
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

                if (bal < 0)
                {
                    bal = 0;
                }
            }

            if (balance != null)
            {
                balance.Amount = bal;

                await _Entities.SaveChangesAsync();
            }
            else
            {
                if (bal != 0)
                {
                    var studentBalance =
                        new TbStudentBalance();

                    studentBalance.StudentId =
                        StudentId;

                    studentBalance.Amount = bal;

                    studentBalance.IsActive = true;

                    await _Entities.TbStudentBalances
                        .AddAsync(studentBalance);

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;
                }
            }

            if ((TotalAmountPaid != 0) || (bal != 0))
            {
                var studentPaidsAmount =
                    new TbStudentPaidAmount();

                studentPaidsAmount.StudentId =
                    StudentId;

                studentPaidsAmount.PaidAmount =
                    TotalAmountPaid;

                studentPaidsAmount.PreviousBalance =
                    prevBal;

                studentPaidsAmount.BalanceAmount =
                    bal;

                studentPaidsAmount.BillNo = BillNo;

                studentPaidsAmount.IsActive = true;

                await _Entities.TbStudentPaidAmounts
                    .AddAsync(studentPaidsAmount);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;
            }

            if (ispayable)
            {
                var studentPaidsAmount =
                    new TbStudentPaidAmount();

                studentPaidsAmount.StudentId =
                    StudentId;

                studentPaidsAmount.PaidAmount =
                    payableAmount;

                studentPaidsAmount.PreviousBalance =
                    prevBal;

                studentPaidsAmount.BalanceAmount =
                    bal;

                studentPaidsAmount.BillNo = BillNo;

                studentPaidsAmount.IsActive = true;

                studentPaidsAmount.AddAccountStatus =
                    false;

                await _Entities.TbStudentPaidAmounts
                    .AddAsync(studentPaidsAmount);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;
            }
        }
        catch
        {
        }

        var studDetails = await _Entities.TbStudents
            .FirstOrDefaultAsync(z =>
                z.StudentId == StudentId &&
                z.IsActive);

        bool isGateway = false;

        if (ispayable || (prevBal == 0))
        {
            var amountTopay =
                payableAmount == 0
                ? sumAmt
                : payableAmount;

            isGateway = true;

            Guid guid = Guid.NewGuid();

            string reference = guid.ToString()
                .Replace("-", string.Empty)
                .Substring(0, 10)
                .ToUpper();

            HttpContext.Session.SetString(
                "REFERENCE",
                reference);

            PaymentModels pay = new PaymentModels();

            pay.ReferenceNo =
                HttpContext.Session.GetString(
                    "REFERENCE");

            pay.Description = "Payment";

            string baseUrl =
                $"{Request.Scheme}://{Request.Host}";

            pay.ReturnUrl =
                baseUrl + "/Account/ccavResponseHandler";

            pay.Name = studDetails.ParentName;

            pay.Address = studDetails.Address;

            pay.City = studDetails.City;

            pay.State = studDetails.State;

            pay.PostalCode = studDetails.PostalCode;

            pay.PhoneNo = studDetails.ContactNumber;

            pay.Email = studDetails.ParentEmail;

            var bankFees =
                await _Entities.TbBankPercentages
                .FirstOrDefaultAsync(x =>
                    x.SchoolId == SchoolId &&
                    x.IsActive);

            if (bankFees != null)
            {
                var varbankfee =
                    (amountTopay * bankFees.Amount) / 100;

                amountTopay += varbankfee;
            }

            pay.Amount = Convert.ToDouble(amountTopay);

            pay.CourseName = "Bill";

            pay.BillNo = BillNo;

            pay.SchoolId = SchoolId;

            pay.StudentId = StudentId;

            HttpContext.Session.SetString(
                "PaymentPostData",
                JsonSerializer.Serialize(pay));
        }

        return Json(new
        {
            status = status,
            serialNo = BillNo,
            payment = isGateway,
            msg = status
                ? "Bill Paid Successfully"
                : "Failed To Pay Bill"
        });
    }
        public IActionResult PrintAccountBillData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            string[] splitData = id.Split('~');

            if (splitData.Length < 2)
            {
                return BadRequest();
            }

            if (!long.TryParse(splitData[0], out long studentId))
            {
                return BadRequest();
            }

            if (!long.TryParse(splitData[1], out long billNumber))
            {
                return BadRequest();
            }

            var model = new PrintBill
            {
                studentId = studentId,
                billNumber = billNumber
            };

            return PartialView(
                "_pv_PrintAccountBillData",
                model);
        }
        #region PaymentGateway
      

    public async Task<IActionResult> CoursePayment(string id)
    {
        bool status = true;

        int caseSwitch = Convert.ToInt32(id);

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
                msg = "Invalid Course"
            });
        }

        Guid guid = Guid.NewGuid();

        string reference = guid.ToString()
            .Replace("-", string.Empty)
            .Substring(0, 10)
            .ToUpper();

        HttpContext.Session.SetString(
            "REFERENCE",
            reference);

        PaymentModels pay = new PaymentModels();

        pay.ReferenceNo =
            HttpContext.Session.GetString(
                "REFERENCE");

        var student = await _Entities.TbParents
            .FirstOrDefaultAsync(z =>
                z.ParentId == _parentUser.ParentId);

        if (student == null)
        {
            return Json(new
            {
                status = false,
                msg = "Parent not found"
            });
        }

        pay.Description = "Payment";

        string baseUrl =
            $"{Request.Scheme}://{Request.Host}";

        pay.ReturnUrl =
            baseUrl + "/Account/ccavResponseHandler";

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

        HttpContext.Session.SetString(
            "PaymentPostData",
            JsonSerializer.Serialize(pay));

        return Json(new
        {
            status = true
        });
    }
   

    public IActionResult PaymentPost()
    {
        PaymentModels model = new PaymentModels();

        string reference =
            HttpContext.Session.GetString("REFERENCE");

        if (!string.IsNullOrEmpty(reference))
        {
            model.ReferenceNo = reference;
        }

        string paymentData =
            HttpContext.Session.GetString(
                "PaymentPostData");

        if (string.IsNullOrEmpty(paymentData))
        {
            return RedirectToAction("LoginPage");
        }

        PaymentModels pay =
            JsonSerializer.Deserialize<PaymentModels>(
                paymentData);

        if (pay == null)
        {
            return RedirectToAction("LoginPage");
        }

        model.Description = "Payment";

        string baseUrl =
            $"{Request.Scheme}://{Request.Host}";

        model.ReturnUrl = pay.ReturnUrl;

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

        string workingKey =
            "3891AA5249F6E3DBA928422EB4BA18DD";

        string ccaRequest = "";

        string strEncRequest = "";

        string iframeSrc = "";

        string strAccessCode =
            "AVTM75FA75BW58MTWB";

        NameValueCollection nameValue =
            new NameValueCollection();

        foreach (var item in Request.Form)
        {
            nameValue.Add(item.Key, item.Value);
        }

        if (Request.Query.Count > 0)
        {
            foreach (var item in Request.Query)
            {
                nameValue.Add(item.Key, item.Value);
            }
        }

        SortedDictionary<string, string> sortedDict =
            NameValueCreator
                .SortNameValueCollection(nameValue);

        foreach (KeyValuePair<string, string> p
            in sortedDict)
        {
            if (p.Key != null)
            {
                if (!p.Key.StartsWith("_"))
                {
                    ccaRequest +=
                        p.Key + "=" + p.Value + "&";
                }
            }
        }

        strEncRequest =
            ccaCrypto.Encrypt(
                ccaRequest,
                workingKey);

        iframeSrc =
            "https://secure.ccavenue.com/transaction/transaction.do?command=initiateTransaction&encRequest="
            + strEncRequest
            + "&access_code="
            + strAccessCode;

        pay.iframeSrc = iframeSrc;

        return View(pay);
    }

    public async Task<IActionResult> ccavResponseHandler()
    {
        bool status = false;

        PaymentModels model = new PaymentModels();

        string paymentData =
            HttpContext.Session.GetString(
                "PaymentPostData");

        if (string.IsNullOrEmpty(paymentData))
        {
            return RedirectToAction("LoginPage");
        }

        PaymentModels pay =
            JsonSerializer.Deserialize<PaymentModels>(
                paymentData);

        if (pay == null)
        {
            return RedirectToAction("LoginPage");
        }

        string workingKey =
            "3891AA5249F6E3DBA928422EB4BA18DD";

        CCACrypto ccaCrypto = new CCACrypto();

        string encResp = Request.Form["encResp"];

        string encResponse =
            ccaCrypto.Decrypt(
                encResp,
                workingKey);

        NameValueCollection Params =
            new NameValueCollection();

        string[] segments =
            encResponse.Split('&');

        foreach (string seg in segments)
        {
            string[] parts = seg.Split('=');

            if (parts.Length > 1)
            {
                string Key = parts[0].Trim();

                string Value = parts[1].Trim();

                Params.Add(Key, Value);
            }
        }

        var ccavenueTable =
            new TbCcavenueCourseResponse();

        string amt = "";

        bool isSuccess = false;

        for (int i = 0; i < Params.Count; i++)
        {
            if (Params.Keys[i] == "order_id")
            {
                ccavenueTable.OrderId = Params[i];
            }
            else if (Params.Keys[i] == "order_status")
            {
                ccavenueTable.OrderStatus =
                    Params[i] != "Failure";

                model.PaymentStatus =
                    Params[i];

                isSuccess =
                    Params[i] != "Failure";
            }
            else if (Params.Keys[i] == "payment_mode")
            {
                ccavenueTable.PaymentMode =
                    Params[i];
            }
            else if (Params.Keys[i] == "tracking_id")
            {
                ccavenueTable.TrackingId =
                    Params[i];
            }
            else if (Params.Keys[i] == "amount")
            {
                ccavenueTable.Amount =
                    Convert.ToDecimal(Params[i]);

                amt = Params[i];
            }
        }

        ccavenueTable.ParentId = 1;

        ccavenueTable.Course =
            pay.CourseName;

        ccavenueTable.BillNo =
            pay.BillNo;

        ccavenueTable.SchoolId =
            pay.SchoolId;

        await _Entities.TbCcavenueCourseResponses
            .AddAsync(ccavenueTable);

        await _Entities.SaveChangesAsync();

        if (isSuccess)
        {
            var paymentList =
                await _Entities.TbPayments
                .Where(z =>
                    z.StudentId == pay.StudentId &&
                    z.BillNo == pay.BillNo)
                .ToListAsync();

            foreach (var item in paymentList)
            {
                item.IsPaid = true;
            }

            await _Entities.SaveChangesAsync();

            try
            {
                var senderName = "MYSCHO";

                var senderData =
                    await _Entities.TbSchoolSenderIds
                    .FirstOrDefaultAsync(x =>
                        x.SchoolId == pay.SchoolId &&
                        x.IsActive==true);

                if (senderData != null)
                {
                    senderName = senderData.SenderId;
                }

                var smsHead = new TbSmsHead();

                smsHead.Head =
                    "BillDate Payment "
                    + paymentList.FirstOrDefault()
                    ?.Student?.StundentName;

                smsHead.SchoolId =
                    _user.SchoolId;

                smsHead.TimeStamp =
                    DateTime.UtcNow;

                smsHead.IsActive = true;

                smsHead.SenderType =
                    (int)SMSSendType.Student;

                await _Entities.TbSmsHeads
                    .AddAsync(smsHead);

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                string messagepre =
                    "Dear Parent of "
                    + paymentList.FirstOrDefault()
                    ?.Student?.StundentName
                    + ", you have paid Rs."
                    + string.Format("{0:0.00}", amt)
                    + " on "
                    + DateTime.UtcNow;

                var phone =
                    paymentList.FirstOrDefault()
                    ?.Student?.ContactNumber;

                using HttpClient client =
                    new HttpClient();

                var url =
                    "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers="
                    + phone
                    + "&route=2&message="
                    + Uri.EscapeDataString(messagepre)
                    + "&sender="
                    + senderName;

                var responseText =
                    await client.GetStringAsync(url);

                if (status)
                {
                    var sms =
                        new TbSmsHistory();

                    sms.IsActive = true;

                    sms.MessageContent =
                        messagepre;

                    sms.MessageDate =
                        DateTime.UtcNow;

                    sms.ScholId =
                        _user.SchoolId;

                    sms.StuentId =
                        pay.StudentId;

                    sms.MobileNumber =
                        phone;

                    sms.HeadId =
                        smsHead.HeadId;

                    sms.SendStatus = "Success";

                    await _Entities.TbSmsHistories
                        .AddAsync(sms);

                    await _Entities.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }
        else
        {
            var studentPaidAmount =
                await _Entities.TbStudentPaidAmounts
                .FirstOrDefaultAsync(z =>
                    z.StudentId == pay.StudentId &&
                    z.BillNo == pay.BillNo);

            if (studentPaidAmount != null)
            {
                studentPaidAmount.IsActive =
                    false;

                studentPaidAmount.AddAccountStatus =
                    false;

                var studentBalance =
                    await _Entities.TbStudentBalances
                    .FirstOrDefaultAsync(z =>
                        z.StudentId == pay.StudentId);

                if (studentBalance != null)
                {
                    studentBalance.Amount =
                        studentPaidAmount.PreviousBalance ?? 0;
                }

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;
            }
        }

        double amnt = Convert.ToDouble(amt);

        if (amnt != pay.Amount)
        {
            model.PaymentStatus = "Fraud";
        }

        model.Amount = amnt;

        model.CourseName =
            pay.CourseName;

        model.StudentId =
            pay.StudentId;

        model.BillNo =
            pay.BillNo;

        return View(model);
    }

        private HttpClient GetClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");

            client.Timeout = TimeSpan.FromMinutes(5);

            return client;
        }
        #endregion

        //public object SearchAdmission(string id)
        //{
        //    bool Status = false;
        //    string Message = "Failed";
        //    try
        //    {
        //        string[] splitData = id.Split('~');
        //        string admission = splitData[0];
        //        long schoolId = Convert.ToInt64(splitData[1]);
        //        var IsAdmission = _Entities.tb_Student.Where(z => z.StudentSpecialId == admission && z.SchoolId == schoolId).FirstOrDefault();
        //        if (IsAdmission != null)
        //        {
        //            Status = true;
        //            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            Status = false;
        //            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
       

    private bool UpdateSession(long schoolId)
    {
        var user = _schoolRepository.GetUserByIdAsync(schoolId);

        if (user != null)
        {
            HttpContext.Session.SetString(
                "School",
                JsonSerializer.Serialize(user));

            return true;
        }

        return false;
    }

    public IActionResult DummyHome()
        {
            return View();
        }
        public async Task<IActionResult> IsEmailExist(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Json(new
                {
                    status = false
                });
            }

            bool status = await _Entities.TbLogins
                .AnyAsync(z =>
                    z.Username.ToLower() == email.ToLower()
                    && z.IsActive);

            return Json(new
            {
                status = status
            });
        }
        public async Task<IActionResult> CheckEmail(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Json(new
                {
                    Status = false,
                    Message = "Invalid Email"
                });
            }

            bool exists = await _Entities.TbLogins
                .AnyAsync(x =>
                    x.Username.ToLower() == text.ToLower()
                    && x.IsActive);

            return Json(new
            {
                Status = exists,
                Message = exists
                    ? "Username already in use"
                    : "Available"
            });
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ForgotPasswordParent()
        {
            return View();
        }
        public async Task<IActionResult> CheckExistEmail(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Json(new
                {
                    Status = false,
                    Message = "Invalid Email"
                });
            }

            bool exists = await _Entities.TbLogins
                .AnyAsync(x =>
                    x.Username.ToLower().Trim() ==
                    text.ToLower().Trim()
                    && x.RoleId == 1
                    && x.IsActive);

            return Json(new
            {
                Status = !exists,
                Message = !exists
                    ? "Email not exists"
                    : "Email exists"
            });
        }
        public async Task<IActionResult> CheckParentExistEmail(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Json(new
                {
                    Status = false,
                    Message = "Invalid Email"
                });
            }

            bool exists = await _Entities.TbParents
                .AnyAsync(x =>
                    x.Email.ToLower().Trim() ==
                    text.ToLower().Trim()
                    && x.IsActive);

            return Json(new
            {
                Status = !exists,
                Message = !exists
                    ? "Email not exists"
                    : "Email exists"
            });
        }
        public async Task<IActionResult> SendMailForPasswordParent(ForgotPasswordModel model)
        {
            bool sendData = false;

            bool status = false;

            string message = "Failed";

            try
            {
                var parent = await _Entities.TbParents
                    .FirstOrDefaultAsync(x =>
                        x.Email.Trim().ToLower() ==
                        model.email.Trim().ToLower()
                        && x.IsActive);

                if (parent != null)
                {
                    var resetPasswordData =
                        new TbResetPassword
                        {
                            LinkExpireStatus = true,

                            UserId = parent.ParentId,

                            UserGuid = parent.ParentGuid,

                            IsActive = true,

                            TimeStamp = DateTime.UtcNow
                        };

                    await _Entities.TbResetPasswords
                        .AddAsync(resetPasswordData);

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        message =
                            "Success, Please Check Your Email";

                        sendData =await SendMailDataParentAsync(
                                parent.Email,
                                parent.ParentGuid);
                    }
                }
                else
                {
                    message = "Email not found";
                }
            }
            catch (Exception ex)
            {
                status = false;

                message = ex.Message;
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
        public async Task<IActionResult> SendMailForPassword(ForgotPasswordModel model)
        {
            bool sendData = false;

            bool status = false;

            string message = "Failed";

            try
            {
                var schoolPassword =
                    await _Entities.TbLogins
                    .FirstOrDefaultAsync(x =>
                        x.Username.Trim().ToLower() ==
                        model.email.Trim().ToLower()
                        && x.IsActive);

                if (schoolPassword != null)
                {
                    var resetPasswordData =
                        new TbResetPassword
                        {
                            LinkExpireStatus = true,

                            UserId =
                                schoolPassword.SchoolId,

                            UserGuid =
                                schoolPassword.LoginGuid,

                            IsActive = true,

                            TimeStamp = DateTime.UtcNow
                        };

                    await _Entities.TbResetPasswords
                        .AddAsync(resetPasswordData);

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        message =
                            "Success, Please Check Your Email";

                        sendData =
                            await SendMailDataAsync(
                                schoolPassword.Username,
                                schoolPassword.LoginGuid);
                    }
                }
                else
                {
                    message = "Email not found";
                }
            }
            catch (Exception ex)
            {
                status = false;

                message = ex.Message;
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
        private async Task<bool> SendMailDataAsync(string email,Guid loginGuid)
        {
            try
            {
                string sendGuid =
                    loginGuid.ToString();

                var filePath = Path.Combine(
                    _environment.WebRootPath,
                    "Content",
                    "template",
                    "WebResetPassword.html");

                var emailTemplate =
                    await System.IO.File
                        .ReadAllTextAsync(filePath);

                string baseUrl =
                    $"{Request.Scheme}://{Request.Host}";

                var url =
                    $"{baseUrl}/Account/CheckResetSchoolPassword/{sendGuid}";

                var mBody = emailTemplate
                    .Replace("{{resetLink}}", url);

                bool sendMail =
                    await SendMailAsync(
                        "Reset Password",
                        mBody,
                        email);

                return sendMail;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> SendMailDataParentAsync(string email,Guid loginGuid)
        {
            string sendGuid =
                loginGuid.ToString();

            var filePath = Path.Combine(
                _environment.WebRootPath,
                "Content",
                "template",
                "WebResetPassword.html");

            var emailTemplate =
                await System.IO.File
                    .ReadAllTextAsync(filePath);

            string baseUrl =
                $"{Request.Scheme}://{Request.Host}";

            var url =
                $"{baseUrl}/Account/CheckResetParentPassword/{sendGuid}";

            var mBody = emailTemplate
                .Replace("{{resetLink}}", url);

            bool sendMail =
                await SendMailAsync(
                    "Reset Password",
                    mBody,
                    email);

            return sendMail;
        }
        private async Task<bool> SendAsync(string subject,string mailbody,string email)
        {
            try
            {
                using var msg = new MailMessage();

                msg.Subject = subject;

                msg.Body = mailbody;

                msg.From = new MailAddress(
                    "info.schoolman@gmail.com");

                msg.To.Add(
                    new MailAddress(email));

                msg.Bcc.Add(
                    new MailAddress(
                        "archanakv.srishti@gmail.com"));

                msg.IsBodyHtml = true;

                using var client =
                    new SmtpClient(
                        "k2smtp.gmail.com",
                        587);

                client.EnableSsl = true;

                client.UseDefaultCredentials = false;

                client.Credentials =
                    new NetworkCredential(
                        "info.schoolman@gmail.com",
                        "Info@123");

                client.DeliveryMethod =
                    SmtpDeliveryMethod.Network;

                await client.SendMailAsync(msg);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<IActionResult> CheckResetSchoolPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction(
                    "ExpiredResetPassword");
            }

            Guid userGuid;

            bool isValidGuid =
                Guid.TryParse(id, out userGuid);

            if (!isValidGuid)
            {
                return RedirectToAction(
                    "ExpiredResetPassword");
            }

            var resetPasswordData =
                await _Entities.TbResetPasswords
                .FirstOrDefaultAsync(x =>
                    x.UserGuid == userGuid
                    && x.LinkExpireStatus
                    && x.IsActive);

            if (resetPasswordData != null)
            {
                return RedirectToAction(
                    "ResetSchoolPassword",
                    new { id = id });
            }

            return RedirectToAction(
                "ExpiredResetPassword");
        }
        public async Task<IActionResult> CheckResetParentPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction(
                    "ExpiredResetPassword");
            }

            bool isValidGuid =
                Guid.TryParse(id, out Guid userGuid);

            if (!isValidGuid)
            {
                return RedirectToAction(
                    "ExpiredResetPassword");
            }

            var resetPasswordData =
                await _Entities.TbResetPasswords
                .FirstOrDefaultAsync(x =>
                    x.UserGuid == userGuid
                    && x.LinkExpireStatus
                    && x.IsActive);

            if (resetPasswordData != null)
            {
                return RedirectToAction(
                    "ResetParentPassword",
                    new { id = id });
            }

            return RedirectToAction(
                "ExpiredResetPassword");
        }
        public IActionResult ResetSchoolPassword(string id)
        {
            Guid schoolGuid = new Guid(id);
            var model = new ChangePasswordModel();
            model.LoginGuid = schoolGuid;
            return View(model);
        }
        public IActionResult ResetParentPassword(string id)
        {
            Guid schoolGuid = new Guid(id);
            var model = new ChangePasswordModel();
            model.LoginGuid = schoolGuid;
            return View(model);
        }
        public IActionResult ExpiredResetPassword()
        {
            return View();
        }
        public async Task<IActionResult> ChangePasswordWithNew(ChangePasswordModel model)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                var schoolData =
                    await _Entities.TbLogins
                    .FirstOrDefaultAsync(x =>
                        x.LoginGuid == model.LoginGuid
                        && x.IsActive);

                if (schoolData != null)
                {
                    var resetData =
                        await _Entities.TbResetPasswords
                        .FirstOrDefaultAsync(x =>
                            x.UserGuid == model.LoginGuid
                            && x.IsActive);

                    if (resetData != null)
                    {
                        resetData.LinkExpireStatus = false;

                        resetData.IsActive = false;
                    }

                    schoolData.Password =
                        model.password;

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        message = "Success";
                    }
                }
                else
                {
                    message = "User not found";
                }
            }
            catch (Exception ex)
            {
                status = false;

                message = ex.Message;
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
        public async Task<IActionResult> ChangePasswordWithNewParent(ChangePasswordModel model)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                var parentData =
                    await _Entities.TbParents
                    .FirstOrDefaultAsync(x =>
                        x.ParentGuid == model.LoginGuid
                        && x.IsActive);

                if (parentData != null)
                {
                    var resetData =
                        await _Entities.TbResetPasswords
                        .FirstOrDefaultAsync(x =>
                            x.UserGuid == model.LoginGuid
                            && x.IsActive);

                    if (resetData != null)
                    {
                        resetData.LinkExpireStatus = false;

                        resetData.IsActive = false;
                    }

                    parentData.Password =
                        model.password;

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;

                    if (status)
                    {
                        message = "Success";
                    }
                }
                else
                {
                    message = "Parent not found";
                }
            }
            catch (Exception ex)
            {
                status = false;

                message = ex.Message;
            }

            return Json(new
            {
                Status = status,
                Message = message
            });
        }
        public IActionResult Features()
        {
            return View();
        }
        public IActionResult Packages()
        {
            return View();
        }
        public IActionResult OurClients()
        {
            return View();
        }





    }
}
