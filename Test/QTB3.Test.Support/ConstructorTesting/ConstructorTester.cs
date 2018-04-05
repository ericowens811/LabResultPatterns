
using System;
using System.Collections.Generic;
using System.Reflection;

namespace QTB3.Test.Support.ConstructorTesting
{
    public class ConstructorTester<T> : Tester<T>
    {
        private readonly ConstructorInfo _constructorInfo;
        private readonly IList<TestCase<T>> _testCases = new List<TestCase<T>>();

        public ConstructorTester(ConstructorInfo ctorInfo)
        {
            this._constructorInfo = ctorInfo;
        }

        public override Tester<T> Fail(object[] args, Type exceptionType, string failMessage)
        {
            TestCase<T> testCase = new FailTest<T>(this._constructorInfo, args, exceptionType, failMessage);
            _testCases.Add(testCase);
            return this;
        }

        public override Tester<T> Succeed(object[] args, string failMessage)
        {
            TestCase<T> testCase = new SuccessTest<T>(this._constructorInfo, args, failMessage);
            _testCases.Add(testCase);
            return this;
        }

        public override void Assert()
        {
            List<string> errors = new List<string>();
            ExecuteTestCases(errors);
            Assert(errors);
        }

        private void ExecuteTestCases(List<string> errors)
        {
            foreach (TestCase<T> testCase in this._testCases)
                ExecuteTestCase(errors, testCase);
        }

        private void ExecuteTestCase(List<string> errors, TestCase<T> testCase)
        {
            string error = testCase.Execute();
            if (!string.IsNullOrEmpty(error))
                errors.Add("    ----> " + error);
        }

        private void Assert(List<string> errors)
        {
            if (errors.Count > 0)
            {
                string error = string.Format("{0} error(s) occurred:\n{1}",
                    errors.Count,
                    string.Join("\n", errors.ToArray()));
                NUnit.Framework.Assert.Fail(error);
            }
        }

    }
}
