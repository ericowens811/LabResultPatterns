using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QTB3.Client.Abstractions.Services.Validation
{
    public interface IValidator
    {
        bool Validate(object entityToValidate, out Dictionary<string, ReadOnlyCollection<string>> errorDictionary);
    }
}
