using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq.Dynamic
{
    public static partial class DynamicExpression
    {
        public static void ExecuteLambdaExpression(string code, params Tuple<Type, string, object>[] args)
        {
            var arguments = new ParameterExpression[args.Length];
            for (int ii = 0; ii < args.Length; ii++)
            {
                var arg = args[ii];
                arguments[ii] = Expression.Parameter(arg.Item1, arg.Item2);
            }

            var expression = System.Linq.Dynamic.DynamicExpression.ParseLambda(arguments, typeof(void), code);
            var fn = expression.Compile();

            var result = fn.DynamicInvoke(args.Select(x => x.Item3).ToArray());
        }

        public static void ExecuteLambdaExpression<T1>(string code, Tuple<string, T1> t1)
        {
            ExecuteLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2));
        }
        
        public static void ExecuteLambdaExpression<T1, T2>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2)
        {
            ExecuteLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2));
        }
        
        public static void ExecuteLambdaExpression<T1, T2, T3>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3)
        {
            ExecuteLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2));
        }
        
        public static void ExecuteLambdaExpression<T1, T2, T3, T4>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3, Tuple<string, T4> t4)
        {
            ExecuteLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T4), t4.Item1, (object)t4.Item2));
        }


        public static TReturn EvaluateLambdaExpression<TReturn>(string code, params Tuple<Type, string, object>[] args)
        {
            var arguments = new ParameterExpression[args.Length];
            for (int ii = 0; ii < args.Length; ii++)
            {
                var arg = args[ii];
                arguments[ii] = Expression.Parameter(arg.Item1, arg.Item2);
            }

            var expression = System.Linq.Dynamic.DynamicExpression.ParseLambda(arguments, typeof(TReturn), code);
            var fn = expression.Compile();

            var result = fn.DynamicInvoke(args.Select(x => x.Item3).ToArray());

            return (TReturn)result;

            //var genericArguments = args.Select(x => x.Item1).Concat(new[] { typeof(TReturn) }).ToArray();
            //var funcType = typeof(Func<>).MakeGenericType(genericArguments);
            //var expressionType = typeof(Expression<>).MakeGenericType(funcType);

            //var result = fn(t1.Item1, t2.Item1);
            //return result;
        }
        
        public static TReturn EvaluateLambdaExpression<T1, TReturn>(string code, Tuple<string, T1> t1)
        {
            return EvaluateLambdaExpression<TReturn>(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2));
        }
        
        public static TReturn EvaluateLambdaExpression<T1, T2, TReturn>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2)
        {
            return EvaluateLambdaExpression<TReturn>(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2));
        }
        
        public static TReturn EvaluateLambdaExpression<T1, T2, T3, TReturn>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3)
        {
            return EvaluateLambdaExpression<TReturn>(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2));
        }
        
        public static TReturn EvaluateLambdaExpression<T1, T2, T3, T4, TReturn>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3, Tuple<string, T4> t4)
        {
            return EvaluateLambdaExpression<TReturn>(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2), Tuple.Create(typeof(T4), t4.Item1, (object)t4.Item2));
        }

        //bool EvaluateLambdaExpression<T>(Document doc, T arg, string argName, string code)
        //{
        //    var parameterDoc = Expression.Parameter(typeof(Document), "doc");
        //    var parameterParameters = Expression.Parameter(typeof(T), argName);
        //    var expression = (Expression<Func<T, Document, bool>>)System.Linq.Dynamic.DynamicExpression.ParseLambda(new[] { parameterParameters, parameterDoc }, typeof(bool), code);
        //    var fn = expression.Compile();
        //    var result = fn(arg, doc);
        //    return result;
        //}


        public static T EvaluateLambdaExpression<T>(string code, Tuple<string, T> args)
        {
            return EvaluateLambdaExpression<T, T>(code, Tuple.Create(args.Item1, args.Item2));
        }


        public static bool EvaluatePredicateLambdaExpression(string code, params Tuple<Type, string, object>[] args)
        {
            return EvaluateLambdaExpression<bool>(code, args);
        }

        public static bool EvaluatePredicateLambdaExpression<T1>(string code, Tuple<string, T1> t1)
        {
            return EvaluatePredicateLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2));
        }

        public static bool EvaluatePredicateLambdaExpression<T1, T2>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2)
        {
            return EvaluatePredicateLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2));
        }

        public static bool EvaluatePredicateLambdaExpression<T1, T2, T3>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3)
        {
            return EvaluatePredicateLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2));
        }

        public static bool EvaluatePredicateLambdaExpression<T1, T2, T3, T4>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3, Tuple<string, T4> t4)
        {
            return EvaluatePredicateLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2), Tuple.Create(typeof(T4), t4.Item1, (object)t4.Item2));
        }

        //

        //public static bool EvaluateFilterLambdaExpression(string code, params Tuple<Type, string, object>[] args)
        //{
        //    return EvaluateLambdaExpression<bool>(code, args);
        //}

        //public static bool EvaluateFilterLambdaExpression<T1>(string code, Tuple<string, T1> t1)
        //{
        //    return EvaluateFilterLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2));
        //}

        //public static bool EvaluateFilterLambdaExpression<T1, T2>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2)
        //{
        //    return EvaluateFilterLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2));
        //}

        //public static bool EvaluateFilterLambdaExpression<T1, T2, T3>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3)
        //{
        //    return EvaluateFilterLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2));
        //}

        //public static bool EvaluateFilterLambdaExpression<T1, T2, T3, T4>(string code, Tuple<string, T1> t1, Tuple<string, T2> t2, Tuple<string, T3> t3, Tuple<string, T4> t4)
        //{
        //    return EvaluateFilterLambdaExpression(code, Tuple.Create(typeof(T1), t1.Item1, (object)t1.Item2), Tuple.Create(typeof(T2), t2.Item1, (object)t2.Item2), Tuple.Create(typeof(T3), t3.Item1, (object)t3.Item2), Tuple.Create(typeof(T4), t4.Item1, (object)t4.Item2));
        //}

    }
}
