using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class DynamicExpressionTests
    {
        [TestMethod]
        public void Parse_ParameterExpressionMethodCall_ReturnsIntExpression()
        {
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(int), "x") },
                typeof(int),
                "x + 1");
            Assert.AreEqual(typeof(int), expression.Type);
        }

        [TestMethod]
        public void Parse_TupleToStringMethodCall_ReturnsStringLambdaExpression()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(Tuple<int>),
                typeof(string),
                "it.ToString()");
            Assert.AreEqual(typeof(string), expression.ReturnType);
        }

        [TestMethod]
        public void ParseLambda_DelegateTypeMethodCall_ReturnsEventHandlerLambdaExpression()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(EventHandler),
                new[] { Expression.Parameter(typeof(object), "sender"),
                        Expression.Parameter(typeof(EventArgs), "e") },
                null,
                "sender.ToString()");

            Assert.AreEqual(typeof(void), expression.ReturnType);
            Assert.AreEqual(typeof(EventHandler), expression.Type);
        }

        [TestMethod]
        public void ParseLambda_VoidMethodCall_ReturnsActionDelegate()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(System.IO.FileStream),
                null,
                "it.Close()");
            Assert.AreEqual(typeof(void), expression.ReturnType);
            Assert.AreEqual(typeof(Action<System.IO.FileStream>), expression.Type);
        }

        [TestMethod]
        public void CreateClass_TheadSafe()
        {
            const int numOfTasks = 15;

            var properties = new[] { new DynamicProperty("prop1", typeof(string)) };

            var tasks = new List<Task>(numOfTasks);

            for (var i = 0; i < numOfTasks; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => DynamicExpression.CreateClass(properties)));
            }

            Task.WaitAll(tasks.ToArray());
        }

        [TestMethod]
        public void ParseLambda_GenericMethodCall_ReturnsGenericList()
        {
            ItMethodTest<A, List<string>>("it.GetName()");
        }

        [TestMethod]
        public void ParseLambda_RunExpression()
        {
            var a = new A();
            var result = DynamicExpression.EvaluatePredicateLambdaExpression("a.IsTrue()", Tuple.Create(nameof(a), a));
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ParseLambda_GenericMethodCall_ReturnsGenericNewList()
        {
            ItMethodTest<A, List<string>>("new List<string>()");
        }

        public void ItMethodTest<TTarget, TReturn>(string code)
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(TTarget),
                typeof(TReturn),
                code);
            Assert.AreEqual(typeof(TReturn), expression.ReturnType);
            Assert.AreEqual(typeof(Func<TTarget, TReturn>), expression.Type);
        }
    }

    class A
    {
        public List<string> GetName()
        {
            return new List<string>();
        }

        public bool IsTrue()
        {
            return true;
        }
    }
}
