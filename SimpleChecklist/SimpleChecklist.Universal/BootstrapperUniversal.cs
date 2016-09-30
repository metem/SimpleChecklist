using Autofac;

namespace SimpleChecklist.Universal
{
    public static class BootstrapperUniversal
    {
        public static IContainer Configure()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            containerBuilder.RegisterType<UniversalDialogUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<AppUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<UniversalFile>().AsImplementedInterfaces();

            containerBuilder.RegisterType<MainWindowsPage>();

            return containerBuilder.Build();
        }
    }
}