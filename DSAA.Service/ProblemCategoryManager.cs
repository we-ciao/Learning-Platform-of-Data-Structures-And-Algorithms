﻿using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace DSAA.Service
{
    /// <summary>
    /// 题目分类管理服务
    /// </summary>
    public class ProblemCategoryManager : ServiceBase<Category>, IProblemCategoryAppService
    {
        //用户管理仓储接口
        private readonly IProblemCategoryRepository _problemCategoryReporitory;
        private readonly IProblemAppService _problemAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public ProblemCategoryManager(IHttpContextAccessor httpContextAccessor, IProblemAppService problemAppService, IProblemCategoryRepository problemCategoryReporitory) : base(problemCategoryReporitory)
        {
            _problemCategoryReporitory = problemCategoryReporitory;
            _httpContextAccessor = httpContextAccessor;
            _problemAppService = problemAppService;
        }

        /// <summary>
        /// 增加或修改一条分类
        /// </summary>
        /// <param name="entity">分类实体</param>
        /// <returns>是否成功增加</returns>
        public string InsertOrUpdateProblem(Category entity)
        {
            try
            {
                var re = _problemCategoryReporitory.InsertOrUpdate(entity);
                _problemCategoryReporitory.Save();
                return re;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// 分类添加题目
        /// </summary>
        public int CategoryAddProblem(Int32 id, int[] ProblemList)
        {
            var Category = _problemCategoryReporitory.Get(id);
            Category.Problems.Clear();

            foreach (var item in ProblemList)
            {
                Category.Problems.Add(new ProblemCategory{ Category= Category, Problem= _problemAppService.Get(item) } );
            }

            return Save();
        }


    }
}
