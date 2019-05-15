using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Learning_Platform_of_DSAA.Controllers
{
    public class RankController : Controller
    {
        private readonly IUserAppService _userAppService;

        public RankController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        /// <summary>
        /// 排行列表页面
        /// </summary>
        public IActionResult Index()
        {
            List<User> list = _userAppService.GetAllList();

            return View(list.Where(x => x.Group == null || (x.Group != null && x.Group.Permission == PermissionType.Student)));
        }
    }
}