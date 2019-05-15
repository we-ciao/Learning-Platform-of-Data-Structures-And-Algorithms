using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = "Student")]
    public class LearnController : Controller
    {
        private readonly ILearnAppService _learnAppService;

        public LearnController(ILearnAppService learnAppService)
        {
            _learnAppService = learnAppService;
        }

        /// <summary>
        /// 推荐内容页面
        /// </summary>
        public ActionResult List()
        {
            List<Learn> list = _learnAppService.GetAllList();
                        
            return View(list);
        }

        /// <summary>
        /// 推荐内容页面
        /// </summary>
        public ActionResult Show(int id)
        {
            Learn list = _learnAppService.Get(id);

            return View(list);
        }

    }
}