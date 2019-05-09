using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DSAA.EntityFrameworkCore.Entity;

namespace DSAA.EntityFrameworkCore
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Uesrs { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Compiler> Compilers { get; set; }
        public virtual DbSet<Problem> Problem { get; set; }
        public virtual DbSet<Solution> Solution { get; set; }
        public virtual DbSet<Contest> Contest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProblemCategory>()
                .HasKey(t => new { t.ProblemId, t.CategoryId });

            modelBuilder.Entity<ProblemCategory>()
                .HasOne(pt => pt.Problem)
                .WithMany(p => p.Categorys)
                .HasForeignKey(pt => pt.ProblemId);


            modelBuilder.Entity<ProblemCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(p => p.Problems)
                .HasForeignKey(pt => pt.CategoryId);

        }

    }
}