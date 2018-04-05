using System;
using System.Net;

namespace QTB3.Client.Common.Services.Exceptions
{
    public class FailedRequestException: Exception
    {
        public const string NoInformationMessage = "No information available";
        public HttpStatusCode StatusCode { get; private set; }

        public FailedRequestException
        (
            HttpStatusCode statusCode, 
            string message = NoInformationMessage
        ) : base(message)
        {
            StatusCode = statusCode;
        }

        public FailedRequestException()
        {
        }

        public FailedRequestException(string message) : base(message)
        {
        }

        public FailedRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
