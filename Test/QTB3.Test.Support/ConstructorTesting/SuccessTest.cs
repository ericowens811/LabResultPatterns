
using System.Reflection;

namespace QTB3.Test.Support.ConstructorTesting
{
    public class SuccessTest<T> : TestCase<T>
    {
        public SuccessTest(ConstructorInfo ctor, object[] args, string failMessage)
            : base(ctor, args, failMessage)
        {
        }

        public override string Execute()
        {

            try
            {
                base.InvokeConstructor();
            }
            catch (System.Exception ex)
            {
                return base.Fail(string.Format("{0} occurred: {1}",
                    ex.GetType().Name, ex.Message));
            }

            return base.Success();

        }
    }
}
