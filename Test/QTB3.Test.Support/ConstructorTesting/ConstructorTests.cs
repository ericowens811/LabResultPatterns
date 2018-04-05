

using System;
using System.Reflection;

namespace QTB3.Test.Support.ConstructorTesting
{
    // http://codinghelmet.com/?path=howto/constructor-tests

    public static class ConstructorTests<T>
    {
        public static Tester<T> For(params Type[] argTypes)
        {

            ConstructorInfo ctorInfo = typeof(T).GetConstructor(argTypes);

            if (ctorInfo == null)
                return new MissingConstructorTester<T>();

            return new ConstructorTester<T>(ctorInfo);

        }
    }
}
