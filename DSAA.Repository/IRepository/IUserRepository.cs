using DSAA.EntityFrameworkCore.Entity;
using System.Threading.Tasks;

namespace DSAA.Repository.IRepository
{
    /// <summary>
    /// 用户管理仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        Task<User> CheckUser(string userName, string password);

        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        bool CheckUser(string userName);

        /// <summary>
        /// 用户注册
        /// </summary>
        bool SignUp(User userModel);

        string updateUser(User entity);
    }
}
