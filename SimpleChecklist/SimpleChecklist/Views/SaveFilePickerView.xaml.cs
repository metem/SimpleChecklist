﻿using System;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class SaveFilePickerView : ContentPage
    {
        private readonly SaveFilePickerViewModel _saveFilePickerViewModel;
        private TaskCompletionSource<string> _tcs;

        public SaveFilePickerView(IDirectory directory, SaveFilePickerViewModel saveFilePickerViewModel)
        {
            InitializeComponent();

            saveFilePickerViewModel.ChangeListedDirectory(directory);
            saveFilePickerViewModel.FileChoosen += s => _tcs.SetResult(s);
            _saveFilePickerViewModel = saveFilePickerViewModel;
            BindingContext = saveFilePickerViewModel;
        }

        public async Task<string> ShowAsync(string defaultFileName)
        {
            InputEntry.Text = defaultFileName;

            _tcs = new TaskCompletionSource<string>();

            await Navigation.PushAsync(this);

            var result = await _tcs.Task;

            Navigation.RemovePage(this);

            return result;
        }

        private void SaveEntryOnCompleted(object sender, EventArgs e)
        {
            _saveFilePickerViewModel.AddClickCommand.Execute(e);
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