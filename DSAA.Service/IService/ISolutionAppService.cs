using DSAA.EntityFrameworkCore.Entity;

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



    }
}
