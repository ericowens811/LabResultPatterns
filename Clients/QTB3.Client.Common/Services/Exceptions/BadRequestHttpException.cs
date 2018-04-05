using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QTB3.Client.Common.Services.Exceptions
{
    public class BadRequestHttpException : Exception
    {
        public readonly Dictionary<string, ReadOnlyCollection<string>> ErrorDictionary;

        public BadRequestHttpException()
        {
        }

        public BadRequestHttpException(string message)
        :base(message)
        {

        }

        public BadRequestHttpException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public BadRequestHttpException(Dictionary<string, ReadOnlyCollection<string>> errorDictionary)
        {
            ErrorDictionary = errorDictionary;
        }
    }
}
