using System;
using System.Collections.Generic;
using System.Text;

namespace Colin.FilterCore.XUnitTest.EF
{
    public class Blog : ISoftDelete, IDisable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDisabled { get; set; }
    }
}
