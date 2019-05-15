using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Service.IService
{
    public interface ILearnAppService : IService<Learn>
    {

        /// <summary>
        /// 增加或修改一条分类
        /// </summary>
        /// <param name="entity">分类实体</param>
        /// <returns>是否成功增加</returns>
        string InsertOrUpdateProblem(Learn entity);
    }
}
