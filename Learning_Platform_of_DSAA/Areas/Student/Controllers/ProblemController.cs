using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = "Student")]
    public class ProblemController : Controller
    {
        private readonly IProblemAppService _problemAppService;
        private readonly ICompilerAppService _compilerAppService;
        //private readonly Icontexxt _compilerAppService;
        private readonly ISolutionAppService _solutionAppService;


        public ProblemController(IProblemAppService problemAppService, ICompilerAppService compilerAppService, ISolutionAppService solutionAppService)
        {
            _problemAppService = problemAppService;
            _compilerAppService = compilerAppService;
            _solutionAppService = solutionAppService;
        }


        /// <summary>
        /// 题目列表页面
        /// </summary>
        public IActionResult List()
        {
            var list = _problemAppService.GetAllList();
            return View(list);
        }

        /// <summary>
        /// 题目显示
        /// </summary>
        /// <param name="ids">题目ID</param>
        public ActionResult Show(int id)
        {
            //ProblemManager.AdminUpdateProblemIsHide
            var model = _problemAppService.Get(id);
            return View(model);
        }

        /// <summary>
        /// 代码提交页面
        /// </summary>
        /// <param name="id">提交ID</param>
        /// <returns>操作后的结果</returns>
        public ActionResult Submit(int id = -1, int Contest = -1)
        {

            ViewBag.ProblemID = id;

            return View();
        }


        /// <summary>
        /// 代码提交页面
        /// </summary>
        /// <param name="id">提交ID</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        public ActionResult Submit(int id , int compilerid, string code, int contest = -1)
        {
            ViewBag.ProblemID = id;

            Solution entity = new Solution()
            {
                SourceCode = code,
                LanguageType = _compilerAppService.Get(compilerid),
                Problem = _problemAppService.Get(id)
            };
            //  if(contest != -1)
            //       entity.Contest=_con

            if (entity.LanguageType == null)
            {

                ViewBag.SweetInfo = "This problem does not support this programming language.";
                return View();
            }

            string msg = _solutionAppService.InsertSolution(entity);
            if (msg != null)
            {
                ViewBag.SweetInfo = msg;
                return View();
            }

            return RedirectToAction("Index", "Status");
        }



        public IActionResult Category()
        {
            return View();
        }
    }
}