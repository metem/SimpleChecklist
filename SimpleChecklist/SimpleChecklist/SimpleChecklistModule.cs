using Autofac;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Workspaces;
using SimpleChecklist.ViewModels;
using SimpleChecklist.Views;
using Xamarin.Forms;

namespace SimpleChecklist
{
    public class SimpleChecklistModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PortableApp>().SingleInstance();

            builder.RegisterType<MainPage>().SingleInstance();

            builder.RegisterType<WorkspacesManager>().SingleInstance();

            builder.RegisterType<TaskListPage>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.TaskList)
                .SingleInstance();

            builder.RegisterType<DoneListPage>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.DoneList)
                .SingleInstance();

            builder.RegisterType<SettingsPage>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.Settings)
                .SingleInstance();

            builder.RegisterType<AboutPage>()
                .AsSelf()
                .Keyed<ContentPage>(ViewsId.About)
                .SingleInstance();

            builder.RegisterType<FilePickerPage>();

            builder.RegisterType<AboutPageViewModel>().SingleInstance();
            builder.RegisterType<TaskListPageViewModel>().SingleInstance();
            builder.RegisterType<DoneListPageViewModel>().SingleInstance();
            builder.RegisterType<SettingsPageViewModel>().SingleInstance();
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
