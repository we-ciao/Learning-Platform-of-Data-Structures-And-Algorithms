using DSAA.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Controllers
{
    public class StatusController : Controller
    {
        private readonly ISolutionAppService _solutionAppService;

        public StatusController(ISolutionAppService solutionAppService)
        {
            _solutionAppService = solutionAppService;
        }


        public IActionResult Index()
        {

            return View(_solutionAppService.GetAllList());
        }


        /// <summary>
        /// 提交状态列表页面
        /// </summary>
        /// <param name="id">页面索引</param>
        /// <param name="pid">题目ID</param>
        /// <param name="name">用户名</param>
        /// <param name="lang">提交语言</param>
        /// <param name="type">评测结果</param>
        /// <returns>操作后的结果</returns>
        //public ActionResult List(Int32 id = 1, Int32 pid = -1, String name = "", String lang = "", String type = "")
        //{
        //    Contest contest = ViewData["Contest"] as Contest;
        //    Dictionary<String, Byte> langs = LanguageManager.GetSupportLanguages(contest.SupportLanguage);
        //    ViewBag.Languages = langs;

        //    PagedList<SolutionEntity> list = SolutionManager.GetSolutionList(id, contest.ContestID, pid, name, lang, type, null);

        //    ViewBag.ProblemID = pid;
        //    ViewBag.UserName = name;
        //    ViewBag.Language = lang;
        //    ViewBag.SearchType = type;

        //    return ViewWithPager(list, id);
        //}


    }
}