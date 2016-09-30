using System;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class SaveFilePickerView : ContentPage
    {
        private readonly MainView _mainView;
        private readonly SaveFilePickerViewModel _saveFilePickerViewModel;
        private TaskCompletionSource<string> _tcs;

        public SaveFilePickerView(IDirectory directory, SaveFilePickerViewModel saveFilePickerViewModel,
            MainView mainView)
        {
            InitializeComponent();

            saveFilePickerViewModel.ChangeListedDirectory(directory);
            saveFilePickerViewModel.FileChoosen += s => _tcs.SetResult(s);
            _saveFilePickerViewModel = saveFilePickerViewModel;
            _mainView = mainView;
            BindingContext = saveFilePickerViewModel;
        }

        public async Task<string> ShowAsync(string defaultFileName, string extension)
        {
            _saveFilePickerViewModel.Extension = extension;
            _saveFilePickerViewModel.FileName = defaultFileName;

            _tcs = new TaskCompletionSource<string>();

            await _mainView.Navigation.PushAsync(this);

            var result = await _tcs.Task;

            await _mainView.Navigation.PopAsync();

            return result;
        }

        private void SaveEntryOnCompleted(object sender, EventArgs e)
        {
            _saveFilePickerViewModel.SaveClick();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (!_tcs.Task.IsCompleted)
                _tcs.SetResult(null);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            _saveFilePickerViewModel.FileChoosenCommand.Execute(e.Item);
        }
    }
}