using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DSAA.Repository
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Uesrs { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Compiler> Compilers { get; set; }
        public virtual DbSet<Problem> Problem { get; set; }
        public virtual DbSet<Solution> Solution { get; set; }
        public virtual DbSet<Contest> Contest { get; set; }

    }
}