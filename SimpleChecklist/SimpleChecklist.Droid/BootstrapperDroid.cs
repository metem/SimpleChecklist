using Autofac;

namespace SimpleChecklist.Droid
{
    public static class BootstrapperDroid
    {
        public static PortableApp Configure()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            containerBuilder.RegisterType<DroidFileUtils>().AsImplementedInterfaces();

            var container = containerBuilder.Build();

            return container.Resolve<PortableApp>();
        }
    }
}