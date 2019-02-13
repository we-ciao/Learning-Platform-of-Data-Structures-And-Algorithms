using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Controllers
{
    public class BaseController : Controller
    {

        #region 字段
        private Stopwatch _stopWatch;
        private IHttpContextAccessor _accessor;
        #endregion

        #region 构造方法
        protected BaseController(IHttpContextAccessor accessor)
        {
            this._stopWatch = new Stopwatch();
            this._stopWatch.Start();
            _accessor = accessor;
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
        protected String GetCurrentUserIP()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        
       

        #endregion
    }
}