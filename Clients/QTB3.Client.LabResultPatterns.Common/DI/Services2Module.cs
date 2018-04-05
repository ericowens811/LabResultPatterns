using System;
using System.Runtime.CompilerServices;
using Autofac;
using QTB3.Api.LabResultPatterns.Abstractions;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.DeleteItemService;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.DeleteItemService;
using QTB3.Client.Common.Services.HttpService;
using QTB3.Client.Common.Services.ReadItemService;
using QTB3.Client.Common.Services.ReadPageService;
using QTB3.Client.Common.Services.SaveItemService;
using QTB3.Client.Common.Services.Serialization;
using QTB3.Client.Common.Services.UrlBuilding;
using QTB3.Client.Common.Services.Validation;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using Xamarin.Forms;
using HttpReadService = QTB3.Client.Common.Services.HttpService.HttpReadService;
using HttpWriteService = QTB3.Client.Common.Services.HttpService.HttpWriteService;
using IHttpReadService = QTB3.Client.Abstractions.Services.HttpService.IHttpReadService;
using IHttpWriteService = QTB3.Client.Abstractions.Services.HttpService.IHttpWriteService;

namespace QTB3.Client.LabResultPatterns.Common.DI
{
    public class Services2Module : Module
    {
        public const int UomPageSize = 20;

        protected IHttpService BuildHttpServiceCore(IHttpClient httpClient, IComponentContext context)
        {
            var httpServiceSendRequest = new HttpServiceSendRequest(httpClient);
            var acceptedMediaSource = new V1AcceptedMediaSource();
            var httpServiceAddAcceptHeader = new HttpServiceAddAcceptHeader(acceptedMediaSource, httpServiceSendRequest);
            var jwtTokenSource = context.Resolve<IJwtTokenSource>();
            return new HttpServiceAddAuthorizationHeader(jwtTokenSource, httpServiceAddAcceptHeader);
        }

        protected IHttpReadService BuildReadHttpService(IComponentContext context)
        {
            var core = BuildHttpServiceCore(context.Resolve<IHttpReadClient>(), context);
            return new HttpReadService(core);
        }

        protected IHttpWriteService BuildWriteHttpService(IComponentContext context)
        {
            var core = BuildHttpServiceCore(context.Resolve<IHttpWriteClient>(), context);
            return new HttpWriteService(core);
        }

        protected IReadPageService<TItem> BuildReadPageServiceCore<TItem>(IComponentContext context)
        {
            var httpService = BuildReadHttpService(context);
            var deserializer = new JsonContentDeserializer();
            return new ReadPageServiceBuildRequest<TItem>(httpService, deserializer);
        }

        protected IReadPageService<TItem> BuildReadPageService<TItem>(IComponentContext context)
        {
            var readPageServiceBuildRequest = BuildReadPageServiceCore<TItem>(context);
            return new ReadPageService<TItem>(readPageServiceBuildRequest);
        }

        protected IReadPageServiceNewPage<TItem> BuildReadPageServiceNewPage<TItem>(IComponentContext context)
        {
            var readPageServiceBuildRequest = BuildReadPageServiceCore<TItem>(context);
            var pageUrlBuilder = context.Resolve<IPageUrlBuilder<TItem>>();
            return new ReadPageServiceBuildUrl<TItem>(pageUrlBuilder, readPageServiceBuildRequest);
        }

        protected IReadItemService<TItem> BuildReadItemService<TItem>(IComponentContext context)
        {
            var httpService = BuildReadHttpService(context);
            var deserializer = new JsonContentDeserializer();
            var readItemServiceBuildRequest = new ReadItemServiceBuildRequest<TItem>(httpService, deserializer);
            var itemUrlBuilder = context.Resolve<IItemReadUrlBuilder<TItem>>();
            var readItemServiceBuildUrl = new ReadItemServiceBuildUrl<TItem>(itemUrlBuilder, readItemServiceBuildRequest);
            return new ReadItemService<TItem>(readItemServiceBuildUrl);
        }

        protected ISaveItemService<TItem> BuildSaveItemService<TItem>(IComponentContext context)
        where TItem : class, IEntity
        {
            var httpService = BuildWriteHttpService(context);
            var serializer = new JsonContentSerializer();
            var saveItemServiceBuildRequest = new SaveItemServiceBuildRequest<TItem>(httpService, serializer);
            var itemUrlBuilder = context.Resolve<IItemWriteUrlBuilder<TItem>>();
            var saveItemServiceBuildUrl = new SaveItemServiceBuildUrl<TItem>(itemUrlBuilder, saveItemServiceBuildRequest);
            var validator = new Validator();
            var saveItemServiceValidator = new SaveItemServiceValidate<TItem>(validator, saveItemServiceBuildUrl);
            return new SaveItemService<TItem>(saveItemServiceValidator);
        }

        protected IDeleteItemService<TItem> BuildDeleteItemService<TItem>(IComponentContext context)
        {
            var httpService = BuildWriteHttpService(context);
            var deleteItemServiceBuildRequest = new DeleteItemServiceBuildRequest(httpService);
            var itemUrlBuilder = context.Resolve<IItemWriteUrlBuilder<TItem>>();
            var deleteItemServiceBuildUrl = new DeleteItemServiceBuildUrl<TItem>(itemUrlBuilder, deleteItemServiceBuildRequest);
            return new DeleteItemService<TItem>(deleteItemServiceBuildUrl);
        }

        protected override void Load(ContainerBuilder builder)
        {
            //
            // Url building configuration
            //
            builder.RegisterType<LinkTemplateLookup>().As<ILinkTemplateLookup>().SingleInstance();

            builder.Register((c) =>
            {
                return new PageUrlBuilder<Uom>(LinkRelations.UomPageRelation, c.Resolve<ILinkTemplateLookup>(), UomPageSize);
            }).As<IPageUrlBuilder<Uom>>().InstancePerDependency();

            builder.Register((c) =>
            {
                return new ItemUrlBuilder<Uom>(LinkRelations.UomReadItemRelation, c.Resolve<ILinkTemplateLookup>());
            }).As<IItemReadUrlBuilder<Uom>>().InstancePerDependency();

            builder.Register((c) =>
            {
                return new ItemUrlBuilder<Uom>(LinkRelations.UomWriteItemRelation, c.Resolve<ILinkTemplateLookup>());
            }).As<IItemWriteUrlBuilder<Uom>>().InstancePerDependency();

            //
            // HttpService pipe
            //
            builder.Register(BuildReadHttpService).As<IHttpReadService>().InstancePerDependency();
            builder.Register(BuildWriteHttpService).As<IHttpWriteService>().InstancePerDependency();
            //
            // Client service pipes
            //
            builder.Register(BuildReadPageServiceNewPage<Uom>).As<IReadPageServiceNewPage<Uom>>().InstancePerDependency();
            builder.Register(BuildReadPageService<Uom>).As<IReadPageService<Uom>>().InstancePerDependency();
            builder.Register(BuildReadItemService<Uom>).As<IReadItemService<Uom>>().InstancePerDependency();
            builder.Register(BuildSaveItemService<Uom>).As<ISaveItemService<Uom>>().InstancePerDependency();
            builder.Register(BuildDeleteItemService<Uom>).As<IDeleteItemService<Uom>>().InstancePerDependency();
        }
    }
}
