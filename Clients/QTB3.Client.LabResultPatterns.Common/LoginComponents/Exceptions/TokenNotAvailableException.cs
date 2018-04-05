
using System;

namespace QTB3.Client.LabResultPatterns.Common.LoginComponents.Exceptions
{
    public class TokenNotAvailableException : Exception
    {
        public TokenNotAvailableException()
        {
        }

        public TokenNotAvailableException(string message) : base(message)
        {
        }

        public TokenNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
