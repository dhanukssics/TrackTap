using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class AdminBaseController : Controller
    {
        protected readonly SchoolDbContext _context;

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
            SchoolDbContext context,
            SchoolRepository schoolRepository,
            ParentRepository parentRepository,
            TeacherRepository teacherRepository)
        {
            _context = context;

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
                var routeValues =
                    RouteData.Values;
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