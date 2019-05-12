using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        /// <summary>
        /// 题目管理页面
        /// </summary>
        /// <param name="id">页面索引</param>
        /// <returns>操作后的结果</returns>
        public IActionResult List(Int32 id = 1)
        {
            return View(_problemAppService.GetAllList());
        }


        /// <summary>
        /// 题目添加
        /// </summary>
        public IActionResult Add(int? id)
        {
            Problem entity = new Problem()
            {
                TimeLimit = 1000,
                MemoryLimit = 32768
            };
            if (id != null)
                entity = _problemAppService.Find((int)id);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Problem model)
        {

            if (ModelState.IsValid)
            {
                string result = _problemAppService.InsertOrUpdateProblem(model);

                if (result == null)
                {
                    ViewBag.SweetInfo = "操作失败";
                    return View();
                }

                ViewBag.SweetInfo = result + "成功！";
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
        //[DisableRequestSizeLimit] //禁用http限制大小
        [RequestSizeLimit(100 * 1024 * 1024)] //限制http大小
        public ActionResult Import(IFormFile files)
        {
            if (files == null)
            {
                ViewBag.SweetInfo = "文件未上传!";
                return View();
            }
            var filePath = Path.GetTempFileName();


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                files.CopyTo(stream);
            }


            StreamReader sr = new StreamReader(filePath);
            //    content = sr.ReadToEnd();
            //}

            Dictionary<Int32, Boolean> result = _problemAppService.AdminImportProblem(sr.ReadToEnd());

            if (result == null || result.Count == 0)
            {
                ViewBag.SweetInfo = "添加失败";
                return View();
            }


            String successInfo = String.Format("{0} 个题目已经被成功导入!", result.Count.ToString());

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
                successInfo += String.Format("<br/>{0} 个问题 ({1}) 没有数据或者导入失败!", nodataCount.ToString(), nodataItems.ToString());
            }

            ViewBag.SweetInfo = successInfo;
            return View();

        }

        /// <summary>
        /// 题目数据创建页面
        /// </summary>
        /// <param name="id">题目ID</param>
        /// <returns>操作后的结果</returns>
        public ActionResult DataCreate(String id)
        {
            ViewBag.ProblemID = id;

            return View();
        }

        /// <summary>
        /// 题目数据创建
        /// </summary>
        /// <param name="id">题目ID</param>
        /// <param name="form">Form集合</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DataCreate(Int32 id, FormCollection form)
        {
            return View();
            //return _problemAppService.ProblemDataManager.AdminSaveProblemData, id, form, Request.Files, "Your have created problem data successfully!");
        }

        /// <summary>
        /// 题目数据导入页面
        /// </summary>
        /// <param name="id">题目ID</param>
        /// <returns>操作后的结果</returns>
        public ActionResult DataUpload(String id)
        {
            ViewBag.ProblemID = id;

            return View();
        }

        /// <summary>
        /// 题目数据导入
        /// </summary>
        /// <param name="id">题目ID</param>
        /// <param name="file">上传文件</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DataUpload(Int32 id, IFormFile files)
        {
            ViewBag.ProblemID = id;
            ViewBag.SweetInfo = _problemAppService.AdminSaveProblemData(id, files);
            return View();
            //return ResultToMessagePage(ProblemDataManager.AdminSaveProblemData, id, file, "Your have uploaded problem data successfully!");
        }


        /// <summary>
        /// 题目数据下载
        /// </summary>
        /// <param name="id">题目ID</param>
        /// <returns>操作后的结果</returns>
        [HttpGet]
        public ActionResult DataDownload(Int32 id = -1)
        {
            try
            {
                var addrUrl = _problemAppService.AdminGetProblemDataDownloadPath(id);
                FileStream fs = new FileStream(addrUrl, FileMode.Open);
                return File(fs, "application/vnd.android.package-archive", id + ".zip");
            }
            catch
            {
                return File(new byte[1], "application/vnd.android.package-archive", id + "DataNotFound");
            }
        }

        /// <summary>
        /// 题目数据删除
        /// </summary>
        /// <param name="id">题目ID</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        public ActionResult DeleteData(int id)
        {
            return Json(_problemAppService.AdminDeleteProblemDataRealPath(id)); ;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _problemAppService.AdminDeleteProblemDataRealPath(id);

            Problem entity = _problemAppService.Find(id);
            _problemAppService.Delete(entity);

            return Json(_problemAppService.Save()); ;
        }

        public IActionResult Categorylist()
        {
            return View();
        }



    }
}