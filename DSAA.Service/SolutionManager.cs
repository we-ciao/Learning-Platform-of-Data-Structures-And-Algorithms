using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace DSAA.Service
{
    /// <summary>
    /// 答题管理服务
    /// </summary>
    public class SolutionManager : ServiceBase<Solution>, ISolutionAppService
    {

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
    }

}