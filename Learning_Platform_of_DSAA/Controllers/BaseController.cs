using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace Learning_Platform_of_DSAA.Controllers
{
    public abstract class BaseController : Controller
    {

        #region 字段
        private Stopwatch _stopWatch;
        #endregion

        #region 构造方法
        protected BaseController()
        {
            this._stopWatch = new Stopwatch();
            this._stopWatch.Start();
        }

        ~BaseController()
        {
            if (this._stopWatch.IsRunning)
            {
                this._stopWatch.Stop();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取当前用户IP
        /// </summary>
        /// <returns>当前用户IP地址</returns>
        //protected String GetCurrentUserIP()
        //{
        //    return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        //}

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    byte[] result;
        //    filterContext.HttpContext.Session.TryGetValue("CurrentUser", out result);
        //    if (result == null)
        //    {
        //        string path = filterContext.HttpContext.Request.Path;
        //        filterContext.Result = new RedirectResult($"/Login/Index?ReturnUrl={path}");
        //        return;
        //    }
        //    base.OnActionExecuting(filterContext);
        //}


        protected string ModelStateErrors()
        {
            string err = "";
            foreach (var item in ModelState.Values)
            {
                foreach (var item2 in item.Errors)
                {
                    if (!string.IsNullOrWhiteSpace(item2.ErrorMessage))
                    {
                        err += item2.ErrorMessage;
                    }
                }
            }
            return err;
        }

        #endregion
    }
}