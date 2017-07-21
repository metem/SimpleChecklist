using Autofac;
using SimpleChecklist.UI;

namespace SimpleChecklist.Universal
{
    public static class BootstrapperUniversal
    {
        public static IContainer Configure()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistUIModule>();

            containerBuilder.RegisterType<UniversalDialogUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<AppUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<UniversalFile>().AsImplementedInterfaces();

            containerBuilder.RegisterType<MainWindowsPage>();

            return containerBuilder.Build();
        }
    }
}