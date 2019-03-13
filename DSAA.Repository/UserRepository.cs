using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using System.Linq;

namespace DSAA.Repository
{
    /// <summary>
    /// 用户管理仓储实现
    /// </summary>
    public class UserRepository : FonourRepositoryBase<User>, IUserRepository
    {
        public UserRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        public User CheckUser(string userName, string password)
        {
            return _dbContext.Set<User>().FirstOrDefault(it => it.UserName == userName && it.PassWord == password);
        }
    }
}
