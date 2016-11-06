﻿using Autofac;
using SimpleChecklist.Core.Commands;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.Core.Repositories.v1_3;
using SimpleChecklist.Core.Workflow;

namespace SimpleChecklist.Core
{
    public class SimpleChecklistCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorkflowManager>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterType<XmlFileApplicationRepository>()
                .As<IApplicationRepository>()
                .As<IFileApplicationRepository>()
                .SingleInstance();

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
