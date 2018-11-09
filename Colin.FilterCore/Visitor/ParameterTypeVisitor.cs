using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Colin.FilterCore
{
    public class ParameterTypeVisitor : ExpressionVisitor
    {
        private ReadOnlyCollection<ParameterExpression> _parameters;

        private Type stype;
        private Type tType;

        public ParameterTypeVisitor(Type stype, Type tType)
        {
            this.stype = stype;
            this.tType = tType;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameters?.FirstOrDefault(p => p.Name == node.Name) ??
                (node.Type == stype ? Expression.Parameter(tType, node.Name) : node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            _parameters = VisitAndConvert(node.Parameters, "VisitLambda");
            return Expression.Lambda(Visit(node.Body), _parameters);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == stype)
            {
                return Expression.Property(Visit(node.Expression), node.Member.Name);
            }
            return base.VisitMember(node);
        }
    }
}
