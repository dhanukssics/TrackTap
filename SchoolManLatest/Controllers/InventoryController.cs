using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace TrackTap.Controllers
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public IActionResult Stationary_Add_Inventory()
        {
            return View();
        }

        public IActionResult Create_Tender()
        {
            return View();
        }
        public IActionResult Stock_ItemsList()
        {
            return View();
        }

        public IActionResult Stock_Stationary()
        {
            return View();
        }

        public IActionResult Stock_Uniform()
        {
            return View();
        }

        public IActionResult Stock_Books()
        {
            return View();
        }


        public IActionResult Uniform()
        {
            return View();
        }

        public IActionResult Stationary()
        {
            return View();
        }

        public IActionResult Books()
        {
            return View();
        }

        public IActionResult Create_Bill()
        {
            return View();
        }

    }
}