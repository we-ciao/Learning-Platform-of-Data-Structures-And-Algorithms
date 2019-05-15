using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Service
{
    /// <summary>
    /// 用户组管理服务
    /// </summary>
    public class GroupManager : ServiceBase<Group>, IGroupAppService
    {
        //用户管理仓储接口
        private readonly IGroupRepository _groupReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public GroupManager(IHttpContextAccessor httpContextAccessor, IGroupRepository groupReporitory) : base(groupReporitory)
        {
            _groupReporitory = groupReporitory;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// 增加或修改一条题目
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
        public string InsertOrUpdateProblem(Group entity)
        {
            try
            {
                var re = _groupReporitory.InsertOrUpdate(entity);
                _groupReporitory.Save();
                return re;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


    }
}
