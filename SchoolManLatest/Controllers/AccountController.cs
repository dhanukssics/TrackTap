using CCA.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml.Linq;
using TrackTap.ClassLibrary;
using TrackTap.PostModel;
using TrackTap.ClassLibrary.Utility;
using TrackTap.DataLibrary;
using TrackTap.Data;
using TrackTap.Helper;
using TrackTap.Models;
using TrackTap.Models;


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
        private readonly IWebHostEnvironment _environment;

        public AccountController(SchoolDbContext Entities, IWebHostEnvironment environment)
        {
            _Entities = Entities;
            _environment = environment;
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
        private async Task SendMailAsync(string subject,string body,string toEmail)
        {
            using var message = new MailMessage();

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            message.To.Add(toEmail);

            message.From = new MailAddress(
                "info.schoolman@gmail.com");

            using var smtp = new SmtpClient(
                "smtp.gmail.com",
                587);

            smtp.Credentials = new NetworkCredential(
                "info.schoolman@gmail.com",
                "password");

            smtp.EnableSsl = true;

            await smtp.SendMailAsync(message);
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
                //payment = new tb_Payment();
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
                if (_user.UserId != 0)
                {
                    payment.IssuedPerson = _user.UserId;
                }
                
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
                    
                    if ((prevBal < tempSumTotal) && (prevBal != 0))
                    {
                        ispayable = true;
                        payableAmount = tempSumTotal - prevBal;

                    }
                   
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
                    _Entities.tb_StudentPaidAmount.Add(studentPaidsAmount);
                    status = _Entities.SaveChanges() > 0 ? true : false;



                }
                
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
                PaymentModels pay = new PaymentModels();
                pay.ReferenceNo = Session["REFERENCE"].ToString();
                var student = studDetails;
                pay.Description = "Payment";
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                pay.ReturnUrl = baseUrl + "/Account/ccavResponseHandler";
                
                pay.Name = student.ParentName;
                pay.Address = student.Address;
                pay.City = student.City;
                pay.State = student.State;
                pay.PostalCode = student.PostalCode;
                pay.PhoneNo = student.ContactNumber;
                pay.Email = student.ParentEmail;

                //.........This is to Editing.. Field  21..07..2020....

                var bankFees = _Entities.tb_BankPercentage.Where(x => x.SchoolId == SchoolId && x.IsActive == true).FirstOrDefault();
                if (bankFees != null)
                {
                    var varbankfee = (amountTopay * bankFees.Amount) / 100;
                    amountTopay = amountTopay + varbankfee;
                }
                

                pay.Amount = Convert.ToDouble(amountTopay);

                //........End Field...

                pay.CourseName = "Bill";
                pay.BillNo = BillNo;
                pay.SchoolId = SchoolId;
                pay.StudentId = StudentId;
                Session["PaymentPostData"] = pay;
            }

           
            return Json(new { status = status, serialNo = BillNo, payment = isGateway, msg = status ? "Bill Paid Sucessfully" : "Failed To Pay Bill" }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PrintAccountBillData(string id)
        {
            string[] splitData = id.Split('~');
            var model = new PrintBill();
            model.studentId = Convert.ToInt64(splitData[0]);
            model.billNumber = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/Account/_pv_PrintAccountBillData.cshtml", model);
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
            pay.ReturnUrl = baseUrl + "/Account/ccavResponseHandler";
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
            ccavenueTable.ParentId = 1;
            ccavenueTable.Course = pay.CourseName;
            ccavenueTable.BillNo = pay.BillNo;
            ccavenueTable.SchoolId = pay.SchoolId;

            // ccavenueTable.Amount = Convert.ToDecimal(pay.Amount);
            _Entities.tb_CcavenueCourseResponse.Add(ccavenueTable);
            _Entities.SaveChanges();
            if (isSuccess)
            {
                //var studDetails = _Entities.tb_Student.Where(z => z.StudentId == pay.StudentId).FirstOrDefault();
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

        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

            request.Timeout = Convert.ToInt32(new TimeSpan(0, 5, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
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
        private object UpdateSession(long schoolId)
        {
            var user = _schoolRepository.getUserById(schoolId);
            if (user != null)
            {
                Session["School"] = user;
            }
            return true;
        }

        public IActionResult DummyHome()
        {
            return View();
        }
        public object IsEmailExist(string Email)
        {
            bool status = _Entities.tb_Login.Any(z => z.Username.ToUpper() == Email.ToUpper() && z.IsActive == true);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public object CheckEmail(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (_Entities.tb_Login.Any(x => x.Username.ToLower() == text.ToLower() && x.IsActive))
            {
                Status = true;
                Message = "Username already in use";
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ForgotPasswordParent()
        {
            return View();
        }
        public object CheckExistEmail(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (_Entities.tb_Login.Any(x => x.Username.ToLower().Trim() == text.ToLower().Trim() && x.RoleId == 1 && x.IsActive))
            {

            }
            else
            {
                Status = true;
                Message = "Email not exists";
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public object CheckParentExistEmail(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (_Entities.tb_Parent.Any(x => x.Email.ToLower().Trim() == text.ToLower().Trim() && x.IsActive))
            {

            }
            else
            {
                Status = true;
                Message = "Email not exists";
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public object SendMailForPasswordParent(ForgotPasswordModel model)
        {
            bool sendData = false;
            bool status = false;
            string Message = "Failed";
            try
            {
                var parent = _Entities.tb_Parent.Where(x => x.Email.Trim().ToLower() == model.email.Trim().ToLower() && x.IsActive).FirstOrDefault();
                if (parent != null)
                {
                    var resetPasswordData = _Entities.tb_ResetPassword.Create();
                    resetPasswordData.LinkExpireStatus = true;
                    resetPasswordData.UserId = parent.ParentId;
                    resetPasswordData.UserGuid = parent.ParentGuid;
                    resetPasswordData.IsActive = true;
                    resetPasswordData.TimeStamp = CurrentTime;
                    _Entities.tb_ResetPassword.Add(resetPasswordData);
                    status = _Entities.SaveChanges() > 0;
                    Message = "Success ,Please Check Your Email";
                    sendData = SendMailDataParent(parent.Email, parent.ParentGuid);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Status = status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public object SendMailForPassword(ForgotPasswordModel model)
        {
            bool sendData = false;
            bool status = false;
            string Message = "Failed";
            try
            {
                var schoolPassword = _Entities.tb_Login.Where(x => x.Username.Trim().ToLower() == model.email.Trim().ToLower() && x.IsActive).FirstOrDefault();
                if (schoolPassword != null)
                {
                    var resetPasswordData = _Entities.tb_ResetPassword.Create();
                    resetPasswordData.LinkExpireStatus = true;
                    resetPasswordData.UserId = schoolPassword.SchoolId;
                    resetPasswordData.UserGuid = schoolPassword.LoginGuid;
                    resetPasswordData.IsActive = true;
                    resetPasswordData.TimeStamp = CurrentTime;
                    _Entities.tb_ResetPassword.Add(resetPasswordData);
                    status = _Entities.SaveChanges() > 0;
                    Message = "Success ,Please Check Your Email";
                    sendData = SendMailData(schoolPassword.Username, schoolPassword.LoginGuid);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Status = status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        private bool SendMailData(string email, Guid loginGuid)
        {
            string sendGuid = Convert.ToString(loginGuid);
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/WebResetPassword.html");
            var emailTemplate = System.IO.File.ReadAllText(filePath);
            var url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + @"/Account/CheckResetSchoolPassword/" + sendGuid;
            var mBody = emailTemplate.Replace("{{resetLink}}", url);
            bool sendMail = Send("Reset Password", mBody, email);
            return sendMail;
        }
        private bool SendMailDataParent(string email, Guid loginGuid)
        {
            string sendGuid = Convert.ToString(loginGuid);
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/WebResetPassword.html");
            var emailTemplate = System.IO.File.ReadAllText(filePath);
            var url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + @"/Account/CheckResetParentPassword/" + sendGuid;
            var mBody = emailTemplate.Replace("{{resetLink}}", url);
            bool sendMail = Send("Reset Password", mBody, email);
            return sendMail;
        }
        private bool Send(string subject, string mailbody, string email)
        {
            MailMessage msg = new MailMessage();
            SmtpClient client = new System.Net.Mail.SmtpClient();
            msg.Subject = subject;
            msg.Body = mailbody;
            msg.From = new MailAddress("info.schoolman@gmail.com");
            msg.To.Add(new MailAddress(email));
            msg.Bcc.Add(new MailAddress("archanakv.srishti@gmail.com"));
            msg.IsBodyHtml = true;
            client.Host = "k2smtp.gmail.com";
            System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("info.schoolman@gmail.com", "Info@123");
            client.Port = int.Parse("587");//25//465
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicauthenticationinfo;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }
            return true;
        }
        public object CheckResetSchoolPassword(string id)//Check the link is expired
        {
            var userGuid = new Guid(id);
            var resetPasswordData = _Entities.tb_ResetPassword.Where(x => x.UserGuid == userGuid && x.LinkExpireStatus == true && x.IsActive).FirstOrDefault();
            if (resetPasswordData != null)
                return RedirectToAction("ResetSchoolPassword", new { id = id });//Can change password
            else
                return RedirectToAction("ExpiredResetPassword");// expired link 
        }
        public object CheckResetParentPassword(string id)//Check the link is expired
        {
            var userGuid = new Guid(id);
            var resetPasswordData = _Entities.tb_ResetPassword.Where(x => x.UserGuid == userGuid && x.LinkExpireStatus == true && x.IsActive).FirstOrDefault();
            if (resetPasswordData != null)
                return RedirectToAction("ResetParentPassword", new { id = id });//Can change password
            else
                return RedirectToAction("ExpiredResetPassword");// expired link 
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
        public object ChangePasswordWithNew(ChangePasswordModel model)
        {
            bool status = false;
            string message = "failed";
            try
            {
                var schoolData = _Entities.tb_Login.Where(x => x.LoginGuid == model.LoginGuid && x.IsActive).FirstOrDefault();
                if (schoolData != null)
                {
                    var resetData = _Entities.tb_ResetPassword.Where(x => x.UserGuid == model.LoginGuid && x.IsActive).FirstOrDefault();
                    if (resetData != null)
                    {
                        resetData.LinkExpireStatus = false;
                        resetData.IsActive = false;
                        _Entities.SaveChanges();
                    }
                    schoolData.Password = model.password;
                    status = _Entities.SaveChanges() > 0;
                    {
                        status = true;
                        message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }
        public object ChangePasswordWithNewParent(ChangePasswordModel model)
        {
            bool status = false;
            string message = "failed";
            try
            {
                var schoolData = _Entities.tb_Parent.Where(x => x.ParentGuid == model.LoginGuid && x.IsActive).FirstOrDefault();
                if (schoolData != null)
                {
                    var resetData = _Entities.tb_ResetPassword.Where(x => x.UserGuid == model.LoginGuid && x.IsActive).FirstOrDefault();
                    if (resetData != null)
                    {
                        resetData.LinkExpireStatus = false;
                        resetData.IsActive = false;
                        _Entities.SaveChanges();
                    }
                    schoolData.Password = model.password;
                    status = _Entities.SaveChanges() > 0;
                    {
                        status = true;
                        message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
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
