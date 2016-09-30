using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Workspaces;
using SimpleChecklist.Views;
using Xamarin.Forms;
using Module = Autofac.Module;

namespace SimpleChecklist
{
    public class SimpleChecklistModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PortableApp>();

            builder.RegisterType<WorkspacesManager>()
                .SingleInstance();

            builder.Register(context => new NavigationPageAdapter(context.Resolve<MainView>()))
                .As<INavigationService>();

            builder.RegisterType<MainView>()
                .SingleInstance();

            builder.RegisterType<TabbedView>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ContentPage>());

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<Screen>());

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<IWorkspace>())
                .As<IWorkspace>();

            builder.RegisterType<TaskListObservableCollection>()
                .SingleInstance();

            builder.RegisterType<DoneListObservableCollection>()
                .SingleInstance();
        }
    }
}