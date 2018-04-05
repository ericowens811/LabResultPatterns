using System;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.ViewModels;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Client.LabResultPatterns.Common.UomComponents
{
    public class UomItemViewModel: ViewModelBase, IItemViewModel<Uom>
    {
        public const string IdMustBeGreaterThan0Message = "Id must be > 0";

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private readonly ISaveItemService<Uom> _saveService;
        private readonly IReadItemService<Uom> _readService;
        private Uom _item;
        private int _id;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _nameErrors;
        public string NameErrors
        {
            get => _nameErrors;
            set
            {
                SetProperty(ref _nameErrors, value);
                HasNameErrors = !string.IsNullOrWhiteSpace(_nameErrors);
            }
        }

        private bool _hasNameErrors;
        public bool HasNameErrors
        {
            get => _hasNameErrors;
            set => SetProperty(ref _hasNameErrors, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _descriptionErrors;
        public string DescriptionErrors
        {
            get => _descriptionErrors;
            set
            {
                SetProperty(ref _descriptionErrors, value);
                HasDescriptionErrors = !string.IsNullOrWhiteSpace(_descriptionErrors);
            }
        }

        private bool _hasDescriptionErrors;
        public bool HasDescriptionErrors
        {
            get => _hasDescriptionErrors;
            set => SetProperty(ref _hasDescriptionErrors, value);
        }

        private void ClearErrors()
        {
            NameErrors = null;
            DescriptionErrors = null;
        }

        private void UpdateItem(Uom item)
        {
            _item = item;
            Name = item.Name;
            Description = item.Description;
            Title = item.Id != 0 ? 
                $"{LrpConstants.TitleEdit}{LrpConstants.UomTitleRoot}" : 
                $"{LrpConstants.TitleAdd}{LrpConstants.UomTitleRoot}";
        }

        public UomItemViewModel
        (
            ISaveItemService<Uom> saveService, 
            IReadItemService<Uom> readService
        )
        {
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            _readService = readService ?? throw new ArgumentNullException(nameof(readService));
        }

        public async Task GetItemAsync(int id)
        {
            ClearErrors();
            _id = id;
            if (id != 0)
            {
                var item = await _readService.ReadItemAsync(id);
                UpdateItem(item);
            }
            else
            {
                var item = new Uom(0,string.Empty,string.Empty);
                UpdateItem(item);
            }
        }

        public virtual async Task SaveAsync()
        {
            ClearErrors();
            try
            {
                var newItem = new Uom(_item.Id, Name, Description);
                await _saveService.SaveItemAsync(newItem);
            }
            catch (BadRequestHttpException e)
            {
                var errorDictionary = e.ErrorDictionary;
                if (errorDictionary.Count == 0)
                {
                    throw new Exception(LrpConstants.NotSavedNotDeserialized);
                }
                foreach (var entry in errorDictionary)
                {
                    if (entry.Key == nameof(_item.Name))
                    {
                        var nameErrors = string.Join(" ", errorDictionary[nameof(_item.Name)]);
                        if (string.IsNullOrWhiteSpace(nameErrors))
                        {
                            throw new Exception(LrpConstants.NotSavedValidationProblem);
                        }
                        NameErrors = nameErrors;
                    }
                    else if (entry.Key == nameof(_item.Description))
                    {
                        var descriptionErrors = string.Join(" ", errorDictionary[nameof(_item.Description)]);
                        if (string.IsNullOrWhiteSpace(descriptionErrors))
                        {
                            throw new Exception(LrpConstants.NotSavedValidationProblem);
                        }
                        DescriptionErrors = descriptionErrors;
                    }
                    else
                    {
                        throw new Exception(LrpConstants.NotSavedUnexpectedErrors);
                    }
                }
                throw;
            }
        }
    }
}
