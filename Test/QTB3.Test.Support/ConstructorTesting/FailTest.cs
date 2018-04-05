
using System;
using System.Reflection;

namespace QTB3.Test.Support.ConstructorTesting
{
    public class FailTest<T>: TestCase<T>
    {
        private readonly Type _exceptionType;

        public FailTest(ConstructorInfo ctor, object[] args, Type exceptionType, string failMessage)
            : base(ctor, args, failMessage)
        {
            this._exceptionType = exceptionType;
        }

        public override string Execute()
        {

            try
            {
                base.InvokeConstructor();
                return base.Fail(string.Format("{0} not thrown when expected.",
                    this._exceptionType.Name));
            }
            catch (System.Exception ex)
            {
                if (ex.GetType() != this._exceptionType)
                    return base.Fail(string.Format("{0} thrown when {1} was expected.",
                        ex.GetType().Name, this._exceptionType.Name));
            }

            return base.Success();

        }
    }
}
