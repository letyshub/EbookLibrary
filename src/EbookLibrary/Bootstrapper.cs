using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EbookLibrary.ViewModels;

namespace EbookLibrary
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            this.container
                    .Singleton<IWindowManager, WindowManager>()
                    .Singleton<IEventAggregator, EventAggregator>()
                    .Singleton<IDatabaseService, DatabaseService>()
                    .Singleton<IFileService, FileService>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => this.container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("Model") && !type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(modelType => this.container.RegisterPerRequest(modelType, modelType.ToString(), modelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return this.container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }
    }
}
