using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Controllers
{
    public class LoginController : Controller
    {
        private IUserAppService _userAppService;
        public LoginController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = _userAppService.CheckUser("WebAPI", "123");
            return View();
        }
    }
}