using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class FilePickerView : ContentPage
    {
        private readonly INavigation _navigation;
        private TaskCompletionSource<string> _tcs;

        public FilePickerView(INavigation navigation, IDirectory directory, FilePickerViewModel filePickerViewModel)
        {
            _navigation = navigation;
            InitializeComponent();

            filePickerViewModel.ChangeListedDirectory(directory);
            filePickerViewModel.FileChoosen += s => _tcs.SetResult(s);
            BindingContext = filePickerViewModel;
        }

        public async Task<string> ShowAsync()
        {
            _tcs = new TaskCompletionSource<string>();

            await _navigation.PushModalAsync(this);

            var result = await _tcs.Task;

            await _navigation.PopModalAsync();

            return result;
        }
    }
}
