using DSAA.EntityFrameworkCore;
using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSAA.Repository
{
    /// <summary>
    /// 题目管理仓储实现
    /// </summary>
    public class ProblemRepository : RepositoryBase<Problem>, IProblemRepository
    {
        public ProblemRepository(EntityDbContext dbcontext) : base(dbcontext)
        {

        }

    }
}
