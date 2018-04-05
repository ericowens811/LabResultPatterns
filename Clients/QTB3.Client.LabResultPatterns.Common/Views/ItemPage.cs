using QTB3.Model.Abstractions;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Views
{
    public partial class ItemPage<TItem>
        where TItem : class, IEntity
    {
        protected ToolbarItem SaveToolbarItem;

        private const string SaveToolName = "Save";
        private const string SaveIcon = "material-save.png";

        protected virtual void CreateToolbarItems()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    SaveToolbarItem = new ToolbarItem
                    {
                        Text = SaveToolName,
                        Icon = SaveIcon
                    };
                    break;
                case Device.iOS:
                    SaveToolbarItem = new ToolbarItem
                    {
                        Text = SaveToolName
                    };
                    break;
                case "Test":
                    SaveToolbarItem = new ToolbarItem
                    {
                        Text = SaveToolName
                    };
                    break;
            }
        }

        protected virtual void LayoutToolbarItems()
        {
            ToolbarItems.Add(SaveToolbarItem);
        }

        public void CreatePage()
        {
            CreateToolbarItems();
            LayoutToolbarItems();
        }
    }
}
