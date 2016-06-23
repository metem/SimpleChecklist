using Autofac;
using SimpleChecklist.Models;

namespace SimpleChecklist.Droid
{
    public static class BootstrapperWindows
    {
        public static App Configure()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            var container = containerBuilder.Build();
            return container.Resolve<App>();
        }
    }
}