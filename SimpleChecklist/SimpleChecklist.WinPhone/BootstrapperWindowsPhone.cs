using Autofac;
using SimpleChecklist.Models;

namespace SimpleChecklist.WinPhone
{
    public static class BootstrapperWindowsPhone
    {
        public static SimpleChecklist.App Configure()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistModule>();

            var container = containerBuilder.Build();
            return container.Resolve<SimpleChecklist.App>();
        }
    }
}