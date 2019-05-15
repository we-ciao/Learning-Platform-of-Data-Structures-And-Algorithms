using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Learning_Platform_of_DSAA.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Policy = "Administrator")]
    public class GroupController : Controller
    {
        private readonly IGroupAppService _groupAppService;

        public GroupController(IGroupAppService groupAppService)
        {
            _groupAppService = groupAppService;
        }


        public IActionResult Index()
        {
            List<Group> list = _groupAppService.GetAllList();

            return View(list);
        }


        /// <summary>
        /// 分组添加页面
        /// </summary>
        /// <returns>操作后的结果</returns>
        public ActionResult Add(int? id)
        {
            Group entity = new Group();
            if (id != null)
                entity = _groupAppService.Find((int)id);
            return View(entity);
        }

        /// <summary>
        /// 分组添加
        /// </summary>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Group model)
        {

            if (ModelState.IsValid)
            {
                string result = _groupAppService.InsertOrUpdateProblem(model);

                if (result == null)
                {
                    ViewBag.SweetInfo = "操作失败";
                    return View(model);
                }

                ViewBag.SweetInfo = result + "成功！";
                return View(model);
            }

            ViewBag.SweetInfo = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors).FirstOrDefault().ErrorMessage;
            return View(model);
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
            var entity = _groupAppService.Find(id);
            _groupAppService.Delete(entity);
            return Json(_groupAppService.Save()); ;
        }
    }
}