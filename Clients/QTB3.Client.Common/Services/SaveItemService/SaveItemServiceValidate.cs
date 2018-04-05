using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.Services.Validation;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Common.Services.SaveItemService
{
    public class SaveItemServiceValidate<TItem> : ISaveItemService<TItem>
        where TItem : class, IEntity
    {
        private readonly IValidator _validator;
        private readonly ISaveItemService<TItem> _next;

        public SaveItemServiceValidate
        (
            IValidator validator,
            ISaveItemService<TItem> next
        )
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task SaveItemAsync(TItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!_validator.Validate(item, out var errorDictionary))
            {
                throw new BadRequestHttpException(errorDictionary);
            }
            await _next.SaveItemAsync(item);
        }
    }
}
