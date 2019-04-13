using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Learning_Platform_of_DSAA.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        //用户管理仓储接口
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public NavigationViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            byte[] bytes;
            //get the session
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("UserRole", out bytes))
            {
                ViewBag.UserRole = System.Text.Encoding.UTF8.GetString(bytes);
            }

            return  View();
        }
    }
}
