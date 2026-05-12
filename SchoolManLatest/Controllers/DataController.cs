using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace TrackTap.Controllers
{
    public class DataController : Controller
    {
        public object LoadDivision(long id)
        {
            var result = TrackTap.Data.DropdownData.GetDivision(id);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }


        public object LoadFreeDivision(long id)
        {
            var result = TrackTap.Data.DropdownData.GetDivision(id);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadFreeSchoolDivision(long id, long schoolid) 
        {
            var result = TrackTap.Data.DropdownData.GetFreeSchoolDivision(id, schoolid);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadTeacherDivision(long id, long userid)
        {
            long classId = id;
            long teacherId = userid;
            var result = TrackTap.Data.DropdownData.GetTeacherDivision(id, userid);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadExams(long classId, long divisionid, long schoolId)
        {
            var result = TrackTap.Data.DropdownData.GetAllExams(classId, divisionid, schoolId);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadSubjects(long examId)
        {
            var result = TrackTap.Data.DropdownData.GetAllSubjects(examId);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadAllSchoolDivision(long id, long schoolid)
        {
            var result = TrackTap.Data.DropdownData.GetAllSchoolDivision(id, schoolid);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        //---------------- For get subedger corresponding to the account head 
        public object LoadSubLedgerList(long id)
        {
            var result = TrackTap.Data.DropdownData.GetSubLedgerList(id);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadClassesByAcademicYear(string id)
        {
            string[] splitData = id.Split('~');
            long schoolId = Convert.ToInt64(splitData[0]);
            long acYearId = Convert.ToInt64(splitData[1]);

            var result = TrackTap.Data.DropdownData.GetAllClasses(schoolId, acYearId);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object LoadAllSchoolClassWithAcademicYear(long academicYear, long schoolid)
        {
            var result = TrackTap.Data.DropdownData.GetAllSchoolClassesWithAcademicYear(academicYear, schoolid);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }


        //jibin 9/25/2020
        public object GetItemCategories(long id)
        {
            var result = TrackTap.Data.DropdownData.GetItemCategories(id);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }


        public object GetUnitCategories(string id)
        {
            var result = TrackTap.Data.DropdownData.GetUnitCategories(id);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }


        public object GetPriceCategories(string id)
        {
            var result = TrackTap.Data.DropdownData.GetPriceCategories(id);
            return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
        }
        //jibin 9/25/2020

    }
}
