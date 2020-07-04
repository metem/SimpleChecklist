using Autofac;
using SimpleChecklist.Core;
using SimpleChecklist.Core.Commands;
using SimpleChecklist.UI.ViewModels;
using SimpleChecklist.UI.Views;
using Xamarin.Forms;

namespace SimpleChecklist.UI
{
    public class SimpleChecklistUIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SimpleChecklistCoreModule>();

            builder.RegisterType<PortableApp>();

            builder.RegisterType<MainView>()
                .SingleInstance();

            builder.RegisterType<TabbedView>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ContentPage>());

            builder.RegisterType<AboutViewModel>();
            builder.RegisterType<OpenFilePickerViewModel>();
            builder.RegisterType<SaveFilePickerViewModel>();
            builder.RegisterType<TaskListViewModel>().SingleInstance();
            builder.RegisterType<DoneListViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ICommand>());
        }
    }
}