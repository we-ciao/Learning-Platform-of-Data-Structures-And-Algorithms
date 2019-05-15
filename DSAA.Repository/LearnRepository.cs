using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAA.Repository
{
    /// <summary>
    /// 学习管理仓储实现
    /// </summary>
    public class LearnRepository : RepositoryBase<Learn>, ILearnRepository
    {
        public LearnRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }

    }
}
