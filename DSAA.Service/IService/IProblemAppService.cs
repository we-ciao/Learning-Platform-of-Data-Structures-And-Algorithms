using DSAA.EntityFrameworkCore.Entity;
using System;
using System.Collections.Generic;

namespace DSAA.Service.IService
{
    public interface IProblemAppService
    {
        /// <summary>
        /// 增加或修改一条题目
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
        bool InsertOrUpdateProblem(Problem entity);

        /// <summary>
        /// 导入题目（不存在时返回null）
        /// </summary>
        /// <param name="content">文件内容</param>
        /// <returns>题目数据是否插入成功集合（全部失败时为null）</returns>
        Dictionary<Int32, Boolean> AdminImportProblem(string content);
    }
}
