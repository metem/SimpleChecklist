using System;
using Autofac;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Commands;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.Core.Repositories;
using SimpleChecklist.Core.Workflow;
using SimpleChecklist.LegacyDataRepository;

namespace SimpleChecklist.Core
{
    public class SimpleChecklistCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<LegacyDataRepositoryModule>();

            builder.RegisterType<WorkflowManager>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<RootRepository>()
                .Named<IRepository>("repository");

            builder.RegisterDecorator<IRepository>(
                (c, repository) => new FileRepository(repository, c.Resolve<Func<string, IFile>>()),
                "repository",
                "fileRepository");

            builder.RegisterDecorator<IRepository>(
                    (c, repository) => new FileRepositoryCache(repository, c.Resolve<Func<string, IFile>>()),
                    "legacyFileRepository")
                .SingleInstance();

            builder.RegisterType<ApplicationData>().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ICommand>());

            builder.RegisterType<InitializationWorkflow>()
                .Keyed<IWorkflow>(WorkflowIds.Initialization)
                .SingleInstance();

            builder.RegisterType<InitializationFromBackupWorkflow>()
                .Keyed<IWorkflow>(WorkflowIds.InitializationFromBackup)
                .SingleInstance();

            builder.RegisterType<MainWorkflow>()
                .Keyed<IWorkflow>(WorkflowIds.Main)
                .SingleInstance();

            builder.RegisterType<ShuttingDownWorkflow>()
                .Keyed<IWorkflow>(WorkflowIds.ShutdownStarted)
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<IMessage>());

            builder.RegisterType<MessagesStream>()
                .SingleInstance();
        }
    }
}
