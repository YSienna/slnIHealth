using Microsoft.AspNetCore.Mvc;
using prjiHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiHealth.Controllers
{
    public class BodyController : Controller
    {
        public IActionResult BodyCalculatorMain()
        {
            return View();
        }
        public IActionResult BMICalculator()
        {
            return View();
        }        
        public IActionResult getBMI(CBodyRecordViewModel body)
        {
            return Content(body.NumBMI.ToString(), "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult TDEECalculator()
        {
            return View();
        }       
        public IActionResult getTDEE(CBodyRecordViewModel body)
        {
            return Content(body.NumTDEE.ToString());
        }
    }
}
