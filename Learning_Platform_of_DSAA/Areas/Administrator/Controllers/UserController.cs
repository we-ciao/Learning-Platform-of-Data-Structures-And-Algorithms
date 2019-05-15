using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using DSAA.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Learning_Platform_of_DSAA.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Policy = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly IGroupAppService _groupAppService;

        public UserController(IUserAppService userAppService, IGroupAppService groupAppService)
        {
            _userAppService = userAppService;
            _groupAppService = groupAppService;
        }

        public IActionResult Index()
        {
            List<User> list = _userAppService.GetAllList();

            return View(list);
        }


        /// <summary>
        /// 题目分类添加页面
        /// </summary>
        /// <returns>操作后的结果</returns>
        public ActionResult Add(int? id)
        {
            User entity = new User();
            if (id != null)
                entity = _userAppService.Find((int)id);

            ViewBag.groups = _groupAppService.GetAllList();
            return View(entity);
        }

        /// <summary>
        /// 题目分类添加
        /// </summary>
        /// <returns>操作后的结果</returns>
        [HttpPost]
        public IActionResult Add(User model, int? group)
        {
            ViewBag.groups = _groupAppService.GetAllList();

            if (group != null)
                model.Group = _groupAppService.Get((int)group);
            else
                model.Group = null;
            string result = _userAppService.InsertOrUpdateUser(model);

            if (result == null)
            {
                ViewBag.SweetInfo = "操作失败";
                return View(model);
            }

            ViewBag.SweetInfo = result + "成功！";
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
            var entity = _userAppService.Find(id);
            _userAppService.Delete(entity);
            return Json(_userAppService.Save()); ;
        }
    }
}