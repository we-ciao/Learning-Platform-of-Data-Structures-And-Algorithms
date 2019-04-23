using DSAA.EntityFrameworkCore.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DSAA.Service.IService
{
    public interface IProblemAppService : IService<Problem>
    {
        /// <summary>
        /// 增加或修改一条题目
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
        string InsertOrUpdateProblem(Problem entity);

        /// <summary>
        /// 导入题目（不存在时返回null）
        /// </summary>
        /// <param name="content">文件内容</param>
        /// <returns>题目数据是否插入成功集合（全部失败时为null）</returns>
        Dictionary<Int32, Boolean> AdminImportProblem(string content);


        /// <summary>
        /// 删除题目数据
        /// </summary>
        /// <param name="problemID">题目ID</param>
        /// <returns>是否删除成功</returns>
        string AdminDeleteProblemDataRealPath(Int32 problemID);

        /// <summary>
        /// 获取题目数据物理路径
        /// </summary>
        /// <param name="problemID">题目ID</param>
        /// <returns>题目数据物理路径</returns>
        string AdminGetProblemDataDownloadPath(Int32 problemID);


        /// <summary>
        /// 保存题目数据文件到磁盘
        /// </summary>
        /// <param name="problemID">题目ID</param>
        /// <param name="file">上传文件</param>
        /// <returns>是否保存成功</returns>
        String AdminSaveProblemData(Int32 problemID, IFormFile file);
    }
}
