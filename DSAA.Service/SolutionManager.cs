using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DSAA.Service
{
    /// <summary>
    /// 答题管理服务
    /// </summary>
    public class SolutionManager : ServiceBase<Solution>, ISolutionAppService
    {
        #region 常量
        /// <summary>
        /// 问题页面页面大小
        /// </summary>
        private const Int32 STATUS_PAGE_SIZE = 20;
        #endregion

        #region 字段
        /// <summary>
        /// 同时只有一个Judge请求Pending
        /// </summary>
        private static Object _selectLock = new Object();

        /// <summary>
        /// 同时只有一个Judge更新结果
        /// </summary>
        private static Object _updateLock = new Object();
        #endregion

        //用户管理仓储接口
        private readonly IUserAppService _userAppService;
        private readonly ISolutionRepository _solutionReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public SolutionManager(IHttpContextAccessor httpContextAccessor, IUserAppService userAppService, ISolutionRepository solutionReporitory, IConfiguration configuration) : base(solutionReporitory)
        {
            _userAppService = userAppService;
            _solutionReporitory = solutionReporitory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }




        /// <summary>
        /// 增加一条提交
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="userip">用户IP</param>
        /// <returns>是否成功增加</returns>
        public string InsertSolution(Solution entity)
        {

            if (String.IsNullOrEmpty(entity.SourceCode))
            {
                return "代码不能为空!";
            }


            if (entity.Problem == null)//判断题目是否存在
            {
                return "题目不存在";
            }


            entity.User = _userAppService.GetCurrentUser();
            entity.SubmitTime = DateTime.Now;
            //entity.SubmitIP = userip;

            _solutionReporitory.Insert(entity);
            Boolean success = _solutionReporitory.Save() > 0;

            return success ? null : "提交失败";
        }


        /// <summary>
        /// 获取最早等待评测的一个提交
        /// </summary>
        /// <param name="count">获取个数</param>
        /// <param name="languageSupport">支持语言</param>
        /// <returns>提交实体</returns>
        public List<Solution> JudgeGetPendingSolution(Int32 count, Compiler[] languageSupport)
        {
            lock (_selectLock)
            {
                return _solutionReporitory.GetPendingEntities(count, 0, languageSupport);
            }
        }

        /// <summary>
        /// 更新一条提交(更新所有评测信息)
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="error">编译错误信息</param>
        /// <returns>是否成功更新</returns>
        public Boolean JudgeUpdateSolutionAllResult(Solution entity, String error)
        {
            if (entity == null) return false;

            lock (_updateLock)
            {
                entity.JudgeTime = DateTime.Now;
                _solutionReporitory.Update(entity);
                if (_solutionReporitory.Save() > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 更新一条提交(更新所有评测信息)
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="error">编译错误信息</param>
        /// <returns>是否成功更新</returns>
        public int GetStatueCount(int userid)
        {

            return _solutionReporitory.GetStatueCount(userid);
        }



    }

}