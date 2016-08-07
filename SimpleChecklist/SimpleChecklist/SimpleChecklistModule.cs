using Autofac;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Workspaces;
using SimpleChecklist.ViewModels;
using SimpleChecklist.Views;
using Xamarin.Forms;

namespace SimpleChecklist
{
    public class SimpleChecklistModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PortableApp>().SingleInstance();

            builder.RegisterType<MainView>().SingleInstance();

            builder.RegisterType<WorkspacesManager>().SingleInstance();

            builder.RegisterType<TaskListView>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.TaskList)
                .SingleInstance();

            builder.RegisterType<DoneListView>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.DoneList)
                .SingleInstance();

            builder.RegisterType<SettingsView>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.Settings)
                .SingleInstance();

            builder.RegisterType<AboutView>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.About)
                .SingleInstance();

            builder.RegisterType<FilePickerView>();

            builder.RegisterType<AboutViewModel>().SingleInstance();
            builder.RegisterType<TaskListViewModel>().SingleInstance();
            builder.RegisterType<DoneListViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<TaskListObservableCollection>().SingleInstance();
            builder.RegisterType<DoneListObservableCollection>().SingleInstance();
            builder.RegisterType<FilePickerViewModel>().SingleInstance();

            builder.RegisterType<TaskMainPreviewWorkspace>()
                .As<IBaseWorkspace>()
                .SingleInstance();
            builder.RegisterType<DoneListWorkspace>()
                .As<IBaseWorkspace>()
                .SingleInstance();
            builder.Register(context => new BaseWorkspace(ViewsId.About))
                .As<IBaseWorkspace>()
                .SingleInstance();
        }
    }
}
