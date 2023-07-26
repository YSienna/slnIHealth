using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area(areaName:"Admin")]
    public class AdminController : Controller
    {
        public IActionResult AdminHome()
        {
            return View();
        }
        public IActionResult PowerBI()
        {
            return View();
        }
    }
}
