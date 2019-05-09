using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSAA.Repository
{
    /// <summary>
    /// 答题管理仓储实现
    /// </summary>
    public class SolutionRepository : RepositoryBase<Solution>, ISolutionRepository
    {
        public SolutionRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }

        /// <summary>
        /// 获取最早提交等待评测的一个实体
        /// </summary>
        /// <param name="count">获取个数</param>
        /// <param name="getErrorTime">获取异常提交时间</param>
        /// <param name="languageSupport">支持语言</param>
        /// <returns>对象实体</returns>
        public List<Solution> GetPendingEntities(Int32 count, Int32 getErrorTime, Compiler[] languageSupport)
        {

            IQueryable<Solution> list = _dbContext.Set<Solution>().Where(x => x.Result <= ResultType.RejudgePending);

            if (getErrorTime > 0)
            {
                list = list.Where(x => x.Result == ResultType.Judging & x.JudgeTime <= DateTime.Now.AddSeconds(-getErrorTime));
            }

            if (languageSupport != null && languageSupport.Length > 0)//没有和全部时不设定条件
            {
                //temp = c.In(LANGUAGE, () =>
                //{
                //    Byte[] langIds = new Byte[languageSupport.Length];

                //    for (Int32 i = 0; i < langIds.Length; i++)
                //    {
                //        langIds[i] = languageSupport[i].ID;
                //    }

                //    return langIds;
                //});
                //condition = (condition == null ? temp : condition & temp);
            }
            var res = list.Take(count).OrderBy(x => x.Id).ToList();

            //提交获取到的题目状态为Juding且评测时间为当前
            if (res != null && res.Count > 0)
            {

                for (Int32 i = 0; i < res.Count; i++)
                {
                    res[i].Result = ResultType.Judging;
                    res[i].JudgeTime = DateTime.Now;
                }
            }
            Save();

            return res;
        }


        /// <summary>
        /// 更新一条提交(更新所有评测信息)
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <param name="error">编译错误信息</param>
        /// <returns>是否成功更新</returns>
        public int GetStatueCount(int userid)
        {

            return _dbContext.Set<Solution>().Where(x=>x.User.Id== userid&&x.Result== ResultType.Pending).Count();
        }

    }

}
