using Autofac;
using SimpleChecklist.Common;
using SimpleChecklist.Core.Repositories;

namespace SimpleChecklist.Core
{
    public class SimpleChecklistCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileDataRepository>();
            builder.RegisterType<BackupRepository>();
            builder.RegisterType<Workspace>().SingleInstance();
            builder.RegisterType<DefaultDateTimeProvider>().As<IDateTimeProvider>();
        }
    }
}