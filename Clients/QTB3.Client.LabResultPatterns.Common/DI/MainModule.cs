using Autofac;
using QTB3.Client.Abstractions.Controllers;
using QTB3.Client.Abstractions.Linking;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.LinkService;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Abstractions.Services.Validation;
using QTB3.Client.Abstractions.ViewModels;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.HttpService;
using QTB3.Client.Common.Services.LinkService;
using QTB3.Client.Common.Services.Serialization;
using QTB3.Client.Common.Services.Validation;
using QTB3.Client.LabResultPatterns.Abstractions.LoginComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MvcBuilders;
using QTB3.Client.LabResultPatterns.Common.Configuration;
using QTB3.Client.LabResultPatterns.Common.Controllers;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;
using QTB3.Client.LabResultPatterns.Common.MainPageComponents;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Client.LabResultPatterns.Common.ViewModels;
using QTB3.Client.LabResultPatterns.Common.Views;
using QTB3.Client.LabResultPatterns.Common.Linking;
using QTB3.Client.LabResultPatterns.Common.MvcBuilders;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Client.LabResultPatterns.Common.DI
{
    public class MainModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register type X<> implements I<>
            // When constructor needs I<T> it gets X<T>
            builder.RegisterGeneric(typeof(ItemPageController<>)).As(typeof(IItemPageController<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(CollectionPage<>)).As(typeof(ICollectionPage<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(CollectionPageController<>)).As(typeof(ICollectionPageController<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(CollectionPageViewModel<>)).As(typeof(ICollectionPageViewModel<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(CollectionMvcBuilder<>)).As(typeof(ICollectionMvcBuilder<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(ItemMvcBuilder<>)).As(typeof(IItemMvcBuilder<>)).InstancePerDependency();

            // Register type X implements I<T>
            // When constructor needs I<T> it gets X
            builder.RegisterType<UomItemViewModel>().As<IItemViewModel<Uom>>().InstancePerDependency();
            builder.RegisterType<UomItemPage>().As<IItemPage<Uom>>().InstancePerDependency();
            builder.RegisterType<UomViewCell>().As<IListItem<Uom>>().InstancePerDependency();

            // Register type X implements I
            // When constructor needs I it gets X
            builder.RegisterType<HttpRequestBuilder>().As<IHttpRequestBuilder>().InstancePerDependency();
            builder.RegisterType<JsonContentDeserializer>().As<IContentDeserializer>().InstancePerDependency();
            builder.RegisterType<JsonContentSerializer>().As<IContentSerializer>().InstancePerDependency();
            builder.RegisterType<Validator>().As(typeof(IValidator)).InstancePerDependency();
            builder.RegisterType<LinkService>().As(typeof(ILinkService)).InstancePerDependency();

            builder.RegisterType<LoginPageController>().As(typeof(ILoginPageController)).SingleInstance();            
            builder.RegisterType<LoginPage>().As<ILoginPage>().InstancePerDependency();

            builder.RegisterType<MainPage>().As<IMainPage>().InstancePerDependency();
            builder.RegisterType<MainPageViewModel>().As<IMainPageViewModel>();
            builder.RegisterType<MainPageController>().As<IMainPageController>().InstancePerDependency();

            // Links is common across the application
            builder.RegisterType<Links>().As<ILinks>().SingleInstance();
        }
    }
}
