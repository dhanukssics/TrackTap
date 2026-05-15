
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackTap.Data;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class PromotionController : BaseController
    {
        private readonly DropdownData _dropdownData;
        public PromotionController(SchoolDbContext Entities, SchoolRepository schoolRepository, ParentRepository parentRepository, TeacherRepository teacherRepository, DropdownData dropdownData) : base(Entities, schoolRepository, parentRepository, teacherRepository)
        {
            _dropdownData = dropdownData;
        }

        public IActionResult PromoteClassHome()
        {
            SchoolValue model = new SchoolValue();
            model.schoolId = _user.SchoolId;
            return View(model);
        }
        public async Task<IActionResult> UnpublishedClasses(long id)
        {
            int slNo = 0;

            var model =
                new UpPublishedClassList
                {
                    list =
                        new List<UnPublishedClass>()
                };

            IQueryable<TbDivision> query =
                _Entities.TbDivisions
                    .Include(x => x.Class)
                        .ThenInclude(x => x.AcademicYear)
                    .Where(x =>
                        x.Class.SchoolId ==
                            _user.SchoolId

                        && x.IsActive

                        && x.Class.IsActive

                        && !x.Class.PublishStatus);

            if (id != 0)
            {
                query = query.Where(x =>
                    x.Class.ClassId == id);
            }

            var classList =
                await query.ToListAsync();

            if (classList.Any())
            {
                foreach (var item in classList)
                {
                    slNo++;

                    var one =
                        new UnPublishedClass
                        {
                            ClassId =
                                item.ClassId,

                            ClassName =
                                item.Class.Class,

                            DivisionId =
                                item.DivisionId,

                            DivisionName =
                                item.Division,

                            AccademicYear =
                                item.Class
                                    .AcademicYear
                                    .AcademicYear,

                            CurrentYearStatus =
                                item.Class
                                    .AcademicYear
                                    .CurrentYear ?? false,

                            SlNo =
                                slNo
                        };

                    model.list.Add(one);
                }
            }

            return PartialView(
                "~/Views/Promotion/_pv_UnPublishedClassList.cshtml",
                model);
        }
        public IActionResult AddUnPublishedClassParialView()
        {
            AddClassModel model = new AddClassModel();
            model.SchoolId = _user.SchoolId;
            return PartialView("~/Views/Promotion/_pv_AddClassUnPublished.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddClassUnPublished(AddClassModel model)
        {
            bool status = false;

            string msg = "Failed";

            long classId = 0;

            try
            {
                var div = await _Entities
                    .TbDivisions
                    .Include(x => x.Class)
                    .FirstOrDefaultAsync(x =>
                        x.Class.Class ==
                            model.ClassName

                        && x.Division ==
                            model.Division

                        && x.Class.AcademicYearId ==
                            model.AcademicYearId

                        && x.IsActive

                        && x.Class.IsActive

                        && x.Class.SchoolId ==
                            _user.SchoolId);

                if (div != null)
                {
                    msg =
                        "Division and Class already exists in this Academic Year!";
                }
                else
                {
                    using var transaction =
                        await _Entities.Database
                            .BeginTransactionAsync();

                    var classOld = await _Entities
                        .TbClasses
                        .FirstOrDefaultAsync(x =>
                            x.Class ==
                                model.ClassName

                            && x.SchoolId ==
                                _user.SchoolId

                            && x.IsActive

                            && x.AcademicYearId ==
                                model.AcademicYearId);

                    if (classOld != null)
                    {
                        var newDiv =
                            new TbDivision
                            {
                                ClassId =
                                    classOld.ClassId,

                                Division =
                                    model.Division
                                        .ToUpper(),

                                DivisionGuid =
                                    Guid.NewGuid(),

                                IsActive = true,

                                TimeStamp =
                                    CurrentTime
                            };

                        await _Entities
                            .TbDivisions
                            .AddAsync(newDiv);

                        status =
                            await _Entities
                                .SaveChangesAsync() > 0;

                        msg = status
                            ? "Success"
                            : "Failed";

                        classId =
                            classOld.ClassId;
                    }
                    else
                    {
                        int classOrder =
                            await _Entities
                                .TbClassLists
                                .Where(x =>
                                    x.ClassName ==
                                        model.ClassName)
                                .Select(x =>
                                    x.OrderValue)
                                .FirstOrDefaultAsync();

                        var newClass =
                            new TbClass
                            {
                                Class =
                                    model.ClassName,

                                ClassGuild =
                                    Guid.NewGuid(),

                                Timestamp =
                                    CurrentTime,

                                SchoolId =
                                    model.SchoolId,

                                IsActive = true,

                                ClassOrder =
                                    classOrder,

                                AcademicYearId =
                                    model.AcademicYearId,

                                PublishStatus =
                                    false
                            };

                        await _Entities
                            .TbClasses
                            .AddAsync(newClass);

                        await _Entities
                            .SaveChangesAsync();

                        var newDiv =
                            new TbDivision
                            {
                                ClassId =
                                    newClass.ClassId,

                                Division =
                                    model.Division
                                        .ToUpper(),

                                DivisionGuid =
                                    Guid.NewGuid(),

                                IsActive = true,

                                TimeStamp =
                                    CurrentTime
                            };

                        await _Entities
                            .TbDivisions
                            .AddAsync(newDiv);

                        status =
                            await _Entities
                                .SaveChangesAsync() > 0;

                        msg = status
                            ? "Success"
                            : "Failed";

                        classId =
                            newClass.ClassId;
                    }

                    await transaction
                        .CommitAsync();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            var classList =
                await _dropdownData
                    .RefreshClassesAsync(
                        model.SchoolId);

            return Json(new
            {
                status = status,
                msg = msg,
                classId = classId,
                list = classList
            });
        }
        [HttpPost]
        public async Task<IActionResult> PublishUnPublishedClass(
    long id)
        {
            bool status = false;

            string message = "Failed";

            try
            {
                using var transaction =
                    await _Entities.Database
                        .BeginTransactionAsync();

                var division = await _Entities
                    .TbDivisions
                    .FirstOrDefaultAsync(x =>
                        x.DivisionId == id);

                if (division == null)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "Division not found"
                    });
                }

                var currentClass = await _Entities
                    .TbClasses
                    .FirstOrDefaultAsync(x =>
                        x.ClassId ==
                            division.ClassId);

                if (currentClass == null)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "Class not found"
                    });
                }

                // Publish current class
                currentClass.PublishStatus = true;

                // Unpublish previous academic year classes
                var previousClasses = await _Entities
                    .TbClasses
                    .Where(x =>
                        x.Class ==
                            currentClass.Class

                        && x.AcademicYearId !=
                            currentClass.AcademicYearId

                        && x.SchoolId ==
                            _user.SchoolId

                        && x.IsActive)
                    .ToListAsync();

                if (previousClasses.Any())
                {
                    foreach (var item in previousClasses)
                    {
                        item.PublishStatus = false;
                    }
                }

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                await transaction
                    .CommitAsync();

                message = status
                    ? "Published"
                    : "Failed";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            var classList =
                await _dropdownData
                    .RefreshClassesUnPublishedAsync(
                        _user.SchoolId);

            return Json(new
            {
                status = status,
                msg = message,
                list = classList
            });
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
        public async Task<IActionResult>StudentListForPromotion(string id)
        {
            string[] splitData =
                id.Split('~');

            long academicYearId =
                Convert.ToInt64(
                    splitData[0]);

            long classId =
                Convert.ToInt64(
                    splitData[1]);

            long divisionId =
                Convert.ToInt64(
                    splitData[2]);

            var studentList = await _Entities
                .TbStudents
                .Where(x =>
                    x.IsActive

                    && x.SchoolId ==
                        _user.SchoolId

                    && x.ClassId ==
                        classId

                    && x.DivisionId ==
                        divisionId)
                .ToListAsync();

            var model =
                new PromoteStudents
                {
                    OldAcademicyearId =
                        academicYearId,

                    OldClassId =
                        classId,

                    OldDivId =
                        divisionId,

                    SchoolId =
                        _user.SchoolId,

                    StudentList =
                        new List<StudentListForPromote>()
                };

            int count = 1;

            foreach (var item in studentList)
            {
                var one =
                    new StudentListForPromote
                    {
                        StudentId =
                            item.StudentId,

                        StudentName =
                            item.StundentName,

                        SpecialId =
                            item.StudentSpecialId,

                        ContactNumber =
                            item.ContactNumber,

                        SlNo =
                            count++
                    };

                model.StudentList.Add(one);
            }

            return PartialView(
                "~/Views/Promotion/_pv_StudentListForPromotion.cshtml",
                model);
        }
        public IActionResult PromoteStudentsToAnotherParialView(PromoteStudents model)
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

        [HttpPost]
        public async Task<IActionResult> AddPromoteStudents(PromoteStudents model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                using var transaction =
                    await _Entities.Database
                        .BeginTransactionAsync();

                if (string.IsNullOrWhiteSpace(
                    model.StudentListString))
                {
                    return Json(new
                    {
                        status = false,
                        msg = "No students selected"
                    });
                }

                string[] listOfStudents =
                    model.StudentListString
                        .Split(',',
                            StringSplitOptions
                                .RemoveEmptyEntries);

                foreach (var item in listOfStudents)
                {
                    long studentId =
                        Convert.ToInt64(item);

                    var student =
                        await _Entities
                            .TbStudents
                            .FirstOrDefaultAsync(x =>
                                x.StudentId ==
                                    studentId);

                    if (student == null)
                    {
                        continue;
                    }

                    // Remove old last update flag
                    var oldPromotions =
                        await _Entities
                            .TbStudentPremotions
                            .Where(x =>
                                x.StudentId ==
                                    studentId

                                && x.IsActive == 1)
                            .ToListAsync();

                    foreach (var promotion
                        in oldPromotions)
                    {
                        promotion.LastUpdate =
                            false;
                    }

                    // Remove duplicate promotion
                    var duplicateData =
                        await _Entities
                            .TbStudentPremotions
                            .FirstOrDefaultAsync(x =>
                                x.StudentId ==
                                    studentId

                                && x.OldClass ==
                                    student.ClassId

                                && x.IsActive == 1);

                    if (duplicateData != null)
                    {
                        duplicateData.IsActive = 0;
                    }

                    // Add new promotion
                    var newPromotion =
                        new TbStudentPremotion
                        {
                            StudentId =
                                student.StudentId,

                            FromDivision =
                                student.DivisionId,

                            ToDivision =
                                model.NewDivId,

                            TimeStamp =
                                CurrentTime,

                            SchoolId =
                                _user.SchoolId,

                            IsActive = 1,

                            LastUpdate = true,

                            OldClass =
                                student.ClassId
                        };

                    await _Entities
                        .TbStudentPremotions
                        .AddAsync(newPromotion);

                    // Update student class/division
                    student.ClassId =
                        model.NewCLassId;

                    student.DivisionId =
                        model.NewDivId;

                    // Prevent duplicate academic year entries
                    var promotionList =
                        await _Entities
                            .TbStudentPremotions
                            .Where(x =>
                                x.StudentId ==
                                    studentId

                                && x.IsActive == 1)
                            .OrderByDescending(x =>
                                x.PremotionId)
                            .ToListAsync();

                    List<long> academicYears =
                        new List<long>();

                    foreach (var promotion
                        in promotionList)
                    {
                        var classData =
                            await _Entities
                                .TbClasses
                                .FirstOrDefaultAsync(x =>
                                    x.ClassId ==
                                        promotion.OldClass);

                        if (classData == null)
                        {
                            continue;
                        }

                        if (!academicYears.Contains(
                            classData.AcademicYearId))
                        {
                            academicYears.Add(
                                classData.AcademicYearId);
                        }
                        else
                        {
                            promotion.IsActive = 0;
                        }
                    }
                }

                status =
                    await _Entities
                        .SaveChangesAsync() > 0;

                await transaction
                    .CommitAsync();

                if (status)
                {
                    msg = "Successful";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,
                msg = msg
            });
        }
        [HttpPost]
        public async Task<IActionResult> DemoteStudents(PromoteStudents model)
        {
            bool status = false;

            string msg = "Failed";

            int totalCount = 0;

            try
            {
                using var transaction =
                    await _Entities.Database
                        .BeginTransactionAsync();

                if (model.StudentList == null
                    || !model.StudentList.Any())
                {
                    return Json(new
                    {
                        status = false,
                        msg = "No students selected"
                    });
                }

                foreach (var item in model.StudentList)
                {
                    var oldPromotionDetails =
                        await _Entities
                            .TbStudentPremotions
                            .FirstOrDefaultAsync(x =>
                                x.StudentId ==
                                    item.StudentId

                                && x.SchoolId ==
                                    _user.SchoolId

                                && x.LastUpdate==true

                                && x.IsActive == 1);

                    if (oldPromotionDetails != null)
                    {
                        oldPromotionDetails.LastUpdate =
                            false;

                        oldPromotionDetails.TimeStamp =
                            CurrentTime;

                        var student =
                            await _Entities
                                .TbStudents
                                .FirstOrDefaultAsync(x =>
                                    x.StudentId ==
                                        item.StudentId

                                    && x.IsActive);

                        if (student != null)
                        {
                            student.ClassId =
                                oldPromotionDetails
                                    .OldClass ?? 0;

                            student.DivisionId =
                                oldPromotionDetails
                                    .FromDivision;

                            status = true;
                        }
                    }
                    else
                    {
                        totalCount++;
                    }
                }

                if (status)
                {
                    await _Entities
                        .SaveChangesAsync();

                    await transaction
                        .CommitAsync();

                    msg = "Successful";
                }

                if (totalCount != 0)
                {
                    status = false;

                    int success =
                        model.StudentList.Count
                        - totalCount;

                    msg =
                        success
                        + " Students has demoted and "
                        + totalCount
                        + " Students has not demoted. Since their previous informations not available.";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new
            {
                status = status,
                msg = msg
            });
        }
    }
}
