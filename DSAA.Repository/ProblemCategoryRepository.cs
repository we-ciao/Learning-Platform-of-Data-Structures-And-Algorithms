using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;

namespace DSAA.Repository
{
    /// <summary>
    /// 题目分类管理仓储实现
    /// </summary>
    public class ProblemCategoryRepository : RepositoryBase<Category>, IProblemCategoryRepository
    {
        public ProblemCategoryRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }

    }
}
