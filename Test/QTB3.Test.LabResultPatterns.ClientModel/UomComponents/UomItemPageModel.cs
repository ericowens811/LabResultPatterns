using System;
using System.Threading.Tasks;
using QTB3.Api.Abstractions.Repositories;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Test.LabResultPatterns.ClientModel.UomComponents
{
    public class UomItemPageModel
    {
        private readonly Func<IReadRepository<Uom>> _getReadRepository;
        private readonly Func<IWriteRepository<Uom>> _getWriteRepository;
        private int _uomId;
        private Uom _uom;
        private bool _itemIsBeingEdited;

        public UomItemPageModel
        (
            Func<IReadRepository<Uom>> getReadRepository,
            Func<IWriteRepository<Uom>> getWriteRepository
        )
        {
            _getReadRepository = getReadRepository;
            _getWriteRepository = getWriteRepository; 
        }

        public void SetUomId(int id)
        {
            _uomId= id;
        }

        public void SetItemIsBeingEdited(bool value)
        {
            _itemIsBeingEdited = value;
        }

        public void EnterName(string name)
        {
            _uom = _uom.WithName(name);
        }

        public void EnterDescription(string description)
        {
            _uom = _uom.WithDescription(description);
        }

        public async Task Save()
        {
            if (_itemIsBeingEdited)
            {
                await _getWriteRepository().PutAsync(_uom);
            }
            else
            {
                await _getWriteRepository().PostAsync(_uom);
            }
        }

        public async Task OnAppearingAsync()
        {
            if (_itemIsBeingEdited && _uomId != 0)
            {
                _uom = await _getReadRepository().GetAsync(_uomId);
            }
            else
            {
                _uom = new Uom(0,"","");
            }
        }

        public string Name => _uom.Name;
        public string Description => _uom.Description;
        public string TitleText => _itemIsBeingEdited ? 
            $"{LrpConstants.TitleEdit}{LrpConstants.UomTitleRoot}" : 
            $"{LrpConstants.TitleAdd}{LrpConstants.UomTitleRoot}";
    }
}
