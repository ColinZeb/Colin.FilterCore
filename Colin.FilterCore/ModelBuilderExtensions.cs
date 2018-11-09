using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Colin.FilterCore
{
    public static class ModelBuilderExtensions
    {

        public static void AddFilter<F>(this ModelBuilder modelBuilder, Expression<Func<F, bool>> lambdaExpression)
        {
            var ftype = typeof(F);
            GlobaQuerylFilters.Filters.Add(typeof(F).Name, new FilterInfo
            {
                Name = typeof(F).Name,
                Expression = lambdaExpression,
                FilterType = typeof(F)
            });
        }

        public static void EnableFilters(this ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Model;
            foreach (var item in model.GetEntityTypes())
            {
                var entitytype = item.ClrType;
                var param = Expression.Parameter(entitytype, "param");

                LambdaExpression lambdaExpression = null;
                foreach (var f in GlobaQuerylFilters.Filters)
                {
                    var ftype = f.Value.FilterType;
                    if (ftype.IsAssignableFrom(item.ClrType))
                    {
                        var visitor = new ParameterTypeVisitor(ftype, entitytype);
                        var a = (LambdaExpression)visitor.Visit(f.Value.Expression);

                        if (lambdaExpression == null)
                        {
                            lambdaExpression = a;
                        }
                        else
                        {
                            lambdaExpression = lambdaExpression.AndAlso(a);
                        }
                    }
                }
                if (lambdaExpression != null)
                {
                    item.QueryFilter = lambdaExpression;
                }
            }
        }


        public static LambdaExpression AndAlso(this LambdaExpression expr1, LambdaExpression expr2)
        {
            var parameter = Expression.Parameter(expr1.Parameters.Single().Type);

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda(
                Expression.AndAlso(left, right), parameter);
        }
    }
}
