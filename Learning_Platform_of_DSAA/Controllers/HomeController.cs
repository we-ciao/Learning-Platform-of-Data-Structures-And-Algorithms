using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Learning_Platform_of_DSAA.Models;
using DSAA.EntityFrameworkCore;

namespace Learning_Platform_of_DSAA.Controllers
{
    public class HomeController : Controller
    {
        private EntityDbContext _context;

        public HomeController(EntityDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            _context.Problem.Select(x => x.ID == 1);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
