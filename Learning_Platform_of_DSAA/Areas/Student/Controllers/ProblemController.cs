using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Areas.Student.Controllers
{
    public class ProblemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}