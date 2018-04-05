using System;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Model.Abstractions;

namespace QTB3.Test.LabResultPatterns.ClientModel.Pagebar
{
    public class PagebarModel<TItem>
    {
        public bool IsVisible { get; set; }
        public bool PageForwardIsVisible { get; set; }
        public bool PageBackIsVisible { get; set; }
        public bool PageForwardDisabledIsVisible { get; set; }
        public bool PageBackDisabledIsVisible { get; set; }
        public string PageText { get; set; }

        public void Update(IPage<TItem> page)
        {
            IsVisible = page.TotalCount > LrpConstants.PageSize;
            PageForwardIsVisible = page.Skip + page.Take < page.TotalCount;
            PageForwardDisabledIsVisible = !PageForwardIsVisible;
            PageBackIsVisible = page.Skip > 0;
            PageBackDisabledIsVisible = !PageBackIsVisible;
            PageText = PagingTextCalculator.Calculate(page.Skip, page.Take, page.TotalCount);
        }
    }
}
