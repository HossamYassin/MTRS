using Microsoft.EntityFrameworkCore;
using MTRS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTRS.DAL.DbContexts
{
    public class MTRSDBContext : DbContext
    {
        public MTRSDBContext(DbContextOptions<MTRSDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<User> Developers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ActivityUser> ActivityUsers { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TimeSheetDetails> TimeSheetDetails { get; set; }
        public DbSet<TimeSheetApproval> TimeSheetApprovals { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<BaseActivity> BaseActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
