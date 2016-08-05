using Autofac;

namespace SimpleChecklist.Droid
{
    public static class BootstrapperDroid
    {
        public static App Configure()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            containerBuilder.RegisterType<DroidDialogUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<AppUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<DroidFile>().AsImplementedInterfaces();
            containerBuilder.RegisterType<DroidDirectory>().AsImplementedInterfaces();

            containerBuilder.RegisterType<App>().SingleInstance();

            var container = containerBuilder.Build();

            return container.Resolve<App>();
        }
    }
}