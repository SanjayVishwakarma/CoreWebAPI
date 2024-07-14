using CoreWebAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebAPI.Infrastructure.InMomeryDbContext
{
    public class InMomeryDbContext : DbContext
    {
        public InMomeryDbContext(DbContextOptions<InMomeryDbContext> options) : base(options)
        {
        }

        public DbSet<StoryDetail> StoryDetail { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TodoItem>().HasData(
        //        new TodoItem { Id = 1, Name = "Task 1", IsComplete = false },
        //        new TodoItem { Id = 2, Name = "Task 2", IsComplete = true }
        //    );
        //}
    }
}
