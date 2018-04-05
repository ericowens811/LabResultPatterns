
using System;

namespace QTB3.Client.Common.Services.Exceptions
{
    public class LinksException : Exception
    {
        public LinksException()
        {
        }

        public LinksException(string message) : base(message)
        {
        }

        public LinksException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
