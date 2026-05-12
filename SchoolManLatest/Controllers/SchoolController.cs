
//using ChildrenScholarship.Helper;
using TrackTap.Models;
//using CS.EntityLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.Data;
using System.Threading;
//using PagedList;
//using PagedList.Mvc;
//using TrackTap.ClassLibrary.Utility;
using System.Globalization;
using TrackTap.DataLibrary;
using TrackTap.Service.Helper;
using System.Web.Script.Serialization;
using TrackTap.PostModel;
using TrackTap.Helper;
using TrackTap.ClassLibrary;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using TrackTap.ClassLibrary.Utility;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using TrackTap.MapModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data;
using IronBarCode;
using System.Drawing;
using System.Web.UI;
using System.Collections;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Configuration;
//using System.Web.UI; 

namespace TrackTap.Controllers
{
    public class SchoolController : BaseController
    {
        //
        // GET: /School/
        public object CheckEmail(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (_Entities.tb_Login.Any(x => x.Username.ToLower() == text.ToLower() && x.IsActive && x.SchoolId == _user.SchoolId))
            {
                Status = true;
                Message = "Username already in use";
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult Class()
        {
            var model = new SchoolValue();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult AddClassParialView()
        {
            AddClassModel model = new AddClassModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_AddClass.cshtml", model);
        }

        public PartialViewResult DatatableClassList(FilterModel model)
        {
            return PartialView("~/Views/School/_pv_ClassList.cshtml", model);
        }

        public object AddClass(AddClassModel model)
        {
            bool status = false;
            string msg = "failed";
            try
            {
                var academicYear = _Entities.tb_AcademicYear.Where(z => z.IsActive && z.CurrentYear == true).FirstOrDefault();//currentyear need to change
                var getClass = _Entities.tb_Class.Where(z => z.SchoolId == model.SchoolId && z.tb_School.IsActive && z.AcademicYearId == academicYear.YearId && z.IsActive && z.Class.ToLower() == model.ClassName.ToLower()).FirstOrDefault();
                if (getClass != null)
                {
                    if (_Entities.tb_Division.Any(z => z.Division.ToLower() == model.Division.ToLower() && z.ClassId == getClass.ClassId && z.IsActive))
                    {
                        msg = "Division already added";
                    }
                    else
                    {
                        var newDiv = _Entities.tb_Division.Create();
                        newDiv.ClassId = getClass.ClassId;
                        newDiv.Division = model.Division.ToUpper();
                        newDiv.DivisionGuid = Guid.NewGuid();
                        newDiv.IsActive = true;
                        newDiv.TimeStamp = CurrentTime;
                        _Entities.tb_Division.Add(newDiv);

                        status = _Entities.SaveChanges() > 0;
                        msg = status ? "success" : "failed";
                    }
                    return Json(new { status = status, msg = msg, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClasses(model.SchoolId) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var newClass = _Entities.tb_Class.Create();
                    newClass.Class = model.ClassName;
                    newClass.ClassGuild = Guid.NewGuid();
                    newClass.Timestamp = CurrentTime;
                    newClass.SchoolId = model.SchoolId;
                    newClass.IsActive = true;
                    newClass.ClassOrder = model.OrderValue;
                    newClass.AcademicYearId = academicYear.YearId;
                    newClass.PublishStatus = true;
                    _Entities.tb_Class.Add(newClass);

                    var newDiv = _Entities.tb_Division.Create();
                    newDiv.ClassId = newClass.ClassId;
                    newDiv.Division = model.Division.ToUpper();
                    newDiv.DivisionGuid = Guid.NewGuid();
                    newDiv.IsActive = true;
                    newDiv.TimeStamp = CurrentTime;
                    _Entities.tb_Division.Add(newDiv);

                    status = _Entities.SaveChanges() > 0;
                    msg = status ? "success" : "failed";
                    return Json(new { status = status, msg = msg, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClasses(model.SchoolId) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }

        public IActionResult ClassNotPublished()
        {
            var model = new SchoolValue();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult AddClassUnPublishedParialView()
        {
            AddClassModel model = new AddClassModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_AddClassUnPublished.cshtml", model);
        }
        public object AddClassUnPublished(AddClassModel model)
        {
            bool status = false;
            string msg = "failed";
            try
            {
                var academicYear = _Entities.tb_AcademicYear.FirstOrDefault();
                var getClass = _Entities.tb_Class.Where(z => z.SchoolId == model.SchoolId && z.tb_School.IsActive && z.IsActive && z.AcademicYearId == model.AcademicYearId && z.Class.ToLower() == model.ClassName.ToLower()).FirstOrDefault();
                if (getClass != null)
                {
                    if (_Entities.tb_Division.Any(z => z.Division.ToLower() == model.Division.ToLower() && z.ClassId == getClass.ClassId && z.IsActive))
                    {
                        msg = "Division already added";
                    }
                    else
                    {
                        var newDiv = _Entities.tb_Division.Create();
                        newDiv.ClassId = getClass.ClassId;
                        newDiv.Division = model.Division.ToUpper();
                        newDiv.DivisionGuid = Guid.NewGuid();
                        newDiv.IsActive = true;
                        newDiv.TimeStamp = CurrentTime;
                        _Entities.tb_Division.Add(newDiv);

                        status = _Entities.SaveChanges() > 0;
                        msg = status ? "success" : "failed";
                    }
                    return Json(new { status = status, msg = msg, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClasses(model.SchoolId) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var newClass = _Entities.tb_Class.Create();
                    newClass.Class = model.ClassName;
                    newClass.ClassGuild = Guid.NewGuid();
                    newClass.Timestamp = CurrentTime;
                    newClass.SchoolId = model.SchoolId;
                    newClass.IsActive = true;
                    newClass.ClassOrder = model.OrderValue;
                    newClass.AcademicYearId = model.AcademicYearId;
                    newClass.PublishStatus = false;
                    _Entities.tb_Class.Add(newClass);

                    var newDiv = _Entities.tb_Division.Create();
                    newDiv.ClassId = newClass.ClassId;
                    newDiv.Division = model.Division.ToUpper();
                    newDiv.DivisionGuid = Guid.NewGuid();
                    newDiv.IsActive = true;
                    newDiv.TimeStamp = CurrentTime;
                    _Entities.tb_Division.Add(newDiv);

                    status = _Entities.SaveChanges() > 0;
                    msg = status ? "success" : "failed";
                    return Json(new { status = status, msg = msg, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClasses(model.SchoolId) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult DatatableClassListUnPublished(FilterModel model)
        {
            return PartialView("~/Views/School/_pv_ClassListUnPublished.cshtml", model);
        }

        public object DeleteClassUnPublished(string id)
        {
            bool status = false;
            string message = "Failed";
            long DivisionId = Convert.ToInt64(id);
            var Division = _Entities.tb_Division.FirstOrDefault(z => z.DivisionId == DivisionId);
            if (Division != null)
            {
                Division.IsActive = false;
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    if (!_Entities.tb_Division.Any(z => z.ClassId == Division.ClassId && z.IsActive))
                    {
                        var classData = _Entities.tb_Class.FirstOrDefault(z => z.ClassId == Division.ClassId);
                        if (classData != null)
                        {
                            classData.IsActive = false;
                            _Entities.SaveChanges();
                        }
                    }
                }
            }
            message = status ? "Deleted" : "failed";
            return Json(new { status = status, msg = message, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClassesUnPublished(_user.SchoolId) }, JsonRequestBehavior.AllowGet);
        }

        public object PublishClassUnPublished(string id)
        {
            bool status = false;
            string message = "Failed";
            long DivisionId = Convert.ToInt64(id);
            var Division = _Entities.tb_Division.FirstOrDefault(z => z.DivisionId == DivisionId);
            if (Division != null)
            {
                var Class = _Entities.tb_Class.FirstOrDefault(z => z.ClassId == Division.ClassId);
                Class.PublishStatus = true;
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    var prevClass = _Entities.tb_Class.Where(z => z.Class == Class.Class && z.AcademicYearId != Class.AcademicYearId && z.SchoolId == _user.SchoolId).ToList();
                    {
                        foreach (var data in prevClass)
                        {
                            data.PublishStatus = false;
                            status = _Entities.SaveChanges() > 0;
                        }
                    }
                }
            }
            message = status ? "Published" : "failed";
            return Json(new { status = status, msg = message, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClassesUnPublished(_user.SchoolId) }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetToPromotionStudentsByDivGrid(string id)
        {
            StudentModel model = new StudentModel();
            model.divisionId = Convert.ToInt32(id);
            return PartialView("~/Views/School/_pv_PromoteStudent_Student_Grid.cshtml", model);

        }

        [HttpPost]
        public object PremoteStudentClass(StudentModel model)
        {
            bool status = false;
            string message = "Failed";
            List<string> studentsUserId = model.stringStudentId.Split(',').ToList();
            var studentList = _Entities.tb_Student.Where(z => z.DivisionId == model.divisionId && z.IsActive).ToList();

            var premotion = new tb_StudentPremotion();
            foreach (var userId in studentsUserId)
            {
                long userIdLong = Convert.ToInt64(userId);
                var isStudent = studentList.Where(z => z.StudentId == userIdLong && z.IsActive).FirstOrDefault();
                if (isStudent != null)
                {
                    premotion.StudentId = userIdLong;
                    premotion.FromDivision = model.divisionId;
                    premotion.ToDivision = model.toDivisionId;
                    premotion.TimeStamp = CurrentTime;
                    _Entities.tb_StudentPremotion.Add(premotion);
                    status = _Entities.SaveChanges() > 0;
                }

            }
            foreach (var userId in studentsUserId)
            {
                long userIdLong = Convert.ToInt32(userId);
                var isStudent = _Entities.tb_Student.Where(z => z.StudentId == userIdLong && z.IsActive).FirstOrDefault();
                if (isStudent != null)
                {
                    isStudent.DivisionId = model.toDivisionId;
                    isStudent.ClassId = model.classId;
                    status = _Entities.SaveChanges() > 0;
                }

            }
            message = status ? " Student promoted" : "Failed to promote student";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult Home()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult Billing()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult GetUserListByDivision(string id)
        {
            SchoolModel model = new SchoolModel();
            model.divisionId = Convert.ToInt32(id);
            return PartialView("~/Views/School/_pv_Billing_UserByDivision_Grid.cshtml", model);

        }
        public IActionResult BillingDetails(string id)
        {
            long studentId = Convert.ToInt32(id);
            var student = new TrackTap.DataLibrary.Data.Student(studentId);
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentName = student.StundentName;
            model.SchoolModel.classNumber = student.ClasssNumber; //Archana
            model.SchoolModel.className = student.ClassName;
            model.SchoolModel.division = student.DivisionName;
            model.SchoolModel.classInCharge = student.Teacher == null ? "Not Assigned" : student.Teacher.TeacherName;
            model.SchoolModel.classId = student.ClassId;
            model.SchoolModel.studentId = studentId;
            model.SchoolModel.curredntDateTime = CurrentTime;
            model.SchoolId = _user.SchoolId;
            model.DivisionId = student.DivisionId;
            model.AdmissionNo = student.StudentSpecialId;
            model.AcademicYearId = student.AcademicYearId;
            return View(model);
        }
        public IActionResult ClassDetails(long classId, long divId)
        {
            var model = new SchoolValue();
            model.classId = classId;
            model.divId = divId;
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult Student()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult Teacher()
        {
            return View();
        }
        public PartialViewResult AddStudentView()
        {
            StudentModel model = new StudentModel();
            model.schoolId = _user.SchoolId;
            model.state = _user.tb_School.State;
            model.city = _user.tb_School.City;
            //model.busId = 1;
            return PartialView("~/Views/School/_pv_AddStudent_Model.cshtml", model);
        }
        #region Library
        public IActionResult BookCategory()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult EditCategoryView(string id)
        {
            LibraryModels model = new LibraryModels();
            long catId = Convert.ToInt64(id);
            var category = _Entities.tb_BookCategory.Where(z => z.CategoryId == catId).FirstOrDefault();
            model.schoolId = _user.SchoolId;
            model.categoryId = category.CategoryId;
            model.categoryName = category.Category;
            return PartialView("~/Views/School/_pv_BookCategory_Edit.cshtml", model);
        }


        public PartialViewResult AddCategoryView()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_BookCategory_Add.cshtml", model);
        }
        [HttpPost]
        public object AddBookCategory(LibraryModels model)
        {
            bool status = false;
            string message = "Failed";
            var category = _Entities.tb_BookCategory.Create();
            category.Category = model.categoryName;
            category.SchoolId = model.schoolId;
            category.IsActive = true;
            category.TimeStamp = CurrentTime;
            _Entities.tb_BookCategory.Add(category);
            status = _Entities.SaveChanges() > 0;
            message = status ? " Category Added" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object EditBookCategory(LibraryModels model)
        {
            bool status = false;
            string message = "Failed";
            var category = _Entities.tb_BookCategory.Where(z => z.CategoryId == model.categoryId).FirstOrDefault();
            category.Category = model.categoryName;
            status = _Entities.SaveChanges() > 0;
            message = status ? " Category Edited" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteBookCategory(string id)
        {
            bool status = false;
            string message = "Failed";
            long catId = Convert.ToInt64(id);
            var category = _Entities.tb_BookCategory.Where(z => z.CategoryId == catId).FirstOrDefault();

            if (_Entities.tb_LibraryBook.Any(x => x.CategoryId == catId))
            {
                category.IsActive = false;
                status = _Entities.SaveChanges() > 0;
                message = status ? " Category deleted successfully" : "Failed to delete Category";
            }
            else
            {
                _Entities.tb_BookCategory.Remove(category);
                status = _Entities.SaveChanges() > 0 ? true : false;
                message = status ? "Category deleted successfully!" : "Failed to delete Category!";
            }

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult BookCategoryListPartial()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_BookCategory_list.cshtml", model);
        }

        public IActionResult LibraryBook()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult AddLibraryBookView()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            model.bookCount = 1;
            return PartialView("~/Views/School/_pv_LibraryBook_Add.cshtml", model);
        }

        //public JsonResult LibraryBook_Print (string id)
        //{
        //    try
        //    {
        //        string path = Server.MapPath("~/Content/images/Barcodedata/");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        //BarcodeWriter.CreateBarcode(id, BarcodeWriterEncoding.EAN8).ResizeTo(400, 100).SaveAsImage(path+"/EAN8.jpeg");
        //        GeneratedBarcode MyBarCode = BarcodeWriter.CreateBarcode(id, BarcodeWriterEncoding.Code128);
        //        MyBarCode.ResizeTo(250, 75).SetMargins(20).AddAnnotationTextAboveBarcode("").AddBarcodeValueTextBelowBarcode();
        //        MyBarCode.ChangeBackgroundColor(Color.White);
        //        string png = path + id + ".png";
        //        string gif = path + id + ".gif";
        //        string htmls = path + id + ".html";
        //        string imges = path + id + ".image";
        //        string jpegs = path + id + ".jpeg";
        //        string pdfs = path + id + ".pdf";
        //        string tiffs = path + id + ".tiff";
        //        //string bitmaps = path + id + "008.png";

        //        MyBarCode.SaveAsPng(png);
        //        MyBarCode.SaveAsGif(gif);
        //        MyBarCode.SaveAsHtmlFile(htmls);
        //        MyBarCode.SaveAsImage(imges);
        //        MyBarCode.SaveAsJpeg(jpegs);
        //        MyBarCode.SaveAsPdf(pdfs);
        //        MyBarCode.SaveAsTiff(tiffs);
        //        //MyBarCode.SaveAsWindowsBitmap(bitmaps);



        //        //return Json("path = " + path + " && png = "+ png, JsonRequestBehavior.AllowGet);


        //        return Json("/Content/images/Barcodedata/"+ id +".png", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }


        //}


        public JsonResult LibraryBook_Print(string id) // GenerateBarCode(string barcode)
        {
            try
            {


                string path = Server.MapPath("~/Content/images/Barcodedata/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Bitmap bitMap = new Bitmap(id.Length * 40, 150))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitMap))
                        {
                            //Font oFont = new Font("IDAutomationSYC39M Demo Sym", 16);   IDAutomationHC39M                        
                            Font oFont = new Font("IDAutomationHC39M", 16);
                            PointF point = new PointF(2f, 2f);
                            SolidBrush whiteBrush = new SolidBrush(Color.White);
                            SolidBrush blackBrush = new SolidBrush(Color.Black);
                            graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                            graphics.DrawString("*" + id + "*", oFont, blackBrush, point);
                        }
                        string jpegs = path + id + ".Png";
                        bitMap.Save(jpegs, ImageFormat.Png);

                        //bitMap.Save(memoryStream, ImageFormat.Jpeg);

                        //var a1 = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                return Json("/Content/images/Barcodedata/" + id + ".png", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("/Content/images/Barcodedata/" + id + ".png", JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public object AddLibraryBook(LibraryModels model)
        {
            bool status = false;
            string message = "Failed";
            long SlNo = 0;
            var slNo = _Entities.tb_LibraryBookSerialNumber.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (slNo != null)
            {
                SlNo = slNo.SerialNo;
            }

            for (int i = 0; i < model.bookCount; i++)
            {
                var book = _Entities.tb_LibraryBook.Create();
                SlNo = SlNo + 1;
                book.CategoryId = model.categoryId;
                book.Title = model.title;
                book.Author = model.author;
                book.Status = 0; //Available
                book.IsActive = true;
                book.TimeStamp = CurrentTime;
                book.SerialNumber = SlNo;
                book.RandomNumber = RandomNumber();
                _Entities.tb_LibraryBook.Add(book);
                status = _Entities.SaveChanges() > 0;
            }

            if (status)
            {
                var srlNo1 = _Entities.tb_LibraryBookSerialNumber.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                if (srlNo1 != null)
                {
                    srlNo1.SerialNo = SlNo;
                    status = _Entities.SaveChanges() > 0 ? true : false;

                }
                else
                {
                    var slNoTable = new tb_LibraryBookSerialNumber();
                    slNoTable.SchoolId = _user.SchoolId;
                    slNoTable.SerialNo = SlNo;
                    _Entities.tb_LibraryBookSerialNumber.Add(slNoTable);
                    status = _Entities.SaveChanges() > 0 ? true : false;
                }
            }
            message = status ? " Book Added" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult IssueLibraryBookView(string id)
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            model.bookId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_LibraryBook_Issue_Add.cshtml", model);
        }
        public object AcceptLibraryBook(string id)
        {
            bool status = false;
            string message = "Failed";
            long bookId = Convert.ToInt64(id);
            var book = _Entities.tb_LibraryBook.Where(z => z.BookId == bookId).FirstOrDefault();
            book.Status = 0;//Available

            var studentBook = _Entities.tb_LibraryBookStudent.Where(z => z.BookId == bookId && z.Status == true && z.IsActive).ToList().OrderByDescending(z => z.StudentBookId).FirstOrDefault();
            studentBook.Status = false; //accept
            studentBook.AcceptDateTime = CurrentTime;
            DateTime TodayDate = CurrentTime.Date;
            DateTime IssuedDate = studentBook.IssueDateTime.Date;

            status = _Entities.SaveChanges() > 0;
            if (status)
            {
                var dueDays = book.tb_BookCategory.tb_School.LibraryDueDays;
                if (dueDays != null || dueDays != 0)
                {
                    var fineLib = book.tb_BookCategory.tb_School.tb_LibraryFine.FirstOrDefault();
                    if (fineLib != null)
                    {
                        double diffDate = (TodayDate - IssuedDate).TotalDays;
                        if (dueDays <= diffDate)
                        {
                            var feeStudent = _Entities.tb_FeeStudent.Create();
                            feeStudent.Amount = fineLib.FineAmount;
                            feeStudent.StudentId = Convert.ToInt32(studentBook.StudentId);
                            feeStudent.FeeId = fineLib.FeeId;
                            feeStudent.FeeStudentGuid = Guid.NewGuid();
                            feeStudent.IsActive = true;
                            feeStudent.TimeStamp = CurrentTime;
                            feeStudent.DueDate = CurrentTime;
                            feeStudent.Instalment = 1;
                            _Entities.tb_FeeStudent.Add(feeStudent);
                            status = _Entities.SaveChanges() > 0;
                        }
                    }

                }
            }
            message = status ? "Book Accepted successfully" : "Failed to Accept Book";

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object IssueLibraryBook(LibraryModels model)
        {
            bool status = false;
            string message = "Failed";
            var isStudent = _Entities.tb_Student.Where(z => z.StudentSpecialId.ToUpper() == model.admissionNumber.ToUpper() && z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (isStudent != null)
            {
                var student = _Entities.tb_LibraryBookStudent.Create();
                student.StudentId = isStudent.StudentId;
                student.BookId = model.bookId;
                student.Status = true; //issue
                student.IsActive = true;
                student.IssueDateTime = CurrentTime;
                _Entities.tb_LibraryBookStudent.Add(student);
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    var book = _Entities.tb_LibraryBook.Where(z => z.BookId == model.bookId && z.IsActive).FirstOrDefault();
                    book.Status = 1; //Issued
                    _Entities.SaveChanges();
                }
                message = status ? " Book Issued" : "Failed";
                return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                message = "Admission number does not exist";
                return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public object EditLibraryBook(LibraryModels model)
        {
            bool status = false;
            string message = "Failed";
            var book = _Entities.tb_LibraryBook.Where(z => z.BookId == model.bookId).FirstOrDefault();
            book.CategoryId = model.categoryId;
            book.Title = model.title;
            book.Author = model.author;
            status = _Entities.SaveChanges() > 0;
            message = status ? " Book Edited" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteLibraryBook(string id)
        {
            bool status = false;
            string message = "Failed";
            long bookId = Convert.ToInt64(id);
            var book = _Entities.tb_LibraryBook.Where(z => z.BookId == bookId).FirstOrDefault();

            //if (_Entities.tb_LibraryBook.Any(x => x.CategoryId == catId))
            //{
            book.IsActive = false;
            status = _Entities.SaveChanges() > 0;
            message = status ? " Book deleted successfully" : "Failed to delete Book";
            //}
            //else
            //{
            //    _Entities.tb_BookCategory.Remove(category);
            //    status = _Entities.SaveChanges() > 0 ? true : false;
            //    message = status ? "Category deleted successfully!" : "Failed to delete Category!";
            //}

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult BookListPartial()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_LibraryBook_Grid.cshtml", model);

        }


        public PartialViewResult EditLibraryBookView(string id)
        {
            LibraryModels model = new LibraryModels();
            long bookId = Convert.ToInt64(id);
            var book = _Entities.tb_LibraryBook.Where(z => z.BookId == bookId).FirstOrDefault();
            model.schoolId = _user.SchoolId;
            model.categoryId = book.CategoryId;
            model.title = book.Title;
            model.author = book.Author;
            model.status = book.Status;
            return PartialView("~/Views/School/_pv_LibraryBook_Edit.cshtml", model);
        }
        public IActionResult LibraryBookStudent(string id)
        {
            long bookid = Convert.ToInt64(id);
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            model.bookId = bookid;
            return View(model);
        }


        public IActionResult LibraryBookDue()
        {
            LibraryModels model = new LibraryModels();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        #endregion

        #region Calendar

        public IActionResult CalendarEvent()
        {
            CalendarEventModels model = new CalendarEventModels();
            model.schoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return View(model);
        }
        public PartialViewResult AddCalendarEventView()
        {
            return PartialView("~/Views/School/_pv_CalendarEvent_Add.cshtml");
        }
        [HttpPost]
        public object AddCalendarEvent(CalendarEventModels model)
        {
            bool status = false;
            string message = "Failed";
            var calender = _Entities.tb_CalenderEvent.Create();
            calender.EventHead = model.eventHead;
            calender.EventDetails = model.eventDetails;

            string[] splitData = model.eventDate.Split('-');
            var zdd = splitData[0];
            var zmm = splitData[1];
            var zyyyy = splitData[2];
            var Date = zmm + '-' + zdd + '-' + zyyyy;
            calender.EventDate = Convert.ToDateTime(Date);
            calender.SchoolId = _user.SchoolId;
            calender.IsActive = true;
            calender.TimeStamp = CurrentTime;
            _Entities.tb_CalenderEvent.Add(calender);
            status = _Entities.SaveChanges() > 0;
            message = status ? " Event Added" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditCalendarEventView(string id)
        {
            CalendarEventModels model = new CalendarEventModels();
            long eventId = Convert.ToInt64(id);
            var calender = _Entities.tb_CalenderEvent.Where(z => z.EventId == eventId).FirstOrDefault();
            model.eventId = calender.EventId;
            model.eventDate = calender.EventDate.ToString("dd-MM-yyyy");
            model.eventHead = calender.EventHead;
            model.eventDetails = calender.EventDetails;
            return PartialView("~/Views/School/_pv_CalendarEvent_Edit.cshtml", model);
        }
        [HttpPost]
        public object EditCalendarEvent(CalendarEventModels model)
        {
            bool status = false;
            string message = "Failed";
            var calender = _Entities.tb_CalenderEvent.Where(z => z.EventId == model.eventId).FirstOrDefault();
            string[] splitData = model.eventDate.Split('-');
            var zdd = splitData[0];
            var zmm = splitData[1];
            var zyyyy = splitData[2];
            var Date = zmm + '-' + zdd + '-' + zyyyy;
            calender.EventDate = Convert.ToDateTime(Date);
            calender.EventHead = model.eventHead;
            calender.EventDetails = model.eventDetails;
            status = _Entities.SaveChanges() > 0;
            message = status ? " Event Edited" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteCalendarEvent(string id)
        {
            bool status = false;
            string message = "Failed";
            long eventId = Convert.ToInt64(id);
            var calender = _Entities.tb_CalenderEvent.Where(z => z.EventId == eventId).FirstOrDefault();
            _Entities.tb_CalenderEvent.Remove(calender);
            status = _Entities.SaveChanges() > 0 ? true : false;
            message = status ? "Event deleted successfully!" : "Failed to delete Event!";

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult CalendarEventListPartial()
        {
            CalendarEventModels model = new CalendarEventModels();
            model.schoolId = _user.SchoolId;
            model.startDate = CurrentTime;
            model.endDate = CurrentTime;
            return PartialView("~/Views/School/_pv_CalendarEvent_list.cshtml", model);

        }
        public PartialViewResult SearchCalendarEvent(string id)
        {
            string[] splitData = id.Split('~');
            DateTime startTime = Convert.ToDateTime(splitData[0]);
            DateTime endTime = Convert.ToDateTime(splitData[1]);
            CalendarEventModels model = new CalendarEventModels();
            model.schoolId = _user.SchoolId;
            model.startDate = startTime;
            model.endDate = endTime;
            return PartialView("~/Views/School/_pv_CalendarEvent_list.cshtml", model);

        }

        #endregion

        #region PromoteStudent 
        public IActionResult PromoteStudent()
        {
            StudentModel model = new StudentModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        #endregion

        public object AddStudent(StudentModel model)
        {
            Random rnd = new Random();
            bool status = false;
            string msg = string.Empty;
            // var school = Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            tb_Student stud = new tb_Student();
            stud.SchoolId = _user.SchoolId;
            stud.StudentSpecialId = model.admissionNo; //"ST" + RandomStringGenerator.RandomString() + rnd.Next(1, 100);
            stud.StundentName = model.studentName;
            stud.ParentName = model.parentName;
            stud.ParentEmail = model.parentEmail;
            stud.Address = model.address;
            stud.City = model.city;
            stud.ContactNumber = model.contactNo;
            stud.Gender = model.gender;
            stud.BloodGroup = model.bloodGroup;
            stud.BioNumber = model.biometricId;
            if (model.IsSmartPhoneUser == "0")
                stud.IsSamrtPhoneUser = false;
            else
                stud.IsSamrtPhoneUser = true;
            try
            {
                if (model.DOBstring != string.Empty && model.DOBstring != null)
                {
                    string[] splitData = model.DOBstring.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var dob = mm + '-' + dd + '-' + yyyy;
                    stud.DOB = Convert.ToDateTime(dob);
                }
            }
            catch
            {

            }
            // stud.ClasssNumber = _user.tb_School.School;
            stud.ClassId = model.classId;
            stud.DivisionId = model.divisionId;
            string division = model.divisionId.ToString();
            string classId = model.classId.ToString();
            stud.BusId = model.BusId == null ? Convert.ToInt64(1) : Convert.ToInt64(model.BusId);
            stud.TripNo = model.tripNumber;
            stud.TimeStamp = CurrentTime;
            stud.StudentGuid = Guid.NewGuid();
            stud.IsActive = true;
            // stud.ParentId = false;
            stud.State = model.state;
            //Profile Pic
            if (model.profilePic != null)
            {
                string folderPath = Server.MapPath("~/Media/Student/Profile/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var imageString = model.profilePic.Substring(model.profilePic.IndexOf(',') + 1);
                byte[] imageByte = Convert.FromBase64String(imageString);
                string imageName = Guid.NewGuid().ToString() + ".jpeg";
                var imgFilePath = Server.MapPath("~/Media/Student/Profile/" + imageName);
                var fileSave = "/Media/Student/Profile/" + imageName;

                using (var imageFile = new FileStream(imgFilePath, FileMode.Create))
                {
                    imageFile.Write(imageByte, 0, imageByte.Length);
                    imageFile.Flush();
                    stud.FilePath = fileSave;
                }
            }
            //Profile Pic
            _Entities.tb_Student.Add(stud);
            status = _Entities.SaveChanges() > 0 ? true : false;
            string studentId = stud.StudentId.ToString();
            msg = status ? "Student added successfully!" : "Failed to add Student!";
            return Json(new { status = status, msg = msg, division = division, classId = classId, studentId = studentId }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetUserListByDivGrid(string id)
        {
            SchoolModel model = new SchoolModel();
            model.divisionId = Convert.ToInt32(id);
            return PartialView("~/Views/School/_pv_Student_ByDivision_Grid.cshtml", model);

        }
        public PartialViewResult GetUserListByClassDiv(string id)
        {
            string[] splitData = id.Split('~');
            SchoolModel model = new SchoolModel();
            model.classId = Convert.ToInt64(splitData[0]);
            model.divisionId = Convert.ToInt64(splitData[1]);
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_Student_ByDivision_Grid.cshtml", model);

        }
        public PartialViewResult GetUserListByClassBilling(string id)
        {
            string[] splitData = id.Split('~');
            SchoolModel model = new SchoolModel();
            model.classId = Convert.ToInt64(splitData[0]);
            model.divisionId = Convert.ToInt64(splitData[1]);
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_Billing_UserByDivision_Grid.cshtml", model);

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
            DateTime BillDate = model.TimeStamp;
            if (model.TimeStamp.ToString("MM-dd-YYYY") == CurrentTime.ToString("MM-dd-YYYY"))
            {
                BillDate = CurrentTime;
            }
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
            var studDetails = _Entities.tb_Student.Where(z => z.StudentId == StudentId && z.IsActive == true).FirstOrDefault();
            long thisBillVoucherNumber = 0;
            var vouchrTbl = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (vouchrTbl == null)
            {
                var voucherNumber = new tb_VoucherNumber();
                voucherNumber.SchoolId = _user.SchoolId;
                voucherNumber.PaymentVoucher = 1;
                voucherNumber.ReceiptVoucher = 1;
                voucherNumber.ContraVoucher = 1;
                voucherNumber.IsActive = true;
                voucherNumber.TimeStamp = CurrentTime;
                _Entities.tb_VoucherNumber.Add(voucherNumber);
                _Entities.SaveChanges();
                thisBillVoucherNumber = voucherNumber.ReceiptVoucher;
            }
            else
            {
                thisBillVoucherNumber = vouchrTbl.ReceiptVoucher;
            }
            long headIdBill = 0;
            var accountHead = _Entities.tb_AccountHead.Where(x => x.SchoolId == _user.SchoolId && x.ForBill == true).FirstOrDefault();
            if (accountHead == null)
            {
                var AccountHead = new tb_AccountHead();
                AccountHead.AccHeadName = "Fee Income";
                AccountHead.ForBill = true;
                AccountHead.SchoolId = _user.SchoolId;
                AccountHead.IsActive = true;
                AccountHead.TimeStamp = CurrentTime;
                _Entities.tb_AccountHead.Add(AccountHead);
                _Entities.SaveChanges();
                headIdBill = AccountHead.AccountId;
            }
            else
                headIdBill = accountHead.AccountId;

            var previousBalanceAmount = _Entities.tb_StudentBalance.Where(z => z.StudentId == StudentId && z.IsActive).FirstOrDefault();//Archana 30-11-2018
            decimal prevAdv = 0;
            if (previousBalanceAmount != null)
                prevAdv = previousBalanceAmount.Amount;
            bool UpdateVoucher = false;
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
                payment.BillType = Convert.ToInt32(splitData[6]);
                int isAmountEdit = Convert.ToInt16(splitData[5]);

                if (isAmountEdit != 0)
                {
                    var paymentList = new TrackTap.DataLibrary.Data.Student(StudentId).GetStudentPaymentFees().OrderBy(z => z.DueDate).ToList();
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
                            due.ParentGuid = payment.FeeGuid;
                            due.BillNo = BillNo;
                            _Entities.tb_FeeDues.Add(due);
                            // status = _Entities.SaveChanges() > 0 ? true : false;
                        }
                    }
                }

                payment.Amount = paymentAmount;
                sumAmt = sumAmt + payment.Amount;
                payment.PaymentMode = model.PaymentType;
                if (model.PaymentType == 2)
                {
                    payment.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                    payment.BankId = model.BankId;
                    payment.ChequeNumber = model.ChequeNumber;
                }
                else
                {
                    payment.BankId = 0;
                }
                payment.BillNo = BillNo;
                payment.IsPaid = true;
                payment.PaymentType = 1;
                payment.PaymentGuid = PaymentGuid;
                payment.StudentId = StudentId;
                payment.ClassId = ClassId;
                payment.SchoolId = SchoolId;
                payment.TimeStamp = BillDate;
                payment.IsActive = true;
                // Archana (12-12-2018)
                // Checking the bill have a partial paid balance payment, then the bill wants to save the first bill no 
                try
                {
                    long previousBillno = Convert.ToInt64(splitData[8]);
                    payment.PartialPaidParentBillNo = previousBillno;
                }
                catch
                {
                    payment.PartialPaidParentBillNo = 0;
                }
                payment.IssuedPerson = _user.UserId;
                _Entities.tb_Payment.Add(payment);
                status = _Entities.SaveChanges() > 0 ? true : false;

                try
                {
                    var d = BillDate.ToString("MM-dd-yyyy");
                    DateTime todayDate = Convert.ToDateTime(d);
                    var incDetail = _Entities.tb_Income.Where(z => z.AccountHead == "Fee Collected" && z.Date == todayDate && z.SchoolId == _user.SchoolId && z.IsActive).FirstOrDefault();
                    if (incDetail != null)
                    {
                        double? payAmt = Convert.ToDouble(paymentAmount);
                        double? amt = incDetail.Amount;
                        payAmt = payAmt + amt;
                        incDetail.Amount = Convert.ToDouble(payAmt);
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                    else
                    {
                        var income = new tb_Income();
                        income.AccountHead = "Fee Collected";
                        income.Amount = Convert.ToDouble(paymentAmount);
                        income.Particular = "Fee Income";
                        income.SchoolId = _user.SchoolId;
                        income.IsActive = true;
                        income.Date = todayDate;
                        _Entities.tb_Income.Add(income);
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                }
                catch (Exception ex)
                {

                }

                #region Account Sction

                if (prevAdv < Convert.ToDecimal(paymentAmount))
                {
                    decimal currentPaiedAmountPerItem = Convert.ToDecimal(paymentAmount) - prevAdv;
                    #region The Payment mode is Cash
                    if (model.PaymentType == 1)// Cash
                    {
                        UpdateVoucher = true;
                        var cashEntry = new tb_CashEntry();
                        if (vouchrTbl != null)
                            cashEntry.VoucherNumber = thisBillVoucherNumber.ToString();
                        else
                            cashEntry.VoucherNumber = "1";
                        cashEntry.BillNo = BillNo.ToString();
                        cashEntry.VoucherType = "RV";
                        cashEntry.TransactionType = "R";
                        cashEntry.Amount = currentPaiedAmountPerItem;
                        cashEntry.HeadId = headIdBill;
                        cashEntry.SubId = feeId;
                        cashEntry.Narration = "Fee Paid " + studDetails.StundentName;
                        cashEntry.EnterDate = BillDate;
                        cashEntry.UserId = _user.UserId;
                        cashEntry.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                        cashEntry.CancelStatus = false;
                        cashEntry.SchoolId = _user.SchoolId;
                        cashEntry.Migration = false;
                        cashEntry.IsActive = true;
                        cashEntry.TimeStamp = CurrentTime;
                        if (cashEntry.EnterDate.Date == CurrentTime.Date)
                            cashEntry.EditStatus = "N";
                        else if (cashEntry.EnterDate.Date < CurrentTime.Date)
                            cashEntry.EditStatus = "P";
                        else
                            cashEntry.EditStatus = "F";
                        cashEntry.ReverseStatus = false;
                        cashEntry.AdvanceStatus = false;
                        _Entities.tb_CashEntry.Add(cashEntry);
                        _Entities.SaveChanges();

                        #region Data added to Balance table for Account
                        int sourceId = Convert.ToInt32(DataFromStatus.Cash);
                        var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == 0).FirstOrDefault();
                        if (balance != null)
                        {
                            balance.Closing = balance.Closing + currentPaiedAmountPerItem;
                            balance.TimeStamp = CurrentTime;
                            _Entities.SaveChanges();
                        }
                        else
                        {
                            try
                            {
                                var balanceEntry = new tb_Balance();
                                balanceEntry.SchoolId = _user.SchoolId;
                                balanceEntry.CurrentDate = BillDate;
                                balanceEntry.SourceId = sourceId;
                                DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == 0 && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                if (yesterday.Year != 0001)
                                    balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == 0).ToList().Sum(x => x.Closing);
                                else
                                    balanceEntry.Opening = 0;
                                balanceEntry.Closing = balanceEntry.Opening + currentPaiedAmountPerItem;
                                balanceEntry.IsActive = true;
                                balanceEntry.BankId = 0;
                                balanceEntry.TimeStamp = CurrentTime;
                                _Entities.tb_Balance.Add(balanceEntry);
                                _Entities.SaveChanges();

                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        AllUpdatesInBalance(BillDate, sourceId, 0, currentPaiedAmountPerItem);

                        #endregion Data added to Balance table for Account
                    }
                    #endregion The Payment mode is Cash
                    #region The Payment mode is Bank
                    else// Bank
                    {
                        UpdateVoucher = true;
                        var bankEntry = new tb_BankEntry();
                        if (vouchrTbl != null)
                            bankEntry.VoucherNumber = thisBillVoucherNumber.ToString();
                        else
                            bankEntry.VoucherNumber = "1";
                        bankEntry.VoucherType = "RV";
                        bankEntry.BillNo = BillNo.ToString();
                        bankEntry.TransactionType = "R";
                        bankEntry.Amount = currentPaiedAmountPerItem;
                        bankEntry.ModeType = model.PaymentType;
                        if (model.PaymentType == 2)
                        {
                            bankEntry.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                            bankEntry.ChequeNumber = model.ChequeNumber;
                        }

                        bankEntry.HeadId = headIdBill;
                        bankEntry.SubId = feeId;
                        bankEntry.BankId = model.BankId;
                        bankEntry.Narration = "Fee Paid " + studDetails.StundentName;
                        bankEntry.EnterDate = BillDate;
                        bankEntry.UserId = _user.UserId;
                        bankEntry.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                        bankEntry.CancelStatus = false;
                        bankEntry.SchoolId = _user.SchoolId;
                        bankEntry.Migration = false;
                        bankEntry.IsActive = true;
                        bankEntry.TimeStamp = CurrentTime;
                        if (bankEntry.EnterDate.Date == CurrentTime.Date)
                            bankEntry.EditStatus = "N";
                        else if (bankEntry.EnterDate.Date < CurrentTime.Date)
                            bankEntry.EditStatus = "P";
                        else
                            bankEntry.EditStatus = "F";
                        _Entities.tb_BankEntry.Add(bankEntry);
                        _Entities.SaveChanges();


                        #region Data added to Balance table for Account
                        int sourceId = Convert.ToInt32(DataFromStatus.Bank);
                        var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == model.BankId).FirstOrDefault();
                        if (balance != null)
                        {
                            balance.Closing = balance.Closing + currentPaiedAmountPerItem;
                            balance.TimeStamp = CurrentTime;
                            _Entities.SaveChanges();
                        }
                        else
                        {
                            var balanceEntry = new tb_Balance();
                            balanceEntry.SchoolId = _user.SchoolId;
                            balanceEntry.CurrentDate = BillDate;
                            balanceEntry.SourceId = sourceId;
                            DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == model.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                            if (yesterday.Year != 0001)
                                balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                            else
                                balanceEntry.Opening = 0;
                            balanceEntry.Closing = balanceEntry.Opening + currentPaiedAmountPerItem;
                            balanceEntry.IsActive = true;
                            balanceEntry.BankId = model.BankId;
                            balanceEntry.TimeStamp = CurrentTime;
                            _Entities.tb_Balance.Add(balanceEntry);
                            _Entities.SaveChanges();
                        }

                        AllUpdatesInBalance(BillDate, sourceId, model.BankId, currentPaiedAmountPerItem);
                        #endregion Data added to Balance table for Account
                    }
                    #endregion The Payment mode is Bank
                    prevAdv = 0;//Here clear the all previous amount ,becuse it will reduced the current fee amount
                }
                else
                {
                    prevAdv = prevAdv - Convert.ToDecimal(paymentAmount);
                }
                #endregion Account Sction

            }
            if (UpdateVoucher == true)
            {
                var vouchrTbl2 = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                vouchrTbl2.ReceiptVoucher = vouchrTbl2.ReceiptVoucher + 1;
                _Entities.SaveChanges();
            }

            var billNo1 = _Entities.tb_PaymentBillNo.Where(z => z.SchoolId == SchoolId).FirstOrDefault();
            if (billNo1 != null)
            {
                billNo.BillNo = BillNo;
                status = _Entities.SaveChanges() > 0 ? true : false;
            }
            try

            {
                decimal payableAmount = 0;
                decimal bal = 0;
                decimal prevBal = 0;
                //bool balAndCash = false;
                bool ispayable = false;

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
                    studentPaidsAmount.AddAccountStatus = false;
                    studentPaidsAmount.IsActive = true;
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
                #region Archana : Is the student pay more amount than he payable ?
                decimal currentPayableAmt = tempSumTotal - prevBal; // Fee Item Total - Previous Amount
                if (currentPayableAmt > 0)// The Student wnats to pay now
                {
                    decimal advance = TotalAmountPaid - currentPayableAmt;// The actual bill amount after the previous advance
                    long subId = 0;
                    if (advance > 0)
                    {
                        var head = _Entities.tb_AccountHead.Where(x => x.SchoolId == _user.SchoolId && x.IsActive && x.ForBill == true).FirstOrDefault();
                        if (head != null)
                        {
                            var sub = _Entities.tb_SubLedgerData.Where(x => x.AccHeadId == head.AccountId && x.IsActive).FirstOrDefault();
                            if (sub == null)
                            {
                                var subAdd = new tb_SubLedgerData();
                                subAdd.SubLedgerName = "Advance Amount";
                                subAdd.AccHeadId = head.AccountId;
                                subAdd.IsActive = true;
                                subAdd.TimeStamp = CurrentTime;
                                _Entities.tb_SubLedgerData.Add(subAdd);
                                _Entities.SaveChanges();
                                subId = subAdd.LedgerId;
                            }
                            else
                            {
                                subId = sub.LedgerId;
                            }

                            if (model.PaymentType == 1)// Cash
                            {
                                var advCash = new tb_CashEntry();
                                advCash.VoucherNumber = thisBillVoucherNumber.ToString();
                                advCash.VoucherType = "RV";
                                advCash.BillNo = "";
                                advCash.TransactionType = "R";
                                advCash.Amount = advance;
                                advCash.HeadId = head.AccountId;
                                advCash.SubId = subId;
                                advCash.Narration = "Advance Paid " + studDetails.StundentName;
                                advCash.EnterDate = BillDate;
                                advCash.UserId = _user.UserId;
                                advCash.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advCash.CancelStatus = false;
                                advCash.SchoolId = _user.SchoolId;
                                advCash.Migration = false;
                                advCash.IsActive = true;
                                advCash.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advCash.EditStatus = "P";
                                else
                                    advCash.EditStatus = "N";
                                advCash.ReverseStatus = false;
                                advCash.AdvanceStatus = false;
                                _Entities.tb_CashEntry.Add(advCash);
                                _Entities.SaveChanges();




                                int sourceId = Convert.ToInt32(DataFromStatus.Cash);
                                var balanceNow = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == 0).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + advance;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_Balance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + advance;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_Balance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, advance);

                            }
                            else
                            {
                                var advBank = new tb_BankEntry();
                                advBank.VoucherNumber = thisBillVoucherNumber.ToString();
                                advBank.VoucherType = "RV";
                                advBank.BillNo = "";
                                advBank.TransactionType = "R";
                                advBank.Amount = advance;
                                advBank.ModeType = model.PaymentType;
                                if (model.PaymentType == 2)
                                {
                                    advBank.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                                    advBank.ChequeNumber = model.ChequeNumber;
                                }
                                advBank.HeadId = head.AccountId;
                                advBank.SubId = subId;
                                advBank.BankId = model.BankId;
                                advBank.Narration = "Advance Paid " + studDetails.StundentName;
                                advBank.EnterDate = BillDate;
                                advBank.UserId = _user.UserId;
                                advBank.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advBank.CancelStatus = false;
                                advBank.SchoolId = _user.SchoolId;
                                advBank.Migration = false;
                                advBank.IsActive = true;
                                advBank.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advBank.EditStatus = "P";
                                else
                                    advBank.EditStatus = "N";
                                _Entities.tb_BankEntry.Add(advBank);
                                _Entities.SaveChanges();


                                int sourceId = Convert.ToInt32(DataFromStatus.Bank);
                                var balanceNow = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == model.BankId).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + advance;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_Balance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == model.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + advance;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_Balance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, advance);

                            }
                            if (UpdateVoucher == false)
                            {
                                var vouchrTbl2 = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                                vouchrTbl2.ReceiptVoucher = vouchrTbl2.ReceiptVoucher + 1;
                                _Entities.SaveChanges();
                            }
                        }
                    }
                }
                else // The student does not want to pay , the all bill amount will satisfies the previous advance amount
                {
                    if (TotalAmountPaid > 0)//The student paid amount ,when their is no need to paid
                    {
                        long subId = 0;
                        var head = _Entities.tb_AccountHead.Where(x => x.SchoolId == _user.SchoolId && x.IsActive && x.ForBill == true).FirstOrDefault();
                        if (head != null)
                        {
                            var sub = _Entities.tb_SubLedgerData.Where(x => x.AccHeadId == head.AccountId && x.IsActive).FirstOrDefault();
                            if (sub == null)
                            {
                                var subAdd = new tb_SubLedgerData();
                                subAdd.SubLedgerName = "Advance Amount";
                                subAdd.AccHeadId = head.AccountId;
                                subAdd.IsActive = true;
                                subAdd.TimeStamp = CurrentTime;
                                _Entities.tb_SubLedgerData.Add(subAdd);
                                _Entities.SaveChanges();
                                subId = subAdd.LedgerId;
                            }
                            else
                            {
                                subId = sub.LedgerId;
                            }
                            if (model.PaymentType == 1)// Cash
                            {
                                var advCash = new tb_CashEntry();
                                advCash.VoucherNumber = thisBillVoucherNumber.ToString();
                                advCash.VoucherType = "RV";
                                advCash.BillNo = "";
                                advCash.TransactionType = "R";
                                advCash.Amount = TotalAmountPaid;
                                advCash.HeadId = head.AccountId;
                                advCash.SubId = subId;
                                advCash.Narration = "Advance Paid " + studDetails.StundentName;
                                advCash.EnterDate = BillDate;
                                advCash.UserId = _user.UserId;
                                advCash.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advCash.CancelStatus = false;
                                advCash.SchoolId = _user.SchoolId;
                                advCash.Migration = false;
                                advCash.IsActive = true;
                                advCash.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advCash.EditStatus = "P";
                                else
                                    advCash.EditStatus = "N";
                                advCash.ReverseStatus = false;
                                advCash.AdvanceStatus = false;
                                _Entities.tb_CashEntry.Add(advCash);
                                _Entities.SaveChanges();


                                int sourceId = Convert.ToInt32(DataFromStatus.Cash);
                                var balanceNow = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == 0).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + TotalAmountPaid;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_Balance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == 0).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + TotalAmountPaid;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_Balance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, TotalAmountPaid);
                            }
                            else
                            {
                                var advBank = new tb_BankEntry();
                                advBank.VoucherNumber = thisBillVoucherNumber.ToString();
                                advBank.VoucherType = "RV";
                                advBank.BillNo = " ";
                                advBank.TransactionType = "R";
                                advBank.Amount = TotalAmountPaid;
                                advBank.ModeType = model.PaymentType;
                                if (model.PaymentType == 2)
                                {
                                    advBank.ChequeDate = Convert.ToDateTime(model.ChequeDate);
                                    advBank.ChequeNumber = model.ChequeNumber;
                                }
                                advBank.HeadId = head.AccountId;
                                advBank.SubId = subId;
                                advBank.BankId = model.BankId;
                                advBank.Narration = "Advance Paid " + studDetails.StundentName;
                                advBank.EnterDate = BillDate;
                                advBank.UserId = _user.UserId;
                                advBank.DataFromStatus = Convert.ToBoolean(DataFromStatus.Bill);
                                advBank.CancelStatus = false;
                                advBank.SchoolId = _user.SchoolId;
                                advBank.Migration = false;
                                advBank.IsActive = true;
                                advBank.TimeStamp = CurrentTime;
                                if (BillDate.Date > CurrentTime.Date)
                                    advBank.EditStatus = "P";
                                else
                                    advBank.EditStatus = "N";
                                _Entities.tb_BankEntry.Add(advBank);
                                _Entities.SaveChanges();


                                int sourceId = Convert.ToInt32(DataFromStatus.Bank);
                                var balanceNow = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == BillDate.Date && x.SourceId == sourceId && x.BankId == model.BankId).FirstOrDefault();
                                if (balance != null)
                                {
                                    balanceNow.Closing = balanceNow.Closing + TotalAmountPaid;
                                    balanceNow.TimeStamp = CurrentTime;
                                    _Entities.SaveChanges();
                                }
                                else
                                {
                                    var balanceEntry = new tb_Balance();
                                    balanceEntry.SchoolId = _user.SchoolId;
                                    balanceEntry.CurrentDate = BillDate;
                                    balanceEntry.SourceId = sourceId;
                                    DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < BillDate && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == model.BankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                                    if (yesterday.Year != 0001)
                                        balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == model.BankId).ToList().Sum(x => x.Closing);
                                    else
                                        balanceEntry.Opening = 0;
                                    balanceEntry.Closing = balanceEntry.Opening + TotalAmountPaid;
                                    balanceEntry.IsActive = true;
                                    balanceEntry.BankId = model.BankId;
                                    balanceEntry.TimeStamp = CurrentTime;
                                    _Entities.tb_Balance.Add(balanceEntry);
                                    _Entities.SaveChanges();
                                }
                                AllUpdatesInBalance(BillDate, sourceId, 0, TotalAmountPaid);

                            }
                            if (UpdateVoucher == false)
                            {
                                var vouchrTbl2 = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                                vouchrTbl2.ReceiptVoucher = vouchrTbl2.ReceiptVoucher + 1;
                                _Entities.SaveChanges();
                            }
                        }
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {


            }

            var dateTime = BillDate.ToString("dd-MMM-yyyy");

            var description = "failed";
            #region Email
            try
            {
                var smtpDetails = _Entities.tb_SMTPDetail.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                var paidAmount = Convert.ToInt32(payment.Amount);
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/email/FeePayment.html");
                var emailTemplate = System.IO.File.ReadAllText(filePath);
                var mailBody = emailTemplate.Replace("{{schoolname}}", studDetails.tb_School.SchoolName)
                   .Replace("{{parent}}", studDetails.ParentName)
                .Replace("{{student}}", studDetails.StundentName)
                .Replace("{{amount}}", string.Format("{0:0.00}", sumAmt))
                .Replace("{{date}}", dateTime);
                Mail.Send("School Fee Payment", mailBody, studDetails.ParentName, smtpDetails.EmailId, smtpDetails.Password, new System.Collections.ArrayList { studDetails.ParentEmail });
                description = "success";
            }
            catch
            {
                description = "Something went wrong";
            }
            #endregion Email
            var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
            if (package != null)
            {
                try
                {
                    if (model.TimeStamp.ToString("MM-dd-YYYY") == CurrentTime.ToString("MM-dd-YYYY"))
                    {

                        #region  SMS 
                        HttpClient client = new HttpClient();
                        var history = new tb_SmsHistory();
                        var numbers = new List<string>();
                        var MsgId = new List<string>();
                        var numb = "";
                        string messagepre = "";
                        var senderName = "MYSCHO";

                        var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true).FirstOrDefault();
                        if (senderData != null)
                            senderName = senderData.SenderId;
                        message = "success";
                        status = true;
                        var smsHead = new tb_SmsHead();
                        smsHead.Head = "BillDate Payment " + studDetails.StundentName;
                        smsHead.SchoolId = _user.SchoolId;
                        smsHead.TimeStamp = CurrentTime;
                        smsHead.IsActive = true;
                        smsHead.SenderType = (int)SMSSendType.Student;
                        _Entities.tb_SmsHead.Add(smsHead);
                        status = _Entities.SaveChanges() > 0;


                        messagepre = "Dear Parent of " + studDetails.StundentName + ", you have paid Rs." + string.Format("{0:0.00}", sumAmt) + " on " + dateTime;

                        var phone = studDetails.ContactNumber.ToString();
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
                            sms.StuentId = StudentId;
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
                        #endregion  SMS 
                    }
                }
                catch (Exception ex)
                {
                    var x = ex.InnerException;
                }
            }
            return Json(new { status = status, serialNo = BillNo, msg = status ? "Bill Paid Sucessfully" : "Failed To Pay Bill" }, JsonRequestBehavior.AllowGet);
        }

        private void AllUpdatesInBalance(DateTime billDate, int sourceId, long BankId, decimal amount)
        {
            if (sourceId == Convert.ToInt32(DataFromStatus.Cash))
            {
                var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.IsActive == true && x.BankId == 0 && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) > billDate.Date).ToList();
                if (balance != null && balance.Count > 0)
                {
                    foreach (var item in balance)
                    {
                        item.Opening = item.Opening + amount;
                        item.Closing = item.Closing + amount;
                        _Entities.SaveChanges();
                    }
                }
            }
            else
            {
                var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && x.SourceId == sourceId && x.BankId == BankId && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) > billDate.Date).ToList();
                if (balance != null && balance.Count > 0)
                {
                    foreach (var item in balance)
                    {
                        item.Opening = item.Opening + amount;
                        item.Closing = item.Closing + amount;
                        _Entities.SaveChanges();
                    }
                }
            }
        }

        private void AllUpdatesInBalanceCancel(DateTime billDate, int sourceId, long BankId, decimal amount)
        {
            if (sourceId == Convert.ToInt32(DataFromStatus.Cash))
            {
                var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.IsActive == true && x.BankId == 0 && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) >= billDate.Date).ToList();
                if (balance != null && balance.Count > 0)
                {
                    foreach (var item in balance)
                    {
                        if (item.CurrentDate.Date == billDate.Date)
                        {
                            item.Closing = item.Closing + amount;
                        }
                        else
                        {
                            item.Opening = item.Opening + amount;
                            item.Closing = item.Closing + amount;
                        }
                        _Entities.SaveChanges();
                    }
                }
            }
            else
            {
                var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && x.SourceId == sourceId && x.BankId == BankId && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) >= billDate.Date).ToList();
                if (balance != null && balance.Count > 0)
                {
                    foreach (var item in balance)
                    {
                        if (item.CurrentDate.Date == billDate.Date)
                        {
                            item.Closing = item.Closing + amount;
                        }
                        else
                        {
                            item.Opening = item.Opening + amount;
                            item.Closing = item.Closing + amount;
                        }
                        _Entities.SaveChanges();
                    }
                }
            }
        }
        public IActionResult Fee()
        {
            var model = new AddClassFees();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult AddFeePartialView()
        {
            var model = new AddFee();
            model.SchoolId = _user.SchoolId;
            int year = CurrentTime.Year;   //Archana
            DateTime firstDay = new DateTime(year, 1, 1);
            model.DueDateString = firstDay.ToString("dd-MM-yyyy");
            model.EndDateString = firstDay.ToString("dd-MM-yyyy");
            model.HaveFineDate = CurrentTime.AddDays(1);
            model.HaveFineDateString = model.HaveFineDate.ToShortDateString();
            return PartialView("~/Views/School/_pv_AddFee.cshtml", model);
        }

        [HttpPost]
        public object AddFee(AddFee model)
        {
            DateTime dueDate = ConvertDateToServer(model.DueDateString);
            DateTime endDate = ConvertDateToServer(model.EndDateString);

            bool status = false;
            string message = "Failed";
            if (_Entities.tb_Fee.Any(z => z.FeesName.ToLower() == model.FeeName.ToLower() && z.IsActive && z.SchoolId == _user.SchoolId))
            {
                message = "Fee Already Added";
            }
            else
            {
                var fee = _Entities.tb_Fee.Create();
                fee.FeesName = model.FeeName;
                fee.FeeType = model.FeeType;
                fee.IsActive = true;
                fee.Interval = model.Interval;
                fee.SchoolId = model.SchoolId;
                fee.TimeStamp = CurrentTime;
                if (model.IsReccuring == 1)
                {
                    if (model.Interval > 1)
                    {
                        fee.FeeStartDate = dueDate;
                    }
                    else
                        fee.FeeStartDate = CurrentTime;
                }
                else if (model.IsDueDate == 1)
                {
                    fee.FeeStartDate = endDate;
                }
                else
                {
                    fee.FeeStartDate = CurrentTime;
                }

                //--------------Archana new fine calculation-----------------
                try
                {
                    if (model.HaveFineDateString != string.Empty && model.HaveFineDateString != null)
                    {
                        string[] splitData = model.HaveFineDateString.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var date = mm + '-' + dd + '-' + yyyy;
                        fee.DueDate = Convert.ToDateTime(date);
                    }
                }
                catch
                {

                }
                fee.FineAmount = model.FineAmount;
                fee.NoOfDays = model.FineDays;
                //--------------Archana new fine calculation-----------------

                _Entities.tb_Fee.Add(fee);
                status = _Entities.SaveChanges() > 0;
                message = status ? " Fee Added" : "failed";
            }
            return Json(new { status = status, msg = message, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshSchoolFees(model.SchoolId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public object AddFeeClass(AddClassFees model)
        {
            bool status = false;
            string message = "Failed";

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //List<Datalist> routes_list =
            //         (List<Datalist>)json_serializer.DeserializeObject(model.DataList);

            List<Datalist> routes_list = new JavaScriptSerializer().Deserialize<List<Datalist>>(model.DataList);
            long FeeId = Convert.ToInt64(model.FeeId);
            var FeeDetail = _Entities.tb_Fee.Where(z => z.FeeId == FeeId && z.IsActive).FirstOrDefault();
            foreach (var value in routes_list)
            {
                DateTime dueDt = FeeDetail.FeeStartDate;
                var feeclass = _Entities.tb_FeeClass.Create();
                if (FeeDetail.Interval > 1)
                {
                    var addMonth = 12 / FeeDetail.Interval;
                    for (int i = 1; i <= FeeDetail.Interval; i++)
                    {
                        feeclass.Amount = Convert.ToDecimal(value.amount);
                        feeclass.ClassId = value.classId;
                        feeclass.DivisionId = value.divisionId;//Archana add division in FeeClass on 21/06/2019 for KTCT School
                        feeclass.FeeClassGuid = Guid.NewGuid();
                        feeclass.FeeId = FeeId;
                        feeclass.IsActive = true;
                        feeclass.PublishStatus = true;
                        feeclass.TimeStamp = CurrentTime;
                        feeclass.DueDate = dueDt;
                        feeclass.Instalment = i;
                        dueDt = dueDt.AddMonths(addMonth);
                        _Entities.tb_FeeClass.Add(feeclass);
                        status = _Entities.SaveChanges() > 0;
                    }
                }
                else
                {
                    feeclass.Amount = Convert.ToDecimal(value.amount);
                    feeclass.ClassId = value.classId;
                    feeclass.FeeClassGuid = Guid.NewGuid();
                    feeclass.DivisionId = value.divisionId;//Archana add division in FeeClass on 21/06/2019 for KTCT School
                    feeclass.FeeId = Convert.ToInt64(model.FeeId);
                    feeclass.IsActive = true;
                    feeclass.PublishStatus = true;
                    feeclass.TimeStamp = CurrentTime;
                    feeclass.DueDate = dueDt;
                    feeclass.Instalment = FeeDetail.Interval;
                    _Entities.tb_FeeClass.Add(feeclass);
                    status = _Entities.SaveChanges() > 0;
                }
            }
            message = status ? " Fee Added" : "failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ViewFeeClass(string id)
        {
            var model = new ViewFeeClass();
            model.SchoolId = _user.SchoolId;
            model.FeeId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_ViewFeeClass.cshtml", model);
        }
        public IActionResult FeeList()
        {
            var model = new ListFee();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult RefreshFeelistPartial()
        {
            var model = new ListFee();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_Feelist.cshtml", model);
        }

        public object DeleteFee(string id)
        {
            bool status = false;
            string message = "Failed";
            long feeId = Convert.ToInt64(id);
            var fee = _Entities.tb_Fee.FirstOrDefault(z => z.FeeId == feeId);
            if (fee != null)
                fee.IsActive = false;
            status = _Entities.SaveChanges() > 0;
            message = status ? "Fee Deleted" : "failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public object EditFee(EditFee model)
        {
            bool status = false;
            string message = "Failed";
            long feeId = Convert.ToInt64(model.feeId);
            if (_Entities.tb_Fee.Any(z => z.FeesName.ToLower() == model.feename.ToLower() && z.SchoolId == _user.SchoolId && z.FeeId != feeId && z.IsActive))
            {
                message = "Duplicate Feename";
            }
            else
            {
                var fee = _Entities.tb_Fee.FirstOrDefault(z => z.FeeId == feeId);
                if (fee != null)
                {
                    fee.FeesName = model.feename;
                    fee.FeeType = model.feeType;
                    _Entities.SaveChanges();
                    status = true;
                }
                message = status ? "Fee Edited" : "failed";
            }
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditFeePartial(string id)
        {
            var model = new EditFee();
            model.feeId = Convert.ToInt64(id);
            var fee = _Entities.tb_Fee.FirstOrDefault(z => z.FeeId == model.feeId);
            if (fee != null)
            {
                model.feename = fee.FeesName;
                model.feeType = fee.FeeType;
            }
            else
            {
                model.feename = string.Empty;
                model.feeType = Convert.ToInt32(1);
            }
            return PartialView("~/Views/School/_pv_EditFee.cshtml", model);
        }
        public IActionResult FeeClassList(string id)
        {
            var model = new FeeclassList();
            model.schoolId = _user.SchoolId;
            model.feeId = Convert.ToInt64(id);
            return View(model);
        }
        public PartialViewResult RefreshFeeClasslistPartial(string id)
        {
            var model = new FeeclassList();
            model.schoolId = _user.SchoolId;
            model.feeId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_FeeClassList.cshtml", model);
        }

        public object DeleteFeeClass(string id)
        {
            bool status = false;
            string message = "Failed";
            long feeClassId = Convert.ToInt64(id);
            var feeClass = _Entities.tb_FeeClass.FirstOrDefault(z => z.FeeClassId == feeClassId);
            if (feeClass != null)
                feeClass.IsActive = false;
            status = _Entities.SaveChanges() > 0;
            message = status ? "Deleted" : "failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult FeeClassEditPartial(string id)
        {
            var model = new EditFeeClass();
            model.feeClassId = Convert.ToInt64(id);
            var feeclass = _Entities.tb_FeeClass.FirstOrDefault(z => z.FeeClassId == model.feeClassId);
            if (feeclass != null)
            {
                model.amount = string.Format("{0:0.00}", feeclass.Amount);
                model.classname = feeclass.tb_Class.Class;
                model.feeId = feeclass.FeeId;
                model.DueDate = feeclass.DueDate;
            }
            else
            {
                model.amount = Convert.ToString(0);
                model.classname = string.Empty;
                model.feeId = feeclass.FeeId;

            }
            return PartialView("~/Views/School/_pv_FeeClassEdit.cshtml", model);
        }

        [HttpPost]
        public object EditFeeClass(EditFeeClass model)
        {
            bool status = false;
            string message = "Failed";
            long feeClassId = Convert.ToInt64(model.feeClassId);
            var feeClass = _Entities.tb_FeeClass.FirstOrDefault(z => z.FeeClassId == feeClassId);
            if (feeClass != null)
            {
                feeClass.Amount = Convert.ToDecimal(model.amount);
                feeClass.DueDate = Convert.ToDateTime(model.DueDate);
                _Entities.SaveChanges();
                status = true;
            }
            message = status ? "Edited" : "failed";
            return Json(new { status = status, msg = message, feeId = model.feeId }, JsonRequestBehavior.AllowGet);
        }

        public object DeleteClass(string id)
        {
            bool status = false;
            string message = "Failed";
            long DivisionId = Convert.ToInt64(id);
            var Division = _Entities.tb_Division.FirstOrDefault(z => z.DivisionId == DivisionId);
            if (Division != null)
            {
                Division.IsActive = false;
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    if (!_Entities.tb_Division.Any(z => z.ClassId == Division.ClassId && z.IsActive))
                    {
                        var classData = _Entities.tb_Class.FirstOrDefault(z => z.ClassId == Division.ClassId);
                        if (classData != null)
                        {
                            classData.IsActive = false;
                            _Entities.SaveChanges();
                        }
                    }
                }
            }
            message = status ? "Deleted" : "failed";
            return Json(new { status = status, msg = message, list = new TrackTap.DataLibrary.Data.DropdownData().RefreshClasses(_user.SchoolId) }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditStudentModel(string id)
        {
            var model = new EditStudentModel();
            model.studentId = Convert.ToInt64(id);
            model.schoolId = _user.SchoolId;
            var student = new TrackTap.DataLibrary.Data.Student(model.studentId);
            if (student != null)
            {
                model.address = student.Address;
                model.BusId = student.BusId.ToString();
                model.city = student.City;
                model.classId = student.ClassId;
                model.parentEmail = student.ParentEmail;
                model.className = student.ClassName;
                model.bloodGroup = student.BloodGroup;
                model.gender = student.Gender;
                model.contactNo = student.ContactNumber;
                model.division = student.DivisionName;
                model.divisionId = student.DivisionId;
                model.filePath = student.FilePath;
                model.parentName = student.ParentName;
                model.admissionNo = student.StudentSpecialId;
                model.state = student.State;
                model.tripNumber = student.TripNo;
                model.studentName = student.StundentName;
                model.biometricId = student.BioNumber;
                //model.DOBstring = student.DOB?.ToString("dd-MM-yyyy"); // 12-Jun-2018 Archana changed these line from Kathus system , becouse of an error
                model.DOBstring = student.DOB.ToString();


            }


            return PartialView("~/Views/School/_pv_EditStudentModel.cshtml", model);
        }

        [HttpPost]
        public object EditStudent(EditStudentModel model)
        {
            bool status = false;
            string msg = string.Empty;
            var stud = _Entities.tb_Student.FirstOrDefault(z => z.StudentId == model.studentId);
            if (stud != null)
            {
                stud.StundentName = model.studentName;
                stud.ParentName = model.parentName;
                stud.Address = model.address;
                stud.City = model.city;
                stud.ParentEmail = model.parentEmail;
                stud.ContactNumber = model.contactNo;
                stud.Gender = model.gender;
                stud.BloodGroup = model.bloodGroup;
                stud.BusId = model.BusId == null ? Convert.ToInt64(9) : Convert.ToInt64(model.BusId);
                stud.TripNo = model.tripNumber;
                stud.TimeStamp = CurrentTime;
                stud.StudentSpecialId = model.admissionNo;
                stud.State = model.state;
                stud.BioNumber = model.biometricId;
                //Profile Pic
                if (model.profilePic != null)
                {
                    string folderPath = Server.MapPath("~/Media/Student/Profile/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var imageString = model.profilePic.Substring(model.profilePic.IndexOf(',') + 1);
                    byte[] imageByte = Convert.FromBase64String(imageString);
                    string imageName = Guid.NewGuid().ToString() + ".jpeg";
                    var imgFilePath = Server.MapPath("~/Media/Student/Profile/" + imageName);
                    var fileSave = "/Media/Student/Profile/" + imageName;

                    using (var imageFile = new FileStream(imgFilePath, FileMode.Create))
                    {
                        imageFile.Write(imageByte, 0, imageByte.Length);
                        imageFile.Flush();
                        stud.FilePath = fileSave;
                    }
                }
                try
                {
                    if (model.DOBstring != string.Empty && model.DOBstring != null)
                    {
                        string[] splitData = model.DOBstring.Split('-');
                        var dd = splitData[0];
                        var mm = splitData[1];
                        var yyyy = splitData[2];
                        var dob = mm + '-' + dd + '-' + yyyy;
                        stud.DOB = Convert.ToDateTime(dob);
                    }
                }
                catch
                {

                }
                //Profile Pic
                status = _Entities.SaveChanges() > 0 ? true : false;
                msg = status ? "Student edited successfully!" : "Failed to add Student!";
            }
            return Json(new { status = status, msg = msg, division = stud == null ? model.divisionId.ToString() : stud.DivisionId.ToString() }, JsonRequestBehavior.AllowGet);
        }

        public object DeleteStudent(string id)
        {
            bool status = false;
            string message = "Failed";
            long studentId = Convert.ToInt64(id);
            var student = _Entities.tb_Student.FirstOrDefault(z => z.StudentId == studentId);
            if (student != null)
            {
                student.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? "Deleted" : "failed";
            return Json(new { status = status, msg = message, division = student == null ? Convert.ToString(0) : student.DivisionId.ToString() }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult DiscountAssignClassList()
        {
            var model = new FilterModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult FeeDiscount()
        {
            var model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult DiscountDetails(string id)
        {
            var model = new DiscountDetails();
            model.StudentId = Convert.ToInt64(id);
            return View(model);
        }
        public object DeleteDiscount(string id)
        {
            bool status = false;
            string message = "Failed";
            string[] splitData = id.Split('~');
            long studentId = Convert.ToInt32(splitData[0]);
            long feeId = Convert.ToInt32(splitData[1]);


            var discount = _Entities.tb_FeeDiscount.Where(z => z.StudentId == studentId && z.IsActive && z.FeeId == feeId).FirstOrDefault();
            if (discount != null)
            {
                discount.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? " Discount deleted" : "Failed to delete discount";

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult RefreshFeeDiscountGrid()
        {
            var model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_DiscountStudent_Grid.cshtml", model);

        }
        public PartialViewResult EditFeeDiscountStudView(string id)
        {
            var model = new FeeModel();
            string[] splitData = id.Split('~');
            model.StudentId = Convert.ToInt32(splitData[0]);
            model.FeeId = Convert.ToInt32(splitData[1]);
            var discount = _Entities.tb_FeeDiscount.Where(z => z.StudentId == model.StudentId && z.IsActive && z.FeeId == model.FeeId).FirstOrDefault();
            model.FeeName = discount.tb_Fee.FeesName;
            model.StudentName = discount.tb_Student.StundentName;
            model.DiscountAmount = Convert.ToDecimal(String.Format("{0:0.00}", discount.DiscountAmount));
            return PartialView("~/Views/School/_pv_EditFeeDiscountStudent_model.cshtml", model);

        }
        [HttpPost]
        public object EditFeeStudentDiscount(FeeModel model)
        {
            bool status = false;
            string message = "Failed";
            long studentId = model.StudentId;
            long feeId = model.FeeId;
            var discount = _Entities.tb_FeeDiscount.Where(z => z.StudentId == studentId && z.IsActive && z.FeeId == feeId).FirstOrDefault();
            if (discount != null)
            {
                discount.DiscountAmount = model.DiscountAmount;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? " Discount edit" : "Failed to edit discount";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetDiscountSyudentsByDivGrid(string id)
        {
            SchoolModel model = new SchoolModel();
            model.divisionId = Convert.ToInt32(id);
            return PartialView("~/Views/School/_pv_DiscountStudent_ByDivision_Grid.cshtml", model);

        }
        public IActionResult AssignDiscount(int id)
        {
            long divisionId = Convert.ToInt32(id);
            var model = new FeeModel();
            model.DivisionId = divisionId;
            model.SchoolId = _user.SchoolId;
            return View(model);
        }


        [HttpPost]
        public object AssignDiscount(FeeModel model)
        {
            bool status = false;
            string message = "Failed";
            List<string> studentsUserId = model.FeeStudentId.Split(',').ToList();

            var discount = new tb_FeeDiscount();
            foreach (var userId in studentsUserId)
            {
                long userIdLong = Convert.ToInt32(userId);
                var isDiscount = _Entities.tb_FeeDiscount.Where(z => z.StudentId == userIdLong && z.FeeId == model.FeeId && z.IsActive).FirstOrDefault();
                if (isDiscount == null)
                {
                    discount.StudentId = userIdLong;
                    discount.FeeId = model.FeeId;
                    discount.DiscountAmount = model.Amount;
                    discount.TimeStamp = CurrentTime;
                    discount.IsActive = true;
                    _Entities.tb_FeeDiscount.Add(discount);
                    status = _Entities.SaveChanges() > 0;
                    message = status ? " Discount added" : "Failed to add discount";
                }

            }
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetDiscountSyudentsByFeeId(string id)
        {
            //  string[] splitData = id.Split('~');

            FeeModel model = new FeeModel();
            model.SchoolId = _user.SchoolId;
            model.FeeId = Convert.ToInt32(id);
            return PartialView("~/Views/School/_pv_DiscountStudent_ByFee_Grid.cshtml", model);

        }

        public IActionResult TeacherList()
        {
            var model = new SchoolId();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult TeacherListPartial()
        {
            var model = new SchoolId();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_teacher_list.cshtml", model);
        }
        public PartialViewResult TeacherAddModel()
        {
            var model = new TeacherAddModel();
            model.schoolId = _user.SchoolId;
            var salaryTpe = new TrackTap.DataLibrary.Data.School(_user.SchoolId).SalaryTypeStatus();
            if (salaryTpe == 0)
            {
                model.SalaryType = "Basic Salary";
            }
            else
            {

                model.SalaryType = "Gross Salary";
            }
            return PartialView("~/Views/School/_pv_AddTeacher_Model.cshtml", model);
        }
        public object AddTeacher(TeacherAddModel obj)
        {
            bool status = false;
            string message = "Failed";

            var model = new SchoolAddTeacherPostModel();
            model.classId = obj.classId;
            model.divisionId = obj.divisionId;
            model.emailId = obj.emailId;
            model.filePath = obj.filePath;
            model.image = obj.image;
            model.schoolId = obj.schoolId.ToString();
            model.teacherName = obj.teacherName;
            model.contactNumber = obj.contactNumber;
            model.SalaryAmount = obj.SalaryAmount == null ? 0 : obj.SalaryAmount;
            model.PFPercentage = obj.PFPercentage == null ? 0 : obj.PFPercentage;
            model.ESIPercentage = obj.ESIPercentage == null ? 0 : obj.ESIPercentage;
            model.IsPermanent = obj.IsPermanent;

            try
            {
                if (obj.DOJstring != string.Empty && obj.DOJstring != null)
                {
                    string[] splitData = obj.DOJstring.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var dob = mm + '-' + dd + '-' + yyyy;
                    model.DOJ = Convert.ToDateTime(dob);
                }

            }
            catch
            {

            }

            if (obj.UserTypeId != null)
                model.UserTypeId = Convert.ToString(obj.UserTypeId);
            Tuple<bool, string, string> data = _schoolRepository.AddNewTeacher(model);
            status = data.Item1;
            if (status)
                message = "Success";

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult TeacherDetails(string id)
        {
            var model = new SingleTeacherDetails();
            model.TeacherId = Convert.ToInt64(id);
            var teacherData = _Entities.tb_Teacher.Where(x => x.TeacherId == model.TeacherId && x.IsActive).FirstOrDefault();
            if (teacherData != null)
            {
                model.TeacherName = teacherData.TeacherName;
                model.EmailId = teacherData.Email;
                model.ContactNo = teacherData.ContactNumber;
                var classData = _Entities.tb_TeacherClass.Where(x => x.TeacherId == teacherData.TeacherId).FirstOrDefault();
                if (classData != null)
                {
                    model.ClassDivision = classData.tb_Class.Class + " " + classData.tb_Division.Division;
                }
                else
                    model.ClassDivision = "Not Assigned";
                model.SpecialId = teacherData.TeacherSpecialId;
            }
            return View(model);
        }
        public object DeleteTeacher(string id)
        {
            bool status = false;
            string msg = "Failed";

            var model = new SchoolDeleteTeacherPostModel();
            model.schoolId = _user.SchoolId.ToString();
            model.teacherId = id;
            Tuple<bool, string, List<tb_Teacher>> data = _schoolRepository.DeleteTeacher(model);
            status = data.Item1;
            if (status)
                msg = "Success";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TeacherEditPartial(string id)
        {
            var model = new TeacherEditModel();
            model.schoolId = _user.SchoolId;
            model.teacherId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_Teacher_EditModel.cshtml", model);
        }

        public object EditTeacher(TeacherEditModel model)
        {
            bool status = false;
            string msg = "Failed";
            var teacher = _Entities.tb_Teacher.FirstOrDefault(z => z.TeacherId == model.teacherId);
            if (teacher != null)
            {
                teacher.ContactNumber = model.contactNumber;
                teacher.TeacherName = model.teacherName;
                teacher.Email = model.emailId;
                teacher.SalaryAmount = model.SalaryAmount;
                teacher.PFPercentage = model.PFPercentage;
                teacher.ESIPercentage = model.ESIPercentage;
                teacher.IsPermanent = model.IsPermanent;
                if (model.UserTypeId != null && model.UserTypeId != 0)
                    teacher.UserType = model.UserTypeId;
                //----------------------------------

                try
                {
                    string[] splitData = model.DOJstring.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var dob = mm + '-' + dd + '-' + yyyy;
                    teacher.DOJ = Convert.ToDateTime(dob);
                }
                catch
                {

                }
            }

            foreach (var item in _Entities.tb_TeacherClass.Where(z => z.TeacherId == model.teacherId))
            {
                _Entities.tb_TeacherClass.Remove(item);
            }
            if (Convert.ToInt32(model.classId) != 0 && Convert.ToInt32(model.divisionId) != 0)
            {

                var addTeacherClass = _Entities.tb_TeacherClass.Create();
                addTeacherClass.TeacherId = teacher.TeacherId;
                addTeacherClass.ClassId = Convert.ToInt64(model.classId);
                addTeacherClass.DivisionId = Convert.ToInt64(model.divisionId); ;
                _Entities.tb_TeacherClass.Add(addTeacherClass);
            }
            status = _Entities.SaveChanges() > 0;
            msg = status ? "Success" : "Failed";

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult StaffList()
        {
            LoginModel model = new LoginModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }

        public PartialViewResult StaffAddModel()
        {
            StaffModels model = new StaffModels();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_AddStaff_Model.cshtml", model);
        }
        public object AddStaff(StaffModels model)
        {
            bool status = false;
            string msg = "Failed";

            var login = _Entities.tb_Login.Create();
            login.SchoolId = _user.SchoolId;
            login.RoleId = (int)UserRole.Staff;
            login.Name = model.Name;
            login.Username = model.emailId;
            login.Password = model.Password;
            login.IsActive = true;
            login.TimeStamp = CurrentTime;
            login.DisableStatus = false;
            login.LoginGuid = Guid.NewGuid();
            _Entities.tb_Login.Add(login);
            status = _Entities.SaveChanges() > 0;
            if (status)
            {
                var staff = _Entities.tb_Staff.Create();
                staff.UserId = login.UserId;
                staff.StaffName = model.Name;
                staff.Contact = model.Contact;
                staff.Address = model.Address;
                staff.IsActive = true;
                staff.TimeStamp = CurrentTime;
                staff.IsPermanent = model.IsPermanent;
                try
                {
                    string[] splitData = model.DOBstring.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var dob = mm + '-' + dd + '-' + yyyy;
                    staff.DOB = Convert.ToDateTime(dob);
                }
                catch
                {

                }
                try
                {
                    string[] splitData = model.DOJstring.Split('-');
                    var dd = splitData[0];
                    var mm = splitData[1];
                    var yyyy = splitData[2];
                    var dob = mm + '-' + dd + '-' + yyyy;
                    staff.DOJ = Convert.ToDateTime(dob);
                }
                catch
                {

                }
                if (model.UserTypeId != null)
                    staff.UserType = model.UserTypeId;
                staff.SalaryAmount = model.SalaryAmount;
                staff.PFPercentage = model.PFPercentage;
                staff.ESIPercentage = model.ESIPercentage;
                _Entities.tb_Staff.Add(staff);
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? " Staff added" : "Failed to add Staff";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult StaffListPartial()
        {
            var model = new LoginModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_Staff_list.cshtml", model);
        }
        public PartialViewResult StaffEditModel(string id)
        {
            long userId = Convert.ToInt64(id);
            var user = _Entities.tb_Login.Where(z => z.UserId == userId && z.IsActive).FirstOrDefault();
            var staff = _Entities.tb_Staff.Where(z => z.UserId == userId && z.IsActive).FirstOrDefault();
            //var staff = _Entities.tb_Staff.Where(z => z.StaffId==userId && z.IsActive).FirstOrDefault();

            var model = new StaffModels();
            if (staff != null)
            {
                model.UserId = user.UserId;
                model.Name = staff.StaffName;
                model.Email = user.Username;
                model.Password = user.Password;
                model.Address = staff.Address;
                model.Contact = staff.Contact;
                model.DOBstring = staff.DOB.ToString("dd-MM-yyyy");
                model.SalaryAmount = staff.SalaryAmount ?? 00;
                model.PFPercentage = staff.PFPercentage ?? 00;
                model.ESIPercentage = staff.ESIPercentage ?? 00;
                model.IsPermanent = staff.IsPermanent ?? false;
                model.SchoolId = _user.SchoolId;
                if (staff.UserType != null)
                {
                    model.UserTypeId = staff.UserType ?? 0;
                }
                model.DOJstring = staff.DOJ.ToString();
            }
            var salary = _Entities.tb_SalaryType.Where(x => x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
            if (salary != null)
            {
                if (salary.TypeId == 0)
                {
                    model.SalaryType = "Basic Salary";
                }
                else
                {
                    model.SalaryType = "Gross Salary";
                }
            }
            else
            {
                model.SalaryType = "Basic Salary";
            }

            return PartialView("~/Views/School/_pv_EditStaff_Model.cshtml", model);
        }

        public object EditStaff(StaffModels model)
        {
            bool status = false;
            string msg = "Failed";
            var login = _Entities.tb_Login.Where(z => z.UserId == model.UserId && z.IsActive).FirstOrDefault();
            var staff = _Entities.tb_Staff.Where(z => z.UserId == model.UserId && z.IsActive).FirstOrDefault();

            login.Name = model.Name;
            login.Password = model.Password;
            staff.StaffName = model.Name;
            staff.Address = model.Address;
            staff.Contact = model.Contact;
            staff.IsPermanent = model.IsPermanent;
            try
            {
                string[] splitData = model.DOBstring.Split('-');
                var dd = splitData[0];
                var mm = splitData[1];
                var yyyy = splitData[2];
                var dob = mm + '-' + dd + '-' + yyyy;
                staff.DOB = Convert.ToDateTime(dob);
            }
            catch
            {

            }
            try
            {
                string[] splitData = model.DOJstring.Split('-');
                var dd = splitData[0];
                var mm = splitData[1];
                var yyyy = splitData[2];
                var dob = mm + '-' + dd + '-' + yyyy;
                staff.DOJ = Convert.ToDateTime(dob);
            }
            catch
            {

            }
            if (model.UserTypeId != 0 && model.UserTypeId != null)
                staff.UserType = model.UserTypeId;
            staff.SalaryAmount = model.SalaryAmount;
            staff.PFPercentage = model.PFPercentage;
            staff.ESIPercentage = model.ESIPercentage;
            status = _Entities.SaveChanges() > 0;
            msg = status ? " Staff edited" : "No changes made";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteStaff(string id)
        {
            bool status = false;
            string msg = "Failed";
            long userId = Convert.ToInt64(id);
            var staff = _Entities.tb_Login.Where(z => z.UserId == userId && z.IsActive).FirstOrDefault();
            staff.IsActive = false;
            status = _Entities.SaveChanges() > 0;
            msg = status ? " Staff Deleted" : "Failed to delete Staff";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public object GetFeeSuggestByText(string id)
        {
            string[] splitData = id.Split('~');
            bool status = false;
            string message = "Failed";
            string name = id;
            //var school = Entities.tb_School.Where(z => z.IsActive == true && z.ViewStatus == true && z.StateId == stateId && (z.School.StartsWith(name) || z.School.Contains(name))).Select(z => new
            var school = _Entities.tb_Fee.Where(z => z.IsActive && z.SchoolId == _user.SchoolId && (z.FeesName.StartsWith(name))).Select(z => new
            {
                feeId = z.FeeId,
                feeName = z.FeesName,
                schoolId = z.SchoolId,
            }).ToList();
            status = school.Count > 0;
            message = status ? "Success" : "Failed";
            return Json(new { status = status, message = message, list = school }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object AddAdditionalFeeStudent(FeeModel model)
        {
            bool status = false;
            string msg = "Failed";

            var PaidAmount = model.PaidAmount;
            var FeeName = model.FeeName;
            if (model.FeeId == 0)
            {
                if (PaidAmount > 0 && FeeName != null)
                {
                    var isExist1 = _Entities.tb_Fee.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && (z.FeesName.ToLower() == FeeName.ToLower())).ToList();
                    if (isExist1.Count > 0)
                    {
                        var feeStudent1 = _Entities.tb_FeeStudent.Create();
                        feeStudent1.Amount = PaidAmount;
                        feeStudent1.StudentId = model.StudentId;
                        feeStudent1.FeeId = isExist1.FirstOrDefault().FeeId;
                        feeStudent1.FeeStudentGuid = Guid.NewGuid();
                        feeStudent1.IsActive = true;
                        feeStudent1.TimeStamp = CurrentTime;
                        feeStudent1.DueDate = CurrentTime;
                        feeStudent1.Instalment = 1;
                        _Entities.tb_FeeStudent.Add(feeStudent1);
                        status = _Entities.SaveChanges() > 0;
                        msg = status ? " Fee added" : "Failed to add Fee";
                    }
                    else
                    {
                        var fee = _Entities.tb_Fee.Create();
                        var feeStudent = _Entities.tb_FeeStudent.Create();
                        fee.FeesName = FeeName;
                        fee.SchoolId = _user.SchoolId;
                        fee.TimeStamp = CurrentTime;
                        fee.IsActive = true;
                        fee.FeeType = 2;
                        fee.Interval = 1;
                        fee.FeeStartDate = CurrentTime;
                        _Entities.tb_Fee.Add(fee);
                        status = _Entities.SaveChanges() > 0;
                        var feeId = fee.FeeId;

                        feeStudent.Amount = PaidAmount;
                        feeStudent.StudentId = model.StudentId;
                        feeStudent.FeeId = feeId;
                        feeStudent.FeeStudentGuid = Guid.NewGuid();
                        feeStudent.IsActive = true;
                        feeStudent.TimeStamp = CurrentTime;
                        feeStudent.DueDate = CurrentTime;
                        feeStudent.Instalment = 1;
                        _Entities.tb_FeeStudent.Add(feeStudent);
                        status = _Entities.SaveChanges() > 0;
                        msg = status ? " Fee added" : "Failed to add fee";
                    }
                }
                if (model.FeeDetails != null)
                {
                    List<string> feeDetails = model.FeeDetails.Split(',').ToList();

                    foreach (var data in feeDetails)
                    {
                        string[] splitDataAdd = data.Split('~');
                        long addFeeid = Convert.ToInt64(splitDataAdd[0]);
                        decimal addAmount = Convert.ToDecimal(splitDataAdd[1]);

                        var isExist = _Entities.tb_Fee.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.FeeId == addFeeid).ToList();
                        if (isExist.Count > 0)
                        {
                            var feeStudent1 = _Entities.tb_FeeStudent.Create();
                            feeStudent1.Amount = addAmount;
                            feeStudent1.StudentId = model.StudentId;
                            feeStudent1.FeeId = isExist.FirstOrDefault().FeeId;
                            feeStudent1.FeeStudentGuid = Guid.NewGuid();
                            feeStudent1.IsActive = true;
                            feeStudent1.TimeStamp = CurrentTime;
                            feeStudent1.DueDate = CurrentTime;
                            feeStudent1.Instalment = 1;
                            _Entities.tb_FeeStudent.Add(feeStudent1);
                            status = _Entities.SaveChanges() > 0;
                            msg = status ? " Fee added" : "Failed to add Fee";
                        }
                    }
                }

            }

            else
            {
                var feeStudent = _Entities.tb_FeeStudent.Create();
                feeStudent.Amount = model.Amount;
                feeStudent.StudentId = model.StudentId;
                feeStudent.FeeId = model.FeeId;
                feeStudent.FeeStudentGuid = Guid.NewGuid();
                feeStudent.IsActive = true;
                feeStudent.TimeStamp = CurrentTime;
                feeStudent.DueDate = CurrentTime;
                feeStudent.Instalment = 1;
                _Entities.tb_FeeStudent.Add(feeStudent);
                status = _Entities.SaveChanges() > 0;
                msg = status ? " Fee added" : "Failed to add Fee";
            }
            var nowDate = String.Format("{0:y}", CurrentTime);
            return Json(new { status = status, msg = msg, feeName = model.FeeName, date = nowDate, amount = model.Amount }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AddPaymentPartialView(string id)
        {
            var model = new FeeModel();
            model.StudentId = Convert.ToInt64(id);
            model.SchoolId = _user.SchoolId;

            return PartialView("~/Views/School/_pv_AdditionalBilling.cshtml", model);
        }
        public PartialViewResult AddAdvancePaymentPartialView(string id)
        {
            var model = new FeeModel();
            model.StudentId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_AdvancePayment_Add.cshtml", model);
        }
        public PartialViewResult BillingPaymentDeteils(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id); ;
            return PartialView("~/Views/School/_pv_Billing_PaymentDeteils_Grid.cshtml", model);
        }
        [HttpPost]
        public object EditStudentFee(FeeModel model)
        {
            bool status = true;
            string message = "Failed";
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //List<Datalist> routes_list =
            //         (List<Datalist>)json_serializer.DeserializeObject(model.DataList);
            List<Datalist> routes_list = new JavaScriptSerializer().Deserialize<List<Datalist>>(model.FeeDetails);
            //long FeeId = Convert.ToInt64(model.FeeId);
            foreach (var value in routes_list)
            {
                long feeStudentId = Convert.ToInt32(value.feeStudentId);
                var FeeDetail = _Entities.tb_FeeStudent.Where(z => z.FeeStudentId == feeStudentId && z.IsActive).FirstOrDefault();
                if (FeeDetail != null && value.amount != null)
                {
                    FeeDetail.DiscountAmount = (FeeDetail.Amount) - Convert.ToDecimal(value.amount);// Add Archana for store the updated amount as discount 11-10-2019
                    FeeDetail.Amount = Convert.ToDecimal(value.amount);
                    status = _Entities.SaveChanges() > 0;
                }
            }

            message = status ? " Fee Added" : "Fee Added";
            return Json(new { status = true, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditBillingPayment(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id); ;
            return PartialView("~/Views/School/_pv_Edit_BillingDetails.cshtml", model);
        }
        public PartialViewResult EditCommonBillingPayment(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id); ;
            return PartialView("~/Views/School/_pv_Edit_CommonBillingDetails.cshtml", model);
        }

        public object DeleteStudentEditableFee(string id)
        {
            bool status = false;
            string message = "Failed";
            string[] splitData = id.Split('~');
            long studentId = Convert.ToInt32(splitData[0]);
            Guid feeGuid = new Guid(splitData[1]);
            var fee = _Entities.tb_FeeStudent.Where(z => z.FeeStudentGuid == feeGuid && z.StudentId == studentId).FirstOrDefault();
            if (fee != null)
            {
                _Entities.tb_FeeStudent.Remove(fee);
                status = _Entities.SaveChanges() > 0;
            }
            //fee.IsActive = false;
            message = status ? "Fee Deleted" : "failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteStudentCommonFee(string id)
        {
            bool status = false;
            string message = "Failed";
            string[] splitData = id.Split('~');
            long studentId = Convert.ToInt32(splitData[0]);
            Guid feeGuid = new Guid(splitData[1]);
            long classId = Convert.ToInt64(splitData[2]);
            var fee = _Entities.tb_FeeClass.Where(z => z.FeeClassGuid == feeGuid && z.ClassId == classId).FirstOrDefault();
            if (fee != null)
            {
                var delStudentFee = _Entities.tb_DeletedFeeStudent.Where(z => z.StudentId == studentId && z.FeeClassId == fee.FeeClassId).FirstOrDefault();
                if (delStudentFee == null)
                {
                    var DeleteFeeStudent = _Entities.tb_DeletedFeeStudent.Create();
                    DeleteFeeStudent.StudentId = studentId;
                    DeleteFeeStudent.FeeClassId = fee.FeeClassId;
                    DeleteFeeStudent.ParentGuid = feeGuid;
                    DeleteFeeStudent.IsActive = true;
                    DeleteFeeStudent.TimeStamp = CurrentTime;
                    _Entities.tb_DeletedFeeStudent.Add(DeleteFeeStudent);
                    status = _Entities.SaveChanges() > 0;
                }
            }
            //fee.IsActive = false;
            message = status ? "Fee Deleted" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult SpecialFeeClassList(string id)
        {
            long feeId = Convert.ToInt32(id);
            var model = new FilterModel();
            model.schoolId = _user.SchoolId;
            model.FeeId = feeId;
            return View(model);
        }
        public IActionResult AssignSpecialFee(string id)
        {
            string[] splitData = id.Split('~');
            long divisionId = Convert.ToInt32(splitData[0]);
            long feeId = Convert.ToInt32(splitData[1]);
            var model = new FeeModel();
            model.DivisionId = divisionId;
            model.SchoolId = _user.SchoolId;
            model.FeeId = feeId;
            return View(model);
        }
        [HttpPost]
        public object AssignSpecialFee(FeeModel model)
        {
            bool status = false;
            string msg = "Failed";
            List<string> studentsUserId = model.FeeStudentId.Split(',').ToList();

            var FeeDetail = _Entities.tb_Fee.Where(z => z.FeeId == model.FeeId && z.IsActive).FirstOrDefault();
            var discount = new tb_FeeDiscount();
            foreach (var userId in studentsUserId)
            {


                if (FeeDetail.Interval > 1)
                {
                    DateTime dueDt = FeeDetail.FeeStartDate;
                    var addMonth = 12 / FeeDetail.Interval;
                    for (int i = 1; i <= FeeDetail.Interval; i++)
                    {
                        var feeStudent = _Entities.tb_FeeStudent.Create();
                        feeStudent.Amount = model.Amount;
                        feeStudent.StudentId = Convert.ToInt32(userId);
                        feeStudent.FeeId = model.FeeId;
                        feeStudent.FeeStudentGuid = Guid.NewGuid();
                        feeStudent.IsActive = true;
                        feeStudent.TimeStamp = CurrentTime;
                        feeStudent.DueDate = dueDt;
                        dueDt = dueDt.AddMonths(addMonth);
                        feeStudent.Instalment = i;
                        _Entities.tb_FeeStudent.Add(feeStudent);
                        status = _Entities.SaveChanges() > 0;
                        msg = status ? " Fee added" : "Failed to add Fee";

                    }
                }
                else
                {
                    var feeStudent = _Entities.tb_FeeStudent.Create();
                    feeStudent.Amount = model.Amount;
                    feeStudent.StudentId = Convert.ToInt32(userId);
                    feeStudent.FeeId = model.FeeId;
                    feeStudent.FeeStudentGuid = Guid.NewGuid();
                    feeStudent.IsActive = true;
                    feeStudent.TimeStamp = CurrentTime;
                    feeStudent.DueDate = CurrentTime;
                    feeStudent.Instalment = 1;
                    _Entities.tb_FeeStudent.Add(feeStudent);
                    status = _Entities.SaveChanges() > 0;
                    msg = status ? " Fee added" : "Failed to add Fee";
                }
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetStudentFeeByDivGrid(string id)
        {
            string[] splitData = id.Split('~');
            FeeModel model = new FeeModel();
            long divisionId = Convert.ToInt32(splitData[0]);
            long feeId = Convert.ToInt32(splitData[1]);

            model.DivisionId = divisionId;
            model.FeeId = feeId;
            return PartialView("~/Views/School/_pv_StudentFee_ByDivision_Grid.cshtml", model);

        }
        public IActionResult SpecialFeeStudent(string id)
        {
            var model = new FeeModel();
            long feeId = Convert.ToInt32(id);
            model.FeeId = feeId;
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object DeleteStudentFee(string id)
        {
            bool status = false;
            string message = "Failed";
            string[] splitData = id.Split('~');
            long studentId = Convert.ToInt32(splitData[0]);
            long studentFeeId = Convert.ToInt32(splitData[1]);


            var feeStudent = _Entities.tb_FeeStudent.Where(z => z.StudentId == studentId && z.IsActive && z.FeeStudentId == studentFeeId).FirstOrDefault();
            if (feeStudent != null)
            {
                feeStudent.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? " Fee deleted" : "Failed to delete fee";

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult RefreshFeeStudentGrid(string id)
        {
            var model = new FeeModel();
            model.SchoolId = _user.SchoolId;
            model.FeeId = Convert.ToInt32(id);
            return PartialView("~/Views/School/_pv_StudentFee_Student_Grid.cshtml", model);
        }
        public PartialViewResult EditFeeStudAmountView(string id)
        {
            var model = new FeeModel();
            string[] splitData = id.Split('~');
            model.StudentId = Convert.ToInt32(splitData[0]);
            model.SpecialFeeId = Convert.ToInt32(splitData[1]);
            var feeDetail = _Entities.tb_FeeStudent.Where(z => z.StudentId == model.StudentId && z.IsActive && z.FeeStudentId == model.SpecialFeeId).FirstOrDefault();
            model.FeeName = feeDetail.tb_Fee.FeesName;
            model.StudentName = feeDetail.tb_Student.StundentName;
            model.Amount = Convert.ToDecimal(String.Format("{0:0.00}", feeDetail.Amount));
            return PartialView("~/Views/School/_pv_Edit_SpecialFee_StudentAmount_Model.cshtml", model);

        }
        [HttpPost]
        public object EditFeeStudentAmount(FeeModel model)
        {
            bool status = false;
            string message = "Failed";
            long studentId = model.StudentId;
            long specialFeeId = model.SpecialFeeId;
            var feeDetails = _Entities.tb_FeeStudent.Where(z => z.StudentId == studentId && z.IsActive && z.FeeStudentId == specialFeeId).FirstOrDefault();
            if (feeDetails != null)
            {
                feeDetails.Amount = model.Amount;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? " Fee edited" : "Failed to edit fee";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PrintAccountBillData(string id)
        {
            string[] splitData = id.Split('~');
            var model = new PrintBill();
            model.studentId = Convert.ToInt64(splitData[0]);
            model.billNumber = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/School/_pv_PrintAccountBillData.cshtml", model);
        }
        public object CheckAdmissionNo(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (_Entities.tb_Student.Any(x => x.StudentSpecialId.ToLower() == text.ToLower() && x.SchoolId == _user.SchoolId && x.IsActive))
            {
                Status = true;
                Message = "Admission Number already in use";
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        public object CheckTeacherContactNumberNo(string text)
        {
            bool Status = false;
            string Message = "Failed";
            if (_Entities.tb_Teacher.Any(x => x.ContactNumber.Trim() == text.Trim() && x.IsActive))
            {
                Status = true;
                Message = "Contact Number already in use";
            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        #region SMTP
        public IActionResult SMTPDetails()
        {
            SMTPDetailsModel model = new SMTPDetailsModel();
            long schoolId = _user.SchoolId;
            var smtpDetail = _Entities.tb_SMTPDetail.Where(z => z.SchoolId == schoolId).FirstOrDefault();
            if (smtpDetail != null)
            {
                model.email = smtpDetail.EmailId;
                model.password = smtpDetail.Password;
            }
            return View(model);
        }

        [HttpPost]
        public object AddEmailerSetup(SMTPDetailsModel model)
        {
            bool status = false;
            string message = "Failed";
            long schoolId = _user.SchoolId;
            var smtp = new tb_SMTPDetail();
            var smtpDetail = _Entities.tb_SMTPDetail.Where(z => z.SchoolId == schoolId).FirstOrDefault();
            if (smtpDetail == null)
            {
                try
                {
                    smtp.EmailId = model.email;
                    smtp.Password = model.password;
                    smtp.SchoolId = schoolId;
                    _Entities.tb_SMTPDetail.Add(smtp);
                    status = _Entities.SaveChanges() > 0 ? true : false;
                }
                catch (Exception ex)
                {

                }
            }
            message = status ? " SMTP Added" : "Failed to add SMTP";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public object EditEmailerSetup(SMTPDetailsModel model)
        {
            bool status = false;
            string message = "Failed";
            long schoolId = _user.SchoolId;
            var smtpDetail = _Entities.tb_SMTPDetail.Where(z => z.SchoolId == schoolId).FirstOrDefault();
            if (smtpDetail != null)
            {
                smtpDetail.EmailId = model.email;
                smtpDetail.Password = model.password;
                status = _Entities.SaveChanges() > 0;
            }
            message = status ? " SMTP Edit Successfully" : "Failed to edit SMTP";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public PartialViewResult AddStudentBillPartialView(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_History_Billing_StudentFee_Model.cshtml", model);
        }

        public PartialViewResult LoadTableForBilling(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            string[] splitData = id.Split('~');
            model.SchoolModel.studentId = Convert.ToInt64(splitData[1]);
            model.BillNumber = Convert.ToInt64(splitData[0]);
            return PartialView("~/Views/School/_pv_History_PopupGrid.cshtml", model);
        }

        public IActionResult CollectionReport()
        {
            FeeModel model = new FeeModel();
            model.StartDate = CurrentTime;
            return View(model);
        }
        public PartialViewResult CollectionReportByDate(string id)
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
            return PartialView("~/Views/School/_pv_CollectionReport_Grid.cshtml", model);
        }

        public IActionResult CollectionReportDetail(string id)
        {
            string[] split = id.Split('~');

            long billno = Convert.ToInt64(split[0]);
            long studentId = Convert.ToInt64(split[1]);

            FeeModel model = new FeeModel();
            model.BillNumber = billno;
            model.StudentId = studentId;
            return View(model);
        }
        //gayathri(23/11/2022) For thermal printing 
        public PartialViewResult Thermalprintview(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            string[] splitData = id.Split('~');
            model.SchoolModel.studentId = Convert.ToInt64(splitData[1]);
            model.BillNumber = Convert.ToInt64(splitData[0]);
            return PartialView("~/Views/School/Thermalprintview.cshtml", model);
  
        }

       
       
      
     
        public object CancelBill(string id)
        {
            bool status = false;
            string msg = string.Empty;
            string[] splitData = id.Split('~');

            long billNo = Convert.ToInt64(splitData[0]);
            long studentId = Convert.ToInt64(splitData[1]);
            decimal preiousCr = 0;
            decimal currentCr = 0;
            var Bills_Which_paid_Balance_Of_This_Bill = _Entities.tb_Payment.Where(x => x.IsActive == true && x.IsPaid == true && x.PartialPaidParentBillNo == billNo).ToList();
            var studentPaidAmt = _Entities.tb_StudentPaidAmount.Where(z => z.StudentId == studentId && z.BillNo == billNo && z.StudentId == studentId && z.IsActive).FirstOrDefault();
            if (studentPaidAmt != null)
            {
                preiousCr = studentPaidAmt.PreviousBalance ?? 0;
                currentCr = studentPaidAmt.BalanceAmount;
                var studentBalance = _Entities.tb_StudentBalance.Where(z => z.StudentId == studentId).FirstOrDefault();
                if (studentPaidAmt.PaidAmount == 0)
                {
                    var paymentBill = _Entities.tb_Payment.Where(z => z.SchoolId == _user.SchoolId && z.BillNo == billNo && z.StudentId == studentId).ToList();
                    foreach (var payment in paymentBill)
                    {
                        var billDue = _Entities.tb_FeeDues.Where(z => z.StudentId == payment.StudentId && z.IsActive && z.BillNo == payment.BillNo).FirstOrDefault();
                        if (billDue != null)
                        {
                            #region Partial Paid after this bill calculation 
                            // Here we checking that the cancelling bill have a partial payment . If yes, then checking that the balance payment payed in the other bill.
                            //if yes, then we wants to regenreate the feedues of the cancelling bill which is minusing from the payed bill amount .
                            if (Bills_Which_paid_Balance_Of_This_Bill.Count > 0 && Bills_Which_paid_Balance_Of_This_Bill != null)
                            {
                                var afterPaidPartial = Bills_Which_paid_Balance_Of_This_Bill.Where(x => x.FeeGuid == billDue.FeeDuesGuid).FirstOrDefault();
                                if (afterPaidPartial != null)
                                {
                                    var thisBillPartial = _Entities.tb_FeeDues.Where(x => x.StudentId == studentId && x.BillNo == afterPaidPartial.BillNo && x.ParentGuid == afterPaidPartial.FeeGuid).FirstOrDefault();
                                    if (thisBillPartial != null)
                                    {
                                        thisBillPartial.ParentGuid = payment.FeeGuid;
                                    }
                                    afterPaidPartial.FeeGuid = payment.FeeGuid;
                                    afterPaidPartial.MaxAmount = afterPaidPartial.MaxAmount + payment.Amount;
                                    var feeDueNow = new tb_FeeDues();
                                    feeDueNow.Amount = payment.Amount;
                                    feeDueNow.FeeId = payment.FeeId;
                                    feeDueNow.StudentId = payment.StudentId;
                                    feeDueNow.FeeDuesGuid = Guid.NewGuid();
                                    feeDueNow.IsActive = true;
                                    feeDueNow.DueDate = billDue.DueDate;
                                    feeDueNow.TimeStamp = CurrentTime;
                                    feeDueNow.ParentGuid = payment.FeeGuid;
                                    feeDueNow.BillNo = afterPaidPartial.BillNo;
                                    _Entities.tb_FeeDues.Add(feeDueNow);
                                    _Entities.SaveChanges();
                                }
                            }
                            #endregion Partial Paid after this bill calculation 

                            _Entities.tb_FeeDues.Remove(billDue);
                        }
                        var current = _Entities.tb_Payment.Where(x => x.PaymentId == payment.PaymentId).FirstOrDefault();
                        current.IsActive = false;
                        status = _Entities.SaveChanges() > 0 ? true : false;

                    }
                    if (status)
                    {
                        studentPaidAmt.AddAccountStatus = false;
                        studentPaidAmt.IsActive = false;
                        status = _Entities.SaveChanges() > 0 ? true : false;
                    }
                    if (status)
                    {
                        decimal diffBal = preiousCr - currentCr;
                        decimal bal = studentBalance.Amount;
                        if (studentBalance.Amount != (bal - diffBal))
                        {
                            studentBalance.Amount = bal - diffBal;
                            status = _Entities.SaveChanges() > 0 ? true : false;
                        }
                    }
                    msg = status ? "Bill cancelled!" : "Failed to cancel bill!";
                }
                else
                {
                    var listtodelete = _Entities.tb_StudentPaidAmount.Where(z => z.PaidId >= studentPaidAmt.PaidId && z.StudentId == studentId && z.IsActive).ToList();
                    foreach (var data in listtodelete)
                    {
                        data.IsActive = false;
                        data.AddAccountStatus = false;
                        var paymentBill = _Entities.tb_Payment.Where(z => z.SchoolId == _user.SchoolId && z.BillNo == data.BillNo && z.StudentId == studentId).ToList();
                        foreach (var payment in paymentBill)
                        {
                            var pay = _Entities.tb_Payment.Where(x => x.PaymentId == payment.PaymentId).FirstOrDefault();
                            pay.IsActive = false;
                            status = _Entities.SaveChanges() > 0 ? true : false;
                        }
                    }
                    if (status)
                    {
                        if (studentBalance.Amount != preiousCr)
                        {
                            studentBalance.Amount = preiousCr;
                            status = _Entities.SaveChanges() > 0 ? true : false;
                        }
                    }

                    msg = status ? "Bill cancelled!" : "Failed to cancel bill!";
                }
            }
            else
            {
                var paymentBill = _Entities.tb_Payment.Where(z => z.SchoolId == _user.SchoolId && z.BillNo == billNo && z.StudentId == studentId).ToList();
                foreach (var payment in paymentBill)
                {
                    var billDue = _Entities.tb_FeeDues.Where(z => z.StudentId == payment.StudentId && z.IsActive && z.BillNo == payment.BillNo).FirstOrDefault();
                    if (billDue != null)
                    {
                        #region Partial Paid after this bill calculation 
                        // Here we checking that the cancelling bill have a partial payment . If yes, then checking that the balance payment payed in the other bill.
                        //if yes, then we wants to regenreate the feedues of the cancelling bill which is minusing from the payed bill amount .
                        if (Bills_Which_paid_Balance_Of_This_Bill.Count > 0 && Bills_Which_paid_Balance_Of_This_Bill != null)
                        {
                            var afterPaidPartial = Bills_Which_paid_Balance_Of_This_Bill.Where(x => x.FeeGuid == billDue.FeeDuesGuid).FirstOrDefault();
                            if (afterPaidPartial != null)
                            {
                                var thisBillPartial = _Entities.tb_FeeDues.Where(x => x.StudentId == studentId && x.BillNo == afterPaidPartial.BillNo && x.ParentGuid == afterPaidPartial.FeeGuid).FirstOrDefault();
                                if (thisBillPartial != null)
                                {
                                    thisBillPartial.ParentGuid = payment.FeeGuid;
                                }
                                afterPaidPartial.FeeGuid = payment.FeeGuid;
                                afterPaidPartial.MaxAmount = afterPaidPartial.MaxAmount + payment.Amount;
                                var feeDueNow = new tb_FeeDues();
                                feeDueNow.Amount = payment.Amount;
                                feeDueNow.FeeId = payment.FeeId;
                                feeDueNow.StudentId = payment.StudentId;
                                feeDueNow.FeeDuesGuid = Guid.NewGuid();
                                feeDueNow.IsActive = true;
                                feeDueNow.DueDate = billDue.DueDate;
                                feeDueNow.TimeStamp = CurrentTime;
                                feeDueNow.ParentGuid = payment.FeeGuid;
                                feeDueNow.BillNo = afterPaidPartial.BillNo;
                                _Entities.tb_FeeDues.Add(feeDueNow);
                                _Entities.SaveChanges();
                            }
                        }
                        #endregion Partial Paid after this bill calculation 
                        _Entities.tb_FeeDues.Remove(billDue);
                    }
                    var currentPaymnet = _Entities.tb_Payment.Where(x => x.PaymentId == payment.PaymentId).FirstOrDefault();
                    currentPaymnet.IsActive = false;
                    status = _Entities.SaveChanges() > 0 ? true : false;
                }
                msg = status ? "Bill cancelled!" : "Failed to cancel bill!";
            }

            #region Account from Cancel
            try// Archana 29-11-2018 New bill cancel section for Account section
            {
                var student = new TrackTap.DataLibrary.Data.Student(studentId);
                var studentPaidFee = student.StudentPaidAmountByBillNo(billNo);
                decimal refundAmount = 0;
                if (studentPaidFee == null)
                {
                    refundAmount = _Entities.tb_Payment.Where(x => x.StudentId == studentId && x.SchoolId == _user.SchoolId && x.BillNo == billNo && x.IsActive == false).ToList().Sum(x => x.Amount);
                }
                else
                {
                    refundAmount = studentPaidFee.PaidAmount;
                }
                if (status == true && refundAmount != 0)
                {
                    int mode = Convert.ToInt32(splitData[2]);
                    long headId = 0;
                    long subId = 0;
                    var vouchr = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
                    if (vouchr == null)
                    {
                        var vou = new tb_VoucherNumber();
                        vou.IsActive = true;
                        vou.PaymentVoucher = 1;
                        vou.ReceiptVoucher = 1;
                        vou.SchoolId = _user.SchoolId;
                        vou.ContraVoucher = 1;
                        _Entities.tb_VoucherNumber.Add(vou);
                        _Entities.SaveChanges();
                        vouchr = _Entities.tb_VoucherNumber.Where(x => x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
                    }
                    var head = _Entities.tb_AccountHead.Where(x => x.SchoolId == _user.SchoolId && x.IsActive && x.ForBill == true).FirstOrDefault();
                    if (head != null)
                    {
                        headId = head.AccountId;
                        var sub = _Entities.tb_SubLedgerData.Where(x => x.AccHeadId == head.AccountId && x.IsActive).ToList();
                        if (sub.Count > 0 && sub != null)
                        {
                            if (sub.Count == 1)
                            {
                                var subAdd = new tb_SubLedgerData();
                                subAdd.SubLedgerName = "Bill Cancel";
                                subAdd.AccHeadId = head.AccountId;
                                subAdd.IsActive = true;
                                subAdd.TimeStamp = CurrentTime;
                                _Entities.tb_SubLedgerData.Add(subAdd);
                                _Entities.SaveChanges();
                                subId = subAdd.LedgerId;
                            }
                            else
                            {
                                subId = sub[1].LedgerId;
                            }
                        }
                        else
                        {
                            var subAdd = new tb_SubLedgerData();
                            subAdd.SubLedgerName = "Advance Amount";
                            subAdd.AccHeadId = head.AccountId;
                            subAdd.IsActive = true;
                            subAdd.TimeStamp = CurrentTime;
                            _Entities.tb_SubLedgerData.Add(subAdd);
                            _Entities.SaveChanges();

                            var subAdd2 = new tb_SubLedgerData();
                            subAdd2.SubLedgerName = "Bill Cancel";
                            subAdd2.AccHeadId = head.AccountId;
                            subAdd2.IsActive = true;
                            subAdd2.TimeStamp = CurrentTime;
                            _Entities.tb_SubLedgerData.Add(subAdd2);
                            _Entities.SaveChanges();
                            subId = subAdd2.LedgerId;
                        }
                    }
                    else
                    {
                        var AccountHead = new tb_AccountHead();
                        AccountHead.AccHeadName = "Fee Income";
                        AccountHead.ForBill = true;
                        AccountHead.SchoolId = _user.SchoolId;
                        AccountHead.IsActive = true;
                        AccountHead.TimeStamp = CurrentTime;
                        _Entities.tb_AccountHead.Add(AccountHead);
                        _Entities.SaveChanges();
                        headId = AccountHead.AccountId;

                        var subAdd = new tb_SubLedgerData();
                        subAdd.SubLedgerName = "Advance Amount";
                        subAdd.AccHeadId = AccountHead.AccountId;
                        subAdd.IsActive = true;
                        subAdd.TimeStamp = CurrentTime;
                        _Entities.tb_SubLedgerData.Add(subAdd);
                        _Entities.SaveChanges();

                        var subAdd2 = new tb_SubLedgerData();
                        subAdd2.SubLedgerName = "Bill Cancel";
                        subAdd2.AccHeadId = AccountHead.AccountId;
                        subAdd2.IsActive = true;
                        subAdd2.TimeStamp = CurrentTime;
                        _Entities.tb_SubLedgerData.Add(subAdd2);
                        _Entities.SaveChanges();
                        subId = subAdd2.LedgerId;
                    }
                    if (mode == 1)//Cash
                    {
                        var cash = new tb_CashEntry();
                        cash.AdvanceStatus = false;
                        cash.Amount = refundAmount;
                        cash.VoucherNumber = Convert.ToString(vouchr.PaymentVoucher);
                        cash.BillNo = "";
                        cash.CancelStatus = false;
                        cash.DataFromStatus = false;
                        cash.EditStatus = "P";
                        cash.EnterDate = CurrentTime;
                        cash.HeadId = headId;
                        cash.IsActive = true;
                        cash.Migration = false;
                        cash.Narration = "Bill Cancel " + billNo;
                        cash.ReverseStatus = false;
                        cash.SchoolId = _user.SchoolId;
                        cash.SubId = subId;
                        cash.TimeStamp = CurrentTime;
                        cash.TransactionType = "P";
                        cash.UserId = _user.UserId;
                        cash.VoucherNumber = Convert.ToString(vouchr.PaymentVoucher);
                        cash.VoucherType = "PV";
                        _Entities.tb_CashEntry.Add(cash);
                        _Entities.SaveChanges();

                        vouchr.PaymentVoucher = vouchr.PaymentVoucher + 1;
                        _Entities.SaveChanges();

                        var cancelledBills = _Entities.tb_Payment.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == false && x.BillNo == billNo && x.IsPaid == true).ToList();
                        if (cancelledBills.Count > 0 && cancelledBills != null)
                        {
                            foreach (var item in cancelledBills)
                            {
                                var cancelAccount = new tb_BillCancelAccounts();
                                cancelAccount.SchoolId = _user.SchoolId;
                                cancelAccount.CashBankType = false;//Cash
                                cancelAccount.CashBankId = cash.Id;
                                cancelAccount.ItemId = item.FeeId;
                                cancelAccount.Amount = item.Amount;
                                cancelAccount.CancelDate = CurrentTime;
                                cancelAccount.IsActive = true;
                                _Entities.tb_BillCancelAccounts.Add(cancelAccount);
                                _Entities.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var bank = new tb_BankEntry();
                        bank.Amount = refundAmount;
                        bank.BankId = Convert.ToInt64(splitData[3]);
                        bank.BillNo = "";
                        bank.CancelStatus = false;
                        if (mode == 2)//Cheque
                        {
                            bank.ChequeDate = Convert.ToDateTime(splitData[5]);
                            bank.ChequeNumber = Convert.ToString(splitData[4]);
                        }
                        bank.DataFromStatus = false;
                        bank.EditStatus = "P";
                        bank.EnterDate = CurrentTime;
                        bank.HeadId = headId;
                        bank.SubId = subId;
                        bank.IsActive = true;
                        bank.Migration = false;
                        bank.ModeType = mode;
                        bank.Narration = "Bill Cancel " + billNo;
                        bank.SchoolId = _user.SchoolId;
                        bank.TimeStamp = CurrentTime;
                        bank.TransactionType = "P";
                        bank.UserId = _user.UserId;
                        bank.VoucherNumber = Convert.ToString(vouchr.PaymentVoucher);
                        bank.VoucherType = "PV";
                        _Entities.tb_BankEntry.Add(bank);
                        _Entities.SaveChanges();

                        vouchr.PaymentVoucher = vouchr.PaymentVoucher + 1;
                        _Entities.SaveChanges();

                        var cancelledBills = _Entities.tb_Payment.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == false && x.BillNo == billNo && x.IsPaid == true).ToList();
                        if (cancelledBills.Count > 0 && cancelledBills != null)
                        {
                            foreach (var item in cancelledBills)
                            {
                                var cancelAccount = new tb_BillCancelAccounts();
                                cancelAccount.SchoolId = _user.SchoolId;
                                cancelAccount.CashBankType = true;//Bank
                                cancelAccount.CashBankId = bank.Id;
                                cancelAccount.ItemId = item.FeeId;
                                cancelAccount.Amount = item.Amount;
                                cancelAccount.CancelDate = CurrentTime;
                                cancelAccount.IsActive = true;
                                _Entities.tb_BillCancelAccounts.Add(cancelAccount);
                                _Entities.SaveChanges();
                            }
                        }
                    }

                    #region Data added to Balance table for Account
                    int sourceId = 0;
                    if (mode == 1)//cash
                        sourceId = Convert.ToInt32(DataFromStatus.Cash);
                    else
                        sourceId = Convert.ToInt32(DataFromStatus.Bank);
                    long bankId = Convert.ToInt64(splitData[3]);
                    var balance = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == CurrentTime.Date && x.SourceId == sourceId && x.BankId == bankId).FirstOrDefault();
                    if (balance != null)
                    {
                        balance.Closing = balance.Closing - refundAmount;
                        balance.TimeStamp = CurrentTime;
                        _Entities.SaveChanges();
                    }
                    else
                    {
                        try
                        {
                            var balanceEntry = new tb_Balance();
                            balanceEntry.SchoolId = _user.SchoolId;
                            balanceEntry.CurrentDate = CurrentTime;
                            balanceEntry.SourceId = sourceId;
                            DateTime yesterday = _Entities.tb_Balance.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) < CurrentTime.Date && x.SchoolId == _user.SchoolId && x.SourceId == sourceId && x.BankId == bankId).OrderByDescending(x => x.CurrentDate).Select(x => x.CurrentDate).FirstOrDefault();
                            if (yesterday.Year != 0001)
                                balanceEntry.Opening = _Entities.tb_Balance.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && System.Data.Entity.DbFunctions.TruncateTime(x.CurrentDate) == yesterday.Date && x.SourceId == sourceId && x.BankId == bankId).ToList().Sum(x => x.Closing);
                            else
                                balanceEntry.Opening = 0;
                            balanceEntry.Closing = balanceEntry.Opening - refundAmount;
                            balanceEntry.IsActive = true;
                            balanceEntry.BankId = bankId;
                            balanceEntry.TimeStamp = CurrentTime;
                            _Entities.tb_Balance.Add(balanceEntry);
                            _Entities.SaveChanges();

                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    #endregion Data added to Balance table for Account
                }
            }
            catch (Exception ex)
            {

            }
            #endregion Account from Cancel
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        #region SMS
        public IActionResult Messages(bool id = false)
        {
            if (id == true)
            {
                var model = new SchoolModelForId();
                model.SchoolId = _user.SchoolId;
                model.SmartPhoneUser = true;
                return View(model);
            }
            else
            {
                var model = new SchoolModelForId();
                model.SchoolId = _user.SchoolId;
                model.SmartPhoneUser = false;
                return View(model);
            }

        }

        public object SMSOLD(SchoolModelForId model)
        {
            HttpClient client = new HttpClient();
            string message = "Failed";
            var status = false;
            // List<string> numbers = model.Numbers.Split(',').ToList();
            //  List<string> studentsUserId = model.StudentId.Split(',').ToList();
            //  var phone = model.Numbers;
            var phone = "9961797049";
            var url = "http://bhashsms.com//api/sendmsg.php?user=srishtitrans&pass=123456&sender=MCHILD&phone=" + phone + "&text=" + model.Description + "&priority=ndnd&stype=normal";
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest request = this.GetRequest(url);
            WebResponse webResponse = request.GetResponse();
            var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
            List<string> MessageIds = newresponse.Split(' ').ToList();

            if (newresponse != null)
            {
                message = "success";
                status = true;
                var history = new tb_SmsHistory();


                //foreach (var Id in studentsUserId)
                //{
                //    history.StuentId = Convert.ToInt64(Id);
                //    history.MessageDate = DateTime.UtcNow;
                //    history.MessageContent = model.Description;
                //    history.IsActive = true;
                //    history.ScholId = _user.SchoolId;
                //    _Entities.tb_SmsHistory.Add(history);
                //    _Entities.SaveChanges();
                //    //foreach (var num in MessageIds)
                //    //{
                //    //    var s = _Entities.tb_SmsHistory.Where(x => x.Id == history.Id).FirstOrDefault();
                //    //    if (s != null)
                //    //    {
                //    //        s.SendStatus = num;
                //    //    }


                //    //}
                //}


            }
            return responseText;
        }

        [HttpPost]
        public object SMS(SchoolModelForId model)
        {
            HttpClient client = new HttpClient();
            var history = new tb_SmsHistory();
            var numbers = new List<string>();
            var MsgId = new List<string>();
            var statusFail = "100";
            var numb = "";
            string message = "Failed";
            var status = false;
            string messagepre = "";
            long studentId = 0;
            List<SendMessage> Userdata = JsonConvert.DeserializeObject<List<SendMessage>>(model.Data).ToList();
            //foreach (var item in Userdata)

            var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (school.SmsActive)
            {
                var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
                if (package != null)
                {
                    if (package.ToDate >= CurrentTime)
                    {
                        //     ---------------------------------

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
                            smsHead.Head = Userdata[0].Description;
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
                                    messagepre = "Dear Parent of " + studentDetails.StundentName;
                                    messagepre = messagepre + ", " + Userdata[0].Description;
                                    //-----SPECIAL CHARACTER SENDING -------------------
                                    messagepre = messagepre.Replace("#", "%23");
                                    messagepre = messagepre.Replace("&", "%26");
                                    //--------------------------------------------------
                                    var phone = ms.Number.ToString();
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
                                        statusFail = "0";

                                        tb_SmsHistory sms = new tb_SmsHistory();
                                        sms.IsActive = true;
                                        sms.MessageContent = Userdata[0].Description;
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
                                        try
                                        {
                                            _Entities.tb_SmsHistory.Add(sms);
                                            _Entities.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                        string emailId = studentDetails.ParentEmail;
                                        string subject = "Message from School";
                                        string schoolName = studentDetails.tb_School.SchoolName;
                                        SendSMSEmail(messagepre, emailId, subject, schoolName);// Send EMAIL for student SMS
                                        SendSMSPush(studentDetails, messagepre);// Send PUSH for student SMS

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        message = "Package validity expired";
                        statusFail = "2";
                    }
                }
                else
                {
                    message = "Please update your package";
                    statusFail = "3";
                }
            }
            else
            {
                message = "SMS cannot be send please contact our support team";
                statusFail = "1";
            }
            return Json(new { Status = status, statusFail = statusFail, message = message }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public object HomeworkSMS(SchoolModelForId model)
        {
            HttpClient client = new HttpClient();
            var history = new tb_SmsHistory();
            var numbers = new List<string>();
            var MsgId = new List<string>();

            var numb = "";
            string message = "Failed to send sms";
            var status = false;
            string messagepre = "";
            long studentId = 0;
            List<SendMessage> Userdata = JsonConvert.DeserializeObject<List<SendMessage>>(model.Data).ToList();
            //foreach (var item in Userdata)
            var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (school.SmsActive)
            {
                var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
                if (package != null)
                {
                    if (package.ToDate >= CurrentTime)
                    {
                        //     ---------------------------------


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
                            smsHead.Head = Userdata[0].Description;
                            smsHead.SchoolId = _user.SchoolId;
                            smsHead.TimeStamp = CurrentTime;
                            smsHead.IsActive = true;
                            smsHead.SenderType = (int)SMSSendType.Student;
                            _Entities.tb_SmsHead.Add(smsHead);
                            status = _Entities.SaveChanges() > 0;


                            var homeworkSms = new tb_HomeworkSms();
                            homeworkSms.HeadId = smsHead.HeadId;
                            homeworkSms.SchoolId = _user.SchoolId;
                            homeworkSms.TimeStamp = CurrentTime;
                            homeworkSms.IsActive = true;
                            _Entities.tb_HomeworkSms.Add(homeworkSms);
                            status = _Entities.SaveChanges() > 0;
                            if (Userdata[0].list.Count > 0 && Userdata[0].list != null)
                            {
                                foreach (var ms in Userdata[0].list)
                                {
                                    studentId = Convert.ToInt64(ms.StudentId);
                                    var studentDetails = _Entities.tb_Student.Where(z => z.StudentId == studentId).FirstOrDefault();
                                    messagepre = "Dear Parent of " + studentDetails.StundentName;
                                    messagepre = messagepre + ", " + Userdata[0].Description;
                                    //-----SPECIAL CHARACTER SENDING -------------------
                                    messagepre = messagepre.Replace("#", "%23");
                                    messagepre = messagepre.Replace("&", "%26");
                                    //--------------------------------------------------
                                    var phone = ms.Number.ToString();
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
                                        sms.MessageContent = Userdata[0].Description;
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

                                        string emailId = studentDetails.ParentEmail;
                                        string subject = "Message from School";
                                        string schoolName = studentDetails.tb_School.SchoolName;
                                        SendSMSEmail(messagepre, emailId, subject, schoolName);// Send EMAIL for student SMS
                                        SendSMSPush(studentDetails, messagepre);// Send PUSH for student SMS

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return message;
        }
        private void SendSMSPush(tb_Student studentDetails, string messagepre)
        {
            try
            {
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
                            body = messagepre,
                            title = "Message From School"
                        },
                        priority = "high",
                        data = new
                        {
                            Role = "School",
                            Function = "Message"
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

        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

            request.Timeout = Convert.ToInt32(new TimeSpan(0, 5, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
        }

        public IActionResult SmsHistory()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.Selecteddate = CurrentTime;
            model.Selecteddate_From = CurrentTime.ToString();
            model.Selecteddate_To = CurrentTime.ToString();
            Tuple<string, string, List<SmsHead>> tt = new TrackTap.DataLibrary.Data.WebsiteService().GetAllSmsHeadByDate(model.Selecteddate_From, model.Selecteddate_To, model.schoolId);
            ViewBag.msgCount = tt.Item1;
            ViewBag.TotalCount = tt.Item2;
            ViewBag.Result = tt.Item3;
            string allowedsms = "0";
            string extraSms = "0";
            string extraCost = "0";
            var package = _Entities.SP_GetSmsPackage(_user.SchoolId).ToList().Select(y => new SpSmsPackage(y)).ToList();
            if (package.Count > 0)
            {
                var data = package.Where(z => z.IsDisabled == false && z.IsActive).FirstOrDefault();
                if (data != null)
                {
                    allowedsms = data.AllowedSms.ToString();
                    extraSms = data.ExtraSmsCount.ToString();
                    extraCost = data.ExtraAmount.ToString();
                }
            }
            ViewBag.Allowedsms = allowedsms;
            ViewBag.ExtraSms = extraSms;
            ViewBag.ExtraCost = extraCost;
            return View(model);

        }
        public object GetAllSmsHistoryOnDate()
        {
            var model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.Selecteddate = CurrentTime;
            return PartialView("~/Views/School/_pv_SmsHistory.cshtml", model);
        }
        public IActionResult SmsHistoryDetail(string id)
        {
            SMSHistoryModel model = new SMSHistoryModel();
            long headId = Convert.ToInt64(id);
            //ViewBag.Result = new TrackTap.DataLibrary.Data.SmsHead(headId).SmsHistory;
            var data = new TrackTap.DataLibrary.Data.SmsHead(headId);
            if (data != null)
            {
                model.headId = data.headId;
                model.head = data.head;
                model.SenderType = data.SenderType;
            }
            return View(model);
        }

        public IActionResult HomeworkSms()
        {
            var model = new HomeWorkSmsModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }

        #endregion


        public IActionResult Report()
        {
            var model = new DateClass();
            model.Selecteddate = CurrentTime;
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public object GetAllFeeHead()
        {
            long schoolId = _user.SchoolId;
            var result = new TrackTap.DataLibrary.Data.WebsiteService().gtGetAllFeeHead(schoolId);
            return Json(new { Status = true, Message = "", result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object GetAllExpenceAccountHeads()
        {
            long schoolId = _user.SchoolId;
            var result = new TrackTap.DataLibrary.Data.WebsiteService().gtAllExpenceAccountHead(schoolId);
            return Json(new { Status = true, Message = "", result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object GetAllIncomeAccountHeads()
        {
            long schoolId = _user.SchoolId;
            var result = new TrackTap.DataLibrary.Data.WebsiteService().gtAllIncomeAccountHead(schoolId);
            return Json(new { Status = true, Message = "", result = result }, JsonRequestBehavior.AllowGet);
        }

        public object GetIncomeAccountHeads(string id)
        {
            var result = new TrackTap.DataLibrary.Data.WebsiteService().GetIncomeAccountHeads(id);
            return Json(new { Status = true, Message = "", result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object SubmitIncomeData(IncomeData model)
        {
            var message = "failed";
            var status = false;
            var income = new tb_Income();
            income.AccountHead = model.HeadValue;
            income.Amount = Convert.ToDouble(model.Amount);
            income.Particular = model.ParticularValue;
            income.SchoolId = _user.SchoolId;
            income.IsActive = true;
            var d = string.Format(model.SelectedDate, "MM/dd/yyyy");
            income.Date = Convert.ToDateTime(d);

            _Entities.tb_Income.Add(income);
            if (_Entities.SaveChanges() > 0)
            {
                message = "success";
                status = true;
            }



            return Json(new { Status = status, Message = message, Id = income.Id }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public object SubmitIncomeDataEdit(IncomeData model)
        {
            var message = "failed";
            var status = false;
            var income = _Entities.tb_Income.Where(x => x.Id == model.Id).FirstOrDefault();
            var amount = income.Amount;
            if (income != null)
            {
                income.AccountHead = model.HeadValue;
                income.Amount = Convert.ToDouble(model.Amount);
                income.Particular = model.ParticularValue;
                income.SchoolId = _user.SchoolId;
                income.IsActive = true;
                var d = string.Format(model.SelectedDate, "MM/dd/yyyy");
                income.Date = Convert.ToDateTime(d);
            }

            if (_Entities.SaveChanges() > 0)
            {
                message = "success";
                status = true;
            }



            return Json(new { Status = status, Message = message, Amount = amount }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public object SubmitExpenceDataEdit(IncomeData model)
        {
            var message = "failed";
            var status = false;
            var expence = _Entities.tb_Expense.Where(x => x.Id == model.Id).FirstOrDefault();
            var amount = expence.Amount;
            if (expence != null)
            {
                expence.AccountHead = model.HeadValue;
                expence.Amount = Convert.ToDouble(model.Amount);
                expence.Particular = model.ParticularValue;
                expence.SchoolId = _user.SchoolId;
                expence.IsActive = true;
                var d = string.Format(model.SelectedDate, "MM/dd/yyyy");
                expence.Date = Convert.ToDateTime(d);
            }

            if (_Entities.SaveChanges() > 0)
            {
                message = "success";
                status = true;
            }



            return Json(new { Status = status, Message = message, Amount = amount }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public object SubmitExpenceData(IncomeData model)
        {
            var message = "failed";
            var status = false;
            var expence = new tb_Expense();
            expence.AccountHead = model.HeadValue;
            expence.Amount = Convert.ToDouble(model.Amount);
            expence.Particular = model.ParticularValue;
            expence.SchoolId = _user.SchoolId;
            expence.IsActive = true;
            var d = string.Format(model.SelectedDate, "MM/dd/yyyy");
            expence.Date = Convert.ToDateTime(d);

            _Entities.tb_Expense.Add(expence);
            if (_Entities.SaveChanges() > 0)
            {
                message = "success";
                status = true;
            }

            return Json(new { Status = status, Message = message, Id = expence.Id }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult GetAllReportOnDate(string id)
        {

            var model = new DateClass();
            model.Selecteddate = Convert.ToDateTime(id);
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_ReportViewTable.cshtml", model);
        }
        [HttpGet]
        public PartialViewResult GetAllSmsHistoryReportOnDate(string Selecteddate_From, string Selecteddate_To)
        {

            var model = new SchoolModel();
            if (Selecteddate_From == null && Selecteddate_To == null)
            {

                model.Selecteddate = CurrentTime;
                model.schoolId = _user.SchoolId;
            }
            else
            {
                model.Selecteddate_From = Selecteddate_From;
                model.Selecteddate_To = Selecteddate_To;
                model.schoolId = _user.SchoolId;
            }
            Tuple<string, string, List<SmsHead>> tt = new TrackTap.DataLibrary.Data.WebsiteService().GetAllSmsHeadByDate(model.Selecteddate_From, model.Selecteddate_To, model.schoolId);
            ViewBag.msgCount = tt.Item1;
            ViewBag.TotalCount = tt.Item2;
            ViewBag.Result = tt.Item3;

            return PartialView("~/Views/School/_pv_SmsHistory.cshtml", model);
        }

        public PartialViewResult PrintIncomeExpenceData(string id)
        {

            var model = new DateClass();
            model.Selecteddate = Convert.ToDateTime(id);
            model.SchoolId = _user.SchoolId;
            //model.studentId = Convert.ToInt64(splitData[0]);
            //model.billNumber = Convert.ToInt64(splitData[1]);
            return PartialView("~/Views/School/_pv_PrintIncomeExpence.cshtml", model);
        }
        //---------------Archana 02-Feb-2018--------------
        public IActionResult CircularNotification()
        {
            var model = new CircularList();
            model.schoolId = _user.SchoolId;
            return View(model);
        }


        public PartialViewResult AddCircularNotifications()
        {
            var model = new AddCircularNotification();
            //model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_AddCircularNotifications.cshtml", model);
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
                string folderPath = Server.MapPath("~/Media/" + schoolId + "/CircularData/");
                string OrginalName = randomFolder + "" + httpPostedFile.FileName.ToString();
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var fileSavePath = Path.Combine(folderPath, OrginalName);
                httpPostedFile.SaveAs(fileSavePath);
                filepath = Path.Combine("/Media/" + schoolId + "/CircularData/", OrginalName);
                msg = "Success";
                status = true;
            }
            return Json(new { Status = status, Message = msg, UserData = filepath }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object SubmitAddCircularNotifications(AddCircularNotification model)
        {
            long userId = _user.UserId;
            long schoolId = _user.SchoolId;
            string msg = "Failed";
            bool status = false;
            var circular = _Entities.tb_Circular.Create();
            try
            {
                circular.SchoolId = schoolId;
                circular.LoginType = _user.RoleId;
                circular.USerId = _user.UserId;
                string[] splitData = model.DocumentDateString.ToString().Split('-');
                var zdd = splitData[0];
                var zmm = splitData[1];
                var zyyyy = splitData[2];
                var Date = zmm + '-' + zdd + '-' + zyyyy;
                circular.CircularDate = Convert.ToDateTime(Date);
                circular.Description = model.DocumentDetails;
                circular.FilePath = model.FilePath;
                circular.IsActive = true;
                circular.TimeStamp = CurrentTime;
                _Entities.tb_Circular.Add(circular);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
                ///---------------Send Push and Mail for the all student's parents
                var student = _Entities.tb_Student.Where(x => x.IsActive && x.ParentId != null && x.SchoolId == _user.SchoolId).ToList();
                if (student.Count > 0)
                {
                    foreach (var item in student)
                    {
                        var parentDetails = _Entities.tb_DeviceToken.Where(x => x.UserId == item.ParentId && x.LoginStatus == 1).ToList().GroupBy(x => new { x.UserId, x.TokenId }).Select(x => x.FirstOrDefault()).ToList();
                        foreach (var data in parentDetails)
                        {
                            School schoolData = new School(_user.SchoolId);
                            string school = schoolData.SchoolName;
                            long studentUserId = Convert.ToInt64(item.StudentId);
                            var message = "Your Kid's  School have a circular, " + circular.Description + " , " + circular.CircularDate.ToShortDateString();
                            onlyCircularPushandroid(data.Token, message, school, studentUserId, item.SchoolId, circular);
                            SendMailsForOnlyCircularNotification(item, circular, school);
                            status = true;
                            msg = "Successful";
                        }
                    }
                }


                ///

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = status ? "Circular notification added successfully" : "Failed to Circular notification documents", UserData = circular.SchoolId }, JsonRequestBehavior.AllowGet);
        }

        private bool SendMailsForOnlyCircularNotification(tb_Student item, tb_Circular circular, string school)
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/CircularNotification.html");
            var emailTemplate = System.IO.File.ReadAllText(filePath);
            string Message = "Your kid " + item.StundentName + "'s School have a Circular";
            string message2 = circular.Description + " , " + circular.CircularDate.ToShortDateString();
            string filePathDoc = "http://www.schoolman.in" + circular.FilePath;
            string downLoad = "Download";
            if (circular.FilePath == null || circular.FilePath == "")
            {
                downLoad = "No files for Download";
            }
            var mBody = emailTemplate.Replace("{{resetLink}}", message2).Replace("{{resetLink1}}", school).Replace("{{resetLink2}}", Message).Replace("{{resetLink3}}", filePathDoc).Replace("{{resetLink4}}", downLoad);
            bool sendMail = Send("Circular", mBody, item.ParentEmail);
            return sendMail;
        }

        [HttpGet]
        public PartialViewResult CircularNotificationDataList()
        {
            CircularList model = new CircularList();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_CircularNotification.cshtml", model);
        }
        public object DeleteCircularNotification(string id)
        {
            bool status = false;
            string msg = "False";
            long circularId = Convert.ToInt64(id);
            var note = _Entities.tb_Circular.FirstOrDefault(x => x.CircularId == circularId);
            if (note != null)
            {
                if (note.FilePath != null)
                {
                    string filePath = note.FilePath;
                    string path = Server.MapPath("~" + filePath);
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                note.IsActive = false;
                note.FilePath = null;
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditDocument(string id)
        {
            long circularId = Convert.ToInt64(id);
            AddCircularNotification model = new AddCircularNotification();
            var data = _Entities.tb_Circular.Where(x => x.CircularId == circularId && x.IsActive).FirstOrDefault();
            if (data != null)
            {
                model.DocumentDateString = data.CircularDate.ToString("dd-MM-yyyy");
                model.DocumentDetails = data.Description;
                model.FilePath = data.FilePath;
                model.CircularId = data.CircularId;
            }
            return PartialView("~/Views/School/_pv_EditCircularNotifications.cshtml", model);
        }
        public object SubmitEditCircularNotifications(AddCircularNotification model)
        {
            bool status = false;
            string msg = "Failed";
            var circular = _Entities.tb_Circular.FirstOrDefault(x => x.CircularId == model.CircularId && x.IsActive);
            if (circular != null)
            {
                if (circular.FilePath != null)
                {
                    if (circular.FilePath != model.FilePath)
                    {
                        string filePath = circular.FilePath;
                        string path = Server.MapPath("~" + filePath);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        string[] splitData = model.DocumentDateString.ToString().Split('-');
                        var zdd = splitData[0];
                        var zmm = splitData[1];
                        var zyyyy = splitData[2];
                        var Date = zmm + '-' + zdd + '-' + zyyyy;
                        circular.CircularDate = Convert.ToDateTime(Date);
                        circular.FilePath = model.FilePath;
                        circular.Description = model.DocumentDetails;
                        status = _Entities.SaveChanges() > 0;
                    }
                    else
                    {
                        string[] splitData = model.DocumentDateString.ToString().Split('-');
                        var zdd = splitData[0];
                        var zmm = splitData[1];
                        var zyyyy = splitData[2];
                        var Date = zmm + '-' + zdd + '-' + zyyyy;
                        circular.CircularDate = Convert.ToDateTime(Date);
                        circular.Description = model.DocumentDetails;
                        status = _Entities.SaveChanges() > 0;
                    }

                }
                else
                {
                    circular.CircularDate = model.DocumentDate;
                    circular.FilePath = model.FilePath;
                    circular.Description = model.DocumentDetails;
                    status = _Entities.SaveChanges() > 0;
                }
            }
            if (status)
                msg = "Success";
            else
                msg = "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult NavigationHistory()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.curredntDateTime = CurrentTime;
            return View(model);
        }
        public PartialViewResult GpsHistoryGrid(string id)
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.curredntDateTime = Convert.ToDateTime(id);

            return PartialView("~/Views/School/_pv_GPSHistory_Grid.cshtml", model);
        }
        public PartialViewResult NavigationHistoryDetails(string id)
        {
            string[] splitData = id.Split('~');
            long busId = Convert.ToInt64(splitData[0]);
            var selDate = splitData[1];
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            model.curredntDateTime = Convert.ToDateTime(selDate);
            model.busId = busId;
            return PartialView("~/Views/School/_pv_GPSHistoryDetail_Grid.cshtml", model);

        }
        public object GetLatestExpParticulr(string id)
        {
            var msg = "Failed";
            var status = false;
            long schoolId = _user.SchoolId;
            var res = new TrackTap.DataLibrary.Data.WebsiteService().GetLatestExpParticular(id, schoolId);
            if (res != null)
            {
                status = true;
                msg = "Success";

            }

            return Json(new { status = status, msg = msg, value = res }, JsonRequestBehavior.AllowGet);

        }
        public object GetLatestIncParticulr(string id)
        {
            var msg = "Failed";
            var status = false;
            long schoolId = _user.SchoolId;

            var res = new TrackTap.DataLibrary.Data.WebsiteService().GetLatestIncParticular(id, schoolId);
            if (res != null)
            {
                status = true;
                msg = "Success";

            }

            return Json(new { status = status, msg = msg, value = res }, JsonRequestBehavior.AllowGet);

        }

        #region Accounts
        public IActionResult TrialBalance()
        {
            SchoolModel model = new SchoolModel();
            model.curredntDateTime = CurrentTime;
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult BalanceSheet()
        {
            SchoolModel model = new SchoolModel();
            model.curredntDateTime = CurrentTime;
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult IncomeAndExpenditure()
        {
            SchoolModel model = new SchoolModel();
            model.curredntDateTime = CurrentTime;
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        #endregion
        //--------------------Archana 19-Feb-2018 -----------------
        [HttpGet]
        public IActionResult StudentDetailedView(string id)
        {
            long studentId = Convert.ToInt64(id);

            var student = _Entities.tb_Student.Where(x => x.IsActive && x.StudentId == studentId).FirstOrDefault();
            StudentModel model = new StudentModel();
            model.studentId = studentId;
            model.studentName = student.StundentName;
            model.className = student.tb_Class.Class;
            model.division = student.tb_Division.Division;
            model.admissionNo = student.StudentSpecialId;
            model.address = student.Address;
            model.filePath = student.FilePath;
            model.gender = student.Gender;
            model.classId = student.ClassId;
            model.divisionId = student.DivisionId;
            model.contactNo = student.ContactNumber;
            if (student.ParentId != null)
            {
                model.parentName = student.tb_Parent.ParentName ?? "";
                model.parentEmail = student.tb_Parent.Email;
            }
            else
            {
                var xx = student.ParentName.ToString();
                model.parentName = student.ParentName;
                model.parentEmail = student.ParentEmail;
            }
            model.rollNo = student.ClasssNumber;
            return View(model);
        }
        public object GetAttendanceCountData(string id)
        {
            string msg = "Faile";
            int totalCount = 0;
            int absentCount = 0;
            long studentId = 0;
            DateTime date = CurrentTime;
            string variable = Convert.ToString(id);
            if (id != null)
            {
                string[] data = Regex.Split(variable, "~");
                if (data.Count() == 2)
                {
                    date = Convert.ToDateTime("01-" + data[0] + "-" + CurrentTime.Year);
                    studentId = Convert.ToInt64(data[1]);
                    //var returnData = _Entities.sp_TotalAttendance(date, studentId).ToList();
                    //if (returnData.Count > 0)
                    //{
                    //    msg = "Success";
                    //    totalCount = returnData.Count();
                    //    absentCount = returnData.Where(x => x.AttendanceData == false).ToList().Count();
                    //}
                    List<AttendanceDetails> attendanceDataList = new List<AttendanceDetails>();
                    var attendanceData = _Entities.tb_Attendance.Where(x => x.AttendanceDate.Year == CurrentTime.Year && x.AttendanceDate.Month == date.Month && x.StudentId == studentId && x.IsActive).ToList().Select(x => new Attendance(x)).ToList();
                    List<DateTime> attendanceTime = attendanceData.Select(z => z.AttendanceDate.Date).Distinct().ToList();
                    if (attendanceData.Count > 0)
                    {
                        foreach (var item in attendanceTime)
                        {
                            AttendanceDetails obj = new AttendanceDetails();
                            obj.attendanceDate = item.ToShortDateString();
                            obj.mornignShift = false;
                            obj.eveningShift = false;
                            var eachDay = attendanceData.Where(z => z.AttendanceDate.Date == item.Date).ToList();
                            if (eachDay.Count > 0)
                            {
                                var morningStatus = eachDay.FirstOrDefault(z => z.ShiftStatus == 0);
                                var eveningStatus = eachDay.FirstOrDefault(z => z.ShiftStatus == 1);
                                if (morningStatus != null)
                                {
                                    if (morningStatus.AttendanceData)
                                        obj.mornignShift = true;
                                }
                                if (eveningStatus != null)
                                {
                                    if (eveningStatus.AttendanceData)
                                        obj.eveningShift = true;
                                }
                            }

                            attendanceDataList.Add(obj);
                        }
                    }
                    if (attendanceDataList.Count > 0)
                    {
                        msg = "Success";
                        //totalCount = 2 * attendanceDataList.Count();
                        //absentCount = attendanceDataList.Where(x => x.mornignShift == false).ToList().Count();
                        //absentCount = absentCount + attendanceDataList.Where(x => x.eveningShift == false).ToList().Count();
                        totalCount = attendanceData.Count();
                        absentCount = attendanceData.Where(x => x.AttendanceData == false).ToList().Count();
                    }
                }
            }
            return Json(new { msg = msg, totalCount = totalCount, absentCount = absentCount }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult CurriculumDetails()
        {
            var model = new SchoolValue();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult DatatableExamList(FilterModel model)
        {
            //return PartialView("~/Views/School/_pv_SchoolExaminationList.cshtml", model);
            return PartialView("~/Views/School/_pv_SchoolExamList.cshtml", model);
        }

        public PartialViewResult AddNewExams()
        {
            var model = new ExamsModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_AddNewExams.cshtml", model);
        }
        public object SubmitAddExam(ExamsModel model)
        {
            long userId = _user.UserId;
            long schoolId = _user.SchoolId;
            string msg = "Failed";
            bool status = false;
            var exam = _Entities.tb_Exams.Create();
            try
            {
                exam.SchoolId = schoolId;
                exam.ClassId = model.ClassId;
                exam.DivisionId = model.DivisionId;
                exam.ExamName = model.ExamName;
                exam.ExamDate = Convert.ToDateTime(model.ExamDate.ToString("MM/dd/yyyy"));
                exam.IsActive = true;
                exam.TimeStamp = CurrentTime;
                exam.UserId = userId;
                _Entities.tb_Exams.Add(exam);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = status ? "Exam added successfully" : "Failed to Exam documents", UserData = exam.SchoolId }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult ExamSubjectDetails(string id)
        {
            var model = new ExamSubjectDetailsModel();
            model.ExamId = Convert.ToInt64(id);
            return View(model);
        }

        public object DeleteSchoolexams(string id)
        {
            bool status = false;
            string msg = "False";
            long examId = Convert.ToInt64(id);
            var exam = _Entities.tb_Exams.FirstOrDefault(x => x.ExamId == examId && x.IsActive);
            if (exam != null)
            {
                exam.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditExam(string id)
        {
            long examId = Convert.ToInt64(id);
            ExamsModel model = new ExamsModel();
            var data = _Entities.tb_Exams.Where(x => x.ExamId == examId && x.IsActive).FirstOrDefault();
            if (data != null)
            {
                model.ExamId = data.ExamId;
                model.SchoolId = data.SchoolId;
                model.UserId = data.UserId;
                model.ClassId = data.ClassId;
                model.DivisionId = data.DivisionId;
                model.ExamName = data.ExamName;
                model.ExamDate = data.ExamDate;
            }
            return PartialView("~/Views/School/_pv_EditExam.cshtml", model);
        }

        public object SubmitEditExam(ExamsModel model)
        {
            bool status = false;
            string msg = "Failed";
            var exam = _Entities.tb_Exams.Where(z => z.ExamId == model.ExamId && z.IsActive).FirstOrDefault();
            exam.ClassId = model.ClassId;
            exam.DivisionId = model.DivisionId;
            exam.ExamName = model.ExamName;
            exam.ExamDate = model.ExamDate;
            status = _Entities.SaveChanges() > 0;
            msg = status ? " Successfull" : "No changes made";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddNewSubjects(string id)
        {
            var model = new ExamSubjectDetailsModel();
            model.ExamId = Convert.ToInt64(id);
            model.SchoolId = _user.SchoolId;
            model.ExamDate = CurrentTime;
            return PartialView("~/Views/School/_pv_AddNewSubjects.cshtml", model);
        }
        public object SubmitAddSubject(ExamSubjectDetailsModel model)
        {
            string msg = "Failed";
            bool status = false;
            var sub = _Entities.tb_ExamSubjects.Create();
            try
            {
                sub.ExamId = model.ExamId;
                sub.Subject = new TrackTap.DataLibrary.Data.Subjects(model.SubjectId).SubjectName;
                sub.InternalMarks = model.Internal;
                sub.ExternalMark = model.External;
                //sub.Mark = model.Total;
                sub.Mark = sub.InternalMarks + sub.ExternalMark;
                sub.IsActive = true;
                sub.TimeStamp = CurrentTime;
                sub.SubjectId = model.SubjectId;
                sub.ExamDate = model.ExamDate.Add(model.ExamTime);
                _Entities.tb_ExamSubjects.Add(sub);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, message = status ? "Subject added successfully" : "Subject to Exam documents", UserData = model.ExamId }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ExamSubjectList(string id)
        {
            ExamSubjectDetailsModel model = new ExamSubjectDetailsModel();
            model.ExamId = Convert.ToInt64(id);
            return PartialView("~/Views/School/_pv_ExamSubjectList.cshtml", model);
        }

        public object DeleteSchoolSubject(string id)
        {
            bool status = false;
            string msg = "False";
            long subId = Convert.ToInt64(id);
            var sub = _Entities.tb_ExamSubjects.FirstOrDefault(x => x.SubId == subId && x.IsActive);
            if (sub != null)
            {
                sub.IsActive = false;
                status = _Entities.SaveChanges() > 0;
            }
            msg = status ? "Deleted" : "Failed";
            return Json(new { status = status, msg = msg, UserData = subId }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditSubjects(string id)
        {
            long subjectId = Convert.ToInt64(id);
            ExamSubjectDetailsModel model = new ExamSubjectDetailsModel();
            var data = _Entities.tb_ExamSubjects.Where(x => x.SubId == subjectId && x.IsActive).FirstOrDefault();
            if (data != null)
            {
                model.ExamId = data.ExamId;
                model.Subject = data.Subject;
                model.Internal = data.InternalMarks;
                model.External = data.ExternalMark;
                model.Total = data.Mark;
                model.ExamSubjectId = data.SubId;
                model.SubjectId = data.SubjectId;
                model.SchoolId = _user.SchoolId;
                model.ExamDate = Convert.ToDateTime(data.ExamDate.ToShortDateString());
                //string[] data = Regex.Split(variable, "~");
                string timeSpan = "";
                string[] split = data.ExamDate.ToShortTimeString().Split(':');
                if (split.Count() > 1)
                {
                    if (split[0].Length == 2)
                    {
                        timeSpan = split[0];
                    }
                    else
                    {
                        timeSpan = "0" + split[0];
                    }
                    //timeSpan = timeSpan+":"+ split[1].Substring(0,2) + ":00";
                    timeSpan = timeSpan + ":" + split[1].Substring(0, 2);
                }
                model.ExamTime = TimeSpan.Parse(timeSpan);
            }
            return PartialView("~/Views/School/_pv_EditSubject.cshtml", model);
        }
        public object SubmitEditSubject(ExamSubjectDetailsModel model)
        {
            bool status = false;
            string msg = "Failed";
            var sub = _Entities.tb_ExamSubjects.Where(z => z.ExamId == model.ExamId && z.SubId == model.ExamSubjectId && z.IsActive).FirstOrDefault();
            sub.Subject = new TrackTap.DataLibrary.Data.Subjects(model.SubjectId).SubjectName;
            sub.InternalMarks = model.Internal;
            sub.ExternalMark = model.External;
            //sub.Mark = model.Total;
            sub.Mark = sub.InternalMarks + sub.ExternalMark;
            sub.SubjectId = model.SubjectId;
            sub.ExamDate = model.ExamDate.Add(model.ExamTime);
            status = _Entities.SaveChanges() > 0;
            msg = status ? " Successful" : "No changes made";
            return Json(new { status = status, msg = msg, UserData = sub.ExamId }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult DatatableSubjectList(string id)
        {
            AddStudentMarkModel model = new AddStudentMarkModel();
            model.StudentList = new List<StudentList>();
            model.SubjectId = Convert.ToInt64(id);
            model.SchoolId = _user.SchoolId;
            var data = new TrackTap.DataLibrary.Data.Exams(model.SubjectId, model.SchoolId);
            model.DivisionId = data.DivisionId;
            model.ExamId = data.ExamId;
            model.ClassId = data.ClassId;
            var totalCount = _Entities.tb_Student.Where(x => x.SchoolId == _user.SchoolId && x.ClassId == model.ClassId && x.DivisionId == model.DivisionId && x.IsActive).ToList();// Total Students
            #region 
            //model.TotalMark=new TrackTap.DataLibrary.Data
            //var markListData = _Entities.tb_StudentMarks.Where(x => x.ExamId == model.ExamId && x.SubjectId == model.SubjectId && x.IsActive).ToList();
            //model.StudentList = new List<StudentList>();
            //if(markListData.Count>0)
            //{
            //    foreach(var item in markListData)
            //    {
            //        StudentList one = new StudentList();
            //        one.StudentId = item.StudentId;
            //        one.StudentName = new TrackTap.DataLibrary.Data.Student(item.StudentId).StundentName;
            //        one.InternalMark = item.InternalMark??00;
            //        one.ExternalMark = item.ExternalMark ?? 00;
            //        one.Total = new TrackTap.DataLibrary.Data.ExamSubjects(model.SubjectId).Mark;
            //    }
            //}
            //else
            //{
            //}
            #endregion 

            var dataList = _Entities.sp_StudentMarkList(model.ExamId, model.SubjectId).ToList(); //Students Mark Details
            if (dataList.Count > 0)
            {
                var subject = data.ExamSubjectsList.Where(x => x.SubId == model.SubjectId && x.IsActive).FirstOrDefault();
                model.TotalInternalMark = subject.InternalMarks;
                model.TotalExternalMark = subject.ExternalMark;

                if (dataList.Count == totalCount.Count)// Full Students attend the exam
                {
                    foreach (var item in dataList)
                    {
                        StudentList one = new StudentList();
                        one.StudentId = item.StudentId ?? 0;
                        one.StudentName = item.StundentName;
                        one.InternalMark = item.StudentInternalMark;
                        one.ExternalMark = item.StudentExternalMark;
                        one.Total = item.StudentTotalMark;
                        one.ExamId = item.ExamId;
                        one.SubjectId = item.SubId;
                        model.StudentList.Add(one);
                    }
                }
                else
                {
                    foreach (var item in dataList)// Full Students attend the exam
                    {
                        StudentList one = new StudentList();
                        one.StudentId = item.StudentId ?? 0;
                        one.StudentName = item.StundentName;
                        one.InternalMark = item.StudentInternalMark;
                        one.ExternalMark = item.StudentExternalMark;
                        one.Total = item.StudentTotalMark;
                        one.ExamId = item.ExamId;
                        one.SubjectId = item.SubId;
                        model.StudentList.Add(one);
                    }
                    var result = totalCount.Where(p => !dataList.Any(p1 => p1.StudentId == p.StudentId)).ToList();
                    foreach (var item in result)// Remainig Students whoes not attend the exam
                    {
                        StudentList one = new StudentList();
                        one.StudentId = item.StudentId;
                        one.StudentName = item.StundentName;
                        one.InternalMark = 0;
                        one.ExternalMark = 0;
                        one.Total = 0;
                        one.ExamId = data.ExamId;
                        one.SubjectId = model.SubjectId;
                        model.StudentList.Add(one);
                    }
                }
            }
            else
            {
                //basheer on 25/01/2019 to view marks who did not write exam
                var subject = data.ExamSubjectsList.Where(x => x.SubId == model.SubjectId && x.IsActive).FirstOrDefault();
                model.TotalInternalMark = subject.InternalMarks;
                model.TotalExternalMark = subject.ExternalMark;
                foreach (var item in totalCount) // No entry of result
                {

                    StudentList one = new StudentList();
                    one.StudentId = item.StudentId;
                    one.StudentName = item.StundentName;
                    one.InternalMark = 0;
                    one.ExternalMark = 0;
                    one.Total = 0;
                    one.ExamId = data.ExamId;
                    one.SubjectId = model.SubjectId;
                    model.StudentList.Add(one);
                }
            }
            model.StudentList = model.StudentList.OrderBy(XmlSiteMapProvider => XmlSiteMapProvider.StudentName).ToList();
            return PartialView("~/Views/School/_pv_AddStudentsMark.cshtml", model);
        }
        [HttpPost]
        public object SubmitAddStudentsMarks(StudentsMarkListData data1)
        {
            List<StudentList> data = new List<StudentList>();
            data = data1._ListData;
            bool status = false;
            string msg = "Failed";
            if (data.Count > 0)
            {
                long examId = Convert.ToInt64(data1.ExamId);
                long subjectId = Convert.ToInt64(data1.SubjectId);
                var olddata = _Entities.tb_StudentMarks.Where(x => x.ExamId == examId && x.SubjectId == subjectId).ToList();
                if (olddata.Count > 0)
                {
                    foreach (var item in olddata)
                    {
                        _Entities.tb_StudentMarks.Remove(item);
                        status = _Entities.SaveChanges() > 0;
                    }
                }
                foreach (var item in data)
                {
                    var mark = _Entities.tb_StudentMarks.Create();
                    mark.StudentId = item.StudentId;
                    mark.ExamId = examId;
                    mark.SubjectId = subjectId;
                    mark.Mark = item.Total;
                    mark.IsActive = true;
                    mark.TimeStamp = CurrentTime;
                    mark.InternalMark = item.InternalMark;
                    mark.ExternalMark = item.ExternalMark;
                    _Entities.tb_StudentMarks.Add(mark);
                    status = _Entities.SaveChanges() > 0;
                    msg = "Success";
                }
                //var dataList = _Entities.sp_StudentMarkList(examId, subjectId).ToList(); //Students Mark Details
                //AddStudentMarkModel model = new AddStudentMarkModel();
                //model.StudentList = new List<StudentList>();
                //model.SubjectId = subjectId;
                //model.SchoolId = _user.SchoolId;
                //model.ExamId = examId;
                //foreach (var item in dataList)
                //{
                //    StudentList one = new StudentList();
                //    one.StudentId = item.StudentId ?? 0;
                //    one.StudentName = item.StundentName;
                //    one.InternalMark = item.StudentInternalMark ?? 0;
                //    one.ExternalMark = item.StudentExternalMark ?? 0;
                //    one.Total = item.StudentTotalMark;
                //    one.ExamId = item.ExamId;
                //    one.SubjectId = item.SubId;
                //    model.StudentList.Add(one);
                //}
                //model.StudentList = model.StudentList.OrderBy(XmlSiteMapProvider => XmlSiteMapProvider.StudentName).ToList();
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                msg = "Success";
                status = true;
                return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public IActionResult StudentMarks()
        {
            StudentMarksEntry model = new StudentMarksEntry();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult DiaryUpload()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public object DiaryUploadPDF()
        {
            bool status = false;
            string msg = "failed";
            //String FileExt = Path.GetExtension(files.FileName).ToUpper();

            if (Request.Files.Count > 0)
            {
                var httpPostedFile = Request.Files[0];
                string folderPath = Server.MapPath("~/Media/School/Diary/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string pdfName = _user.tb_School.SchoolName + Guid.NewGuid().ToString() + ".pdf";
                var pdfFilePath = Server.MapPath("~/Media/School/Diary/" + pdfName);
                var fileSave = "/Media/School/Diary/" + pdfName;
                httpPostedFile.SaveAs(pdfFilePath);
                msg = "Success";
                status = true;


                try
                {
                    tb_File file = new tb_File();
                    file.FilePath = fileSave;
                    file.FileModule = 1;//Diary
                    file.FileType = 3;//pdf
                    file.SchoolId = _user.SchoolId;
                    file.IsActive = true;
                    file.TimeStamp = CurrentTime;
                    _Entities.tb_File.Add(file);
                    status = _Entities.SaveChanges() > 0;
                    msg = "Success";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }


            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //}
        //------------------------ARCHANA K V --------16-Mar-2018-----------------
        public IActionResult StaffMessages()
        {
            var model = new SchoolModelForId();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public object SMSStaff(SchoolModelForId model)
        {
            HttpClient client = new HttpClient();
            var history = new tb_SmsHistory();
            var numbers = new List<string>();
            var MsgId = new List<string>();

            var numb = "";
            string message = "Failed";
            var statusFail = "100";
            var status = false;
            string messagepre = "";
            long staffId = 0;
            int userType = 0;
            List<SendStaffMessage> Userdata = JsonConvert.DeserializeObject<List<SendStaffMessage>>(model.Data);

            var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (school.SmsActive)
            {
                var package = _Entities.tb_SmsPackage.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.IsDisabled == false).FirstOrDefault();
                if (package != null)
                {
                    if (package.ToDate >= CurrentTime)
                    {
                        //     ---------------------------------


                        if (Userdata.Count > 0 && Userdata != null)
                        {
                            //foreach (var Userdata1 in Userdata)
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
                                smsHead.Head = Userdata[0].Description;
                                smsHead.SchoolId = _user.SchoolId;
                                smsHead.TimeStamp = CurrentTime;
                                smsHead.IsActive = true;
                                smsHead.SenderType = (int)SMSSendType.Staff;
                                _Entities.tb_SmsHead.Add(smsHead);
                                status = _Entities.SaveChanges() > 0;

                                foreach (var item in Userdata[0].list)
                                {
                                    staffId = Convert.ToInt64(item.StaffId);
                                    userType = Convert.ToInt32(item.Type);
                                    var staffDetails = _Entities.tb_Login.Where(z => z.UserId == staffId && z.IsActive).FirstOrDefault();
                                    messagepre = "Dear " + staffDetails.Name;
                                    messagepre = messagepre + " ,  " + Userdata[0].Description;
                                    //-----SPECIAL CHARACTER SENDING -------------------
                                    messagepre = messagepre.Replace("#", "%23");
                                    messagepre = messagepre.Replace("&", "%26");
                                    //--------------------------------------------------
                                    var phone = item.Number.ToString();
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
                                        statusFail = "0";
                                        tb_StaffSMSHistory sms = new tb_StaffSMSHistory();
                                        sms.StaffId = staffId;
                                        sms.MessageContent = Userdata[0].Description;
                                        sms.SendStatus = Convert.ToString(respList.success);
                                        sms.MessageDate = CurrentTime;
                                        sms.IsActive = true;
                                        sms.ScholId = _user.SchoolId;
                                        sms.MobileNumber = phone;
                                        sms.SmsSentPerStudent = smsCount;
                                        sms.HeadId = smsHead.HeadId;
                                        sms.UserType = userType;
                                        if (respList.data != null)
                                        {
                                            sms.MessageReturnId = respList.data[0].messageId;
                                            sms.DelivaryStatus = "Pending";
                                        }
                                        _Entities.tb_StaffSMSHistory.Add(sms);
                                        _Entities.SaveChanges();
                                    }
                                    string emailId = staffDetails.Username;
                                    string subject = "Message from School";
                                    string schoolName = staffDetails.tb_School.SchoolName;
                                    SendSMSEmail(messagepre, emailId, subject, schoolName);
                                }
                            }
                        }
                    }
                    else
                    {
                        message = "Package validity expired";
                        statusFail = "2";
                    }
                }
                else
                {
                    message = "Please update your package";
                    statusFail = "3";
                }
            }
            else
            {
                //Basheer on 25/01/2019 changed spelling mistake
                message = "SMS cannot be send please contact our support team";
                statusFail = "1";
            }
            return Json(new { Status = status, statusFail = statusFail, message = message }, JsonRequestBehavior.AllowGet);

        }

        private void SendSMSEmail(string msg, string emailId, string subject, string schoolName)
        {
            try
            {
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/BirthdayWishesEmail.html");
                var emailTemplate = System.IO.File.ReadAllText(filePath);
                var mBody = emailTemplate.Replace("{{resetLink}}", msg).Replace("{{resetLink1}}", schoolName);
                bool sendMail = Send(subject, mBody, emailId);
            }
            catch (Exception ex)
            { }
        }

        //--------------------------ARCHANA K V ------------20-MAR-2018
        public IActionResult SettingsHome()
        {
            FeeAlertDataModel model = new FeeAlertDataModel();
            var data = _Entities.tb_FeeAlertData.Where(x => x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
            if (data != null)
            {
                model.Id = data.Id;
                model.AlertDate = data.AlertDate;
            }

            var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            model.SchoolId = _user.SchoolId;
            model.FooterMessage = school.BillingFooterMessage;
            model.libraryDueDay = school.LibraryDueDays ?? 0;
            var libDueFine = school.tb_LibraryFine.FirstOrDefault();
            if (libDueFine != null)
            {
                model.feeId = libDueFine.FeeId;
                model.libFineAmount = libDueFine.FineAmount;
            }
            else
            {
                model.feeId = 0;
            }
            model.SenderDetails = new SchoolSenderIdModel();
            var sender = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (sender != null)
            {
                model.SenderDetails.SenderData = sender.SenderId;
                model.SenderDetails.SenderId = sender.Id;
            }
            else
            {
                model.SenderDetails.SenderData = "MYSCHO";
            }
            var type = new TrackTap.DataLibrary.Data.School(_user.SchoolId).SalaryTypeStatus();
            if (type == 0)
                model.SalaryType = SalaryType.Basic;
            else
                model.SalaryType = SalaryType.Gross;
            return View(model);
        }
        [HttpPost]
        public object AddBillingFooter(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                school.BillingFooterMessage = model.FooterMessage;
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public object AddLibraryDueDays(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var school = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                school.LibraryDueDays = model.libraryDueDay;
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object AddLibraryDueFine(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var libFine = _Entities.tb_LibraryFine.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                if (libFine != null)
                {
                    libFine.FeeId = model.feeId;
                    libFine.FineAmount = model.libFineAmount;
                }
                else
                {
                    tb_LibraryFine LibraryFine = new tb_LibraryFine();
                    LibraryFine.FeeId = model.feeId;
                    LibraryFine.FineAmount = model.libFineAmount;
                    LibraryFine.SchoolId = _user.SchoolId;
                    LibraryFine.IsActive = true;
                    LibraryFine.TimeStamp = CurrentTime;
                    _Entities.tb_LibraryFine.Add(LibraryFine);
                }
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object AddNewFeeAlertDate(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var date = _Entities.tb_FeeAlertData.Create();
                date.AlertDate = Convert.ToDateTime(model.AlertDate);
                date.SchoolId = _user.SchoolId;
                date.IsActive = true;
                date.TimeStamp = CurrentTime;
                _Entities.tb_FeeAlertData.Add(date);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //Code added by Gayathri(06/12/2023)
        [HttpPost]
        public object EditNewFeeAlertDate(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var date = _Entities.tb_FeeAlertData.Where(x=>x.IsActive && x.SchoolId==model.SchoolId).FirstOrDefault();
                if(date !=null)
                {
                date.AlertDate = Convert.ToDateTime(model.AlertDate);
                date.SchoolId = _user.SchoolId;
                date.IsActive = true;
                date.TimeStamp = CurrentTime;
                //_Entities.tb_FeeAlertData.Add(date);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public object CheckSMSDeliveryData(string id)
        {
            string msg = "Failed";
            bool status = false;
            long head = Convert.ToInt64(id);
            var data = new TrackTap.DataLibrary.Data.SmsHead(head).StaffSMSHistory;
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item.MessageReturnId != null)
                    {
                        var url = "http://alvosms.in/api/v1/dlr?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&message-id=" + item.MessageReturnId;
                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest request = this.GetRequest(url);
                        WebResponse webResponse = request.GetResponse();
                        var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                        var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                        DelivaryCheck respList = new JavaScriptSerializer().Deserialize<DelivaryCheck>(responseText);
                        var sms = _Entities.tb_StaffSMSHistory.Where(x => x.Id == item.Id).FirstOrDefault();
                        sms.DelivaryStatus = respList.dlr;
                        _Entities.SaveChanges();
                        status = true;
                        msg = "Successful";
                    }
                }
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAllSmsHistoryWithStatus(string id)
        {
            long headId = Convert.ToInt64(id);
            var model = new SMSHistoryModel();
            model.headId = headId;
            model.SenderType = 1;
            return PartialView("~/Views/School/_pv_StaffSMSHistory.cshtml", model);
        }
        public object CheckSMSDeliveryDataStudent(string id)
        {
            string msg = "Failed";
            bool status = false;
            long head = Convert.ToInt64(id);
            var data = new TrackTap.DataLibrary.Data.SmsHead(head).SmsHistory;
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item.MessageReturnId != null)
                    {
                        var url = "http://alvosms.in/api/v1/dlr?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&message-id=" + item.MessageReturnId;
                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest request = this.GetRequest(url);
                        WebResponse webResponse = request.GetResponse();
                        var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                        var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                        DelivaryCheck respList = new JavaScriptSerializer().Deserialize<DelivaryCheck>(responseText);
                        var sms = _Entities.tb_SmsHistory.Where(x => x.Id == item.Id).FirstOrDefault();
                        sms.DelivaryStatus = respList.dlr;
                        _Entities.SaveChanges();
                        status = true;
                        msg = "Successful";
                    }
                }
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetAllSmsHistoryWithStatusStudent(string id)
        {
            long headId = Convert.ToInt64(id);
            var model = new SMSHistoryModel();
            model.headId = headId;
            model.SenderType = 0;
            return PartialView("~/Views/School/_pv_StudentSMSHistory.cshtml", model);
        }
        [HttpGet]
        public object SendBirthdayWishes()
        {
            string msg = "Successful";
            bool status = false;
            long schoolId = _user.SchoolId;
            try
            {
                var studentList = _Entities.tb_Student.Where(x => x.SchoolId == schoolId && x.DOB.Value.Month == CurrentTime.Month && x.DOB.Value.Day == CurrentTime.Day).ToList().Select(x => new DataLibrary.Data.Student(x)).ToList();
                if (studentList.Count > 0)
                {
                    var smsHead = new tb_SmsHead();
                    smsHead.Head = "Todays Birthday Wishes from your School";
                    smsHead.SchoolId = schoolId;
                    smsHead.TimeStamp = CurrentTime;
                    smsHead.IsActive = true;
                    smsHead.SenderType = 0;//For student
                    _Entities.tb_SmsHead.Add(smsHead);
                    status = _Entities.SaveChanges() > 0;
                    foreach (var item in studentList)
                    {
                        long parentId = item.ParentId ?? 0;
                        var parent = new Parent(parentId);
                        var schoolData = new School(item.SchoolId);

                        #region SMS
                        SendBirthDayWishSMS(item, schoolData, smsHead.HeadId);
                        #endregion SMS

                        #region PUSH
                        if (item.ParentId != null && item.ParentId != 0)
                        {
                            SendBirthDayWishPush(item, schoolData);
                        }
                        #endregion PUSH

                        #region EMAIL
                        SendBirthDayWishEMAIL(item, schoolData, parent);
                        #endregion EMAIL
                    }
                    status = true;
                    msg = "Successful";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        private void SendBirthDayWishEMAIL(DataLibrary.Data.Student item, School school, Parent parent)
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/BirthdayWishesEmail.html");
            var emailTemplate = System.IO.File.ReadAllText(filePath);
            //string Message = "Happy Birthday to you " + item.StundentName + " from " + school.SchoolName;
            string Message = "Dear " + item.StundentName + ",  May the joy that you have spread in the past come back to you on this day. Wishes you a very happy birthday! - " + _user.tb_School.SchoolName;
            var mBody = emailTemplate.Replace("{{resetLink}}", Message).Replace("{{resetLink1}}", school.SchoolName);
            bool sendMail = Send("Birthday Wishes", mBody, item.ParentEmail);
        }

        private void SendBirthDayWishPush(DataLibrary.Data.Student item, School school)
        {
            try
            {
                var tokenData = _Entities.tb_DeviceToken.Where(x => x.UserId == item.ParentId && x.IsActive == true && x.LoginStatus == 1).OrderByDescending(x => x.TokenId).FirstOrDefault();
                if (tokenData != null)
                {
                    string message = "Happy Birthday to you " + item.StundentName + " from " + school.SchoolName;
                    var applicationID = "";
                    var senderId = "";
                    var pushData = _Entities.tb_PushData.Where(x => x.SchoolId == item.SchoolId).FirstOrDefault();
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
                            title = "Birthday Wishes"
                        },
                        priority = "high",
                        data = new
                        {
                            Role = "School",
                            Function = "Birthday"
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

        private void SendBirthDayWishSMS(DataLibrary.Data.Student student, School school, long headId)
        {



            var phone = student.ContactNumber.ToString();
            var senderName = "MYSCHO";
            //if (student.SchoolId == 10116)
            //{
            //    senderName = "PARDSE";
            //}
            //else if (student.SchoolId == 10117)
            //{
            //    senderName = "HOLYIN";
            //}
            var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true).FirstOrDefault();
            if (senderData != null)
                senderName = senderData.SenderId;
            //string message = "Happy Birthday to you " + student.StundentName + " from " + school.SchoolName;
            //string message = "Dear, May the joy that you have spread in the past come back to you on this day. Wishes you a very happy birthday!";
            string message = "Dear " + student.StundentName + ",  May the joy that you have spread in the past come back to you on this day. Wishes you a very happy birthday!- " + _user.tb_School.SchoolName;
            var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + message + "&sender=" + senderName;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest request = this.GetRequest(url);
            WebResponse webResponse = request.GetResponse();
            var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
            alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);

            //--------------For History --------------------
            tb_SmsHistory sms = new tb_SmsHistory();
            sms.IsActive = true;
            sms.MessageContent = message;
            sms.MessageDate = CurrentTime;
            sms.ScholId = school.SchoolId;
            sms.StuentId = student.StudentId;
            sms.MobileNumber = phone;
            sms.HeadId = headId;
            sms.SendStatus = Convert.ToString(respList.success);
            if (respList.data != null)
            {
                sms.MessageReturnId = respList.data[0].messageId;
                sms.DelivaryStatus = "Pending";
            }
            sms.SmsSentPerStudent = 1;
            _Entities.tb_SmsHistory.Add(sms);
            _Entities.SaveChanges();
            //--------------------------------------------------
        }
        private bool Send(string subject, string mailbody, string email)
        {
            ////try
            ////{
            ////    MailMessage msg = new MailMessage();
            ////    //SmtpClient client = new SmtpClient();
            ////    msg.Subject = subject;
            ////    msg.Body = mailbody;
            ////    msg.From = new MailAddress("info.schoolman@gmail.com");
            ////    msg.To.Add(new MailAddress(email));
            ////    msg.IsBodyHtml = true;

            ////    SmtpClient client = new SmtpClient();
            ////    //client.Host = "k2smtp.gmail.com";
            ////    client.Host = "k2smtpout.secureserver.net";
            ////    //System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("info.schoolman@gmail.com", "Info@123");
            ////    NetworkCredential basicauthenticationinfo = new NetworkCredential("info.schoolman@gmail.com", "Info@123");
            ////    client.Port = int.Parse("587");//25//465
            ////    client.EnableSsl = true;
            ////    client.UseDefaultCredentials = false;
            ////    client.Credentials = basicauthenticationinfo;
            ////    client.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
            ////            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            ////            System.Security.Cryptography.X509Certificates.X509Chain chain,
            ////            System.Net.Security.SslPolicyErrors sslPolicyErrors)
            ////    {
            ////        return true;
            ////    };
            ////    try
            ////    {
            ////        client.Send(msg);
            ////        client.Dispose();
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////    }

            ////}
            ////catch (Exception ex)
            ////{

            ////}
            try
            {
                MailMessage msg = new MailMessage();
                msg.Subject = subject;
                msg.Body = mailbody;
                msg.From = new MailAddress("schoolman@srishtis.com");
                msg.To.Add(new MailAddress(email, "Dear"));
                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                //client.Host = "k2smtpout.secureserver.net";
                client.Host = "smtpout.secureserver.net";
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("schoolman@srishtis.com", "ca@12345");
                //client.Port = int.Parse("25");
                client.Port = int.Parse("80");
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
            }
            catch (Exception ex)
            {

            }
            return true;
        }
        public object SendFeeDueAlert()
        {
            bool status = false;
            string msg = "Failed";
            long InsertHead = 0;
            long SchoolId = _user.SchoolId;
            try
            {
                var alarmDate = _Entities.tb_FeeAlertData.Where(x => x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
                if (alarmDate != null)
                {
                    var mainData = _Entities.sp_FeeAlertDetails().ToList();
                    var data = mainData.Where(x => x.SchoolId == SchoolId).ToList();
                    if (data != null && data.Count > 0)
                    {
                        var students = data.Select(o => o.StudentId).Distinct().ToList();

                        var smsHead = new tb_SmsHead();
                        smsHead.Head = "Fee Due Alert From Site";
                        smsHead.SchoolId = SchoolId;
                        smsHead.TimeStamp = CurrentTime;
                        smsHead.IsActive = true;
                        smsHead.SenderType = 0;//For Student
                        _Entities.tb_SmsHead.Add(smsHead);
                        status = _Entities.SaveChanges() > 0;
                        var schoolAlertDate = _Entities.tb_FeeAlertData.Where(x => x.SchoolId == SchoolId && x.IsActive).FirstOrDefault();
                        schoolAlertDate.IsActive = false;
                        _Entities.SaveChanges();
                        InsertHead = smsHead.HeadId;
                        foreach (var item in students)
                        {
                            var isNeedStatus = false;// If the student have bill amount, but it's less than the advance amount , then there is no need to send the SMS
                            var Amount = data.Where(z => z.StudentId == item).Sum(z => z.Amount);
                            if (Amount != null && Amount != 0)
                            {
                                var student = data.Where(x => x.StudentId == item).FirstOrDefault();
                                var advance = _Entities.tb_StudentBalance.Where(z => z.IsActive == true && z.StudentId == student.StudentId).FirstOrDefault();
                                if (advance != null)
                                {
                                    if (advance.Amount > Amount)
                                    {
                                        isNeedStatus = true;
                                    }
                                    else
                                    {
                                        Amount = Amount - advance.Amount;
                                    }
                                }
                                if (isNeedStatus == false)
                                {
                                    if (student.ContactNumber != null && student.ContactNumber != string.Empty)
                                    {
                                        #region SMS
                                        SendFeeAlertSMS(Amount, student, InsertHead);
                                        #endregion

                                        var studentData = new TrackTap.DataLibrary.Data.Student(student.StudentId);
                                        #region Push
                                        if (studentData.ParentId != null)
                                        {
                                            SendFeeAlertPush(Amount ?? 0, studentData);
                                        }
                                        #endregion

                                        #region Email
                                        try
                                        {
                                            SendFeeAlertMail(Amount, studentData.ParentEmail, studentData.SchoolName, student.StundentName);
                                        }
                                        catch { }
                                        #endregion
                                        msg = "Successful";
                                        status = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        msg = "No dues";
                    }
                }
                else
                {
                    msg = "Please save a date for due start !";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        private void SendFeeAlertSMS(decimal? Amount, sp_FeeAlertDetails_Result student, long InsertHead)
        {
            var dueAmount = Math.Round((double)Amount, 2);
            var school = new TrackTap.DataLibrary.Data.School(student.SchoolId);
            var phone = student.ContactNumber.ToString();
            var senderName = "MYSCHO";
            //if (student.SchoolId == 10116)
            //{
            //    senderName = "PARDSE";
            //}
            //else if (student.SchoolId == 10117)
            //{
            //    senderName = "HOLYIN";
            //}
            var senderData = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true).FirstOrDefault();
            if (senderData != null)
                senderName = senderData.SenderId;
            //string message = "Your kid " + student.StundentName + " have RS : " + dueAmount + " /- fee due from " + school.SchoolName + " School";
            string message = "Dear parent of " + student.StundentName + " , Gentle reminder that there is a fees due of Rs." + dueAmount + " /- .Kindly remit the fee at the earliest.";

            var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + message + "&sender=" + senderName;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest request = this.GetRequest(url);
            WebResponse webResponse = request.GetResponse();
            var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
            alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);

            tb_SmsHistory sms = new tb_SmsHistory();
            sms.IsActive = true;
            sms.MessageContent = message;
            sms.MessageDate = CurrentTime;
            sms.ScholId = student.SchoolId;
            sms.StuentId = student.StudentId;
            sms.MobileNumber = phone;
            sms.HeadId = InsertHead;
            sms.SendStatus = Convert.ToString(respList.success);
            sms.SmsSentPerStudent = 1;
            if (respList.data != null)
            {
                sms.MessageReturnId = respList.data[0].messageId;
                sms.DelivaryStatus = "Pending";
            }
            _Entities.tb_SmsHistory.Add(sms);
            _Entities.SaveChanges();
        }
        private void SendFeeAlertPush(decimal? Amount, TrackTap.DataLibrary.Data.Student student)
        {
            try
            {
                var tokenData = _Entities.tb_DeviceToken.Where(x => x.UserId == student.ParentId && x.IsActive == true && x.LoginStatus == 1).OrderBy(x => x.TokenId).FirstOrDefault();
                if (tokenData != null)
                {
                    var dueAmount = Math.Round((double)Amount, 2);
                    //string message = "Happy Birthday to you " + student.StundentName + " from " + student.SchoolName;
                    string message = "Your kid " + student.StundentName + " have RS : " + dueAmount + " /- fee due from " + student.SchoolName + " School";
                    var applicationID = "";
                    var senderId = "";
                    var pushData = _Entities.tb_PushData.Where(x => x.SchoolId == student.SchoolId).FirstOrDefault();
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
                            title = "Birthday Wishes"
                        },
                        priority = "high",
                        data = new
                        {
                            Role = "School",
                            Function = "Birthday"
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
        private bool SendFeeAlertMail(decimal? amount, string emailId, string schoolName, string StundentName)
        {
            try
            {
                var dueAmount = Math.Round((double)amount, 2);
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/BirthdayWishesEmail.html");
                var emailTemplate = System.IO.File.ReadAllText(filePath);
                //string Message = "Your kid " + StundentName + " have RS : " + dueAmount + " /- fee due from " + schoolName + " School";
                string Message = "Dear parent of " + StundentName + " , Gentle reminder that there is a fees due of Rs." + dueAmount + " /- .Kindly remit the fee at the earliest.";
                var mBody = emailTemplate.Replace("{{resetLink}}", Message).Replace("{{resetLink1}}", schoolName);
                bool sendMail = Send("Fee due alert", mBody, emailId);
            }
            catch (Exception ex) { }
            return true;
        }
        public object SendCircularNotificationMessages()
        {
            bool status = false;
            string msg = "Faile";
            bool sendData = false;
            long schoolId = _user.SchoolId;
            try
            {
                var notificationMain = _Entities.SP_CircularNotification(CurrentTime).ToList();
                var notification = notificationMain.Where(x => x.SchoolId == schoolId).ToList();
                if (notification.Count > 0)
                {
                    foreach (var item2 in notification)
                    {
                        var student = _Entities.tb_Student.Where(x => x.IsActive && x.ParentId != null && x.SchoolId == item2.SchoolId).ToList();
                        if (student.Count > 0)
                        {
                            foreach (var item in student)
                            {
                                string school = item.tb_School.SchoolName;
                                string kidName = item.StundentName;
                                long parentId = item.ParentId ?? 0;
                                SendMailsForCircularNotification(item, item2, school, kidName);
                                if (item.ParentId != null)
                                {
                                    var parentDetails = _Entities.tb_DeviceToken.Where(x => x.UserId == item.ParentId && x.LoginStatus == 1).ToList().GroupBy(x => new { x.UserId, x.TokenId }).Select(x => x.FirstOrDefault()).ToList();
                                    foreach (var data in parentDetails)
                                    {
                                        long studentUserId = Convert.ToInt64(item.StudentId);
                                        var message = "Your Kid's  School have an event " + item2.Description + " on " + item2.CircularDate.ToShortDateString();
                                        circularPushandroid(data.Token, message, school, studentUserId, item.SchoolId, item2);
                                    }
                                    status = true;
                                    msg = "Successful";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        private bool circularPushandroid(string deviceId, string message, string tittle, long kidId, long schoolId, SP_CircularNotification_Result note)
        {
            try
            {
                var applicationID = "";
                var senderId = "";
                var pushData = _Entities.tb_PushData.Where(x => x.SchoolId == schoolId).FirstOrDefault();
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
                var dataMain = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = message,
                        title = tittle,
                        sound = "Enabled"
                    },
                    priority = "high",
                    data = new
                    {
                        KidId = kidId,
                        SchoolId = schoolId,
                        Date = note.CircularDate,
                        Description = note.Description,
                        Filepath = note.FilePath,
                        Role = "Teacher",
                        Function = "Events"
                    },
                    from = "Teacher",
                    Type = "Events"
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(dataMain);
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
                                //Response.Write(sResponseFromServer);
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
            return true;
        }
        private bool SendMailsForCircularNotification(tb_Student data, SP_CircularNotification_Result eventData, string school, string kidName)
        {
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/template/CircularNotification.html");
            var emailTemplate = System.IO.File.ReadAllText(filePath);
            string Message = "Your kid " + kidName + "'s School have an event ";
            string message2 = eventData.Description + " on " + eventData.CircularDate.ToShortDateString();
            string filePathDoc = "http://www.schoolman.in" + eventData.FilePath;
            string downLoad = "Download";
            if (eventData.FilePath == null || eventData.FilePath == "")
            {
                downLoad = "No files for Download";
            }
            var mBody = emailTemplate.Replace("{{resetLink}}", message2).Replace("{{resetLink1}}", school).Replace("{{resetLink2}}", Message).Replace("{{resetLink3}}", filePathDoc).Replace("{{resetLink4}}", downLoad);
            bool sendMail = Send("Events", mBody, data.ParentEmail);
            return sendMail;
        }
        [HttpPost]
        public object DiaryUploadPDFEdit()
        {
            bool status = false;
            string msg = "failed";
            var fileSave = "";
            //String FileExt = Path.GetExtension(files.FileName).ToUpper();
            var isDiary = _Entities.tb_File.Where(z => z.SchoolId == _user.SchoolId && z.IsActive && z.FileModule == 1 && z.FileType == 3).FirstOrDefault();
            if (isDiary != null)
            {
                string path = Server.MapPath(isDiary.FilePath);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
                if (Request.Files.Count > 0)
                {
                    var httpPostedFile = Request.Files[0];
                    string folderPath = Server.MapPath("~/Media/School/Diary/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    string pdfName = _user.tb_School.SchoolName + Guid.NewGuid().ToString() + ".pdf";
                    var pdfFilePath = Server.MapPath("~/Media/School/Diary/" + pdfName);
                    fileSave = "/Media/School/Diary/" + pdfName;
                    httpPostedFile.SaveAs(pdfFilePath);
                    msg = "Success";
                    status = true;

                    try
                    {
                        isDiary.FilePath = fileSave;
                        isDiary.TimeStamp = CurrentTime;
                        status = _Entities.SaveChanges() > 0;
                        msg = "Success";
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
            }
            return Json(new { status = status, msg = msg, url = fileSave }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public object LogoUploadEdit()
        {
            bool status = false;
            string msg = "failed";
            var fileSave = "";
            //String FileExt = Path.GetExtension(files.FileName).ToUpper();
            var isLogo = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId && z.IsActive).FirstOrDefault();
            if (isLogo != null)
            {
                string path = Server.MapPath(isLogo.FilePath);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
                if (Request.Files.Count > 0)
                {
                    var httpPostedFile = Request.Files[0];
                    string folderPath = Server.MapPath("~/Media/School/Logo/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    string pdfName = _user.tb_School.SchoolName + Guid.NewGuid().ToString() + ".jpeg";
                    var pdfFilePath = Server.MapPath("~/Media/School/Logo/" + pdfName);
                    fileSave = "/Media/School/Logo/" + pdfName;
                    httpPostedFile.SaveAs(pdfFilePath);
                    msg = "Success";
                    status = true;

                    try
                    {
                        isLogo.FilePath = fileSave;
                        status = _Entities.SaveChanges() > 0;
                        msg = "Success";
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
            }
            return Json(new { status = status, msg = msg, url = fileSave }, JsonRequestBehavior.AllowGet);
        }

        public object LogoUpload()
        {
            bool status = false;
            string msg = "failed";
            var fileSave = "";
            //String FileExt = Path.GetExtension(files.FileName).ToUpper();
            var isLogo = _Entities.tb_School.Where(z => z.SchoolId == _user.SchoolId && z.IsActive).FirstOrDefault();
            if (isLogo != null)
            {
                string path = Server.MapPath(isLogo.FilePath);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
                if (Request.Files.Count > 0)
                {
                    var httpPostedFile = Request.Files[0];
                    string folderPath = Server.MapPath("~/Media/School/Logo/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    string pdfName = _user.tb_School.SchoolName + Guid.NewGuid().ToString() + ".jpeg";
                    var pdfFilePath = Server.MapPath("~/Media/School/Logo/" + pdfName);
                    fileSave = "/Media/School/Logo/" + pdfName;
                    httpPostedFile.SaveAs(pdfFilePath);
                    msg = "Success";
                    status = true;

                    try
                    {
                        isLogo.FilePath = fileSave;
                        isLogo.TimeStamp = CurrentTime;
                        status = _Entities.SaveChanges() > 0;
                        msg = "Success";
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
            }
            return Json(new { status = status, msg = msg, url = fileSave }, JsonRequestBehavior.AllowGet);
        }


        #region Lab
        public IActionResult LaboratoryCategory()
        {
            LaboratoryInventoryModels model = new LaboratoryInventoryModels();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult EditLaboratoryCategoryView(string id)
        {
            LaboratoryInventoryModels model = new LaboratoryInventoryModels();
            long catId = Convert.ToInt64(id);
            var category = _Entities.tb_LaboratoryCategory.Where(z => z.CategoryId == catId).FirstOrDefault();
            model.schoolId = _user.SchoolId;
            model.categoryId = category.CategoryId;
            model.laboratoryName = category.LaboratoryName;
            return PartialView("~/Views/School/_pv_LaboratoryCategory_Edit.cshtml", model);
        }


        public PartialViewResult AddLaboratoryCategoryView()
        {
            LaboratoryInventoryModels model = new LaboratoryInventoryModels();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_LaboratoryCategory_Add.cshtml", model);
        }
        [HttpPost]
        public object AddLaboratoryCategory(LaboratoryInventoryModels model)
        {
            bool status = false;
            string message = "Failed";
            var category = _Entities.tb_LaboratoryCategory.Create();
            category.LaboratoryName = model.laboratoryName;
            category.SchoolId = model.schoolId;
            category.IsActive = true;
            _Entities.tb_LaboratoryCategory.Add(category);
            status = _Entities.SaveChanges() > 0;
            message = status ? " Category Added" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object EditLaboratoryCategory(LaboratoryInventoryModels model)
        {
            bool status = false;
            string message = "Failed";
            var category = _Entities.tb_LaboratoryCategory.Where(z => z.CategoryId == model.categoryId).FirstOrDefault();
            category.LaboratoryName = model.laboratoryName;
            status = _Entities.SaveChanges() > 0;
            message = status ? " Category Edited" : "Failed";
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public object DeleteLaboratoryCategory(string id)
        {
            bool status = false;
            string message = "Failed";
            long catId = Convert.ToInt64(id);
            var category = _Entities.tb_LaboratoryCategory.Where(z => z.CategoryId == catId).FirstOrDefault();

            if (_Entities.tb_LaboratoryCategory.Any(x => x.CategoryId == catId))
            {
                category.IsActive = false;
                status = _Entities.SaveChanges() > 0;
                message = status ? " Category deleted successfully" : "Failed to delete Category";
            }
            else
            {
                _Entities.tb_LaboratoryCategory.Remove(category);
                status = _Entities.SaveChanges() > 0 ? true : false;
                message = status ? "Category deleted successfully!" : "Failed to delete Category!";
            }

            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult LaboratoryCategoryListPartial()
        {
            LaboratoryInventoryModels model = new LaboratoryInventoryModels();
            model.schoolId = _user.SchoolId;
            return PartialView("~/Views/School/_pv_LaboratoryCategory_list.cshtml", model);

        }
        #endregion



        #region Forms
        public IActionResult DOCertificate(string id)
        {
            long studentId = Convert.ToInt64(id);
            var student = new TrackTap.DataLibrary.Data.Student(studentId);
            StudentModel model = new StudentModel();
            model.studentName = student.StundentName;
            model.studentId = student.StudentId;
            model.classId = student.ClassId;
            model.className = student.ClassName + " / " + student.DivisionName;
            model.division = student.DivisionName;
            model.divisionId = student.DivisionId;
            model.parentName = student.ParentName;
            if (student.DOB != null && student.DOB.Value.Year != 01)
            {
                model.DOBstring = Convert.ToString(Convert.ToDateTime(student.DOB).ToString("dd/MM/yyyy")) ?? "";
            }
            else
            {
                model.DOBstring = " ";
            }
            model.CurrentDate = CurrentTime.ToString("dd/MM/yyyy");
            if (CurrentTime.Month < 6)
            {
                model.AcademinYear = CurrentTime.AddYears(-1).Year + " - " + CurrentTime.Year;
            }
            else
            {
                model.AcademinYear = CurrentTime.Year + " - " + CurrentTime.AddYears(-1).Year;
            }
            return View(model);
        }
        public IActionResult AadharCardForm(string id)
        {
            long studentId = Convert.ToInt64(id);
            var student = new TrackTap.DataLibrary.Data.Student(studentId);
            StudentModel model = new StudentModel();
            model.studentName = student.StundentName;
            model.studentId = student.StudentId;
            model.classId = student.ClassId;
            model.className = student.ClassName + " / " + student.DivisionName;
            model.division = student.DivisionName;
            model.divisionId = student.DivisionId;
            model.parentName = student.ParentName;
            if (student.DOB != null && student.DOB.Value.Year != 01)
            {
                model.DOBstring = Convert.ToString(Convert.ToDateTime(student.DOB).ToString("dd/MM/yyyy")) ?? "";
            }
            else
            {
                model.DOBstring = " ";
            }
            model.CurrentDate = CurrentTime.ToString("dd/MM/yyyy");
            if (CurrentTime.Month < 6)
            {
                model.AcademinYear = CurrentTime.AddYears(-1).Year + " - " + CurrentTime.Year;
            }
            else
            {
                model.AcademinYear = CurrentTime.Year + " - " + CurrentTime.AddYears(-1).Year;
            }
            return View(model);
        }
        public IActionResult FeeRemittance(string id)
        {
            long studentId = Convert.ToInt64(id);
            var student = new TrackTap.DataLibrary.Data.Student(studentId);
            StudentModel model = new StudentModel();
            model.studentName = student.StundentName;
            model.studentId = student.StudentId;
            model.classId = student.ClassId;
            model.className = student.ClassName + " / " + student.DivisionName;
            model.division = student.DivisionName;
            model.divisionId = student.DivisionId;
            model.parentName = student.ParentName;
            if (student.DOB != null && student.DOB.Value.Year != 01)
            {
                model.DOBstring = Convert.ToString(Convert.ToDateTime(student.DOB).ToString("dd/MM/yyyy")) ?? "";
            }

            model.CurrentDate = CurrentTime.ToString("dd/MM/yyyy");
            if (CurrentTime.Month < 6)
            {
                model.AcademinYear = CurrentTime.AddYears(-1).Year + " - " + CurrentTime.Year;
            }
            else
            {
                model.AcademinYear = CurrentTime.Year + " - " + CurrentTime.AddYears(-1).Year;
            }
            return View(model);
        }
        #endregion Forms

        private DateTime ConvertDateToServer(string stringDate)
        {
            string[] splitData = stringDate.Split('-');
            var dd = splitData[0];
            var mm = splitData[1];
            var yyyy = splitData[2];
            var retDate = mm + '-' + dd + '-' + yyyy;
            return Convert.ToDateTime(retDate);
        }

        private bool onlyCircularPushandroid(string deviceId, string message, string tittle, long kidId, long schoolId, tb_Circular note)
        {
            try
            {
                var applicationID = "";
                var senderId = "";
                var pushData = _Entities.tb_PushData.Where(x => x.SchoolId == schoolId).FirstOrDefault();
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
                var dataMain = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = message,
                        title = tittle,
                        sound = "Enabled"
                    },
                    priority = "high",
                    data = new
                    {
                        KidId = kidId,
                        SchoolId = schoolId,
                        Date = note.CircularDate,
                        Description = note.Description,
                        Filepath = note.FilePath,
                        Role = "Teacher",
                        Function = "Circular"
                    },
                    from = "Teacher",
                    Type = "Circular"
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(dataMain);
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
                                //Response.Write(sResponseFromServer);
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
            return true;
        }
        public object SendAppDetails()
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = _user.SchoolId;
            var appdet = _Entities.tb_PushData.Where(x => x.SchoolId == schoolId).Select(x => x.PlayStore).FirstOrDefault();
            if (appdet != null)
            {
                var studentsList = _Entities.tb_Student.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (studentsList.Count > 0)
                {
                    status = true;
                    msg = "Successful";
                    Thread Thread = new Thread(() => SendAppDetailsSMS(studentsList, appdet));
                    Thread.Start();
                }
            }
            else
            {
                status = false;
                msg = "Sorry , you don't have an app !";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        private void SendAppDetailsSMS(List<tb_Student> studentsList, string appdet)
        {
            try
            {
                var smsHead = new tb_SmsHead();
                smsHead.Head = "From School APP Details";
                smsHead.SchoolId = _user.SchoolId;
                smsHead.TimeStamp = CurrentTime;
                smsHead.IsActive = true;
                smsHead.SenderType = 0;
                _Entities.tb_SmsHead.Add(smsHead);
                _Entities.SaveChanges();
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
                foreach (var item in studentsList)
                {
                    string message = "Dear  parent of " + item.StundentName + " ( Admission No : " + item.StudentSpecialId + " ), Kindly follow the below link to install the school APP,  to get in touch with your child’s school and to know about the attendance and marks. " + appdet;
                    var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + item.ContactNumber + "&route=2&message=" + message + "&sender=" + senderName;
                    ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                    HttpWebRequest request = this.GetRequest(url);
                    WebResponse webResponse = request.GetResponse();
                    var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                    var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                    alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);

                    tb_SmsHistory sms = new tb_SmsHistory();
                    sms.IsActive = true;
                    sms.MessageContent = message;
                    sms.MessageDate = CurrentTime;
                    sms.ScholId = item.SchoolId;
                    sms.StuentId = item.StudentId;
                    sms.MobileNumber = item.ContactNumber;
                    sms.HeadId = smsHead.HeadId;
                    sms.SendStatus = Convert.ToString(respList.success);
                    sms.SmsSentPerStudent = 1;
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
        }
        public PartialViewResult UpcomingEventView(string id)
        {
            long eventId = Convert.ToInt64(id);
            var data = _Entities.tb_CalenderEvent.Where(x => x.EventId == eventId).FirstOrDefault();
            CalendarEventModels model = new CalendarEventModels();
            model.eventId = data.EventId;
            model.schoolId = _user.SchoolId;
            model.eventHead = data.EventHead;
            model.eventDetails = data.EventDetails;
            model.eventDate = data.EventDate.ToShortDateString();
            return PartialView("~/Views/School/_pv_UpcomingEventView.cshtml", model);
        }
        public object CheckAdmissionNoEdit(string text)
        {
            bool Status = false;
            string Message = "Failed";
            string[] splitData = text.Split('~');
            try
            {
                long studentId = Convert.ToInt64(splitData[0]);
                string specialId = splitData[1];
                if (_Entities.tb_Student.Where(x => x.StudentId != studentId).Any(x => x.StudentSpecialId.ToLower() == specialId.ToLower() && x.IsActive))
                {
                    Status = true;
                    Message = "Admission Number already in use";
                }
            }
            catch
            {

            }
            return Json(new { Status = Status, Message = Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public object AddSMSSenderIdData(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var date = _Entities.tb_SchoolSenderId.Create();
                date.SchoolId = _user.SchoolId;
                date.SenderId = model.SenderDetails.SenderData;
                date.IsActive = true;
                _Entities.tb_SchoolSenderId.Add(date);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public object EditSMSSenderIdData(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var date = _Entities.tb_SchoolSenderId.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
                date.SenderId = model.SenderDetails.SenderData;
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult LoadTableForBillingNew(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            string[] splitData = id.Split('~');
            model.SchoolModel.studentId = Convert.ToInt64(splitData[1]);
            model.BillNumber = Convert.ToInt64(splitData[0]);
            return PartialView("~/Views/School/_pv_CollectionDetaildView.cshtml", model);
        }
        public IActionResult AttendanceDivisionApp()
        {
            SchoolModel model = new SchoolModel();
            model.schoolId = _user.SchoolId;
            return View(model);
        }

        [HttpPost]
        public object AttendanceDivisionApp(SchoolModel model)
        {
            bool status = false;
            string message = "Failed";
            long schoolId = _user.SchoolId;
            List<string> DivisionStringId = model.DivisionStringId.Split(',').ToList();

            var biometricDivision = new tb_BiometricDivision();
            _Entities.Database.ExecuteSqlCommand("Delete from [dbo].[tb_BiometricDivision] Where SchoolId = " + schoolId);
            foreach (var divisionId in DivisionStringId)
            {
                long DivisionId = Convert.ToInt32(divisionId);
                biometricDivision.DivisionId = DivisionId;
                biometricDivision.SchoolId = schoolId;
                _Entities.tb_BiometricDivision.Add(biometricDivision);
                status = _Entities.SaveChanges() > 0;
                message = status ? " Updated" : "Failed to update";

            }
            return Json(new { status = status, msg = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object AddFeeAccountHeadData(FeeAlertDataModel model) //***
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var date = _Entities.tb_AccountHead.Create();
                date.SchoolId = _user.SchoolId;
                date.AccHeadName = model.FeeIncomeHead.AccountHead;
                date.IsActive = true;
                date.TimeStamp = CurrentTime;
                date.ForBill = true;
                _Entities.tb_AccountHead.Add(date);
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public object EditFeeAccountHeadData(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                var date = _Entities.tb_AccountHead.Where(x => x.SchoolId == _user.SchoolId && x.IsActive == true && x.ForBill == true).FirstOrDefault();
                date.AccHeadName = model.FeeIncomeHead.AccountHead;
                status = _Entities.SaveChanges() > 0;
                msg = "Success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult LoadReturnPaymentMode(string id)
        {
            FeeModel model = new FeeModel();
            model.SchoolModel = new SchoolModel();
            string[] splitData = id.Split('~');
            model.SchoolModel.studentId = Convert.ToInt64(splitData[1]);
            model.BillNumber = Convert.ToInt64(splitData[0]);
            model.SchoolId = _user.SchoolId;
            model.SchoolModel.curredntDateTime = CurrentTime;
            model.ChequeDate = CurrentTime.ToShortDateString();
            return PartialView("~/Views/School/_pv_ReturnPaymnetModeView.cshtml", model);
        }
        public PartialViewResult LoadBankEntrySearchView()
        {
            BankEntryModel model = new BankEntryModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/SchoolAccount/_pv_BankEntrySearch.cshtml", model);
        }

        //basheer on 25/01/2019
        public IActionResult ChangePassword()
        {
            PasswordChangeModel model = new PasswordChangeModel();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public object PasswordChange(PasswordChangeModel model)
        {
            string msg = "Failed";
            bool status = false;
            var checkpass = _Entities.tb_Login.Where(x => x.SchoolId == model.SchoolId && x.Password == model.OldPassword).FirstOrDefault();
            if (checkpass != null)
            {
                try
                {
                    checkpass.Password = model.ConfirmPassword.Trim();
                    _Entities.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                if (status == true)
                {
                    msg = "Password Changed Successfully !";
                    Thread Thread = new Thread(() => SendPasswordChangePush(model));
                    Thread.Start();
                }
                else
                    msg = "Failed to Change Password !";
            }
            else
            {
                msg = "Incorrect old password";
            }

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //Basheer on 29/01/2019 
        private void SendPasswordChangePush(PasswordChangeModel model)
        {
            try
            {
                string schoolName = "Message from " + _user.tb_School.SchoolName;
                var tokenData = _Entities.tb_DeviceToken.Where(x => x.UserId == model.SchoolId && x.IsActive == true && x.LoginStatus == 1).OrderBy(x => x.TokenId).FirstOrDefault();
                if (tokenData != null)
                {
                    var applicationID = "";
                    var senderId = "";
                    var pushData = _Entities.tb_PushData.Where(x => x.SchoolId == model.SchoolId).FirstOrDefault();
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
                            body = "Password is changed.Please Login again",
                            title = schoolName
                        },
                        priority = "high",
                        data = new
                        {
                            Role = "School",
                            Function = "PasswordChange"
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
        public PartialViewResult GetStudentBillDataWithAcademicYear(string id)
        {
            FeeModel model = new FeeModel();
            string[] split = id.Split('~');
            model.SchoolModel = new SchoolModel();
            model.SchoolModel.studentId = Convert.ToInt64(split[1]);
            model.AcademicYearId = Convert.ToInt64(split[0]);
            return PartialView("~/Views/School/_pv_Billing_PaymentDeteils_Grid.cshtml", model);

        }

        public IActionResult TeacherFileUploadHome(string id)
        {
            TeacherFileUploadModel model = new TeacherFileUploadModel();
            model.TeacherId = Convert.ToInt64(id);
            return View(model);
        }

        public object AddTeacherFiles(TeacherFileUploadModel model)
        {
            bool status = false;
            string msg = "Failed";
            string TeacherId = model.TeacherId.ToString();
            if (model.FilePath == null)
            {
                msg = "Please select any file !";
            }
            else
            {
                try
                {
                    var data = _Entities.tb_TeacherFiles.Create();
                    data.TeacherId = model.TeacherId;
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
                    _Entities.tb_TeacherFiles.Add(data);
                    status = _Entities.SaveChanges() > 0;
                    if (status)
                        msg = "Successful";
                }
                catch
                {
                    msg = "Failed";
                }
            }
            return Json(new { status = status, msg = msg, TeacherId = TeacherId }, JsonRequestBehavior.AllowGet);
        }


        public IActionResult StaffDetails(string id)
        {
            var model = new SingleStaffDetails();
            model.StaffId = Convert.ToInt64(id);
            var staffData = _Entities.tb_Staff.Where(x => x.StaffId == model.StaffId && x.IsActive).FirstOrDefault();
            if (staffData != null)
            {
                model.StaffName = staffData.StaffName;
                model.EmailId = staffData.tb_Login.Username;
                model.ContactNo = staffData.Contact;
                model.SpecialId = staffData.tb_Login.Password;
            }
            return View(model);
        }

        public IActionResult StaffFileUploadHome(string id)
        {
            StaffFileUploadModel model = new StaffFileUploadModel();
            model.StaffId = Convert.ToInt64(id);
            return View(model);
        }


        public object AddStaffFiles(StaffFileUploadModel model)
        {
            bool status = false;
            string msg = "Failed";
            string StaffId = model.StaffId.ToString();
            if (model.FilePath == null)
            {
                msg = "Please select any file !";
            }
            else
            {
                try
                {
                    var data = _Entities.tb_StaffFileCollection.Create();
                    data.StaffId = model.StaffId;
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
                    _Entities.tb_StaffFileCollection.Add(data);
                    status = _Entities.SaveChanges() > 0;
                    if (status)
                        msg = "Successful";
                }
                catch
                {
                    msg = "Failed";
                }
            }
            return Json(new { status = status, msg = msg, StaffId = StaffId }, JsonRequestBehavior.AllowGet);
        }
        public object SaveSalaryType(FeeAlertDataModel model)
        {
            bool status = false;
            string msg = "Failed";
            var data = _Entities.tb_SalaryType.Where(x => x.SchoolId == _user.SchoolId && x.IsActive).FirstOrDefault();
            if (data != null)
            {
                if (data.TypeId == Convert.ToInt32(model.SalaryType))
                {
                    msg = "No Changes ";
                    status = true;
                }
                else
                {
                    data.TypeId = Convert.ToInt32(model.SalaryType);
                    status = _Entities.SaveChanges() > 0;
                    if (status)
                        msg = "Successfuly Edited!";
                }
            }
            else
            {
                var sal = _Entities.tb_SalaryType.Create();
                sal.SchoolId = _user.SchoolId;
                sal.TypeId = Convert.ToInt32(model.SalaryType);
                sal.IsActive = true;
                sal.TimeStamp = CurrentTime;
                _Entities.tb_SalaryType.Add(sal);
                status = _Entities.SaveChanges() > 0;
                if (status)
                    msg = "Successful";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //added by sibi..
        /// <summary>
        /// <create> SIBI       
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>


        public IActionResult ChangeWageEmployeeListingSettings(TrackTap.Models.FeeAlertDataModel model)
        {

            var data = _Entities.tb_WagesShowsSettings.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (data == null)
            {

                var results = _Entities.tb_WagesShowsSettings.Create();
                results.SchoolId = _user.SchoolId;
                results.IsActive = true;
                results.IsWagesShows = model.IsShowWagesEmployees;
                results.TimeStap = DateTime.Now;
                _Entities.tb_WagesShowsSettings.Add(results);
                _Entities.SaveChanges();
            }
            else
            {
                var results = _Entities.tb_WagesShowsSettings.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                results.IsWagesShows = model.IsShowWagesEmployees;
                _Entities.SaveChanges();
            }

            return null;
        }

        public IActionResult Exampublished()
        {
            try
            {
                StudentMarksEntry model = new StudentMarksEntry();
                model.SchoolId = _user.SchoolId;
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpPost]
        public object Exampublished_Save(int number)
        {
            string msg = "Failed";
            bool status = false;
            TeacherAttendancePostModel.TeacherAttendanceDataListPostModel model = new TeacherAttendancePostModel.TeacherAttendanceDataListPostModel();
            try
            {

                var var_check = _Entities.tb_ExamPublish.Where(x => x.ExamId == number && x.IsActive == true).FirstOrDefault();
                if (var_check == null)
                {
                    var var_getItems = _Entities.tb_Exams.Where(x => x.ExamId == number && x.IsActive == true).FirstOrDefault();
                    if (var_getItems != null)
                    {
                        var var_save = _Entities.tb_ExamPublish.Create();
                        var_save.SchoolId = var_getItems.SchoolId;
                        var_save.ClassId = var_getItems.ClassId;
                        var_save.DivisionId = var_getItems.DivisionId;
                        var_save.ExamId = var_getItems.ExamId;
                        var_save.ExamName = var_getItems.ExamName;
                        var_save.ExamDate = var_getItems.ExamDate;
                        var_save.IsActive = true;
                        var_save.TimeStamp = DateTime.Now;

                        _Entities.tb_ExamPublish.Add(var_save);
                        _Entities.SaveChanges();

                        model.classId = var_getItems.ClassId.ToString();
                        model.divisionId = var_getItems.DivisionId.ToString();


                        status = true;
                        msg = "Sent successfully";


                        long classId = Convert.ToInt64(model.classId);
                        long divisionId = Convert.ToInt64(model.divisionId);

                        var var_ExamId = _Entities.tb_Exams.Where(x => x.SchoolId == _user.SchoolId && x.ClassId == classId && x.DivisionId == divisionId && x.ExamId == number && x.IsActive == true).FirstOrDefault();

                        var fullStudentList = _Entities.tb_Student.Where(x => x.ClassId == classId && x.DivisionId == divisionId && x.IsActive).ToList();

                        ProgressCardReportModel PCModel = new ProgressCardReportModel();
                        ArrayList arList = new ArrayList();
                        List<string> string_li = new List<string>();

                        string path = Server.MapPath("~/Content/StudentsProgressReport/" + var_ExamId.ExamId);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        foreach (var item in fullStudentList)
                        {
                            string id = item.StudentId + "~" + var_ExamId.ExamId;
                            PCModel.SchoolName = id;

                            /////////////////////////////////////////////////////////////////////////////

                            GenerateProgressCard(PCModel);

                            ///////////////////////////////////////////////////////////////////////////////

                            string asd = RenderViewToString(this.ControllerContext, "_pv_ProgressCardGeneration", PCModel); //~/Views/School/_pv_ProgressCardGeneration.cshtml
                            System.IO.File.WriteAllText(path + "/" + item.StudentId + ".txt", asd);




                            string_li.Add(asd);
                        }

                        pushandroid(msg, status, string_li);
                        return true;

                    }
                    else
                    {
                        msg = "Exam data is not found";
                    }

                }

                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                //return Request.CreateResponse(HttpStatusCode.OK, new { status = status, msg = ex.Message, });
            }
            return null;
        }

        [HttpPost]
        public object Exampublished_Resent(int number)
        {
            string msg = "Failed";
            bool status = false;
            TeacherAttendancePostModel.TeacherAttendanceDataListPostModel model = new TeacherAttendancePostModel.TeacherAttendanceDataListPostModel();
            try
            {

                //var var_check = _Entities.tb_ExamPublish.Where(x => x.ExamId == number && x.IsActive == true).FirstOrDefault();
                var var_getItems = _Entities.tb_Exams.Where(x => x.ExamId == number && x.IsActive == true).FirstOrDefault();
                if (var_getItems != null)
                {
                    //var var_save = _Entities.tb_ExamPublish.Create();
                    //var_save.SchoolId = var_getItems.SchoolId;
                    //var_save.ClassId = var_getItems.ClassId;
                    //var_save.DivisionId = var_getItems.DivisionId;
                    //var_save.ExamId = var_getItems.ExamId;
                    //var_save.ExamName = var_getItems.ExamName;
                    //var_save.ExamDate = var_getItems.ExamDate;
                    //var_save.IsActive = true;
                    //var_save.TimeStamp = DateTime.Now;

                    //_Entities.tb_ExamPublish.Add(var_save);
                    //_Entities.SaveChanges();

                    model.classId = var_getItems.ClassId.ToString();
                    model.divisionId = var_getItems.DivisionId.ToString();


                    status = true;
                    msg = "Sent successfully";


                    long classId = Convert.ToInt64(model.classId);
                    long divisionId = Convert.ToInt64(model.divisionId);

                    var var_ExamId = _Entities.tb_Exams.Where(x => x.SchoolId == _user.SchoolId && x.ClassId == classId && x.DivisionId == divisionId && x.ExamId == number && x.IsActive == true).FirstOrDefault();

                    var fullStudentList = _Entities.tb_Student.Where(x => x.ClassId == classId && x.DivisionId == divisionId && x.IsActive).ToList();

                    ProgressCardReportModel PCModel = new ProgressCardReportModel();
                    ArrayList arList = new ArrayList();
                    List<string> string_li = new List<string>();

                    string path = Server.MapPath("~/Content/StudentsProgressReport/" + var_ExamId.ExamId);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    foreach (var item in fullStudentList)
                    {
                        string id = item.StudentId + "~" + var_ExamId.ExamId;
                        PCModel.SchoolName = id;

                        /////////////////////////////////////////////////////////////////////////////

                        GenerateProgressCard(PCModel);

                        ///////////////////////////////////////////////////////////////////////////////

                        string asd = RenderViewToString(this.ControllerContext, "_pv_ProgressCardGeneration", PCModel); //~/Views/School/_pv_ProgressCardGeneration.cshtml

                        /////////////////////////////////////////

                        System.IO.File.WriteAllText(path + "/" + item.StudentId + ".txt", asd);

                        ////////////////////////////////////////





                        string_li.Add(asd);
                    }

                    pushandroid(msg, status, string_li);
                    return true;

                }
                else
                {
                    msg = "Exam data is not found";
                }


            }
            catch (Exception ex)
            {
                msg = ex.Message;
                //return Request.CreateResponse(HttpStatusCode.OK, new { status = status, msg = ex.Message, });
            }
            return null;
        }



        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = context.RouteData.GetRequiredString("action");

                var viewData = new ViewDataDictionary(model);
                var sw = new StringWriter();

                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();


            }
            catch (Exception ex)
            {

            }
            return null;

        }


        public bool pushandroid(string message, bool status, List<string> HtmlView)
        {
            try
            {

                var applicationID = "";
                var senderId = "";

                applicationID = "AIzaSyAGcW_XdoA-bwVtUQ4IcnncTM2Toso3sv4";
                senderId = "47900857750";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var dataMain = new
                {

                    notification = new
                    {
                        message = message,
                        status = status
                    },
                    priority = "high",
                    data = new
                    {
                        Role = "Parent",
                        HtmlView = HtmlView,
                        Function = "ProgressCard"
                    }


                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(dataMain);
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
                                //Response.Write(sResponseFromServer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
            return true;
        }

        public ProgressCardReportModel GenerateProgressCard(ProgressCardReportModel model)
        {
            string[] splitdata = model.SchoolName.Split('~');
            long studnetId = Convert.ToInt64(splitdata[0]);
            string[] exams = splitdata[1].Split(',');
            int examcount = exams.Length; //basheer on 28/01/2019 to get the count of exams
            var student = _Entities.tb_Student.Where(x => x.StudentId == studnetId && x.IsActive).FirstOrDefault();
            model.IsFromApp = true;
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
                var one = _Entities.tb_Exams.Where(x => x.ExamId == examOne && x.IsActive).ToList().Select(x => new DataLibrary.Data.Exams(x)).FirstOrDefault();
                var two = _Entities.tb_Exams.Where(x => x.ExamId == examTwo && x.IsActive).ToList().Select(x => new DataLibrary.Data.Exams(x)).FirstOrDefault();
                model.ExamOne = one.ExamName;
                model.ExamTwo = two.ExamName;
                List<ExamSubjects> oneSub = one.ExamSubjectsList.Union(two.ExamSubjectsList).ToList();
                var subjects = oneSub.Select(x => x.SubjectId).Distinct().ToList().Select(x => new Subjects(x)).ToList();
                List<DataLibrary.Data.StudentMarks> markOne = student.tb_StudentMarks.Where(x => x.ExamId == one.ExamId && x.IsActive).ToList().Select(x => new DataLibrary.Data.StudentMarks(x)).ToList();
                List<DataLibrary.Data.StudentMarks> markTwo = student.tb_StudentMarks.Where(x => x.ExamId == two.ExamId && x.IsActive).ToList().Select(x => new DataLibrary.Data.StudentMarks(x)).ToList();

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
                var one = _Entities.tb_Exams.Where(x => x.ExamId == examOne && x.IsActive).ToList().Select(x => new DataLibrary.Data.Exams(x)).FirstOrDefault();
                //var two = _Entities.tb_Exams.Where(x => x.ExamId == examTwo && x.IsActive).ToList().Select(x => new Exams(x)).FirstOrDefault();
                model.ExamOne = one.ExamName;
                //model.ExamTwo = two.ExamName;
                List<ExamSubjects> oneSub = one.ExamSubjectsList.ToList();
                var subjects = oneSub.Select(x => x.SubjectId).Distinct().ToList().Select(x => new Subjects(x)).ToList();
                List<DataLibrary.Data.StudentMarks> markOne = student.tb_StudentMarks.Where(x => x.ExamId == one.ExamId && x.IsActive).ToList().Select(x => new DataLibrary.Data.StudentMarks(x)).ToList();
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
            return model;
        }

        public PartialViewResult _pv_ProgressCardGeneration(ProgressCardReportModel model)
        {


            return PartialView("~/Views/School/_pv_ProgressCardGeneration.cshtml", model);

        }

        public long RandomNumber()
        {
            Random random = new Random();
            long randomNum = 0;
            bool loopstart = false;
            while (loopstart == false)
            {
                randomNum = random.Next(1000, 99999999);
                var a1 = _Entities.tb_LibraryBook.Where(x => x.RandomNumber == randomNum && x.IsActive == true).ToList();
                if (a1.Count == 0 || a1 == null)
                {
                    loopstart = true;
                }
            }
            return randomNum;
        }
        public IActionResult AssignDiscountFromBilling(string id)
        {
            string[] splitdata = id.Split('~');
            long studnetId = Convert.ToInt64(splitdata[1]);
            long divisionId = Convert.ToInt32(splitdata[0]);
            var model = new FeeModel();
            model.DivisionId = divisionId;
            model.SchoolId = _user.SchoolId;
            model.StudentId = studnetId;
            return View(model);
        }
        public object GeneratePrepaidInvoice(FeeModel model) // Archana for generate pre-paid invoice for Jawahar School
        {
            List<string> feeDetails = model.FeeDetails.Split(',').ToList();
            PrepaidInvoiceDataList returnData = new PrepaidInvoiceDataList();
            returnData.CurrentDatetime = CurrentTime;
            returnData.StudentId = model.StudentId;
            List<PrepaidInvoiceData> list = new List<PrepaidInvoiceData>();
            int count = 0;
            foreach (var item in feeDetails)
            {
                count = count + 1;
                var one = item.Split('^').ToList();
                PrepaidInvoiceData data = new PrepaidInvoiceData();
                data.ActualAmt = Convert.ToDecimal(one[3]);
                data.DiscountAmt = Convert.ToDecimal(one[4]);
                data.AfterDiscountAmt = Convert.ToDecimal(one[4]);
                if (data.AfterDiscountAmt == 0)
                    data.AfterDiscountAmt = data.ActualAmt;
                var x = new TrackTap.DataLibrary.Data.Fee(Convert.ToInt64(one[1]));
                data.FeeName = x.FeeName;
                data.DueDate = x.DueDate ?? CurrentTime;
                data.SlNo = count;
                list.Add(data);
            }
            returnData.data = list;
            //return PartialView("~/Views/School/_pv_PrepaidBillInvoice.cshtml", returnData);
            return Json(new { status = true, msg = "Success", data = returnData }, JsonRequestBehavior.AllowGet);
        }
        #region Email
        //Basheer for Email
        public IActionResult Email()
        {
            var model = new SchoolModelForEmail();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        [HttpPost]
        public object EmailStudent(SchoolModelForEmail model)
        {
            string msg = "Failed";
            var status = false;

            var usercredentials = _Entities.tb_SMTPDetail.Where(x => x.SchoolId == _user.SchoolId).FirstOrDefault();
            if (usercredentials != null)
            {
                try
                {


                    //.Replace("{{email}}", model.Email)
                    //.Replace("{{contactNo}}", model.contactNo)
                    //.Replace("{{schoolName}}", model.schoolName);


                    //Mailing section


                    List<SendEmail> Userdata = JsonConvert.DeserializeObject<List<SendEmail>>(model.Data).ToList();

                    var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/email/EmailStudent.html");
                    var emailTemplate = System.IO.File.ReadAllText(filePath);
                    var mailBody = emailTemplate.Replace("{{messgae}}", Userdata[0].Description);

                    if (Userdata[0].list.Count > 0 && Userdata[0].list != null)
                    {
                        foreach (var ms in Userdata[0].list)
                        {

                            SmtpClient client = new SmtpClient();
                            string userName = usercredentials.EmailId;
                            string password = usercredentials.Password;
                            string fromName = "SchoolMan";
                            MailAddress address = new MailAddress(userName, fromName);
                            MailMessage message = new MailMessage();
                            message.To.Add(new MailAddress(ms.Email, "Receiver"));
                            message.From = address;
                            message.Subject = "Schoolman - Student Mail";
                            message.IsBodyHtml = true;
                            message.Body = mailBody;
                            client.Host = "smtp.gmail.com";//ConfigurationManager.AppSettings["smptpserver"];
                            client.Port = 587;//Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                                              //client.Port = 465; 587
                            client.EnableSsl = true;
                            client.UseDefaultCredentials = true;
                            //client.UseDefaultCredentials = true;
                            client.Credentials = new NetworkCredential(userName, password);
                            try
                            {
                                client.Send(message);

                            }
                            catch (Exception e)
                            {
                                status = false;
                            }
                        }
                    }


                    //Mailing section enf

                    status = true;
                    msg = "success";

                }
                catch (Exception exx)
                {
                    status = false;
                    msg = "Something went wrong";
                }
            }
            else
            {
                status = false;
                msg = "Please enter Credentials in settings";
            }

            //  msg = status ? "Success!" : "Failed to add user!";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        [HttpPost]
        public object CommonFeeDiscountStudentFee(FeeModel model) //Archana 11/10/2019 Give discount for Common fee too.
        {
            bool status = true;
            string message = "Failed";
            long StudentId = model.StudentId;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            try
            {
                List<DatalistCommon> routes_list = new JavaScriptSerializer().Deserialize<List<DatalistCommon>>(model.FeeDetails);

                foreach (var value in routes_list)
                {
                    long feeId = Convert.ToInt32(value.feeStudentId);
                    var data = _Entities.tb_CommonFeeStudentDiscount.Where(x => x.StudentId == StudentId && x.SchoolId == _user.SchoolId && x.FeeId == feeId && x.IsActive && x.DiscountAllowFeeDate.Month == value.dueDate.Month && x.DiscountAllowFeeDate.Year == value.dueDate.Year).FirstOrDefault();
                    if (data != null)
                    {
                        decimal discount = data.OriginalAmount - Convert.ToDecimal(value.amount) ?? 0;
                        if (data.DiscountAmount != discount)
                        {
                            data.DiscountAmount = discount;
                            status = _Entities.SaveChanges() > 0;
                        }
                    }
                    else
                    {
                        if (Convert.ToDecimal(value.oldAmount) != Convert.ToDecimal(value.amount))
                        {
                            decimal discount = Convert.ToDecimal(value.oldAmount) - Convert.ToDecimal(value.amount);
                            var newData = _Entities.tb_CommonFeeStudentDiscount.Create();
                            newData.DiscountAllowFeeDate = value.dueDate;
                            newData.DiscountAmount = discount;
                            newData.FeeId = feeId;
                            newData.IsActive = true;
                            newData.OriginalAmount = Convert.ToDecimal(value.oldAmount);
                            newData.SchoolId = _user.SchoolId;
                            newData.StudentId = StudentId;
                            newData.TimeStamp = CurrentTime;
                            newData.UserId = _user.UserId;
                            _Entities.tb_CommonFeeStudentDiscount.Add(newData);
                            status = _Entities.SaveChanges() > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var xx = ex.Message;
            }
            message = status ? " Fee Added" : "Fee Added";
            return Json(new { status = true, msg = message }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PrepaidPrintAccountBillData(string id)
        {
            //string[] splitData = id.Split('~');
            //var model = new PrintBill();
            //model.studentId = Convert.ToInt64(splitData[0]);
            //model.billNumber = Convert.ToInt64(splitData[1]);
            PrepaidInvoiceDataList model = new PrepaidInvoiceDataList();
            model.data = new List<PrepaidInvoiceData>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            model = js.Deserialize<PrepaidInvoiceDataList>(id);
            return PartialView("~/Views/School/_pv_PrepaidBillInvoice.cshtml", model);
        }
        public IActionResult ChangeAccountHide(FeeAlertDataModel model)
        {

            var data = _Entities.tb_AccountsHide.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
            if (data == null)
            {

                var results = _Entities.tb_AccountsHide.Create();
                results.SchoolId = _user.SchoolId;
                results.IsActive = true;
                results.IsAccountHide = model.IsHideAccounts;
                results.TimeStap = DateTime.Now;
                _Entities.tb_AccountsHide.Add(results);
                _Entities.SaveChanges();
            }
            else
            {
                var results = _Entities.tb_AccountsHide.Where(z => z.SchoolId == _user.SchoolId).FirstOrDefault();
                results.IsAccountHide = model.IsHideAccounts;
                _Entities.SaveChanges();
            }

            return null;
        }


        //Region start for account hiding by basheer on 22/11/2019

        public IActionResult AccoundHandling()
        {
            var model = new AccountHide();
            model.SchoolId = _user.SchoolId;
            model.FromDate = CurrentTime.Date;
            model.EndDate = CurrentTime.Date;
            return View(model);
        }
        public PartialViewResult DayBookReportHide(string id)
        {
            string[] splitDate = id.Split('~');
            DateTime start = Convert.ToDateTime(splitDate[0]);
            DateTime end = Convert.ToDateTime(splitDate[1]);
            int vouchertype = Convert.ToInt32(splitDate[2]);
            Models.AccountHide model = new Models.AccountHide();
            model.SchoolName = _user.tb_School.SchoolName;
            model.Heading = start + " to " + end;
            model.SchoolId = _user.SchoolId;
            model.FromDate = start;
            model.EndDate = end;
            if (vouchertype == 0)
            {
                return PartialView("~/Views/School/_pv_Receipt_AccountHandling.cshtml", model);
            }
            else if (vouchertype == 1)
            {
                return PartialView("~/Views/School/_pv_Payment_AccountHandling.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/School/_pv_Contra_AccountHandling.cshtml", model);
            }

        }

        public object SubmitAccountHidedetails(AccountHide model)
        {
            bool status = false;
            string msg = string.Empty;
            string vouchertype = "";
            DateTime stdate;
            DateTime enddate;

            try
            {
                stdate = Convert.ToDateTime(model.startdate);
                enddate = Convert.ToDateTime(model.endingdate);
            }
            catch
            {

                string[] splitData = model.startdate.Split('-');
                var dd = splitData[0];
                var mm = splitData[1];
                var yyyy = splitData[2];
                var date = mm + '-' + dd + '-' + yyyy;
                stdate = Convert.ToDateTime(date);


                string[] splitData1 = model.endingdate.Split('-');
                var dd1 = splitData1[0];
                var mm1 = splitData1[1];
                var yyyy1 = splitData1[2];
                var date1 = mm1 + '-' + dd1 + '-' + yyyy1;
                enddate = Convert.ToDateTime(date1);
            }
            if (model.VoucherType == 0)
            {
                vouchertype = "R";
            }
            else if (model.VoucherType == 1)
            {
                vouchertype = "P";
            }
            else
            {
                vouchertype = "C";
            }

            var accountdetails = JsonConvert.DeserializeObject<List<AccountHideDetails>>(model.accounthidedetails);

            var removeList = _Entities.tb_AccountsHideDetails.Where(z => z.Schoolid == model.SchoolId && z.VoucherType == vouchertype && (z.EnterDate >= stdate && z.EnterDate <= enddate)).ToList();
            foreach (var item in removeList)
            {
                _Entities.tb_AccountsHideDetails.Remove(item);
            }
            _Entities.SaveChanges();

            foreach (var item in accountdetails)
            {
                var accounthidedetails = _Entities.tb_AccountsHideDetails.Create();
                accounthidedetails.EnterDate = item.EnterDate;
                accounthidedetails.VoucherNo = item.VoucherNo;
                accounthidedetails.AccountHeadName = item.AccountHeadName;
                accounthidedetails.SubLedger = item.SubLedger;
                accounthidedetails.IncomeAmount = item.IncomeAmount;
                accounthidedetails.ExpenseAmount = item.ExpenseAmount;
                accounthidedetails.Naration = item.Narration;
                accounthidedetails.TransactionType = item.TransactionType;
                accounthidedetails.FromStatus = item.FromStatus;
                accounthidedetails.Schoolid = model.SchoolId;
                accounthidedetails.SchoolName = model.SchoolName;
                accounthidedetails.TimeStamp = CurrentTime;
                accounthidedetails.VoucherType = vouchertype;

                _Entities.tb_AccountsHideDetails.Add(accounthidedetails);
            }

            status = _Entities.SaveChanges() > 0;
            msg = status ? "Success" : "Failed";

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }


    }
}



