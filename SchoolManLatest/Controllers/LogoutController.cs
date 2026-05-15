using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace TrackTap.Controllers
{
    public class LogoutController : Controller
    {
        public async Task<IActionResult> SchoolLogout()
        {
            HttpContext.Session.Clear();

            await HttpContext
                .SignOutAsync();

            return RedirectToAction(
                "SchoolLogin",
                "Account");
        }

        public async Task<IActionResult> ParentLogout()
        {
            HttpContext.Session.Clear();

            await HttpContext
                .SignOutAsync();

            return RedirectToAction(
                "ParentLogin",
                "Account");
        }

        public async Task<IActionResult> StaffLogout()
        {
            HttpContext.Session.Clear();

            await HttpContext
                .SignOutAsync();

            return RedirectToAction(
                "StaffLogin",
                "Account");
        }

        public async Task<IActionResult> AdminLogout()
        {
            HttpContext.Session.Clear();

            await HttpContext
                .SignOutAsync();

            return RedirectToAction(
                "SuperAdminLogin",
                "Account");
        }
    }
}