using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class PromotionController : BaseController
    {
        public IActionResult PromoteClassHome()
        {
            SchoolValue model = new SchoolValue();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult UnpublishedClasses(string id)
        {
            long Classid = Convert.ToInt64(id);
            int slNo = 0;
            UpPublishedClassList model = new UpPublishedClassList();
            model.list = new List<UnPublishedClass>();
            if (Classid == 0)
            {
                var classList = _Entities.tb_Division.Where(x => x.tb_Class.SchoolId == _user.SchoolId && x.IsActive && x.tb_Class.IsActive && x.tb_Class.PublishStatus == false).ToList();
                if (classList.Count > 0 && classList != null)
                {
                    foreach (var item in classList)
                    {
                        UnPublishedClass one = new UnPublishedClass();
                        one.ClassId = item.ClassId;
                        one.ClassName = item.tb_Class.Class;
                        one.DivisionId = item.DivisionId;
                        one.DivisionName = item.Division;
                        one.AccademicYear = item.tb_Class.tb_AcademicYear.AcademicYear;
                        one.CurrentYearStatus = item.tb_Class.tb_AcademicYear.CurrentYear ?? false;
                        one.SlNo = slNo + 1;
                        model.list.Add(one);
                        slNo = slNo + 1;
                    }
                }
            }
            else
            {
                var classList = _Entities.tb_Division.Where(x => x.tb_Class.SchoolId == _user.SchoolId && x.IsActive && x.tb_Class.IsActive && x.tb_Class.PublishStatus == false && x.tb_Class.ClassId == Classid).ToList();
                if (classList.Count > 0 && classList != null)
                {
                    foreach (var item in classList)
                    {
                        UnPublishedClass one = new UnPublishedClass();
                        one.ClassId = item.ClassId;
                        one.ClassName = item.tb_Class.Class;
                        one.DivisionId = item.DivisionId;
                        one.DivisionName = item.Division;
                        one.AccademicYear = item.tb_Class.tb_AcademicYear.AcademicYear;
                        one.CurrentYearStatus = item.tb_Class.tb_AcademicYear.CurrentYear ?? false;
                        one.SlNo = slNo + 1;
                        model.list.Add(one);
                        slNo = slNo + 1;
                    }
                }
            }

            return PartialView("~/Views/Promotion/_pv_UnPublishedClassList.cshtml", model);
        }
        public PartialViewResult AddUnPublishedClassParialView()
        {
            AddClassModel model = new AddClassModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Promotion/_pv_AddClassUnPublished.cshtml", model);
        }
        public object AddClassUnPublished(AddClassModel model)
        {
            bool status = false;
            string msg = "Failed";
            long ClassId = 0;
            var div = _Entities.tb_Division.Where(x => x.tb_Class.Class == model.ClassName && x.Division == model.Division && x.tb_Class.AcademicYearId == model.AcademicYearId && x.IsActive && x.tb_Class.IsActive && x.tb_Class.SchoolId == _user.SchoolId).FirstOrDefault();
            if (div != null)
            {
                msg = "Division and Class is exists in this Academic Year!";
            }
            else
            {
                var classOld = _Entities.tb_Class.Where(x => x.Class == model.ClassName && x.SchoolId == _user.SchoolId && x.IsActive && x.AcademicYearId == model.AcademicYearId).FirstOrDefault();
                if (classOld != null)
                {
                    var newDiv = _Entities.tb_Division.Create();
                    newDiv.ClassId = classOld.ClassId;
                    newDiv.Division = model.Division.ToUpper();
                    newDiv.DivisionGuid = Guid.NewGuid();
                    newDiv.IsActive = true;
                    newDiv.TimeStamp = CurrentTime;
                    _Entities.tb_Division.Add(newDiv);
                    status = _Entities.SaveChanges() > 0;
                    msg = status ? "success" : "failed";
                    ClassId = classOld.ClassId;
                }
                else
                {
                    var newClass = _Entities.tb_Class.Create();
                    newClass.Class = model.ClassName;
                    newClass.ClassGuild = Guid.NewGuid();
                    newClass.Timestamp = CurrentTime;
                    newClass.SchoolId = model.SchoolId;
                    newClass.IsActive = true;
                    //newClass.ClassOrder = model.OrderValue;
                    newClass.ClassOrder = _Entities.tb_ClassList.Where(x => x.ClassName == model.ClassName).Select(x => x.OrderValue).FirstOrDefault();
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
                    ClassId = newClass.ClassId;
                }
            }
            return Json(new { status = status, msg = msg, classId = ClassId, list = new TrackTap.Data.DropdownData().RefreshClasses(model.SchoolId) }, JsonRequestBehavior.AllowGet);
        }
        public object PublishUnPublishedClass(string id)
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
                    var prevClass = _Entities.tb_Class.Where(z => z.Class == Class.Class && z.AcademicYearId != Class.AcademicYearId && z.SchoolId == _user.SchoolId && z.IsActive).ToList();
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
            return Json(new { status = status, msg = message, list = new TrackTap.Data.DropdownData().RefreshClassesUnPublished(_user.SchoolId) }, JsonRequestBehavior.AllowGet);
        }
        public IActionResult PromoteStudents()
        {
            SchoolValue model = new SchoolValue();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public IActionResult StudentPromotionHome()
        {
            PromoteStudents model = new PromoteStudents();
            model.SchoolId = _user.SchoolId;
            return View(model);
        }
        public PartialViewResult StudentListForPromotion(string id)
        {
            string[] splitData = id.Split('~');
            long AcademicYearId = Convert.ToInt64(splitData[0]);
            long ClassId = Convert.ToInt64(splitData[1]);
            long DivisionId = Convert.ToInt64(splitData[2]);
            int count = 0;
            var studentList = _Entities.tb_Student.Where(x => x.IsActive && x.SchoolId == _user.SchoolId && x.ClassId == ClassId && x.DivisionId == DivisionId).ToList();
            PromoteStudents model = new Models.PromoteStudents();
            model.OldAcademicyearId = AcademicYearId;
            model.OldClassId = ClassId;
            model.OldDivId = DivisionId;
            model.SchoolId = _user.SchoolId;
            model.StudentList = new List<StudentListForPromote>();
            foreach (var item in studentList)
            {
                StudentListForPromote one = new StudentListForPromote();
                one.StudentId = item.StudentId;
                one.StudentName = item.StundentName;
                one.SpecialId = item.StudentSpecialId;
                one.ContactNumber = item.ContactNumber;
                one.SlNo = count + 1;
                model.StudentList.Add(one);
                count = count + 1;
            }
            return PartialView("~/Views/Promotion/_pv_StudentListForPromotion.cshtml", model);
        }
        public PartialViewResult PromoteStudentsToAnotherParialView(PromoteStudents model)
        {
            model.SchoolId = _user.SchoolId;
            foreach (var item in model.StudentList)
            {
                if (model.StudentListString == null)
                    model.StudentListString = item.StudentId.ToString();
                else
                    model.StudentListString = model.StudentListString + ',' + item.StudentId.ToString();

            }
            return PartialView("~/Views/Promotion/_pv_PromoteStudentsToAnotherParialView.cshtml", model);
        }

        public object AddPromoteStudents(PromoteStudents model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                string[] listOfStudents = model.StudentListString.Split(',');
                foreach (var item in listOfStudents)
                {
                    var studentId = Convert.ToInt64(item);
                    var student = _Entities.tb_Student.Where(x => x.StudentId == studentId).FirstOrDefault();
                    var oldPromotion = _Entities.tb_StudentPremotion.Where(x => x.StudentId == studentId && x.IsActive == 1).ToList();
                    foreach (var item1 in oldPromotion)
                    {
                        item1.LastUpdate = false;
                        _Entities.SaveChanges();
                    }

                    var chekduplicatedata = _Entities.tb_StudentPremotion.Where(x => x.StudentId == studentId && x.OldClass == student.ClassId && x.IsActive == 1).FirstOrDefault();
                    if (chekduplicatedata != null)
                    {
                        chekduplicatedata.IsActive = 0;
                        _Entities.SaveChanges();
                    }

                    
                    var newPromotion = _Entities.tb_StudentPremotion.Create();
                    newPromotion.StudentId = student.StudentId;
                    newPromotion.FromDivision = student.DivisionId;
                    newPromotion.ToDivision = model.NewDivId;
                    newPromotion.TimeStamp = CurrentTime;
                    newPromotion.SchoolId = _user.SchoolId;
                    newPromotion.IsActive = 1;
                    newPromotion.LastUpdate = true;
                    newPromotion.OldClass = student.ClassId;
                    _Entities.tb_StudentPremotion.Add(newPromotion);
                    status = _Entities.SaveChanges() > 0;


                    student.ClassId = model.NewCLassId;
                    student.DivisionId = model.NewDivId;
                    _Entities.SaveChanges();


                    ///this to start...................
                    ///

                    List<PromotionStudentDummyDataModel> li_dummy = new List<PromotionStudentDummyDataModel>();

                    var chekduplicatedata_differentId = _Entities.tb_StudentPremotion.Where(x => x.StudentId == studentId && x.IsActive == 1).ToList().OrderByDescending(x => x.PremotionId);
                    foreach (var a1 in chekduplicatedata_differentId)
                    {
                        PromotionStudentDummyDataModel mo = new PromotionStudentDummyDataModel();
                                                
                        var temp1_class = _Entities.tb_Class.Where(x => x.ClassId == a1.OldClass).FirstOrDefault();

                        var temp2_chekdata = li_dummy.Where(x => x.AcademicYearId == temp1_class.AcademicYearId).FirstOrDefault();

                        if (temp2_chekdata == null)
                        {
                            mo.PremotionId = a1.PremotionId;
                            mo.StudentId = a1.StudentId;
                            mo.SchoolId = a1.SchoolId;
                            mo.IsActive = a1.IsActive;
                            mo.OldClass = a1.OldClass;
                            mo.AcademicYearId = temp1_class.AcademicYearId;
                            li_dummy.Add(mo);
                        }
                        else
                        {
                            a1.IsActive = 0;
                            _Entities.SaveChanges();

                        }


                        


                    }


                    //end........




                }
                if (status)
                    msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public object DemoteStudents(PromoteStudents model)
        {
            bool status = false;
            string msg = "Failed";
            int totalCount = 0;
            try
            {
                foreach (var item in model.StudentList)
                {
                    var oldPromotionDetails = _Entities.tb_StudentPremotion.Where(x => x.StudentId == item.StudentId && x.SchoolId == _user.SchoolId && x.LastUpdate == true && x.IsActive == 1).FirstOrDefault();
                    if (oldPromotionDetails != null)
                    {
                        oldPromotionDetails.LastUpdate = false;
                        oldPromotionDetails.TimeStamp = CurrentTime;
                        _Entities.SaveChanges();
                        var student = _Entities.tb_Student.Where(x => x.StudentId == item.StudentId && x.IsActive).FirstOrDefault();
                        student.ClassId = oldPromotionDetails.OldClass ?? 0;
                        student.DivisionId = oldPromotionDetails.FromDivision;
                        status = _Entities.SaveChanges() > 0;
                        if (status)
                            msg = "Successful";
                    }
                    else
                    {
                        var student = _Entities.tb_Student.Where(x => x.StudentId == item.StudentId && x.IsActive).FirstOrDefault();
                        totalCount = totalCount + 1;
                    }
                }
                if (totalCount!=0)
                {
                    status = false;
                    int success = model.StudentList.Count - totalCount;
                    msg = success + " Students has demoted and " + totalCount + " Students has not demoted. Since their previous informations not available .";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

    }
}
