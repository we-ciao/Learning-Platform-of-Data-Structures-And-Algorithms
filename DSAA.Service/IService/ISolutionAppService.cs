using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Collections.Generic;

namespace DSAA.Service.IService
{
    public interface ISolutionAppService : IService<Solution>
    {
        /// <summary>
        /// 增加一条提交
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="userip">用户IP</param>
        /// <returns>是否成功增加</returns>
        string InsertSolution(Solution entity);

        /// <summary>
        /// 获取最早等待评测的一个提交
        /// </summary>
        /// <param name="count">获取个数</param>
        /// <param name="languageSupport">支持语言</param>
        /// <returns>提交实体</returns>
        List<Solution> JudgeGetPendingSolution(Int32 count, Compiler[] languageSupport);


        /// <summary>
        /// 更新一条提交(更新所有评测信息)
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="error">编译错误信息</param>
        /// <returns>是否成功更新</returns>
         Boolean JudgeUpdateSolutionAllResult(Solution entity, String error);

        /// <summary>
        /// 更新一条提交(更新所有评测信息)
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="error">编译错误信息</param>
        /// <returns>是否成功更新</returns>
        int GetStatueCount(int userid);
    }
}
