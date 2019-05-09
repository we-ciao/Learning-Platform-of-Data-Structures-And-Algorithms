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
    public class CategoryController : Controller
    {
        private readonly IProblemAppService _problemAppService;
        private readonly IProblemCategoryAppService _problemCategoryAppService;

        public CategoryController(IProblemAppService problemAppService, IProblemCategoryAppService problemCategoryAppService)
        {
            _problemAppService = problemAppService;
            _problemCategoryAppService = problemCategoryAppService;
        }


        /// <summary>
        /// 题目分类管理页面
        /// </summary>
        public ActionResult List()
        {
            List<Category> list = _problemCategoryAppService.GetAllList();

            return View(list);
        }

        /// <summary>
        /// 题目分类添加页面
        /// </summary>
        /// <returns>操作后的结果</returns>
        public ActionResult Add(int? id)
        {
            Category entity = new Category()
            {
                Order = 255
            };
            if (id != null)
                entity = _problemCategoryAppService.Find((int)id);
            return View(entity);
        }

        /// <summary>
        /// 题目分类添加
        /// </summary>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category model)
        {

            if (ModelState.IsValid)
            {
                string result = _problemCategoryAppService.InsertOrUpdateProblem(model);

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
        /// 题目分类编辑页面
        /// </summary>
        /// <param name="id">题目分类ID</param>
        /// <returns>操作后的结果</returns>
        public IActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 题目分类删除
        /// </summary>
        /// <param name="id">题目分类ID</param>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //TODO： 分类外键被删除
            var entity = _problemCategoryAppService.Find(id);
            _problemCategoryAppService.Delete(entity);
            return Json(_problemCategoryAppService.Save()); ;
        }

        /// <summary>
        /// 题目分类编辑页面
        /// </summary>
        /// <param name="id">题目分类ID</param>
        /// <returns>操作后的结果</returns>
        public ActionResult CategorySet(Int32 id = -1)
        {
            ViewBag.ProblemID = (id >= 0 ? id.ToString() : "");
            ViewBag.CategoryList = _problemCategoryAppService.GetAllList();
            return View();
        }

        ///// <summary>
        ///// 题目分类设置
        ///// </summary>
        ///// <param name="id">题目ID</param>
        ///// <param name="form">Form集合</param>
        ///// <returns>操作后的结果</returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CategorySet(Int32 id, FormCollection form)
        //{
        //    return ResultToMessagePage(ProblemCategoryItemManager.AdminUpdateProblemCategoryItems, id, form["source"], form["target"], "Your have updated problem type successfully!");
        //}

    }
}