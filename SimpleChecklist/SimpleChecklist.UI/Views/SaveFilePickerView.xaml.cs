using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.UI.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class SaveFilePickerView : ContentPage
    {
        private readonly MainView _mainView;
        private readonly SaveFilePickerViewModel _saveFilePickerViewModel;
        private TaskCompletionSource<string> _tcs;

        public SaveFilePickerView(IDirectory directory, SaveFilePickerViewModel saveFilePickerViewModel,
            MainView mainView)
        {
            saveFilePickerViewModel.ChangeListedDirectory(directory);
            saveFilePickerViewModel.FileChoosen += (s) => _tcs.SetResult(s);
            _saveFilePickerViewModel = saveFilePickerViewModel;
            _mainView = mainView;
            BindingContext = saveFilePickerViewModel;

            InitializeComponent();
        }

        public async Task<string> ShowAsync(string defaultFileName, string extension)
        {
            _saveFilePickerViewModel.Extension = extension;
            _saveFilePickerViewModel.FileName = defaultFileName;

            _tcs = new TaskCompletionSource<string>();

            await _mainView.Navigation.PushAsync(this, false);

            var result = await _tcs.Task;

            await _mainView.Navigation.PopToRootAsync(false);

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
            _saveFilePickerViewModel.FileChoosenCommand.Execute(e.Item);
        }

        private void SaveEntryOnCompleted(object sender, EventArgs e)
        {
            _saveFilePickerViewModel.SaveCommand.Execute(null);
        }
    }
}