using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using DSAA.Utilities;
using DSAA.Utilities.FreeProblem;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace DSAA.Service
{
    /// <summary>
    /// 题目管理服务
    /// </summary>
    public class ProblemManager : IProblemAppService
    {
        //用户管理仓储接口
        private readonly IProblemRepository _problemReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        private string ProblemDataPath;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public ProblemManager(IHttpContextAccessor httpContextAccessor, IProblemRepository problemReporitory, IConfiguration configuration)
        {
            _problemReporitory = problemReporitory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            ProblemDataPath = _configuration.GetSection("Upload")["ProblemDataPath"];
        }

        /// <summary>
        /// 增加或修改一条题目
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
        public bool InsertOrUpdateProblem(Problem entity)
        {

            entity.IsHide = true;
            entity.LastDate = DateTime.Now;
            try
            {
                _problemReporitory.InsertOrUpdate(entity);
                _problemReporitory.Save();
                return true;
            }
            catch
            {

                return false;
            }
        }

        /// <summary>
        /// 导入题目（不存在时返回null）
        /// </summary>
        /// <param name="content">文件内容</param>
        /// <returns>题目数据是否插入成功集合（全部失败时为null）</returns>
        public  Dictionary<Int32, Boolean> AdminImportProblem(String content)
        {


            //转换题库模型
            List<Problem> problems = null;
            List<Byte[]> datas = null;
            List<Dictionary<String, Byte[]>> images = null;
            Dictionary<String, Byte[]> imagefiles = new Dictionary<String, Byte[]>();

            if (!ProblemImport.TryImportFreeProblemSet(content, out problems, out datas, out images))
            {
                //File content is INVALID!
                return null;
            }

            if (problems == null || problems.Count == 0)
            {
                //No problem was imported!
                return null;
            }

            //处理题目及图片路径
            for (Int32 i = 0; i < problems.Count; i++)
            {
                problems[i].IsHide = true;
                problems[i].LastDate = DateTime.Now;

                if (images[i] == null)
                {
                    continue;
                }

                String uploadRoot = _configuration.GetSection("Upload")["UploadDirectoryUrl"];

                foreach (KeyValuePair<String, Byte[]> pair in images[i])
                {
                    if (pair.Value == null || !pair.Key.Contains("."))
                    {
                        continue;
                    }

                    String oldUrl = pair.Key;
                    String fileNewName = MD5Encrypt.EncryptToHexString(oldUrl + DateTime.Now.ToString("yyyyMMddHHmmssffff"), true) + pair.Key.Substring(pair.Key.LastIndexOf('.'));
                    String newUrl = uploadRoot + fileNewName;

                    problems[i].Description = problems[i].Description.Replace(oldUrl, newUrl);
                    problems[i].Input = problems[i].Input.Replace(oldUrl, newUrl);
                    problems[i].Output = problems[i].Output.Replace(oldUrl, newUrl);
                    problems[i].Hint = problems[i].Hint.Replace(oldUrl, newUrl);

                    imagefiles[fileNewName] = pair.Value;
                }
            }

            //将题目插入到数据库
            //List<Int32> pids = _problemReporitory.InsertList(problems);

            //插入成功的数据 在problems中应有id
            if (_problemReporitory.InsertList(problems) > 0)
            {
                //Failed to import problem!
                return null;
            }

            //保存题目数据
            Dictionary<Int32, Boolean> dataadded = new Dictionary<Int32, Boolean>();

            for (Int32 i = 0; i < problems.Count; i++)
            {
                if (problems[i].Id <= 0)
                {
                    continue;
                }

                try
                {
                    if (datas[i] != null)
                    {
                        bool ret = InternalAdminSaveProblemData(problems[i].Id, datas[i]);

                        if (!ret)
                        {
                            //Failed to import problem!
                            return null;
                        }

                        dataadded[problems[i].Id] = true;
                    }
                }
                catch
                {
                    dataadded[problems[i].Id] = false;
                }

            }



            //保存题目图片
            foreach (KeyValuePair<String, Byte[]> pair in imagefiles)
            {
                try
                {
                   // UploadsManager.InternalAdminSaveUploadFile(pair.Value, pair.Key);
                }
                catch { }
            }

            return dataadded;
        }








        #region 管理方法

        /// <summary>
        /// 获取题目数据真实路径
        /// </summary>
        /// <param name="pid">题目ID</param>
        /// <returns>数据真实路径，若不存在返回null</returns>
        public String GetProblemDataRealPath(Int32 pid)
        {
            string filePath = Path.Combine(ProblemDataPath, pid.ToString() + ".zip");

            return (File.Exists(filePath) ? filePath : null);
        }

        /// <summary>
        /// 保存题目数据文件到磁盘
        /// </summary>
        /// <param name="problemID">题目ID</param>
        /// <param name="problemdata">题目数据文件</param>
        /// <returns>是否保存成功</returns>
        internal bool InternalAdminSaveProblemData(Int32 problemID, Byte[] problemdata)
        {
            if (problemID < 1)
            {
                return false;
            }

            String fileNewName = problemID.ToString() + ".zip";
            String savePath = Path.Combine(ProblemDataPath, fileNewName);
            File.WriteAllBytes(savePath, problemdata);

            return true;
        }

        /// <summary>
        /// 获取题目数据物理路径
        /// </summary>
        /// <param name="problemID">题目ID</param>
        /// <returns>题目数据物理路径</returns>
        public string AdminGetProblemDataDownloadPath(Int32 problemID)
        {
            String dataPath = GetProblemDataRealPath(problemID);

            if (String.IsNullOrEmpty(dataPath))
            {
                return null;
            }

            return dataPath;
        }

        /// <summary>
        /// 删除题目数据
        /// </summary>
        /// <param name="problemID">题目ID</param>
        /// <returns>是否删除成功</returns>
        public bool AdminDeleteProblemDataRealPath(Int32 problemID)
        {

            if (problemID < 1)
            {
                return false;
            }

            String dataPath = GetProblemDataRealPath(problemID);

            if (String.IsNullOrEmpty(dataPath))
            {
                return false;
            }

            File.Delete(dataPath);

            return true;
        }
        #endregion

    }
}
