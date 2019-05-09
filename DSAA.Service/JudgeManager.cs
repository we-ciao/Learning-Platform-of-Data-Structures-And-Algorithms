using DSAA.EntityFrameworkCore.Entity;
using DSAA.Service.IService;
using DSAA.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Service
{
    public class JudgeManager : IJudgeAppService
    {
        #region 常量
        /// <summary>
        /// 自动重测最多尝试次数
        /// </summary>
        private const Int32 AUTO_REJUDGE_MAX_TIMES = 5;
        #endregion

        #region 字段
        private static Dictionary<Int32, Int32> _rejudgeTimesMap;
        #endregion


        //用户管理仓储接口
        private readonly IUserAppService _userAppService;
        private readonly ISolutionAppService _solutionAppService;
        private readonly IProblemAppService _problemAppService;

        public JudgeManager(IUserAppService userAppService, ISolutionAppService solutionAppService, IProblemAppService problemAppService)
        {
            _solutionAppService = solutionAppService;
            _userAppService = userAppService;
            _problemAppService = problemAppService;
            _rejudgeTimesMap = new Dictionary<Int32, Int32>();
        }

        #region 属性
        /// <summary>
        /// 获取当前评测机用户名
        /// </summary>
        public String JudgeUserName()
        {
            return _userAppService.GetCurrentUserName();
        }
        #endregion

        #region 评测机登录
        /// <summary>
        /// 尝试评测机登录
        /// </summary>
        /// <param name="serverID">评测机ID</param>
        /// <param name="secretKey">评测机密钥</param>
        /// <param name="userip">用户IP</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否登录成功</returns>
        public Boolean TryJudgeServerLogin(String serverID, String secretKey, out String error)
        {

            User user = null;
            user = _userAppService.CheckUser(serverID, secretKey).Result;

            if (user == null)
            {
                error = "用户不存在!";
                return false;
            }

            if (user.Group.Permission != PermissionType.HttpJudge)
            {
                error = "You do not have httpjudge privilege!";
                return false;
            }

            try
            {
                _userAppService.SetCurrentUser(user);
                //JudgeOnlineStatus.SetJudgeStatus(serverID);
            }
            catch { }
            error = null;
            return true;
        }
        #endregion

        #region 评测机状态
        /// <summary>
        /// 获取用户登录状态
        /// </summary>
        /// <returns>若用户已登录则返回空，否则返回出错状态</returns>
        public String GetJudgeServerLoginStatus()
        {

            if (_userAppService.GetCurrentUserName() == null)
            {
                return "unlogin";
            }

            if (_userAppService.GetCurrentUser().Group.Permission != PermissionType.HttpJudge)
            {
                return "no privilege";
            }

            try
            {
                // JudgeOnlineStatus.SetJudgeStatus(UserManager.CurrentUserName);
            }
            catch { }

            return String.Empty;
        }
        #endregion


        //JudgeSolution


        #region 评测机获取评测列表
        /// <summary>
        /// 获取评测列表的Json信息
        /// </summary>
        /// <param name="lanaugeSupport">评测机支持语言</param>
        /// <param name="count">评测机请求个数</param>
        /// <returns>评测列表Json信息</returns>
        public Boolean TryGetPendingListJson(String lanaugeSupport, String count, out String result, out String error)
        {
            result = String.Empty;

            try
            {
                error = GetJudgeServerLoginStatus();

                if (!String.IsNullOrEmpty(error))
                {
                    return false;
                }

                StringBuilder ret = new StringBuilder();
                Int32 requestCount = Math.Max(1, Convert.ToInt32(count));
                //TODO: 第二参数 支持评测列表
                List<Solution> pendingList = _solutionAppService.JudgeGetPendingSolution(requestCount, null);

                Dictionary<Int32, Problem> problemCache = new Dictionary<Int32, Problem>();
                Dictionary<Int32, String> problemVersionCache = new Dictionary<Int32, String>();

                Problem problem = null;
                String problemDataVersion = String.Empty;
                Solution solution = null;
                Int32 listCount = (pendingList == null ? 0 : pendingList.Count);

                ret.Append("[");

                for (Int32 i = 0; i < listCount; i++)
                {
                    if (i > 0) ret.Append(",");

                    solution = pendingList[i];

                    if (!problemCache.TryGetValue(solution.Problem.Id, out problem))
                    {
                        problem = _problemAppService.Get(solution.Problem.Id);
                        problemCache[solution.Problem.Id] = problem;
                    }

                    if (!problemVersionCache.TryGetValue(solution.Problem.Id, out problemDataVersion))
                    {
                        problemDataVersion = _problemAppService.GetProblemDataVersion(solution.Problem.Id);
                        problemVersionCache[solution.Problem.Id] = problemDataVersion;
                    }

                    if (problem != null)
                    {
                        Double scale = 1.0;
                        Int32 timeLimit = (Int32)(problem.TimeLimit * scale);
                        Int32 memoryLimit = (Int32)(problem.MemoryLimit * scale);

                        ret.Append("{");
                        ret.Append("\"sid\":\"").Append(solution.Id.ToString()).Append("\",");
                        ret.Append("\"pid\":\"").Append(solution.Problem.Id.ToString()).Append("\",");
                        ret.Append("\"username\":\"").Append(solution.User.UserName).Append("\",");
                        ret.Append("\"dataversion\":\"").Append(problemDataVersion).Append("\",");
                        ret.Append("\"timelimit\":\"").Append(timeLimit.ToString()).Append("\",");
                        ret.Append("\"memorylimit\":\"").Append(memoryLimit.ToString()).Append("\",");
                        ret.Append("\"language\":\"").Append(solution.LanguageType.Name).Append("[]\",");
                        ret.Append("\"sourcecode\":\"").Append(JsonEncoder.JsonEncode(solution.SourceCode)).Append("\"");
                        ret.Append("}");
                    }
                }

                ret.Append("]");

                result = ret.ToString();
                return true;
            }
            catch (System.Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        #endregion

        #region 评测机更新评测状态
        /// <summary>
        /// 更新评测状态
        /// </summary>
        /// <param name="sid">提交ID</param>
        /// <param name="pid">题目ID</param>
        /// <param name="username">用户名</param>
        /// <param name="result">评测结果</param>
        /// <param name="detail">出错信息</param>
        /// <param name="tcost">花费时间</param>
        /// <param name="mcost">花费内存</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否更新成功</returns>
        public Boolean TryUpdateSolutionStatus(String sid, String pid, String username, String result, String detail, String tcost, String mcost, out String error)
        {
            try
            {
                error = GetJudgeServerLoginStatus();

                if (!String.IsNullOrEmpty(error))
                {
                    return false;
                }

                Solution entity = _solutionAppService.Get(Int32.Parse(sid));
                entity.Result = (ResultType)Convert.ToByte(result);
                entity.TimeCost = Convert.ToInt32(tcost);
                entity.MemoryCost = Convert.ToInt32(mcost);


                if (entity.Result > ResultType.Accepted)//评测失败
                {
                    Boolean hasProblemData = !String.IsNullOrEmpty(_problemAppService.GetProblemDataRealPath(entity.Id));

                    //没有题目的不重新评测
                    Boolean canAutoRejudge = hasProblemData;

                    Int32 triedTimes = 0;
                    if (!_rejudgeTimesMap.TryGetValue(entity.Id, out triedTimes))
                    {
                        triedTimes = 0;
                    }

                    if (triedTimes > AUTO_REJUDGE_MAX_TIMES)
                    {
                        _rejudgeTimesMap.Remove(entity.Id);
                        canAutoRejudge = false;
                    }
                    else
                    {
                        _rejudgeTimesMap[entity.Id] = triedTimes + 1;
                    }

                    entity.Result = canAutoRejudge ? ResultType.RejudgePending : ResultType.JudgeFailed;
                }

                _solutionAppService.JudgeUpdateSolutionAllResult(entity, detail);

                return true;
            }
            catch (System.Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取评测机支持的语言
        /// </summary>
        /// <param name="languageSupport">评测机支持的语言</param>
        /// <returns>评测机支持的语言</returns>
        //private static LanguageType[] GetJudgeSupportLanguages(String languageSupport)
        //{
        //    if (String.IsNullOrEmpty(languageSupport))
        //    {
        //        return new LanguageType[] { };
        //    }

        //    String[] languages = languageSupport.Split(',');
        //    List<LanguageType> languageTypes = new List<LanguageType>();

        //    for (Int32 i = 0; i < languages.Length; i++)
        //    {
        //        String type = (languages[i].IndexOf('[') > 0 ? languages[i].Substring(0, languages[i].IndexOf('[')) : languages[i]);
        //        LanguageType langType = LanguageType.FromLanguagType(type);

        //        if (!LanguageType.IsNull(langType) && !languageTypes.Contains(langType))
        //        {
        //            languageTypes.Add(langType);
        //        }
        //    }

        //    return languageTypes.ToArray();
        //}
        #endregion



        #region 评测机获取题目数据
        /// <summary>
        /// 获取题目数据
        /// </summary>
        /// <param name="pid">题目ID</param>
        /// <param name="dataPath">题目数据路径</param>
        /// <param name="error">错误信息</param>
        /// <returns>获取是否成功</returns>
        public Boolean TryGetProblemDataPath(String pid, out String dataPath, out String error)
        {
            dataPath = String.Empty;

            try
            {
                error = GetJudgeServerLoginStatus();

                if (!String.IsNullOrEmpty(error))
                {
                    return false;
                }

                Int32 problemID = Convert.ToInt32(pid);
                String path = _problemAppService.GetProblemDataRealPath(problemID);

                if (String.IsNullOrEmpty(path))
                {
                    error = "Problem data does not exist!";
                    return false;
                }

                dataPath = path;
                return true;
            }
            catch (System.Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        #endregion
    }
}