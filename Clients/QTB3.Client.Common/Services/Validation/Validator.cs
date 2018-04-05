using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using QTB3.Client.Abstractions.Services.Validation;

namespace QTB3.Client.Common.Services.Validation
{
    // Derived from
    // https://github.com/davidbritch/xamarin-forms/blob/master/Validation/MVVMUtopia/Validation/Validator.cs

    public class Validator : IValidator
    {
        public bool Validate(object entityToValidate, out Dictionary<string, ReadOnlyCollection<string>> errorDictionary)
        {
            if(entityToValidate == null) throw new ArgumentNullException(nameof(entityToValidate));

            errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>();
            var propertiesToValidate = entityToValidate.GetType()
                .GetRuntimeProperties()
                .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (var propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                ValidateProperty(propertyInfo, propertyErrors, entityToValidate);
                if (propertyErrors.Any())
                {
                    errorDictionary.Add(propertyInfo.Name, new ReadOnlyCollection<string>(propertyErrors));
                }
            }
            return errorDictionary.Values.Count == 0;
        }

        private void ValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors, object entityToValidate)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(entityToValidate) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(entityToValidate);

            System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(propertyValue, context, results);
            if (results.Any())
            {
                propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
            }
        }
    }
}


