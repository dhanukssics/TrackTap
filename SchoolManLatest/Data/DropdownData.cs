using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using TrackTap.Models;

namespace TrackTap.Data
{
    public class DropdownData
    {
        protected readonly SchoolDbContext _Entities;
        public DropdownData(SchoolDbContext Entities)
        {
            _Entities = Entities;
        }
        public async Task<List<SelectListItem>>GetUnPublishedClassesAsync(long schoolId)
        {
            return await _Entities
                .TbClasses
                .Where(x =>
                    x.IsActive
                    && x.PublishStatus == false
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetOtherAcademicYearAsync()
        {
            return await _Entities
                .TbAcademicYears
                .Where(x =>
                    x.IsActive)
                .OrderByDescending(x =>
                    x.YearId)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.AcademicYear,

                        Value =
                            x.YearId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetClassesAsync(long schoolId)
        {
            return await _Entities
                .TbClasses
                .Where(x =>
                    x.IsActive
                    && x.PublishStatus
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllSchoolAsync()
        {
            return await _Entities
                .TbSchools
                .Where(x =>
                    x.IsActive)
                .OrderBy(x =>
                    x.SchoolName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.SchoolName,

                        Value =
                            x.SchoolId
                                .ToString()
                    })
                .ToListAsync();
        }


        public async Task<List<SelectListItem>>GetClassListAsync()
        {
            return await _Entities
                .TbClassLists
                .Where(x =>
                    x.IsActive)
                .OrderBy(x =>
                    x.OrderValue)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.ClassName,

                        Value =
                            x.OrderValue
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetDivisionAsync(long classId)
        {
            return await _Entities
                .TbDivisions
                .Where(x =>
                    x.IsActive
                    && x.ClassId == classId)
                .OrderBy(x =>
                    x.Division)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Division,

                        Value =
                            x.DivisionId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetBusAsync(long schoolId)
        {
            return await _Entities
                .TbBus
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.BusSpecialId)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.BusSpecialId,

                        Value =
                            x.BusId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetBusNameAsync(long schoolId)
        {
            return await _Entities
                .TbBus
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.BusSpecialId)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.BusName,

                        Value =
                            x.BusId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetFreeSchoolDivisionAsync(long id,long schoolId)
        {
            var input =
                await _Entities
                    .UnassignedDivisionResults
                    .FromSqlInterpolated(
                        $@"EXEC SP_UnassignedDivisions
                    @schoolId={schoolId},
                    @classId={id}")
                    .ToListAsync();

            return input
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Division,

                        Value =
                            x.DivisionId
                                .ToString()
                    })
                .ToList();
        }

        public async Task<List<SelectListItem>>GetSchoolListAsync()
        {
            return await _Entities
                .TbLogins
                .Include(x => x.School)
                .Where(x =>
                    x.IsActive
                    && x.RoleId ==
                        (int)UserRole.School)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.School.SchoolName,

                        Value =
                            x.SchoolId
                                .ToString()
                    })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>>GetSchoolPaymentGatwayListAsync()
        {
            return await _Entities
                .TbLogins
                .Include(x => x.School)
                .Where(x =>
                    x.IsActive
                    && x.RoleId ==
                        (int)UserRole.School
                    && x.School != null
                    && x.School.PaymentOption == true)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.School.SchoolName,

                        Value =
                            x.SchoolId
                                .ToString()
                    })
                .ToListAsync();
        }
        //Non-Static

        public async Task<List<SelectListItem>>GetFreeClassesAsync(long schoolId)
        {
            var classes =
                await _Entities
                    .UnassignedTeacherResults
                    .FromSqlInterpolated(
                        $@"EXEC SP_UnassignedTeachers
                    @schoolId={schoolId}")
                    .ToListAsync();

            return classes
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.ClassName,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToList();
        }
        public async Task<List<SelectListItem>>GetFreeDivisionAsync(long classId)
        {
            return await _Entities
                .TbDivisions
                .Where(d =>
                    d.IsActive

                    && d.Class.ClassId ==
                        classId

                    && !_Entities
                        .TbTeacherClasses
                        .Any(tc =>
                            tc.DivisionId ==
                                d.DivisionId))
                .OrderBy(d =>
                    d.Class.ClassOrder)
                .Select(d =>
                    new SelectListItem
                    {
                        Text =
                            d.Division,

                        Value =
                            d.DivisionId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>> RefreshClassesAsync(long schoolId)
        {
            return await _Entities
                .TbClasses
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>RefreshClassesUnPublishedAsync(long schoolId)
        {
            var year = await _Entities
                .TbAcademicYears
                .FirstOrDefaultAsync();

            if (year == null)
            {
                return new List<SelectListItem>();
            }

            return await _Entities
                .TbClasses
                .Where(x =>
                    x.IsActive

                    && x.SchoolId ==
                        schoolId

                    && year.YearId !=
                        x.AcademicYearId)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>RefreshSchoolFeesAsync(long schoolId)
        {
            return await _Entities
                .TbFees
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId
                    && x.FeeType ==
                        (int)FeeType.CommonFee)
                .OrderBy(x =>
                    x.FeesName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.FeesName,

                        Value =
                            x.FeeId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>SchoolSpecialFeesListAsync(long schoolId)
        {
            return await _Entities
                .TbFees
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId
                    && x.FeeType ==
                        (int)FeeType.SpecialFee)
                .OrderBy(x =>
                    x.FeeId)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.FeesName,

                        Value =
                            x.FeeId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetFreeTeacherClassesAsync(long schoolId,long teacherId)
        {
            var assignedDivisionIds =
                await _Entities
                    .TbTeacherClasses
                    .Where(x =>
                        x.Class.SchoolId == schoolId
                        && x.TeacherId != teacherId)
                    .Select(x =>
                        x.DivisionId)
                    .ToListAsync();

            var classIds =
                await _Entities
                    .TbDivisions
                    .Where(x =>
                        x.IsActive
                        && !assignedDivisionIds
                            .Contains(x.DivisionId)
                        && x.Class.SchoolId == schoolId
                        && x.Class.PublishStatus == true
                        && x.Class.IsActive == true)
                    .Select(x =>
                        x.ClassId)
                    .Distinct()
                    .ToListAsync();

            return await _Entities
                .TbClasses
                .Where(x =>
                    classIds.Contains(x.ClassId)
                    && x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetFreeTeacherDivisionAsync(string classIdValue,long teacherId)
        {
            long classId =
                Convert.ToInt64(classIdValue);

            var teacherDivisionIds =
                await _Entities
                    .TbTeacherClasses
                    .Where(x =>
                        x.Class.ClassId == classId
                        && x.TeacherId != teacherId)
                    .Select(x =>
                        x.DivisionId)
                    .ToListAsync();

            return await _Entities
                .TbDivisions
                .Where(x =>
                    x.IsActive
                    && !teacherDivisionIds
                        .Contains(x.DivisionId)
                    && x.Class.ClassId == classId
                    && x.Class.PublishStatus == true
                    && x.Class.IsActive == true)
                .OrderBy(x =>
                    x.Class.ClassOrder)
                .ThenBy(x =>
                    x.Division)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Division,

                        Value =
                            x.DivisionId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetTeacherDivisionAsync(long classId,long userId)
        {
            var teacher =
                await _Entities
                    .TbTeachers
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId);

            if (teacher == null)
            {
                return new List<SelectListItem>();
            }

            return await _Entities
                .TbTeacherClasses
                .Where(x =>
                    x.TeacherId == teacher.TeacherId
                    && x.ClassId == classId)
                .OrderBy(x =>
                    x.Division.Division)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Division.Division,

                        Value =
                            x.DivisionId
                                .ToString()
                    })
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetTeacherClassAsync(long userId)
        {
            var teacher =
                await _Entities
                    .TbTeachers
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId);

            if (teacher == null)
            {
                return new List<SelectListItem>();
            }

            return await _Entities
                .TbTeacherClasses
                .Where(x =>
                    x.TeacherId == teacher.TeacherId
                    && x.Class.PublishStatus == true)
                .OrderBy(x =>
                    x.Class.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetBookCategoryAsync(long schoolId)
        {
            return await _Entities
                .TbBookCategories
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.Category)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Category,

                        Value =
                            x.CategoryId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllExamsAsync(long classId,long divisionId,long schoolId)
        {
            return await _Entities
                .TbExams
                .Where(x =>
                    x.ClassId == classId
                    && x.DivisionId == divisionId
                    && x.SchoolId == schoolId
                    && x.IsActive)
                .OrderBy(x =>
                    x.ExamName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.ExamName,

                        Value =
                            x.ExamId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllSubjectsAsync(long examId)
        {
            return await _Entities
                .TbExamSubjects
                .Where(x =>
                    x.ExamId == examId
                    && x.IsActive)
                .OrderBy(x =>
                    x.Subject)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Subject,

                        Value =
                            x.SubId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllSchoolDivisionAsync(long id,long schoolId)
        {
            return await _Entities
                .TbDivisions
                .Where(x =>
                    x.ClassId == id
                    && x.Class.SchoolId == schoolId
                    && x.IsActive)
                .OrderBy(x =>
                    x.Division)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Division,

                        Value =
                            x.DivisionId
                                .ToString()
                    })
                .ToListAsync();
        }
        //---------------- Add Sub ledger  05-Apr-2018
        public async Task<List<SelectListItem>>GetAccountHeadsAsync(long schoolId)
        {
            return await _Entities
                .TbAccountHeads
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId
                    && x.ForBill != true)
                .OrderBy(x =>
                    x.AccHeadName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.AccHeadName,

                        Value =
                            x.AccountId
                                .ToString()
                    })
                .ToListAsync();
        }
        //-----------------Select subLedger corresponding to the account head 
        public async Task<List<SelectListItem>>GetSubLedgerListAsync(long accHeadId)
        {
            return await _Entities
                .TbSubLedgerData
                .Where(x =>
                    x.IsActive
                    && x.AccHeadId == accHeadId)
                .OrderBy(x =>
                    x.SubLedgerName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.SubLedgerName,

                        Value =
                            x.LedgerId
                                .ToString()
                    })
                .ToListAsync();
        }
        //--------------------Select full Bank details 
        public async Task<List<SelectListItem>>GetBankListsAsync(long schoolId)
        {
            return await _Entities
                .TbBanks
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.BankName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.BankName,

                        Value =
                            x.BankId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAccountHeadListsAsync(long schoolId)
        {
            var headList =
                await _Entities
                    .LedgerFilterResults
                    .FromSqlInterpolated(
                        $@"EXEC sp_LedgerFilter
                    @schoolId1={schoolId}")
                    .AsNoTracking()
                    .ToListAsync();

            return headList
                .OrderBy(x =>
                    x.AccHeadName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.AccHeadName,

                        Value =
                            x.ValueId
                    })
                .ToList();
        }
        public async Task<List<SelectListItem>>GetTeachersAsync(long schoolId)
        {
            return await _Entities
                .TbTeachers
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.TeacherName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.TeacherName,

                        Value =
                            x.TeacherId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetSubjectsAsync(long schoolId)
        {
            return await _Entities
                .TbSubjects
                .Where(x =>
                    x.IsActive
                    && x.SchoolI == schoolId)
                .OrderBy(x =>
                    x.SubjectName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.SubjectName,

                        Value =
                            x.SubId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetFeesesAsync(long schoolId)
        {
            return await _Entities
                .TbFees
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.FeesName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.FeesName,

                        Value =
                            x.FeeId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetLabCategoriesAsync(long schoolId)
        {
            return await _Entities
                .TbLaboratoryCategories
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.LaboratoryName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.LaboratoryName,

                        Value =
                            x.CategoryId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAccountHeadsWithFeeHeadAsync(long schoolId)
        {
            return await _Entities
                .TbAccountHeads
                .Where(x =>
                    x.IsActive
                    && x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.AccHeadName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.AccHeadName,

                        Value =
                            x.AccountId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetCurrentAcademicYearAsync()
        {
            return await _Entities
                .TbAcademicYears
                .Where(x =>
                    x.IsActive
                    && x.CurrentYear == true)
                .OrderBy(x =>
                    x.YearId)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.AcademicYear,

                        Value =
                            x.YearId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetClassAllListAsync()
        {
            return await _Entities
                .TbClassLists
                .Where(x =>
                    x.IsActive)
                .OrderBy(x =>
                    x.OrderValue)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.ClassName,

                        Value =
                            x.ClassName
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllClassesAsync(long schoolId,long acYearId)
        {
            return await _Entities
                .TbClasses
                .Where(x =>
                    x.AcademicYearId == acYearId
                    && x.SchoolId == schoolId
                    && x.IsActive)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllSchoolClassesWithAcademicYearAsync(long academicYear,long schoolId)
        {
            return await _Entities
                .TbClasses
                .Where(x =>
                    x.AcademicYearId == academicYear
                    && x.SchoolId == schoolId
                    && x.IsActive)
                .OrderBy(x =>
                    x.ClassOrder)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Class,

                        Value =
                            x.ClassId
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllReligionsAsync()
        {
            return await _Entities
                .TbReligions
                .Where(x =>
                    x.IsActive)
                .OrderBy(x =>
                    x.ReligionName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.ReligionName,

                        Value =
                            x.Id
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllCategoriesAsync()
        {
            return await _Entities
                .TbCategories
                .Where(x =>
                    x.IsActive)
                .OrderBy(x =>
                    x.CategoryName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.CategoryName,

                        Value =
                            x.Id
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllStaffUserTypesDataAsync(long schoolId)
        {
            return await _Entities
                .TbUserModuleMains
                .Where(x =>
                    x.SchoolId == schoolId
                    && x.IsActive
                    && x.IsTeacher == false)
                .OrderBy(x =>
                    x.UserTypeName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.UserTypeName,

                        Value =
                            x.Id
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllTeacherUserTypeDataAsync(long schoolId)
        {
            return await _Entities
                .TbUserModuleMains
                .Where(x =>
                    x.SchoolId == schoolId
                    && x.IsActive
                    && x.IsTeacher == true)
                .OrderBy(x =>
                    x.UserTypeName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.UserTypeName,

                        Value =
                            x.Id
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllTeacherUserTypeDataAddAsync(long schoolId)
        {
            return await _Entities
                .TbUserModuleMains
                .Where(x =>
                    x.SchoolId == schoolId
                    && x.IsActive
                    && x.IsTeacher == true)
                .OrderBy(x =>
                    x.UserTypeName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.UserTypeName,

                        Value =
                            x.Id
                                .ToString()
                    })
                .ToListAsync();
        }
        public async Task<List<SelectListItem>>GetAllStaffUserTypesDataAddAsync(long schoolId)
        {
            return await _Entities
                .TbUserModuleMains
                .Where(x =>
                    x.SchoolId == schoolId
                    && x.IsActive
                    && x.IsTeacher == false)
                .OrderBy(x =>
                    x.UserTypeName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.UserTypeName,

                        Value =
                            x.Id
                                .ToString()
                    })
                .ToListAsync();
        }



        // @jibin 9/14/2020
        public async Task<List<SelectListItem>>GetLabCategoriesAllAsync()
        {
            return await _Entities
                .TbLaboratoryCategories
                .OrderBy(x =>
                    x.LaboratoryName)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.LaboratoryName,

                        Value =
                            x.CategoryId
                                .ToString()
                    })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>>GetItemCategoriesAsync(long categoryId)
        {
            return await _Entities
                .TbAddCategories
                .Where(x =>
                    x.CategoryId == categoryId)
                .OrderBy(x =>
                    x.Item)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Item,

                        Value =
                            x.Item
                    })
                .Distinct()
                .ToListAsync();
        }



        public async Task<List<SelectListItem>>GetUnitCategoriesAsync(string id)
        {
            return await _Entities
                .TbAddCategories
                .Where(x =>
                    x.Item == id)
                .OrderBy(x =>
                    x.Item)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Unit,

                        Value =
                            x.Unit
                    })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<SelectListItem>>GetAdmissionNumberBySchoolIdAsync(long schoolId)
        {
            return await _Entities
                .TbStudents
                .Where(x =>
                    x.SchoolId == schoolId)
                .OrderBy(x =>
                    x.StudentSpecialId)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.StudentSpecialId,

                        Value =
                            x.StudentSpecialId
                    })
                .Distinct()
                .ToListAsync();
        }


        public async Task<List<SelectListItem>>GetPriceCategoriesAsync(string id)
        {
            return await _Entities
                .TbStockUpdates
                .Where(x =>
                    x.Item == id)
                .Select(x =>
                    new SelectListItem
                    {
                        Text =
                            x.Item,

                        Value =
                            x.Price
                                .ToString()
                    })
                .Distinct()
                .ToListAsync();
        }

        // @jibin 9/14/2020

    }
}

