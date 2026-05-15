using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class AdminBaseController : Controller
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

        public AdminBaseController(
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
            ActionExecutingContext Entities)
        {
            if (User.Identity != null &&
                User.Identity.IsAuthenticated)
            {
                var routeValues =
                    RouteData.Values;
            }
            else
            {
                Entities.Result =
                    Redirect("/Account/Home");

                return;
            }

            base.OnActionExecuting(Entities);
        }
    }
}