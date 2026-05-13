using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrackTap.Models;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class PreLoginController : Controller
    {
        protected readonly SchoolDbContext _context;

        protected readonly SchoolRepository _schoolRepository;

        protected readonly ParentRepository _parentRepository;

        public DateTime CurrentTime =>
            TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById(
                    "India Standard Time"));

        public TbLogin _user = new TbLogin();

        public TbParent _parentUser = new TbParent();

        public PreLoginController(
            SchoolDbContext context,
            SchoolRepository schoolRepository,
            ParentRepository parentRepository)
        {
            _context = context;

            _schoolRepository = schoolRepository;

            _parentRepository = parentRepository;
        }

        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (User.Identity != null &&
                User.Identity.IsAuthenticated)
            {
                var userType =
                    Request.Cookies["UserType"];

                if (!string.IsNullOrEmpty(userType))
                {
                    long parsedUserType =
                        Convert.ToInt64(userType);

                    if (parsedUserType ==
                        (int)UserRole.School)
                    {
                        context.Result =
                            Redirect("/School/Home");
                    }
                    else if (parsedUserType ==
                        (int)UserRole.Staff)
                    {
                        context.Result =
                            Redirect("/School/Home");
                    }
                    else if (parsedUserType ==
                        (int)UserRole.Teacher)
                    {
                        context.Result =
                            Redirect("/School/Home");
                    }
                    else if (parsedUserType ==
                        (int)UserRole.Parent)
                    {
                        context.Result =
                            Redirect("/Parent/Home");
                    }

                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}