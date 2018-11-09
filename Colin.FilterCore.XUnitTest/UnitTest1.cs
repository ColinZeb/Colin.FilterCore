using Colin.FilterCore.XUnitTest.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Colin.FilterCore.XUnitTest
{
    //"EFπ˝¬À∆˜≤‚ ‘"
    public class UnitTest1
    {

        [Fact(DisplayName = "»Ì…æ≥˝≤‚ ‘")]
        public void TestSoftDelete()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestSoftDelete")
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
                var c = db.Blogs.Count();
                Assert.Equal(3, c);
            }
        }

        [Fact(DisplayName = "»ÌΩ˚”√≤‚ ‘")]
        public void TestSoftDisable()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestSoftDisable")
               .Options;

            // Run the test against one instance of the context
            using (var context = new BloggingContext(options))
            {
                context.Blogs.AddRange(new Blog[] {
                    new Blog{ Id= 1, Url = "test 1", IsDisabled = false },
                    new Blog{ Id= 2, Url = "test 2", IsDisabled = false},
                    new Blog{ Id= 3, Url = "test 3", IsDisabled = true},
                    new Blog{ Id= 4, Url = "test 4", IsDisabled = false},
                });
                context.SaveChanges();
            }
            using (var db = new BloggingContext(options))
            {
                var c = db.Blogs.Count();
                Assert.Equal(3, c);
            }
        }

        [Fact(DisplayName ="◊È∫œ≤‚ ‘")]
        public void TestSoftCombine()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseInMemoryDatabase(databaseName: "TestSoftCombine")
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
                var c = db.Blogs.Count();
                Assert.Equal(1, c);
            }
        }
    }
}
