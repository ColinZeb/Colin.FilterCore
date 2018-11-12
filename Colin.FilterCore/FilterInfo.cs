using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Colin.FilterCore
{
    public class FilterInfo
    {
        public FilterInfo()
        {
        }
        public string Name { get; set; }

        public Type FilterType { get; set; }

        public LambdaExpression Expression { get; set; }
    }

    public class FilterInfo<F> : FilterInfo
    {
        public FilterInfo()
        {
            Name = typeof(F).Name;
        }
        public FilterInfo(string filterName)
        {
            if (string.IsNullOrWhiteSpace(filterName))
            {
                throw new ArgumentException("message", nameof(filterName));
            }

            Name = filterName;
        }
        new public Expression<Func<F, bool>> Expression { get => base.Expression as Expression<Func<F, bool>>; set => base.Expression = value; }
    }
}
