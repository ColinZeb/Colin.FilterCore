using System;
using System.Collections.Generic;
using System.Text;

namespace Colin.FilterCore
{
    public class GlobaQuerylFilters
    {
        static GlobaQuerylFilters()
        {
            Filters = new Dictionary<string, FilterInfo>();
        }
        public static Dictionary<string, FilterInfo> Filters { get; set; }
    }
}
