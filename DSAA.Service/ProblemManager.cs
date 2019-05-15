using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using DSAA.Utilities;
using DSAA.Utilities.FreeProblem;
using Microsoft.AspNetCore.Hosting;
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
    public class ProblemManager : ServiceBase<Problem>, IProblemAppService
    {
        //用户管理仓储接口
        private readonly IProblemRepository _problemReporitory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly string ProblemDataPath;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public ProblemManager(IHttpContextAccessor httpContextAccessor, IProblemRepository problemReporitory, IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(problemReporitory)
        {
            _problemReporitory = problemReporitory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            ProblemDataPath = _hostingEnvironment.WebRootPath + _configuration.GetSection("Upload")["ProblemDataPath"];
        }

        /// <summary>
        /// 增加或修改一条题目
        /// </summary>
        /// <param name="entity">题目实体</param>
        /// <returns>是否成功增加</returns>
        public string InsertOrUpdateProblem(Problem entity)
        {

            entity.IsHide = true;
            entity.LastDate = DateTime.Now;
            try
            {
                var re = _problemReporitory.InsertOrUpdate(entity);
                _problemReporitory.Save();
                return re;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// 导入题目（不存在时返回null）
        /// </summary>
        /// <param name="content">文件内容</param>
        /// <returns>题目数据是否插入成功集合（全部失败时为null）</returns>
        public Dictionary<Int32, Boolean> AdminImportProblem(String content)
        {


            //转换题库模型
            List<Problem> problems;
            List<Byte[]> datas;
            List<Dictionary<String, Byte[]>> images;
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
            int change = _problemReporitory.InsertList(problems);
            if (change == 0)
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
                catch (Exception ex)
                {
                    dataadded[problems[i].Id] = false;
                }

            }



            //保存题目图片
            foreach (KeyValuePair<String, Byte[]> pair in imagefiles)
            {
                try
                {
                    InternalAdminSaveUploadFile(pair.Value, pair.Key);
                }
                catch { }
            }

            return dataadded;
        }







        #region 常量
        private static readonly String[] ALLOW_EXTENSTIONS = new String[] { ".zip", ".rar", ".7z", ".bmp", ".jpg", ".png", ".gif", ".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf" };
        #endregion

        #region 管理方法

        /// <summary>
        /// 检查文件扩展名是否有效
        /// </summary>
        /// <param name="ext">文件扩展名</param>
        /// <param name="allowExts">允许的扩展名集合</param>
        /// <returns>文件扩展名是否有效</returns>
        private static Boolean CheckFileExtension(String ext, String[] allowExts)
        {
            if (allowExts == null || allowExts.Length == 0)
            {
                return true;
            }

            for (Int32 i = 0; i < allowExts.Length; i++)
            {
                if (allowExts[i].Equals(ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 保存单个文件到磁盘
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileNewName">文件新名称</param>
        /// <returns>是否保存成功</returns>
        internal bool InternalAdminSaveUploadFile(Byte[] fileContent, String fileNewName)
        {
            if (String.IsNullOrEmpty(fileNewName))
            {
                return false;
            }

            FileInfo fi = new FileInfo(fileNewName);

            if (!CheckFileExtension(fi.Extension, ALLOW_EXTENSTIONS))
            {
                return false;
            }

            String savePath = Path.Combine(ProblemDataPath, fileNewName);

            if (File.Exists(savePath))
            {
                return false;
            }

            File.WriteAllBytes(savePath, fileContent);

            return true;
        }

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
        /// <param name="file">上传文件</param>
        /// <returns>是否保存成功</returns>
        public String AdminSaveProblemData(Int32 problemID, IFormFile file)
        {

            if (file == null)
            {
                return "没有上传文件!";
            }


            FileInfo fi = new FileInfo(file.FileName);
            if (!".zip".Equals(fi.Extension, StringComparison.OrdinalIgnoreCase))
            {
                return "文件格式不为.zip!";
            }

            //if (!String.Equals(problemID.ToString(), Path.GetFileNameWithoutExtension(fi.Name)))
            //{
            //    return "文件名应为题目ID!";
            //}

            String fileNewName = problemID.ToString() + ".zip";
            String savePath = Path.Combine(ProblemDataPath, fileNewName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return string.Format("上传题目数据成功, id = {0}", problemID.ToString());
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
        public string AdminDeleteProblemDataRealPath(Int32 problemID)
        {

            if (problemID < 1)
            {
                return "题目不存在";
            }

            String dataPath = GetProblemDataRealPath(problemID);

            if (String.IsNullOrEmpty(dataPath))
            {
                return "数据不存在";
            }

            File.Delete(dataPath);

            return null;
        }


        /// <summary>
        /// 获取题目数据最后更新日期
        /// </summary>
        /// <param name="pid">题目ID</param>
        /// <returns>最后更新日期</returns>
        public  String GetProblemDataVersion(Int32 pid)
        {
            String version = "";

            if (String.IsNullOrEmpty(version))
            {
                String filePath = GetProblemDataRealPath(pid);

                if (!String.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        ProblemDataReader reader = new ProblemDataReader(filePath);
                        version = reader.LastModified;
                    }
                    catch { }
                }
            }

            return version;
        }
        #endregion

    }
}
