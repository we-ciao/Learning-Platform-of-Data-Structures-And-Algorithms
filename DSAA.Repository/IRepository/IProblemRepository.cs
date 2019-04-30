using DSAA.EntityFrameworkCore.Entity;

namespace DSAA.Repository.IRepository
{
    /// <summary>
    /// 题目管理仓储接口
    /// </summary>
    public interface IProblemRepository : IRepository<Problem>
    {
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        //Task<User> CheckUser(string userName, string password);



    }
}
