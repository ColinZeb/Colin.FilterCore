using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Colin.FilterCore.XUnitTest.EF
{
    public class BloggingContext : DbContext
    {
        public BloggingContext()
        { }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddFilter((ISoftDelete x) => !x.IsDeleted);

            modelBuilder.AddFilter((IDisable x) => !x.IsDisabled);

            modelBuilder.EnableFilters();
        }
    }
}
