using Autofac;
using SimpleChecklist.Core;
using SimpleChecklist.UI.Commands;
using SimpleChecklist.Universal;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SimpleChecklist.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            MoveDataFile();
            _container = BootstrapperUniversal.Configure();
            InitializeComponent();
            Suspending += OnSuspending;
        }

        private static void MoveDataFile()
        {
            Task.Run(async () =>
            {
                try
                {
                    var dir = await StorageFolder.GetFolderFromPathAsync(ApplicationData.Current.LocalFolder.Path);
                    var appData = await dir.GetFileAsync(AppSettings.ApplicationDataFileName);
                    await appData.CopyAsync(
                        await StorageFolder.GetFolderFromPathAsync(ApplicationData.Current.LocalCacheFolder.Path),
                        $"{appData.Name}_",
                        NameCollisionOption.GenerateUniqueName);
                    await appData.MoveAsync(await StorageFolder.GetFolderFromPathAsync(ApplicationData.Current.LocalCacheFolder.Path));
                    foreach (var file in await dir.GetFilesAsync())
                    {
                        await file.MoveAsync(await StorageFolder.GetFolderFromPathAsync(ApplicationData.Current.LocalCacheFolder.Path));
                    }
                }
                catch { }
            }).Wait();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Xamarin.Forms.Forms.Init(e);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Content = _container.Resolve<MainWindowsPage>();
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            Task.Run(_container.Resolve<SaveApplicationDataCommand>().ExecuteAsync).Wait();
            deferral.Complete();
        }
    }
}