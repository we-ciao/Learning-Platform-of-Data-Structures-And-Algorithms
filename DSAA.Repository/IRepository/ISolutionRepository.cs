using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Repository.IRepository
{
    /// <summary>
    /// 答题管理仓储接口
    /// </summary>
    public interface ISolutionRepository : IRepository<Solution>
    {


        /// <summary>
        /// 获取最早提交等待评测的一个实体
        /// </summary>
        /// <param name="count">获取个数</param>
        /// <param name="getErrorTime">获取异常提交时间</param>
        /// <param name="languageSupport">支持语言</param>
        /// <returns>对象实体</returns>
        List<Solution> GetPendingEntities(Int32 count, Int32 getErrorTime, Compiler[] languageSupport);

        /// <summary>
        /// 更新一条提交(更新所有评测信息)
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="error">编译错误信息</param>
        /// <returns>是否成功更新</returns>
        int GetStatueCount(int userid);
    }
}
