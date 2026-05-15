using Microsoft.AspNetCore.Mvc;
using TrackTap.Data;

namespace TrackTap.Controllers
{
    public class DataController : Controller
    {
        private readonly DropdownData _dropdownData;
        public DataController(DropdownData dropdownData)
        {
            _dropdownData = dropdownData;
        }
        public async Task<IActionResult> LoadDivision(long id)
        {
            var result =await _dropdownData
                    .GetDivisionAsync(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadFreeDivision(long id)
        {
            var result =await _dropdownData.GetDivisionAsync(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadFreeSchoolDivision(long id,long schoolid)
        {
            var result =await _dropdownData.GetFreeSchoolDivisionAsync(
                        id,
                        schoolid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadTeacherDivision(
            long id,
            long userid)
        {
            var result = await _dropdownData.GetTeacherDivisionAsync(
                        id,
                        userid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadExams(long classId,long divisionid,long schoolId)
        {
            var result = await _dropdownData.GetAllExamsAsync(
                        classId,
                        divisionid,
                        schoolId);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadSubjects(long examId)
        {
            var result = await _dropdownData.GetAllSubjectsAsync(examId);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadAllSchoolDivision(
            long id,
            long schoolid)
        {
            var result = await _dropdownData.GetAllSchoolDivisionAsync(
                        id,
                        schoolid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadSubLedgerList(long id)
        {
            var result = await _dropdownData.GetSubLedgerListAsync(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadClassesByAcademicYear(string id)
        {
            string[] splitData =
                id.Split('~');

            long schoolId =
                Convert.ToInt64(splitData[0]);

            long acYearId =
                Convert.ToInt64(splitData[1]);

            var result = await _dropdownData.GetAllClassesAsync(
                        schoolId,
                        acYearId);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> LoadAllSchoolClassWithAcademicYear(long academicYear,long schoolid)
        {
            var result = await _dropdownData.GetAllSchoolClassesWithAcademicYearAsync(
                        academicYear,
                        schoolid);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> GetItemCategories(long id)
        {
            var result = await _dropdownData.GetItemCategoriesAsync(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> GetUnitCategories(string id)
        {
            var result = await _dropdownData.GetUnitCategoriesAsync(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }

        public async Task<IActionResult> GetPriceCategories(string id)
        {
            var result =await _dropdownData.GetPriceCategoriesAsync(id);

            return Json(new
            {
                status = result.Count > 0,
                list = result
            });
        }
    }
}