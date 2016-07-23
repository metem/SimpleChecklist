using Autofac;

namespace SimpleChecklist.Universal
{
    public static class BootstrapperUniversal
    {
        public static App Configure()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            containerBuilder.RegisterType<UniversalDialogUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<AppUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<UniversalFile>().AsImplementedInterfaces();

            containerBuilder.RegisterType<MainWindowsPage>().SingleInstance();
            containerBuilder.RegisterType<App>().SingleInstance();

            var container = containerBuilder.Build();

            return container.Resolve<App>();
        }
    }
}