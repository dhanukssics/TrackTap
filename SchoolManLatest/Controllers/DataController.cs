using Microsoft.AspNetCore.Mvc;

namespace TrackTap.Controllers
{
    public class DataController : Controller
    {
        public IActionResult LoadDivision(long id)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetDivision(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadFreeDivision(long id)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetDivision(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadFreeSchoolDivision(
            long id,
            long schoolid)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetFreeSchoolDivision(
                        id,
                        schoolid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadTeacherDivision(
            long id,
            long userid)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetTeacherDivision(
                        id,
                        userid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadExams(
            long classId,
            long divisionid,
            long schoolId)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetAllExams(
                        classId,
                        divisionid,
                        schoolId);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadSubjects(long examId)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetAllSubjects(examId);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadAllSchoolDivision(
            long id,
            long schoolid)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetAllSchoolDivision(
                        id,
                        schoolid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadSubLedgerList(long id)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetSubLedgerList(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadClassesByAcademicYear(
            string id)
        {
            string[] splitData =
                id.Split('~');

            long schoolId =
                Convert.ToInt64(splitData[0]);

            long acYearId =
                Convert.ToInt64(splitData[1]);

            var result =
                TrackTap.Data.DropdownData
                    .GetAllClasses(
                        schoolId,
                        acYearId);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult LoadAllSchoolClassWithAcademicYear(
            long academicYear,
            long schoolid)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetAllSchoolClassesWithAcademicYear(
                        academicYear,
                        schoolid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult GetItemCategories(long id)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetItemCategories(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult GetUnitCategories(string id)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetUnitCategories(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public IActionResult GetPriceCategories(string id)
        {
            var result =
                TrackTap.Data.DropdownData
                    .GetPriceCategories(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }
    }
}