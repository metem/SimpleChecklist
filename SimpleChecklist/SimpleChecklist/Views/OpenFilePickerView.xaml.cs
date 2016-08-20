using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class OpenFilePickerView : ContentPage
    {
        private readonly IDirectory _directory;
        private readonly OpenFilePickerViewModel _openFilePickerViewModel;
        private TaskCompletionSource<string> _tcs;

        public OpenFilePickerView(IDirectory directory, OpenFilePickerViewModel openFilePickerViewModel)
        {
            InitializeComponent();

            openFilePickerViewModel.FileChoosen += s => _tcs.SetResult(s);
            _directory = directory;
            _openFilePickerViewModel = openFilePickerViewModel;
            BindingContext = openFilePickerViewModel;
        }

        public async Task<string> ShowAsync(IEnumerable<string> allowedFileTypes)
        {
            _openFilePickerViewModel.AllowedFileTypes = allowedFileTypes;
            _openFilePickerViewModel.ChangeListedDirectory(_directory);

            _tcs = new TaskCompletionSource<string>();

            await Navigation.PushAsync(this);

            var result = await _tcs.Task;

            Navigation.RemovePage(this);

            return result;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (!_tcs.Task.IsCompleted)
                _tcs.SetResult(null);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _openFilePickerViewModel.FileChoosenCommand.Execute(e.Item);
        }
    }
}