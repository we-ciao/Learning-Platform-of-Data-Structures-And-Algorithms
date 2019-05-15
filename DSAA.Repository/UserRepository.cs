using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DSAA.Repository
{
    /// <summary>
    /// 用户管理仓储实现
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
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
        public async Task<User> CheckUser(string userName, string password)
        {
            return await _dbContext.Uesrs.Where(it => it.UserName == userName && it.PassWord == password).FirstOrDefaultAsync();
        }

        public bool CheckUser(string userName)
        {
            return _dbContext.Uesrs.Count(it => it.UserName == userName) > 0;
        }

        public bool SignUp(User userModel)
        {
            try
            {
                this.Insert(userModel);
                Save();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public string updateUser(User entity)
        {
            if (entity.PassWord == null)
                entity.PassWord = _dbContext.Uesrs.Where(x => x.Id == entity.Id).Select(x => x.PassWord).FirstOrDefault();
            entity.CreateDate = _dbContext.Uesrs.Where(x => x.Id == entity.Id).Select(x => x.CreateDate).FirstOrDefault();
            Update(entity);
            return "更新";
        }

    }
}
