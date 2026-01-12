using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            // parameter for the combined lambda
            var parameter = Expression.Parameter(typeof(T));

            // replace parameters in both expressions with the new parameter
            var left = Expression.Invoke(expr1, parameter);
            var right = Expression.Invoke(expr2, parameter);

            // combine with ANDAlso
            var body = Expression.AndAlso(left, right);

            // return new lambda
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
