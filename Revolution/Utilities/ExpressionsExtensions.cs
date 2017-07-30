using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Expression = System.Linq.Expressions.Expression;

namespace Utilities
{
    public static class ExpressionsExtensions
    {

        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }

        public static TValue GetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    return (TValue)property.GetValue(target, null);
                }
            }
            return default(TValue);
        }


        public static string GetMemberName(
            this Expression expression)
        {
            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;
                if (memberExpression.Expression.NodeType ==
                    ExpressionType.MemberAccess)
                    return GetMemberName(memberExpression.Expression) + "." + memberExpression.Member.Name;
                return memberExpression.Member.Name;
            }
            if (expression is LambdaExpression)
            {
                var lambdaExpression = (LambdaExpression)expression;
                return GetMemberName(lambdaExpression.Body);
            }
            if (expression is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression;
                if (unaryExpression.NodeType != ExpressionType.Convert)
                    throw new Exception($"Cannot interpret member from {expression}");
                return GetMemberName(unaryExpression.Operand);
            }
            throw new Exception($"Could not determine member from {expression}");
        }

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof (TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }

        public class ReturnTypeVisitor<TSource, TReturnValue> : ExpressionVisitor
        {

            public Expression VisitLambda<T>(Expression<T> node)
            {
                var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), typeof(TReturnValue));
                var parameter = Expression.Parameter(typeof(TSource),  node.GetMemberName());
                var body = Expression.Property(parameter, node.GetMemberName());
                return Expression.Lambda(delegateType, body, node.Parameters);
            }

            public Expression VisitMember(MemberExpression node)
            {
                if (node.Member.DeclaringType == typeof(TSource))
                {
                    return Expression.Property(Visit(node.Expression), node.Member.Name);
                }
                return base.Visit(node);
            }
        }

        public static Expression<Func<T,object>> BuildForType<T>(Type sourceType, string propertyName)
        {

            var oexp = Build(sourceType, propertyName, out Type rtype);
            var fexp = typeof(ExpressionConverter<>).MakeGenericType(rtype).GetMethod("ConvertExpressionReturnType")
                .MakeGenericMethod(sourceType,typeof(object)).Invoke(null, new object[] { oexp});
            return (Expression<Func<T, object>>)fexp;
        }

        public static dynamic Build(Type sourceType, string propertyName, out Type rType)
        {
            var propInfo = sourceType.GetProperty(propertyName);
            if (propInfo == null && sourceType.IsInterface) propInfo = sourceType.GetInterfaces().SelectMany(x => x.GetProperties()).FirstOrDefault(x => x.Name == propertyName);

            var parameter = Expression.Parameter(sourceType, "x");

            var property = Expression.Property(parameter, propInfo);

            var delegateType = typeof(Func<,>)
                .MakeGenericType(sourceType, propInfo.PropertyType);//
            rType = propInfo.PropertyType;
            var lambda = GetExpressionLambdaMethod()
                .MakeGenericMethod(delegateType)
                .Invoke(null, new object[] { property, new[] { parameter } });

           

          

            return lambda ; //lambda;
        }

        public static MethodInfo GetExpressionLambdaMethod()
        {
            return typeof(Expression)
                .GetMethods()
                .Where(m => m.Name == "Lambda")
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments()
                })
                .Where(x => x.Params.Length == 2
                            && x.Args.Length == 1
                )
                .Select(x => x.Method)
                .First();
        }
    }
}
