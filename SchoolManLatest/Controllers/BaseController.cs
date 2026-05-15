using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class BaseController : Controller
    {
        protected readonly SchoolDbContext _Entities;

        protected readonly SchoolRepository _schoolRepository;

        protected readonly ParentRepository _parentRepository;

        protected readonly TeacherRepository _teacherRepository;

        public DateTime CurrentTime =>
            TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById(
                    "India Standard Time"));

        public TbSchool _schoolUser =
            new TbSchool();

        public TbParent _parentUser =
            new TbParent();

        public TbLogin _user =
            new TbLogin();

        public BaseController(
            SchoolDbContext Entities,
            SchoolRepository schoolRepository,
            ParentRepository parentRepository,
            TeacherRepository teacherRepository)
        {
            _Entities = Entities;

            _schoolRepository =
                schoolRepository;

            _parentRepository =
                parentRepository;

            _teacherRepository =
                teacherRepository;
        }

        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (User.Identity != null &&
                User.Identity.IsAuthenticated)
            {
                var userTypeCookie =
                    Request.Cookies["UserType"];

                if (!string.IsNullOrEmpty(userTypeCookie))
                {
                    long userType =
                        Convert.ToInt64(userTypeCookie);

                    if (userType ==
                        (int)UserRole.School
                        || userType ==
                        (int)UserRole.Staff
                        || userType ==
                        (int)UserRole.Teacher)
                    {
                        var userId =
                            long.Parse(
                                User.Identity.Name);

                        var userSession =
                            HttpContext.Session
                            .GetString("User");

                        if (string.IsNullOrEmpty(
                            userSession))
                        {
                            var user =
                                _Entities.TbLogins
                                .FirstOrDefault(x =>
                                    x.UserId ==
                                    userId);

                            if (user != null)
                            {
                                HttpContext.Session
                                    .SetString(
                                        "User",
                                        JsonSerializer
                                        .Serialize(user));

                                HttpContext.Session
                                    .SetString(
                                        "UserType",
                                        userType.ToString());

                                _user = user;
                            }
                        }
                        else
                        {
                            _user =
                                JsonSerializer
                                .Deserialize<TbLogin>(
                                    userSession);
                        }

                        var routeValues =
                            RouteData.Values;
                    }
                    else if (userType ==
                        (int)UserRole.Parent)
                    {
                        var parentId =
                            long.Parse(
                                User.Identity.Name);

                        var parentSession =
                            HttpContext.Session
                            .GetString("Parent");

                        if (string.IsNullOrEmpty(
                            parentSession))
                        {
                            var parent =
                                _Entities.TbParents
                                .FirstOrDefault(x =>
                                    x.ParentId ==
                                    parentId);

                            if (parent != null)
                            {
                                HttpContext.Session
                                    .SetString(
                                        "Parent",
                                        JsonSerializer
                                        .Serialize(parent));

                                HttpContext.Session
                                    .SetString(
                                        "UserType",
                                        userType.ToString());

                                _parentUser =
                                    parent;
                            }
                        }
                        else
                        {
                            _parentUser =
                                JsonSerializer
                                .Deserialize<TbParent>(
                                    parentSession);
                        }

                        var routeValues =
                            RouteData.Values;
                    }
                }
                else
                {
                    context.Result =
                        Redirect("/Account/Home");

                    return;
                }
            }
            else
            {
                context.Result =
                    Redirect("/Account/Home");

                return;
            }

            base.OnActionExecuting(context);
        }
    }
}