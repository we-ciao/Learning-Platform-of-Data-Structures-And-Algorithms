using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Service.IService
{
    public interface IProblemCategoryAppService : IService<Category>
    {
        /// <summary>
        /// 增加或修改一条分类
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
        string InsertOrUpdateProblem(Category entity);

    }
}
