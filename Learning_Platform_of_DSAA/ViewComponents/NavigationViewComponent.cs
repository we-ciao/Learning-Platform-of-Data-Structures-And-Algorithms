using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Learning_Platform_of_DSAA.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        //用户管理仓储接口
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISolutionAppService _solutionAppService;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public NavigationViewComponent(IHttpContextAccessor httpContextAccessor, ISolutionAppService solutionAppService)
        {
            _httpContextAccessor = httpContextAccessor;
            _solutionAppService = solutionAppService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            byte[] bytes;
            //get the session
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("UserRole", out bytes))
            {
                ViewBag.UserRole = System.Text.Encoding.UTF8.GetString(bytes);
                _httpContextAccessor.HttpContext.Session.TryGetValue("UserId", out bytes);
                ViewBag.StatueCount = _solutionAppService.GetStatueCount(Convert.ToInt32(System.Text.Encoding.UTF8.GetString(bytes)));
            }

            return View();
        }
    }
}
