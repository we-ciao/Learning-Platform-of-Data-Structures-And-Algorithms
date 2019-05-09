using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        ///// <summary>
        ///// 题目分类添加
        ///// </summary>
        ///// <param name="form">Form集合</param>
        ///// <returns>操作后的结果</returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CategoryAdd(Category entity)
        //{

        //    return ResultToMessagePage(ProblemCategoryManager.AdminInsertProblemCategory, entity, "Your have added problem category successfully!");
        //}

        ///// <summary>
        ///// 题目分类编辑页面
        ///// </summary>
        ///// <param name="id">题目分类ID</param>
        ///// <returns>操作后的结果</returns>
        //public ActionResult CategoryEdit(Int32 id = -1)
        //{
        //    return ResultToView("CategoryEdit", ProblemCategoryManager.AdminGetProblemCategory, id);
        //}

        ///// <summary>
        ///// 题目分类编辑
        ///// </summary>
        ///// <param name="id">题目分类ID</param>
        ///// <param name="form">Form集合</param>
        ///// <returns>操作后的结果</returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CategoryEdit(Int32 id, FormCollection form)
        //{
        //    ProblemCategoryEntity entity = new ProblemCategoryEntity()
        //    {
        //        TypeID = id,
        //        Title = form["title"],
        //        Order = form["order"].ToInt32(0)
        //    };

        //    return ResultToMessagePage(ProblemCategoryManager.AdminUpdateProblemCategory, entity, "Your have edited problem category successfully!");
        //}

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

        ///// <summary>
        ///// 题目分类编辑页面
        ///// </summary>
        ///// <param name="id">题目分类ID</param>
        ///// <returns>操作后的结果</returns>
        //public ActionResult CategorySet(Int32 id = -1)
        //{
        //    ViewBag.ProblemID = (id >= 0 ? id.ToString() : "");

        //    return ResultToView(ProblemCategoryItemManager.AdminGetProblemCategoryItemList, id);
        //}

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