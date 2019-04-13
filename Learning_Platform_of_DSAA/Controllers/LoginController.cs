using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Learning_Platform_of_DSAA.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserAppService _userAppService;

        public LoginController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }


        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            //移除当前登录人信息
            Logout();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            //检查用户信息
            var user = await _userAppService.CheckUser(model.UserName, model.PassWord);
            if (user == null)
            {
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
            }

            string role = GetRole(user);

            //根据用户角色创建claim声明
            List<Claim> claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, role)
                    };

            var userIdentity = new ClaimsIdentity(role);
            userIdentity.AddClaims(claim);

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = false,
                AllowRefresh = false
            });


            //设置当前用户信息
            _userAppService.SetCurrentUser(user);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToRoute(new
            {
                area = role,
                controller = "Home",
                action = "Index"
            });
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                string result = _userAppService.SignUp(model);

                if (result != null)
                {
                    ViewBag.ErrorInfo = result;
                    return View();
                }

                return View("Index");
            }

            ViewBag.ErrorInfo = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors).FirstOrDefault().ErrorMessage;
            return View(model);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns>操作后的结果</returns>
        public ActionResult Logout()
        {
            //清除Session
            HttpContext.Session.Clear();
            AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }


        #region Method

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static string GetRole(User user)
        {
            if (user.Group == null)
                return "Student";
            string role = string.Empty;
            switch (user.Group.Permission)
            {
                case PermissionType.Administrator:
                    role = "Administrator";
                    break;
                case PermissionType.Teacher:
                    role = "Teacher";
                    break;
                default:
                    role = "Student";
                    break;
            }
            return role;
        }


        #endregion
    }
}