using Microsoft.AspNetCore.Mvc;
using TrackTap.Models;
using System.Diagnostics;

namespace TrackTap.Controllers
{
    public class HomeController : Controller
    {
        public IIActionResult Index()
        {
            return View();
        }

        public IIActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IIActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
