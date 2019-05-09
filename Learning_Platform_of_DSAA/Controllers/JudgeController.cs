using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSAA.Service.IService;
using DSAA.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Platform_of_DSAA.Controllers
{
    [AllowAnonymous]
    public class JudgeController : Controller
    {
        private readonly IJudgeAppService _judgeAppService;

        public JudgeController(IJudgeAppService judgeAppService)
        {
            _judgeAppService = judgeAppService;
        }

        /// <summary>
        /// 评测机登录
        /// </summary>
        /// <returns>Json结果</returns>
        public ActionResult Login()
        {
            var coll = Request.Form;
            String username = coll["username"];
            String password = coll["password"];

            String error = String.Empty;

            if (_judgeAppService.TryJudgeServerLogin(username, password, out error))
            {
                return SuccessJson();
            }
            else
            {
                return ErrorJson(error);
            }
        }

        /// <summary>
        /// 评测机获取评测列表
        /// </summary>
        /// <returns>Json结果</returns>
        public ActionResult GetPending()
        {
            var coll = Request.Form;
            String count = coll["count"];
            String lanaugeSupport = coll["supported_languages"];

            String result = String.Empty;
            String error = String.Empty;

            if (_judgeAppService.TryGetPendingListJson(lanaugeSupport, count, out result, out error))
            {
                return Content(result, "application/json");
            }
            else
            {
                return ErrorJson(error);
            }
        }

        /// <summary>
        /// 评测机获取题目数据
        /// </summary>
        /// <returns>Json结果</returns>
        public ActionResult GetProblem()
        {
            var coll = Request.Form;
            String pid = coll["pid"];

            String dataPath = String.Empty;
            String error = String.Empty;

            if (_judgeAppService.TryGetProblemDataPath(pid, out dataPath, out error))
            {
                FileStream fs = new FileStream(dataPath, FileMode.Open);
                return File(fs, "application/zip");
            }
            else
            {
                return ErrorJson(error);
            }
        }

        /// <summary>
        /// 评测机更新评测状态
        /// </summary>
        /// <returns>Json结果</returns>
        public ActionResult UpdateStatus()
        {
            var coll = Request.Form;
            String sid = coll["sid"];
            String pid = coll["pid"];
            String username = coll["username"];
            String resultcode = coll["resultcode"];
            String detail = coll["detail"];
            String timecost = coll["timecost"];
            String memorycost = coll["memorycost"];

            String error = String.Empty;

            if (_judgeAppService.TryUpdateSolutionStatus(sid, pid, username, resultcode, detail, timecost, memorycost, out error))
            {
                return SuccessJson();
            }
            else
            {
                return ErrorJson(error);
            }
        }


        #region 私有方法
        private ContentResult ErrorJson(String error)
        {
            return Content("{\"status\":\"error\", \"message\":\"" + JsonEncoder.JsonEncode(error) + "\"}", "application/json");
        }

        private ContentResult SuccessJson()
        {
            return Content("{\"status\":\"success\"}", "application/json");
        }

        #endregion
    }
}