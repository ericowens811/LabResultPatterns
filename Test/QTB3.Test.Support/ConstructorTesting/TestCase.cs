
using System.Reflection;

namespace QTB3.Test.Support.ConstructorTesting
{
    public abstract class TestCase<T>
    {
        private readonly ConstructorInfo _constructor;
        private readonly object[] _arguments;
        private readonly string _failMessage;

        protected TestCase(ConstructorInfo ctor, object[] args, string failMessage)
        {
            this._constructor = ctor;
            this._arguments = args;
            this._failMessage = failMessage;
        }

        protected T InvokeConstructor()
        {
            try
            {
                return (T)this._constructor.Invoke(this._arguments);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        protected string Fail(string msg)
        {
            return string.Format("Test failed ({0}): {1}", this._failMessage, msg);
        }

        protected string Success()
        {
            return string.Empty;
        }

        public abstract string Execute();

    }
}
