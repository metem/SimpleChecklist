﻿using Autofac;
using SimpleChecklist.UI;

namespace SimpleChecklist.Droid
{
    public static class BootstrapperDroid
    {
        public static IContainer Configure()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SimpleChecklistUIModule>();

            containerBuilder.RegisterType<DroidDialogUtils>().AsImplementedInterfaces().AsSelf().SingleInstance();
            containerBuilder.RegisterType<AppUtils>().AsImplementedInterfaces();
            containerBuilder.RegisterType<DroidFile>().AsImplementedInterfaces();
            containerBuilder.RegisterType<DroidDirectory>().AsImplementedInterfaces();

            containerBuilder.RegisterType<MainActivity>().SingleInstance();

            return containerBuilder.Build();
        }
    }
}