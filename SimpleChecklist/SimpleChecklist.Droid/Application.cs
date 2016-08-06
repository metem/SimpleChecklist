using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Runtime;
using Caliburn.Micro;
using Android.App;
using Autofac;
using SimpleChecklist.Models.Workspaces;

namespace SimpleChecklist.Droid
{
    [Application]
    public class Application : CaliburnApplication
    {
        private IContainer _container;
        private WorkspacesManager _workspacesManager;

        public Application(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            Initialize();

            Task.Run(async () => await _workspacesManager.LoadWorkspacesStateAsync()).Wait();
        }

        protected override void Configure()
        {
            _container = BootstrapperDroid.Configure();
            _workspacesManager = _container.Resolve<WorkspacesManager>();
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
    }
}