using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;

namespace DSAA.Repository
{
    /// <summary>
    /// 编译器管理仓储实现
    /// </summary>
    public class CompilerRepository : RepositoryBase<Compiler>, ICompilerRepository
    {
        public CompilerRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }



    }
}
