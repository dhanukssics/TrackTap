using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackTap.ClassLibrary;

namespace TrackTap.Data
{
    public class DropdownData
    {
        protected static tb_tracktapEntities _Entities = new tb_tracktapEntities();
        protected tb_tracktapEntities Entities = new tb_tracktapEntities();

        public static List<SelectListItem> GetUnPublishedClasses(long schoolId)
        {
            var input = _Entities.tb_Class.Where(z => z.IsActive && z.PublishStatus == false && z.SchoolId == schoolId).OrderBy(z => z.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }

        public static List<SelectListItem> GetOtherAcademicYear()
        {
            var year = _Entities.tb_AcademicYear.ToList();
            //var notYearId = year.FirstOrDefault().YearId;
            var input = year.Where(z => z.IsActive).OrderByDescending(z => z.YearId).ToList();
            return input.Select(x => new SelectListItem { Text = x.AcademicYear, Value = x.YearId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetClasses(long schoolId)
        {
            var input = _Entities.tb_Class.Where(z => z.IsActive && z.PublishStatus && z.SchoolId == schoolId).OrderBy(z => z.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllSchool()
        {
            var input = _Entities.tb_School.Where(z => z.IsActive == true).OrderBy(z => z.SchoolName).ToList();
            return input.Select(x => new SelectListItem { Text = x.SchoolName, Value = x.SchoolId.ToString() }).ToList();
        }



        public static List<SelectListItem> GetClassList()
        {
            var input = _Entities.tb_ClassList.Where(z => z.IsActive).OrderBy(z => z.OrderValue).ToList();
            return input.Select(x => new SelectListItem { Text = x.ClassName, Value = x.OrderValue.ToString() }).ToList();
        }
        public static List<SelectListItem> GetDivision(long classId)
        {
            var input = _Entities.tb_Division.Where(z => z.IsActive && z.ClassId == classId).OrderBy(z => z.Division).ToList();
            return input.Select(x => new SelectListItem { Text = x.Division, Value = x.DivisionId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetBus(long schoolId)
        {
            var input = _Entities.tb_Bus.Where(z => z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.BusSpecialId).ToList();
            return input.Select(x => new SelectListItem { Text = x.BusSpecialId, Value = x.BusId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetBusName(long schoolId)
        {
            var input = _Entities.tb_Bus.Where(z => z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.BusSpecialId).ToList();
            return input.Select(x => new SelectListItem { Text = x.BusName, Value = x.BusId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetFreeSchoolDivision(long id, long schoolid)
        {
            var input = _Entities.SP_UnassignedDivisions(schoolid, id).ToList();
            return input.Select(x => new SelectListItem { Text = x.Division, Value = x.DivisionId.ToString() }).ToList();
        }

        public static List<SelectListItem> GetSchoolList()
        {
            var input = _Entities.tb_Login.Where(z => z.IsActive && z.RoleId == (int)UserRole.School).ToList();
            return input.Select(x => new SelectListItem { Text = x.tb_School.SchoolName, Value = x.SchoolId.ToString() }).ToList();
        }

        public static List<SelectListItem> GetSchoolPaymentGatwayList()
        {
            var input = _Entities.tb_Login.Where(z => z.IsActive && z.RoleId == (int)UserRole.School && z.tb_School.PaymentOption==true).ToList();
            return input.Select(x => new SelectListItem { Text = x.tb_School.SchoolName, Value = x.SchoolId.ToString() }).ToList();
        }
        //Non-Static

        public List<SelectListItem> GetFreeClasses(long schoolId)
        {
            //var teacherClass = _Entities.tb_TeacherClass.Where(z => z.tb_Class.SchoolId == schoolId).ToList().Select(z => z.DivisionId).ToList();
            //var input = _Entities.tb_Division.Where(z => z.IsActive && !teacherClass.Contains(z.DivisionId) && z.tb_Class.SchoolId == schoolId).ToList().Select(z => z.ClassId).Distinct().ToList();
            //return _Entities.tb_Class.Where(z => input.Contains(z.ClassId) && z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.ClassOrder).ToList().Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
            return _Entities.SP_UnassignedTeachers(schoolId).ToList().Select(x => new SelectListItem { Text = x.ClassName, Value = x.ClassId.ToString() }).ToList();//Archana 
        }
        public List<SelectListItem> GetFreeDivision(long classId)
        {
            var teacherClass = _Entities.tb_TeacherClass.Where(z => z.tb_Class.ClassId == classId).ToList().Select(z => z.DivisionId).ToList();
            var input = _Entities.tb_Division.Where(z => z.IsActive && !teacherClass.Contains(z.DivisionId) && z.tb_Class.ClassId == classId).OrderBy(z => z.tb_Class.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Division, Value = x.DivisionId.ToString() }).ToList();
        }
        public List<SelectListItem> RefreshClasses(long schoolId)
        {
            var input = _Entities.tb_Class.Where(z => z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }
        public List<SelectListItem> RefreshClassesUnPublished(long schoolId)
        {
            var year = _Entities.tb_AcademicYear.FirstOrDefault();
            var input = _Entities.tb_Class.Where(z => z.IsActive && z.SchoolId == schoolId && year.YearId != z.AcademicYearId).OrderBy(z => z.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }
        public List<SelectListItem> RefreshSchoolFees(long schoolId)
        {
            var input = Entities.tb_Fee.Where(z => z.IsActive && z.SchoolId == schoolId && z.FeeType == (int)FeeType.CommonFee).OrderBy(z => z.FeeId).ToList();
            return input.Select(x => new SelectListItem { Text = x.FeesName, Value = x.FeeId.ToString() }).ToList();
        }
        public List<SelectListItem> SchoolSpecialFeesList(long schoolId)
        {
            var input = Entities.tb_Fee.Where(z => z.IsActive && z.SchoolId == schoolId && z.FeeType == (int)FeeType.SpecialFee).OrderBy(z => z.FeeId).ToList();
            return input.Select(x => new SelectListItem { Text = x.FeesName, Value = x.FeeId.ToString() }).ToList();
        }
        public List<SelectListItem> GetFreeTeacherClasses(long schoolId, long teacherId)
        {
            var teacherClass = _Entities.tb_TeacherClass.Where(z => z.tb_Class.SchoolId == schoolId && z.TeacherId != teacherId).ToList().Select(z => z.DivisionId).ToList();
            var input = _Entities.tb_Division.Where(z => z.IsActive && !teacherClass.Contains(z.DivisionId) && z.tb_Class.SchoolId == schoolId && z.tb_Class.PublishStatus == true && z.tb_Class.IsActive == true).ToList().Select(z => z.ClassId).Distinct().ToList();
            return _Entities.tb_Class.Where(z => input.Contains(z.ClassId) && z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.ClassOrder).ToList().Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }
        public List<SelectListItem> GetFreeTeacherDivision(string ClassId, long teacherId)
        {
            long classId = Convert.ToInt64(ClassId);
            var teacherClass = _Entities.tb_TeacherClass.Where(z => z.tb_Class.ClassId == classId && z.TeacherId != teacherId).ToList().Select(z => z.DivisionId).ToList();
            var input = _Entities.tb_Division.Where(z => z.IsActive && !teacherClass.Contains(z.DivisionId) && z.tb_Class.ClassId == classId && z.tb_Class.PublishStatus==true && z.tb_Class.IsActive==true).OrderBy(z => z.tb_Class.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Division, Value = x.DivisionId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetTeacherDivision(long classId, long userId)
        {
            var teacher = _Entities.tb_Teacher.Where(x => x.UserId == userId).FirstOrDefault();

            var teacherData = _Entities.tb_TeacherClass.Where(x => x.TeacherId == teacher.TeacherId && x.ClassId == classId).ToList();
            return teacherData.Select(x => new SelectListItem { Text = x.tb_Division.Division, Value = x.DivisionId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetTeacherClass(long userId)
        {
            var teacher = _Entities.tb_Teacher.Where(x => x.UserId == userId).FirstOrDefault();

            var teacherData = _Entities.tb_TeacherClass.Where(x => x.TeacherId == teacher.TeacherId && x.tb_Class.PublishStatus == true).ToList();
            return teacherData.Select(x => new SelectListItem { Text = x.tb_Class.Class, Value = x.ClassId.ToString() }).ToList();
        }
        public List<SelectListItem> GetBookCategory(long schoolId)
        {
            var input = Entities.tb_BookCategory.Where(z => z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.Category).ToList();
            return input.Select(x => new SelectListItem { Text = x.Category, Value = x.CategoryId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllExams(long classId, long divisionid, long schoolId)//Archana
        {
            var input = _Entities.tb_Exams.Where(x => x.ClassId == classId && x.DivisionId == divisionid && x.SchoolId == schoolId && x.IsActive).ToList();
            return input.Select(x => new SelectListItem { Text = x.ExamName, Value = x.ExamId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllSubjects(long examId)//Archana
        {
            var input = _Entities.tb_ExamSubjects.Where(x => x.ExamId == examId && x.IsActive).ToList();
            return input.Select(x => new SelectListItem { Text = x.Subject, Value = x.SubId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllSchoolDivision(long id, long schoolid)
        {
            var input = _Entities.tb_Division.Where(x => x.ClassId == id && x.tb_Class.SchoolId == schoolid && x.IsActive).ToList();
            return input.Select(x => new SelectListItem { Text = x.Division, Value = x.DivisionId.ToString() }).ToList();
        }
        //---------------- Add Sub ledger  05-Apr-2018
        public static List<SelectListItem> GetAccountHeads(long schoolId)
        {
            var input = _Entities.tb_AccountHead.Where(x => x.IsActive && x.SchoolId == schoolId && (x.ForBill==false || x.ForBill==null)).OrderBy(x => x.AccHeadName).ToList();
            return input.Select(x => new SelectListItem { Text = x.AccHeadName, Value = x.AccountId.ToString() }).ToList();
        }
        //-----------------Select subLedger corresponding to the account head 
        public static List<SelectListItem> GetSubLedgerList(long AccHeadId)
        {
            var input = _Entities.tb_SubLedgerData.Where(z => z.IsActive && z.AccHeadId == AccHeadId).OrderBy(z => z.SubLedgerName).ToList();
            return input.Select(x => new SelectListItem { Text = x.SubLedgerName, Value = x.LedgerId.ToString() }).ToList();
        }
        //--------------------Select full Bank details 
        public static List<SelectListItem> GetBankLists(long schoolId)
        {
            var input = _Entities.tb_Banks.Where(x => x.IsActive && x.SchoolId == schoolId).OrderBy(x => x.BankName).ToList();
            return input.Select(x => new SelectListItem { Text = x.BankName, Value = x.BankId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAccountHeadLists(long schoolId)
        {
            var headList = _Entities.sp_LedgerFilter(schoolId).ToList();
            return headList.Select(x => new SelectListItem { Text = x.AccHeadName, Value = x.ValueId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetTeachers(long schoolId)
        {
            var input = _Entities.tb_Teacher.Where(z => z.IsActive && z.SchoolId == schoolId).OrderBy(z => z.TeacherName).ToList();
            return input.Select(x => new SelectListItem { Text = x.TeacherName, Value = x.TeacherId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetSubjectss(long schoolId)
        {
            var input = _Entities.tb_Subjects.Where(z => z.IsActive && z.SchoolI == schoolId).OrderBy(z => z.SubjectName).ToList();
            return input.Select(x => new SelectListItem { Text = x.SubjectName, Value = x.SubId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetFeeses(long schoolId)
        {
            var input = _Entities.tb_Fee.Where(z => z.IsActive  && z.SchoolId == schoolId).OrderBy(z => z.FeesName).ToList();
            return input.Select(x => new SelectListItem { Text = x.FeesName, Value = x.FeeId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetLabCategories(long schoolId)
        {
            var input = _Entities.tb_LaboratoryCategory.Where(z => z.IsActive  && z.SchoolId == schoolId).OrderBy(z => z.LaboratoryName).ToList();
            return input.Select(x => new SelectListItem { Text = x.LaboratoryName, Value = x.CategoryId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAccountHeadsWithFeeHead(long schoolId)
        {
            var input = _Entities.tb_AccountHead.Where(x => x.IsActive && x.SchoolId == schoolId ).OrderBy(x => x.AccHeadName).ToList();
            return input.Select(x => new SelectListItem { Text = x.AccHeadName, Value = x.AccountId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetCurrentAcademicYear()
        {
            var year = _Entities.tb_AcademicYear.ToList();
            //var notYearId = year.FirstOrDefault().YearId;
            var input = year.Where(z => z.IsActive &&  z.CurrentYear==true).OrderBy(z => z.YearId).ToList();
            return input.Select(x => new SelectListItem { Text = x.AcademicYear, Value = x.YearId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetClassAllList()
        {
            var input = _Entities.tb_ClassList.Where(z => z.IsActive).OrderBy(z => z.OrderValue).ToList();
            return input.Select(x => new SelectListItem { Text = x.ClassName, Value = x.ClassName.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllClasses(long schoolId,long acYearId)
        {
            var input = _Entities.tb_Class.Where(x => x.AcademicYearId == acYearId && x.SchoolId == schoolId && x.IsActive).ToList();
            return input.Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllSchoolClassesWithAcademicYear(long academicYear, long schoolid)
        {
            var input = _Entities.tb_Class.Where(x => x.AcademicYearId == academicYear && x.SchoolId == schoolid && x.IsActive).OrderBy(x=>x.ClassOrder).ToList();
            return input.Select(x => new SelectListItem { Text = x.Class, Value = x.ClassId.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllReligions()
        {
            var input = _Entities.tb_Religion.Where(x => x.IsActive).ToList();
            return input.Select(x => new SelectListItem { Text = x.ReligionName, Value = x.Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllCategories()
        {
            var input = _Entities.tb_Category.Where(x => x.IsActive).ToList();
            return input.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllStaffUserTypesData(long schoolId)
        {
            var input = _Entities.tb_UserModuleMain.Where(x => x.SchoolId == schoolId && x.IsActive && x.IsTeacher == false).OrderBy(x => x.UserTypeName).ToList();
            return input.Select(x => new SelectListItem { Text = x.UserTypeName, Value = x.Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllTeacherUserTypeData(long schoolId)
        {
            var input = _Entities.tb_UserModuleMain.Where(x => x.SchoolId == schoolId && x.IsActive && x.IsTeacher == true).OrderBy(x => x.UserTypeName).ToList();
            return input.Select(x => new SelectListItem { Text = x.UserTypeName, Value = x.Id.ToString() }).ToList();
        }
        public List<SelectListItem> GetAllTeacherUserTypeDataAdd(long schoolId)
        {
            var input = _Entities.tb_UserModuleMain.Where(x => x.SchoolId == schoolId && x.IsActive && x.IsTeacher == true).OrderBy(x => x.UserTypeName).ToList();
            return input.Select(x => new SelectListItem { Text = x.UserTypeName, Value = x.Id.ToString() }).ToList();
        }
        public List<SelectListItem> GetAllStaffUserTypesDataAdd(long schoolId)
        {
            var input = _Entities.tb_UserModuleMain.Where(x => x.SchoolId == schoolId && x.IsActive && x.IsTeacher == false).OrderBy(x => x.UserTypeName).ToList();
            return input.Select(x => new SelectListItem { Text = x.UserTypeName, Value = x.Id.ToString() }).ToList();
        }



        // @jibin 9/14/2020
        public static List<SelectListItem> GetLabCategoriesAll()
        {
            var input = _Entities.tb_LaboratoryCategory.ToList();
            return input.Select(x => new SelectListItem { Text = x.LaboratoryName, Value = x.CategoryId.ToString() }).ToList();
        }


        public static List<SelectListItem> GetItemCategories(long CategoryId)
        {
            var input = _Entities.tb_AddCategory.Where(x => x.CategoryId == CategoryId).OrderBy(x => x.Item).ToList();
            return input.Select(x => new SelectListItem { Text = x.Item, Value = x.Item.ToString() }).ToList();
        }



        public static List<SelectListItem> GetUnitCategories(string id)
        {
            var input = _Entities.tb_AddCategory.Where(x => x.Item == id).OrderBy(x => x.Item).ToList();
            return input.Select(x => new SelectListItem { Text = x.Unit, Value = x.Unit.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAdmissionNumberByschholID(long schoolId)
        {
            var input = _Entities.tb_Student.Where(x => x.SchoolId== schoolId).ToList();
            return input.Select(x => new SelectListItem { Text = x.StudentSpecialId, Value = x.StudentSpecialId.ToString() }).ToList();
        }


        public static List<SelectListItem> GetPriceCategories(string id)
        {
            var input = _Entities.tb_StockUpdate.Where(x => x.Item == id).OrderBy(x => x.Item).ToList();
            return input.Select(x => new SelectListItem { Text = x.Item, Value = x.Price.ToString() }).ToList();
        }

        // @jibin 9/14/2020

    }
}

