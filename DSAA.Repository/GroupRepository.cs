using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;

namespace DSAA.Repository
{
    /// <summary>
    /// 用户管理仓储实现
    /// </summary>
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
