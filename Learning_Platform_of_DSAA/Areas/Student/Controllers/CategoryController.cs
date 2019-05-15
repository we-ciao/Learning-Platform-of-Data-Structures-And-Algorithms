using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Learning_Platform_of_DSAA.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Policy = "Student")]
    public class CategoryController : Controller
    {
        private readonly IProblemAppService _problemAppService;
        private readonly IProblemCategoryAppService _problemCategoryAppService;
        private readonly ISolutionAppService _solutionAppService;
        private readonly IUserAppService _userAppService;


        public CategoryController(IProblemAppService problemAppService, IProblemCategoryAppService problemCategoryAppService, ISolutionAppService solutionAppService, IUserAppService userAppService)
        {
            _problemAppService = problemAppService;
            _problemCategoryAppService = problemCategoryAppService;
            _solutionAppService = solutionAppService;
            _userAppService = userAppService;
        }



        /// <summary>
        /// 题目分类管理页面
        /// </summary>
        public ActionResult List()
        {
            var ls = _solutionAppService.GetAllList()
                .Where(x => x.User.Id == _userAppService.GetCurrentUser().Id && x.Result == ResultType.Accepted)
                .GroupBy(x => x.Problem.Id);


            List<Category> list = _problemCategoryAppService.GetAllList();
            int[] count = null;
            if (list.Count > 0)
            {
                count = new int[list.Max(x => x.Id) + 1];

                foreach (var item in ls)
                {
                    foreach (var pr in item.FirstOrDefault().Problem.Categorys)
                    {
                        count[pr.CategoryId]++;
                    }

                }
            }

            return View(new Tuple<List<Category>, int[]>(list, count));
        }

        /// <summary>
        /// 题目管理页面
        /// </summary>
        /// <param name="id">页面索引</param>
        /// <returns>操作后的结果</returns>
        public IActionResult ProblemList(Int32 id = -1)
        {
            var cate = _problemCategoryAppService.Get(id);
            var sou = _solutionAppService.GetAllList()
                .Where(x => x.User.Id == _userAppService.GetCurrentUser().Id && x.Problem.Categorys.Any(y => y.CategoryId == id));
            var ls = sou.GroupBy(x => x.Problem.Id);
            var ts = sou.Where(x => x.Result == ResultType.Accepted).GroupBy(x => x.Problem.Id);
            var prob = cate.Problems;
            int[] count = new int[prob.Max(x => x.ProblemId) + 1];
            int[] ACcount = new int[prob.Max(x => x.ProblemId) + 1];

            foreach (var item in ls)
            {
                count[item.Key] += item.Count();
            }
            foreach (var item in ts)
            {
                ACcount[item.Key] += item.Count();
            }


            return View(new Tuple<List<ProblemCategory>, int[], int[]>(prob, count, ACcount));
        }



    }
}