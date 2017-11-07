using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Utilities
{
    public static class FuncExtensions
    {
         /// <summary>
    ///     Converts <see cref="Func{object, object}" /> to <see cref="Func{T, TResult}" />.
    /// </summary>
    public static Delegate Convert<T1,T2,R>(this Func<T1, T2,R> func, Type argType, Type argType2, Type resultType)
    {
        // If we need more versions of func then consider using params Type as we can abstract some of the
        // conversion then.

        Contract.Requires(func != null);
        Contract.Requires(resultType != null);

        var param = Expression.Parameter(argType);
        var param2 = Expression.Parameter(argType2);
        var convertedParam = new Expression[] { Expression.Convert(param, typeof(T1)), Expression.Convert(param2, typeof(T2))};

        
        

            // This is gnarly... If a func contains a closure, then even though its static, its first
            // param is used to carry the closure, so its as if it is not a static method, so we need
            // to check for that param and call the func with it if it has one...
            Expression call;
        call = Expression.Convert(
            func.Target == null
            ? Expression.Call(func.Method, convertedParam)
            : Expression.Call(Expression.Constant(func.Target), func.Method, convertedParam), resultType);

        var delegateType = typeof(Func<,,>).MakeGenericType(argType,argType2, resultType);
        return Expression.Lambda(delegateType, call, param, param2).Compile();
    }

        public static Delegate Convert<T1, R>(this Func<T1, R> func, Type argType)
        {
           var param = Expression.Parameter(argType);
           var convertedParam = new Expression[] { Expression.Convert(param, typeof(T1))};
            
            var call = Expression.Convert(
                func.Target == null || func.Target is Closure
                    ? (Expression) Expression.Invoke(Expression.Constant(func),convertedParam[0])// this path causes the error
                    : Expression.Call(Expression.Constant(func.Target), func.Method, convertedParam), typeof(R));

            var delegateType = typeof(Func<,>).MakeGenericType(argType, typeof(R));
            var lambda = Expression.Lambda(delegateType, call, param);
            return lambda.Compile();// BUG: 'MethodInfo must be a runtime MethodInfo object.

        }



        public static Delegate Convert<T1,T2>(this Action<T1,T2> action, Type argType, Type argType2)
        {
            // If we need more versions of func then consider using params Type as we can abstract some of the
            // conversion then.

            Contract.Requires(action != null);
            
            var param = Expression.Parameter(argType);
            var param2 = Expression.Parameter(argType2);
            var convertedParam = new Expression[] { Expression.Convert(param, typeof(T1)), Expression.Convert(param2, typeof(T2)) };




            // This is gnarly... If a func contains a closure, then even though its static, its first
            // param is used to carry the closure, so its as if it is not a static method, so we need
            // to check for that param and call the func with it if it has one...
            Expression call;
            call = Expression.Convert(
                action.Target == null
                ? Expression.Call(action.Method, convertedParam)
                : Expression.Call(Expression.Constant(action.Target), action.Method, convertedParam),typeof(void));

            var delegateType = typeof(Action<,>).MakeGenericType(argType, argType2);
            return Expression.Lambda(delegateType, call, param, param2).Compile();
        }
    }
}
