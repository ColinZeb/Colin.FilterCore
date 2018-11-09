using System;
using System.Collections.Generic;
using System.Text;

namespace Colin.FilterCore.XUnitTest.EF
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
