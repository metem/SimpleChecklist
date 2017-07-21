using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Runtime;
using Caliburn.Micro;
using Android.App;
using Autofac;
using SimpleChecklist.UI.ViewModels;

namespace SimpleChecklist.Droid
{
    [Application]
    public class Application : CaliburnApplication
    {
        private IContainer _container;

        public Application(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            Initialize();
        }

        protected override void Configure()
        {
            _container = BootstrapperDroid.Configure();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.IsRegistered(service))
                    return _container.Resolve(service);
            }
            else
            {
                if (_container.IsRegisteredWithName(key, service))
                    return _container.ResolveNamed(key, service);
            }
            throw new Exception($"Could not resolve {key ?? service.Name}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
            {
                GetType().GetTypeInfo().Assembly,
                typeof(TabbedViewModel).GetTypeInfo().Assembly
            };
        }
    }
}