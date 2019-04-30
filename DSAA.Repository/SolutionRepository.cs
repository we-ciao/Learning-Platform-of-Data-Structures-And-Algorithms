using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;

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



    }
}
