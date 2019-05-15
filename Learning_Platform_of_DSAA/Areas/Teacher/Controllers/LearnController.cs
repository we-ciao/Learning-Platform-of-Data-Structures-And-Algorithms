using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Learning_Platform_of_DSAA.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Policy = "Teacher")]
    public class LearnController : Controller
    {
        private readonly ILearnAppService _learnAppService;

        public LearnController(ILearnAppService learnAppService)
        {
            _learnAppService = learnAppService;
        }



        /// 推荐内容管理页面
        /// </summary>
        public ActionResult List()
        {
            List<Learn> list = _learnAppService.GetAllList();

            return View(list);
        }

        /// <summary>
        /// 推荐内容添加页面
        /// </summary>
        /// <returns>操作后的结果</returns>
        public ActionResult Add(int? id)
        {
            Learn entity = new Learn();
            if (id != null)
                entity = _learnAppService.Find((int)id);
            return View(entity);
        }

        /// <summary>
        /// 推荐内容添加
        /// </summary>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Learn model)
        {

            if (ModelState.IsValid)
            {
                model.SubmitTime = DateTime.Now;
                string result = _learnAppService.InsertOrUpdateProblem(model);

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

        /// <summary>
        /// 推荐内容编辑页面
        /// </summary>
        /// <param name="id">推荐内容ID</param>
        /// <returns>操作后的结果</returns>
        public IActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 推荐内容删除
        /// </summary>
        /// <param name="id">推荐内容ID</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //TODO： 分类外键被删除
            var entity = _learnAppService.Find(id);
            _learnAppService.Delete(entity);
            return Json(_learnAppService.Save()); ;
        }

    }
}