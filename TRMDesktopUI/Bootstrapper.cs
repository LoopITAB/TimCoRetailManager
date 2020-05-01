
using Caliburn.Micro;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using TRMDesktopUI.ViewModels;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Library.Helpers;
using AutoMapper;
using TRMDesktopUI.Models;

namespace TRMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");
        }

        private IMapper ConfigureAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }

        protected override void Configure()
        {
            //// AUTOMAPPER
            //// uses some reflection at the startup.
            //// This is the automapper configuration, step 1)
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<ProductModel, ProductDisplayModel>();
            //    cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            //});
            //// Now create the mapper, step 2)
            //var mapper = config.CreateMapper();
            //// Now add to Dependency Injection System, our Container, step 3)
            //_container.Instance(mapper);
            ////_container.Instance<IMapper>(mapper); it is redundant !!
            //// Use Constructor DI to use the mapper, step 4)
            //// Use mapper.Map<TargetStructure>(Source Object); step 5)
            // Above part was extracted to it's own method, ConfigureAutomapper.

            _container.Instance(ConfigureAutomapper());

            _container.Instance(_container)
                .PerRequest<IProductEndPoint, ProductEndPoint>()
                .PerRequest<ISaleEndPoint, SaleEndPoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IAPIHelper, APIHelper>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
            // we don't create interfaces for each ViewModel for now!
            // that's why we have viewModelType in both places above!


        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}