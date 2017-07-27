﻿using System;
using System.Linq.Expressions;

namespace Utilities
{
    public static class ExpressionConverter<T>
    {
        private static class ExpressionTransformer<TFrom, TTo>
        {
            private class Visitor : System.Linq.Expressions.ExpressionVisitor
            {
                private ParameterExpression _parameter;

                public Visitor(ParameterExpression parameter)
                {
                    _parameter = parameter;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    return _parameter;
                }
            }

            internal static Expression<Func<TTo, U>> Tranform<U>(Expression<Func<TFrom, U>> expression)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TTo));
                Expression body = new Visitor(parameter).Visit(expression.Body);
                return Expression.Lambda<Func<TTo, U>>(body, parameter);
            }

            internal static Expression<Func<U,TTo>> TranformReturnType<U>(Expression<Func<U, TFrom>> expression)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TTo));
               
                Expression body = Expression.Convert(expression.Body, typeof(object));
                return Expression.Lambda<Func<U, TTo>>(body, parameter);
            }
        }


        public static Expression<Func<T1, U>> ConvertExpressionType<T1, U>(Expression<Func<T, U>> filter)
        {
            if (filter == null) return null;
            var newExpression = ExpressionTransformer<T, T1>.Tranform(filter);
            return newExpression;
        }

        public static Expression<Func<U, T1>> ConvertExpressionReturnType<U, T1>(Expression<Func<U, T>> filter)
        {
            if (filter == null) return null;
            var newExpression = ExpressionTransformer<T, T1>.TranformReturnType(filter);
            return newExpression;
        }


    }
}
