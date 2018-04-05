using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.PagingComponents;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Views
{
    public partial class CollectionPage<TItem>
    {
        public ToolbarItem DeleteToolbarItem;
        public ToolbarItem EditToolbarItem;
        public ToolbarItem AddToolbarItem;

        public SearchBar  SearchBar { get; private set; }
        public ListView ItemsListView;
        public Pagebar Pagebar;

        private const string DeleteToolName = "Delete";
        private const string EditToolName = "Edit";
        private const string AddToolName = "Add";

        private const string DeleteIcon = "material-minus.png";
        private const string EditIcon = "material-pen.png";
        private const string AddIcon = "material-plus.png";

        protected virtual void CreateToolbarItems()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    DeleteToolbarItem = new ToolbarItem
                    {
                        Text = DeleteToolName,
                        Icon = DeleteIcon
                    };

                    EditToolbarItem = new ToolbarItem
                    {
                        Text = EditToolName,
                        Icon = EditIcon
                    };

                    AddToolbarItem = new ToolbarItem
                    {
                        Text = AddToolName,
                        Icon = AddIcon
                    };
                    break;
                case Device.iOS:
                    DeleteToolbarItem = new ToolbarItem
                    {
                        Text = DeleteToolName
                    };

                    EditToolbarItem = new ToolbarItem
                    {
                        Text = EditToolName
                    };

                    AddToolbarItem = new ToolbarItem
                    {
                        Text = AddToolName
                    };
                    break;
                case "Test":
                    DeleteToolbarItem = new ToolbarItem
                    {
                        Text = DeleteToolName
                    };

                    EditToolbarItem = new ToolbarItem
                    {
                        Text = EditToolName
                    };

                    AddToolbarItem = new ToolbarItem
                    {
                        Text = AddToolName
                    };
                    break;
            }
        }

        protected virtual void LayoutToolbarItems()
        {
            ToolbarItems.Add(DeleteToolbarItem);
            ToolbarItems.Add(EditToolbarItem);
            ToolbarItems.Add(AddToolbarItem);
        }

        protected virtual void CreateSearchBar()
        {
            SearchBar = new SearchBar { Placeholder = LrpConstants.SearchFor };
            if (Device.RuntimePlatform == Device.iOS)
            {
                var effect = Effect.Resolve(LrpConstants.DoneButtonEffect);
                SearchBar.Effects.Add(effect);
            }
        }

        protected virtual void CreateListView()
        {
            ItemsListView =
                new ListView(ListViewCachingStrategy.RecycleElement)
                {
                    HasUnevenRows = true,
                    IsPullToRefreshEnabled = true,
                };
        }

        protected virtual void CreatePagebar()
        {
            Pagebar = new Pagebar();
        }

        protected void CreatePage()
        {
            CreateToolbarItems();
            LayoutToolbarItems();
            CreateSearchBar();
            CreateListView();
            CreatePagebar();
            var layout = new RelativeLayout();
            layout.Children.Add
            (
                SearchBar,
                Constraint.RelativeToParent(parent => 15),
                Constraint.RelativeToParent(parent => 10),
                Constraint.RelativeToParent(parent => parent.Width - 30)
            );

            layout.Children.Add
            (
                ItemsListView,
                Constraint.RelativeToParent(parent => 15),
                Constraint.RelativeToView(SearchBar, (l, v) => v.Height + 10),
                Constraint.RelativeToParent(parent => parent.Width - 30),
                Constraint.RelativeToView(SearchBar, (l, v) => l.Height - v.Height - 60)
            );

            layout.Children.Add
            (
                Pagebar,
                Constraint.RelativeToParent(parent => 0),
                Constraint.RelativeToParent(parent => parent.Height - 50),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.Constant(50)
            );
            Content = layout;
        }
    }
}
