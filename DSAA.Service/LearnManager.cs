using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using System;

namespace DSAA.Service
{
    /// <summary>
    /// 学习管理服务
    /// </summary>
    public class LearnManager : ServiceBase<Learn>, ILearnAppService
    {
        //用户管理仓储接口
        private readonly ILearnRepository _learnReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public LearnManager(IHttpContextAccessor httpContextAccessor, ILearnRepository learnReporitory) : base(learnReporitory)
        {
            _learnReporitory = learnReporitory;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 增加或修改一条分类
        /// </summary>
        /// <param name="entity">分类实体</param>
        /// <returns>是否成功增加</returns>
        public string InsertOrUpdateProblem(Learn entity)
        {
            try
            {
                var re = _learnReporitory.InsertOrUpdate(entity);
                _learnReporitory.Save();
                return re;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


    }
}
