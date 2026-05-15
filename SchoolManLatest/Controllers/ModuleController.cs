using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class ModuleController : AdminBaseController
    {
        public ModuleController(SchoolDbContext Entities, SchoolRepository schoolRepository, ParentRepository parentRepository, TeacherRepository teacherRepository) : base(Entities, schoolRepository, parentRepository, teacherRepository)
        {
        }

        // GET: Module
        public IActionResult Index()
        {
            return View();
        }

        #region SuperAdmin school module
        public async Task<IActionResult> SchoolTypeDefineHome()
        {
            var model =
                new SchoolModuleModel
                {
                    mainList =
                        new List<SchoolMainModuleList>()
                };

            var list = await _Entities
                .TbSchoolSubModules
                .Include(x => x.Main)
                .Where(x => x.IsActive)
                .ToListAsync();

            var mainList = list
                .Select(x =>
                    x.Main
                        .MainModule)
                .Distinct()
                .ToList();

            foreach (var item in mainList)
            {
                string subIdString = "";

                string main =
                    Convert.ToString(item);

                var sub = list
                    .Where(x =>
                        x.Main
                            .MainModule == main)
                    .ToList();

                var one =
                    new SchoolMainModuleList
                    {
                        Id =
                            sub.FirstOrDefault()?.Id ?? 0,

                        ModuleName =
                            main,

                        subList =
                            new List<SchoolSubModuleList>()
                    };

                foreach (var a in sub)
                {
                    var b =
                        new SchoolSubModuleList
                        {
                            MainId =
                                one.Id,

                            Id =
                                a.Id,

                            SubMosule =
                                a.SchoolSubModule
                        };

                    one.subList.Add(b);

                    if (string.IsNullOrEmpty(
                        subIdString))
                    {
                        subIdString =
                            a.Id.ToString();
                    }
                    else
                    {
                        subIdString =
                            subIdString
                            + ","
                            + a.Id;
                    }
                }

                one.subIdListString =
                    subIdString;

                model.mainList.Add(one);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAddSchoolModule(SchoolModuleModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                bool exists = await _Entities
                    .TbSchoolModuleMains
                    .AnyAsync(x =>
                        x.SchoolName
                            .ToUpper()
                            .Trim() ==
                        model.SchoolName
                            .ToUpper()
                            .Trim()

                        && x.SchoolId ==
                            model.SchoolId

                        && x.IsActive);

                if (exists)
                {
                    msg =
                        "This School module already exists!";

                    return Json(new
                    {
                        status = false,
                        msg = msg
                    });
                }

                using var transaction =
                    await _Entities.Database
                        .BeginTransactionAsync();

                var main =
                    new TbSchoolModuleMain
                    {
                        SchoolId =
                            model.SchoolId,

                        SchoolName =
                            model.SchoolName,

                        IsActive = true,

                        TimeStamp =
                            CurrentTime
                    };

                await _Entities
                    .TbSchoolModuleMains
                    .AddAsync(main);

                await _Entities
                    .SaveChangesAsync();

                if (model.subListOnly != null
                    && model.subListOnly.Any())
                {
                    foreach (var item
                        in model.subListOnly)
                    {
                        var mainDetails =
                            await _Entities
                                .TbSchoolSubModules
                                .FirstOrDefaultAsync(x =>
                                    x.Id == item.Id);

                        if (mainDetails == null)
                        {
                            continue;
                        }

                        var sub =
                            new TbSchoolModuleDetail
                            {
                                SchoolModuleId =
                                    main.Id,

                                MainId =
                                    mainDetails.MainId,

                                SchoolSubModuleId =
                                    item.Id,

                                SchoolId =
                                    model.SchoolId,

                                IsActive = true,

                                TimeStamp =
                                    CurrentTime
                            };

                        await _Entities
                            .TbSchoolModuleDetails
                            .AddAsync(sub);
                    }

                    await _Entities
                        .SaveChangesAsync();
                }

                var schools = await _Entities
                    .TbSchools
                    .Where(x =>
                        x.SchoolId ==
                            model.SchoolId)
                    .ToListAsync();

                if (schools.Any())
                {
                    foreach (var item in schools)
                    {
                        item.SchoolType =
                            main.Id;
                    }

                    await _Entities
                        .SaveChangesAsync();
                }

                await transaction
                    .CommitAsync();

                status = true;

                msg = "Successful";
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
        public async Task<IActionResult> SchoolModuleListView()
        {
            int count = 0;

            var model =
                new SchoolListdata
                {
                    list =
                        new List<ModelList>()
                };

            var data = await _Entities
                .TbSchoolModuleDetails
                .Include(x => x.SchoolSubModule)
                .Include(x => x.Main)
                .Include(x => x.SchoolModule)
                .Where(x => x.IsActive)
                .OrderBy(x =>
                    x.SchoolModule
                        .SchoolName)
                .ThenBy(x =>
                    x.Main
                        .MainModule)
                .ThenBy(x =>
                    x.SchoolSubModule
                        .SchoolSubModule)
                .ToListAsync();

            foreach (var item in data)
            {
                count++;

                var one =
                    new ModelList
                    {
                        Id =
                            item.Main
                                .Id,

                        Schoolname =
                            item.SchoolModule
                                .SchoolName,

                        Main =
                            item.Main
                                .MainModule,

                        Sub =
                            item.SchoolSubModule
                                .SchoolSubModule,

                        SlNo =
                            count,

                        SubId =
                            item.Id
                    };

                model.list.Add(one);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModule(
    long id)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                using var transaction =
                    await _Entities.Database
                        .BeginTransactionAsync();

                var module = await _Entities
                    .TbSchoolModuleDetails
                    .FirstOrDefaultAsync(x =>
                        x.Id == id);

                if (module == null)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "Module not found"
                    });
                }

                module.IsActive = false;

                await _Entities
                    .SaveChangesAsync();

                // Remove from user module table
                var subUserModule = await _Entities
                    .TbUserModuleDetails
                    .Include(x => x.UserModule)
                    .FirstOrDefaultAsync(x =>
                        x.UserModule
                            .SchoolId == module.SchoolId

                        && x.SubModuleId ==
                            module.SchoolSubModuleId);

                if (subUserModule != null)
                {
                    subUserModule.IsActive = false;

                    await _Entities
                        .SaveChangesAsync();
                }

                // Check remaining active modules
                var mainData = await _Entities
                    .TbSchoolModuleDetails
                    .FirstOrDefaultAsync(x =>
                        x.SchoolModuleId ==
                            module.SchoolModuleId

                        && x.IsActive);

                if (mainData == null)
                {
                    var main = await _Entities
                        .TbSchoolModuleMains
                        .FirstOrDefaultAsync(x =>
                            x.Id ==
                                module.SchoolModuleId);

                    if (main != null)
                    {
                        main.IsActive = false;

                        await _Entities
                            .SaveChangesAsync();
                    }
                }

                await transaction
                    .CommitAsync();

                status = true;

                msg = "Successful";
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
        public IActionResult SchoolTypeDataEdit()
        {
            SchoolModuleModel model = new SchoolModuleModel();
            // model.SchoolId = _user.SchoolId;
            model.mainList = new List<SchoolMainModuleList>();
            return View(model);
        }

        public async Task<IActionResult> EditListOfSchoolType(long id)
        {
            long schoolId = id;

            var userData = await _Entities
                .TbSchoolModuleDetails
                .Include(x => x.Main)
                .Where(x =>
                    x.SchoolId == schoolId)
                .ToListAsync();

            var model =
                new SchoolModuleModel
                {
                    SchoolId =
                        schoolId,

                    mainList =
                        new List<SchoolMainModuleList>()
                };

            var list = await _Entities
                .TbSchoolSubModules
                .Include(x => x.Main)
                .Where(x => x.IsActive)
                .ToListAsync();

            var mainList = list
                .Select(x =>
                    x.Main
                        .MainModule)
                .Distinct()
                .ToList();

            foreach (var item in mainList)
            {
                string subIdString = "";

                string main =
                    Convert.ToString(item);

                var sub = list
                    .Where(x =>
                        x.Main
                            .MainModule == main)
                    .ToList();

                var userMainExists = userData
                    .Where(x =>
                        x.Main
                            .MainModule == main
                        && x.IsActive)
                    .ToList();

                var one =
                    new SchoolMainModuleList
                    {
                        Id =
                            sub.FirstOrDefault()?.Id ?? 0,

                        ModuleName =
                            main,

                        IsExistsMain =
                            userMainExists.Count
                            == sub.Count,

                        subList =
                            new List<SchoolSubModuleList>()
                    };

                foreach (var a in sub)
                {
                    var b =
                        new SchoolSubModuleList
                        {
                            MainId =
                                one.Id,

                            Id =
                                a.Id,

                            SubMosule =
                                a.SchoolSubModule
                        };

                    var userSubExists = userData
                        .FirstOrDefault(x =>
                            x.SchoolSubModuleId
                                == a.Id
                            && x.IsActive);

                    b.IsExists =
                        userSubExists != null;

                    one.subList.Add(b);

                    if (string.IsNullOrEmpty(
                        subIdString))
                    {
                        subIdString =
                            a.Id.ToString();
                    }
                    else
                    {
                        subIdString =
                            subIdString
                            + ","
                            + a.Id;
                    }
                }

                one.subIdListString =
                    subIdString;

                model.mainList.Add(one);
            }

            return PartialView(
                "~/Views/Module/_pv_Edit_SchoolModule.cshtml",
                model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitEditSchoolModule(SchoolModuleModel model)
        {
            bool status = false;

            string msg = "Failed";

            try
            {
                using var transaction =
                    await _Entities.Database
                        .BeginTransactionAsync();

                var schoolModuleMain =
                    await _Entities
                        .TbSchoolModuleMains
                        .FirstOrDefaultAsync(x =>
                            x.SchoolId ==
                                model.SchoolId);

                if (schoolModuleMain == null)
                {
                    return Json(new
                    {
                        status = false,
                        msg = "School module not found"
                    });
                }

                var schoolSubModules =
                    await _Entities
                        .TbSchoolModuleDetails
                        .Where(x =>
                            x.SchoolModuleId ==
                                schoolModuleMain.Id)
                        .ToListAsync();

                // Remove old modules
                if (schoolSubModules.Any())
                {
                    foreach (var item
                        in schoolSubModules)
                    {
                        item.IsActive = false;
                    }

                    await _Entities
                        .SaveChangesAsync();
                }

                // Add new modules
                if (model.subListOnly != null
                    && model.subListOnly.Any())
                {
                    foreach (var item
                        in model.subListOnly)
                    {
                        var mainDetails =
                            await _Entities
                                .TbSchoolSubModules
                                .FirstOrDefaultAsync(x =>
                                    x.Id == item.Id);

                        if (mainDetails == null)
                        {
                            continue;
                        }

                        var sub =
                            new TbSchoolModuleDetail
                            {
                                SchoolModuleId =
                                    schoolModuleMain.Id,

                                MainId =
                                    mainDetails.MainId,

                                SchoolSubModuleId =
                                    item.Id,

                                SchoolId =
                                    model.SchoolId,

                                IsActive = true,

                                TimeStamp =
                                    CurrentTime
                            };

                        await _Entities
                            .TbSchoolModuleDetails
                            .AddAsync(sub);
                    }

                    status =
                        await _Entities
                            .SaveChangesAsync() > 0;
                }

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
        #endregion


    }
}