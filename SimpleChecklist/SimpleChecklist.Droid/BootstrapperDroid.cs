using Autofac;

namespace SimpleChecklist.Droid
{
    public static class BootstrapperDroid
    {
        public static PortableApp Configure()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            containerBuilder.RegisterType<DroidDialogUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<AppUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<DroidFile>().AsImplementedInterfaces();

            var container = containerBuilder.Build();

            return container.Resolve<PortableApp>();
        }
    }
}