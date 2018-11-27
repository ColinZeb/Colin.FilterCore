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
        /// <summary>
        /// 添加过滤器
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="lambdaExpression">过滤表达式</param>
        public static void AddFilter<F>(this ModelBuilder modelBuilder, Expression<Func<F, bool>> lambdaExpression)
        {
            var ftype = typeof(F);
            modelBuilder.AddFilter(ftype.Name, lambdaExpression);
        }
        
        /// <summary>
        /// 添加带名称的过滤器
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="filterName">过滤器名称</param>
        /// <param name="lambdaExpression">过滤器表达式</param>
        public static void AddFilter<F>(this ModelBuilder modelBuilder,string filterName, Expression<Func<F, bool>> lambdaExpression)
        {
            var ftype = typeof(F);
            GlobaQuerylFilters.Filters.Add(filterName, new FilterInfo<F>(filterName)
            {
                Expression = lambdaExpression,
                FilterType = ftype
            });
        }
        /// <summary>
        /// 启用过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void EnableFilters(this ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Model;
            foreach (var item in model.GetEntityTypes())
            {
                var entitytype = item.ClrType;
                var param = Expression.Parameter(entitytype, "param");

                LambdaExpression lambdaExpression = null;
                lambdaExpression = BuildExpression(entitytype);
                if (lambdaExpression != null)
                {
                    item.QueryFilter = lambdaExpression;
                }
            }
        }
        /// <summary>
        /// 用过滤器构建表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="disablefilters"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        internal static Expression<Func<T, bool>> BuildExpression<T>(this IQueryable<T> query,  string[] disablefilters,bool rev=false)
        {
            return typeof(T).BuildExpression(disablefilters,rev) as Expression<Func<T, bool>>;
        }
        internal static LambdaExpression BuildExpression(this Type entitytype,params string[] filters)
        {
            return BuildExpression(entitytype, filters, false);
        }
        internal static LambdaExpression BuildExpression(this Type entitytype, string[] filters,bool rev)
        {
            LambdaExpression lambdaExpression = null;
            var usefilters = rev ?
                GlobaQuerylFilters.Filters.Where(x => filters.Contains(x.Key)) :
                GlobaQuerylFilters.Filters.Where(x => !filters.Contains(x.Key));
            foreach (var f in usefilters)
            {
                var ftype = f.Value.FilterType;
                if (ftype.IsAssignableFrom(entitytype))
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

            return lambdaExpression;
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
