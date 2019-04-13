using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Platform_of_DSAA.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Policy = "Teacher")]
    public class ProblemController : Controller
    {
        private readonly IProblemAppService _problemAppService;

        public ProblemController(IProblemAppService problemAppService)
        {
            _problemAppService = problemAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 题目管理页面
        /// </summary>
        /// <param name="id">页面索引</param>
        /// <returns>操作后的结果</returns>
        public async Task<IActionResult> List(Int32 id = 1)
        {
            //await _context.Problem.ToListAsync()
            return View( );
        }


        /// <summary>
        /// 题目添加
        /// </summary>
        public IActionResult Add()
        {
            Problem entity = new Problem()
            {
                TimeLimit = 1000,
                MemoryLimit = 32768
            };
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Problem model)
        {

            if (ModelState.IsValid)
            {
                bool result = _problemAppService.InsertOrUpdateProblem(model);

                if (!result)
                {
                    ViewBag.SweetInfo = "添加失败";
                    return View();
                }

                ViewBag.SweetInfo = "添加成功！";
                return View(model);
            }

            ViewBag.SweetInfo = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors).FirstOrDefault().ErrorMessage;
            return View(model);
        }


        public IActionResult Edit()
        {
            return View();
        }


        /// <summary>
        /// 题目导入页面
        /// </summary>
        /// <returns>操作后的结果</returns>
        public ActionResult Import()
        {
            return View();
        }


        /// <summary>
        /// 题目导入
        /// </summary>
        /// <param name="form">Form集合</param>
        /// <param name="file">上传的文件</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(List<IFormFile> files)
        {
            //todo
            //if (String.Equals("1", uploadType))//从文件上传
            //{
            //    if (file == null)
            //    {
            //        return MethodResult.FailedAndLog("No file was uploaded!");
            //    }

            //    StreamReader sr = new StreamReader(file.InputStream);

            //    content = sr.ReadToEnd();
            //}

            Dictionary<Int32, Boolean> result = _problemAppService.AdminImportProblem(files.ToString());

            if (result == null || result.Count == 0)
            {
                ViewBag.SweetInfo = "添加失败";
                return View();
            }


            String successInfo = String.Format("{0} problem(s) have benn successfully imported!", result.Count.ToString());

            StringBuilder nodataItems = new StringBuilder();
            Int32 nodataCount = 0;

            foreach (KeyValuePair<Int32, Boolean> pair in result)
            {
                if (!pair.Value)
                {
                    if (nodataCount > 0)
                    {
                        nodataItems.Append(',');
                    }

                    nodataItems.Append(pair.Key.ToString());
                    nodataCount++;
                }
            }

            if (nodataCount > 0)
            {
                successInfo += String.Format("<br/>{0} problem(s) ({1}) have no data or fail to import these data!", nodataCount.ToString(), nodataItems.ToString());
            }

            ViewBag.SweetInfo = successInfo;
            return View();

        }


        public IActionResult List()
        {
            return View();
        }

        public IActionResult Categorylist()
        {
            return View();
        }



    }
}