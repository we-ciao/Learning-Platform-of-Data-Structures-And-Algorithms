using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Learning_Platform_of_DSAA.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Policy = "Teacher")]
    public class HomeController : Controller
    {
        private readonly IProblemAppService _problemAppService;
        private readonly ILearnAppService _learnAppService;
        private readonly IProblemCategoryAppService _problemCategoryAppService;
        private readonly IUserAppService _userAppService;
        private readonly IGroupAppService _groupAppService;
        private readonly ISolutionAppService _solutionAppService;

        public HomeController(IProblemAppService problemAppService, ILearnAppService learnAppService, IProblemCategoryAppService problemCategoryAppService, IUserAppService userAppService, IGroupAppService groupAppService, ISolutionAppService solutionAppService)
        {
            _solutionAppService = solutionAppService;
            _userAppService = userAppService;
            _groupAppService = groupAppService;
            _problemCategoryAppService = problemCategoryAppService;
            _learnAppService = learnAppService;
            _problemAppService = problemAppService;
        }



        public IActionResult Index()
        {
            var people = _userAppService.GetAllList();
            ViewBag.peopleCount = people.Count();
            ViewBag.peopleStudentCount = people.Count(x=>x.Group==null || (x.Group != null && x.Group.Permission == PermissionType.Student));
            ViewBag.peopleTeacherCount = people.Count(x => x.Group!= null && x.Group.Permission==PermissionType.Teacher);
            ViewBag.peopleAdminCount = people.Count(x => x.Group != null && x.Group.Permission == PermissionType.Administrator);
            ViewBag.people24Count = people.Where(x => x.CreateDate >= DateTime.Now.AddDays(-1)).Count();
            ViewBag.peopleCountByMonth = people.GroupBy(x => x.CreateDate.Month);

            var submit = _solutionAppService.GetAllList();
            ViewBag.submitCount = submit.Count();
            ViewBag.submit24Count = submit.Where(x => x.SubmitTime >= DateTime.Now.AddDays(-1)).Count();
            ViewBag.submit1Count = submit.Where(x => x.SubmitTime >= DateTime.Now.AddHours(-1)).Count();
            ViewBag.submit30Count = submit.Where(x => x.SubmitTime >= DateTime.Now.AddMonths(-1)).Count();
            ViewBag.rightsubmitCount = submit.Where(x => x.Result == ResultType.Accepted).Count();
            ViewBag.errorsubmitCount = submit.Where(x => x.Result == ResultType.WrongAnswer).Count();
            ViewBag.pendingsubmitCount = submit.Where(x => x.Result == ResultType.Pending).Count();
            ViewBag.submitCountByMonth = submit.GroupBy(x => x.SubmitTime.Month);
            
            var problem = _problemAppService.GetAllList();
            ViewBag.problemCount = problem.Count();
            ViewBag.problem24Count = problem.Where(x => x.LastDate >= DateTime.Now.AddMonths(-1)).Count();

            var learn = _learnAppService.GetAllList();
            ViewBag.learnCount = learn.Count();
            ViewBag.learn24Count = learn.Where(x => x.SubmitTime >= DateTime.Now.AddDays(-1)).Count();


            return View();
        }
    }
}