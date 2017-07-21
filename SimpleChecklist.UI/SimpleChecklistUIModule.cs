using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using SimpleChecklist.Core;
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
        }
    }
}