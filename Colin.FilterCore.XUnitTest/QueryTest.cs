using Colin.FilterCore.XUnitTest.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Colin.FilterCore.XUnitTest
{
    public class QueryTest
    {

        [Fact(DisplayName = "禁用软删除")]
        public void TestQueryDisableSoftDelete()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestQueryDisableSoftDelete")
               .Options;

            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {

                context.Blogs.AddRange(new Blog[] {
                    new Blog{ Id= 1, Url = "test 1", IsDeleted = false },
                    new Blog{ Id= 2, Url = "test 2", IsDeleted = false},
                    new Blog{ Id= 3, Url = "test 3", IsDeleted = true},
                    new Blog{ Id= 4, Url = "test 4", IsDeleted = false},
                });
                context.SaveChanges();
            }
            using (var db = new BloggingContext(options))
            {
                var c = db.Blogs.DisableFilter(nameof(ISoftDelete)).Count();
                Assert.Equal(4, c);
            }
        }

        [Fact(DisplayName = "启用软删除")]
        public void TestQueryEnableSoftDelete()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestQueryEnableSoftDelete")
               .Options;

            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {

                context.Blogs.AddRange(new Blog[] {
                    new Blog{ Id= 1, Url = "test 1", IsDeleted = false },
                    new Blog{ Id= 2, Url = "test 2", IsDeleted = false},
                    new Blog{ Id= 3, Url = "test 3", IsDeleted = true},
                    new Blog{ Id= 4, Url = "test 4", IsDeleted = false},
                });
                context.SaveChanges();
            }
            using (var db = new BloggingContext(options))
            {
                var c = db.Blogs.EnableFilter(nameof(ISoftDelete)).Count();
                Assert.Equal(3, c);
            }
        }

        [Fact(DisplayName = "禁用单个过滤器")]
        public void TestQueryDisableOneFilter()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestQueryDisableOneFilter")
               .Options;

            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {
                context.Blogs.AddRange(new Blog[] {
                   new Blog{ Id= 1, Url = "test 1", IsDeleted = false, IsDisabled = false },
                    new Blog{ Id= 2, Url = "test 2", IsDeleted = true, IsDisabled = false},
                    new Blog{ Id= 3, Url = "test 3", IsDeleted = true, IsDisabled = true},
                    new Blog{ Id= 4, Url = "test 4", IsDeleted = false, IsDisabled = true},
                });
                context.SaveChanges();
            }
            using (var db = new BloggingContext(options))
            {
                var c = db.Blogs.DisableFilter(nameof(ISoftDelete)).ToList();
                Assert.Equal(2, c.Count);
                Assert.DoesNotContain(c, x => x.IsDisabled);
                Assert.Contains(c, x => x.IsDeleted);
                var c2 = db.Blogs.DisableFilter(nameof(IDisable)).ToList();
                Assert.Equal(2, c2.Count);
                Assert.DoesNotContain(c2, x => x.IsDeleted);
                Assert.Contains(c2, x => x.IsDisabled);

                var ca = db.Blogs.DisableFilter(nameof(IDisable), nameof(ISoftDelete)).ToList();
                Assert.Equal(4, ca.Count);
                Assert.Contains(ca, x => x.IsDeleted);
                Assert.Contains(ca, x => x.IsDisabled);
            }
        }

        [Fact(DisplayName = "启用单个过滤器")]
        public void TestQuerEnableOneFilter()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestQuerEnableOneFilter")
               .Options;

            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {
                context.Blogs.AddRange(new Blog[] {
                   new Blog{ Id= 1, Url = "test 1", IsDeleted = false, IsDisabled = false },
                    new Blog{ Id= 2, Url = "test 2", IsDeleted = true, IsDisabled = false},
                    new Blog{ Id= 3, Url = "test 3", IsDeleted = true, IsDisabled = true},
                    new Blog{ Id= 4, Url = "test 4", IsDeleted = false, IsDisabled = true},
                });
                context.SaveChanges();
            }
            using (var db = new BloggingContext(options))
            {
                var c = db.Blogs.EnableFilter(nameof(ISoftDelete)).ToList();
                Assert.Equal(2, c.Count);
                Assert.DoesNotContain(c, x => x.IsDeleted);
                Assert.Contains(c, x => x.IsDisabled);
                var c2 = db.Blogs.EnableFilter(nameof(IDisable)).ToList();
                Assert.Equal(2, c2.Count);
                Assert.DoesNotContain(c2, x => x.IsDisabled);
                Assert.Contains(c2, x => x.IsDeleted);

                var ca = db.Blogs.EnableFilter(nameof(IDisable), nameof(ISoftDelete)).ToList();
                Assert.Single(ca);
                Assert.DoesNotContain(ca, x => x.IsDeleted);
                Assert.DoesNotContain(ca, x => x.IsDisabled);
            }
        }
    }
}
