using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Service.IService
{
    public interface IGroupAppService : IService<Group>
    {

        /// <summary>
        /// 增加或修改一条题目
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
         string InsertOrUpdateProblem(Group entity);
    }
}
