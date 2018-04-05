using System;

namespace QTB3.Client.LabResultPatterns.Common.LoginComponents.Exceptions
{
    public class LoginRequestedException : Exception
    {
        public LoginRequestedException()
        {
        }

        public LoginRequestedException(string message) : base(message)
        {
        }

        public LoginRequestedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
