﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleChecklist.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpenFilePickerView : ContentPage
    {
        private readonly IDirectory _directory;
        private readonly MainView _mainView;
        private readonly OpenFilePickerViewModel _openFilePickerViewModel;
        private TaskCompletionSource<string> _tcs;

        public OpenFilePickerView(IDirectory directory, OpenFilePickerViewModel openFilePickerViewModel,
            MainView mainView)
        {
            InitializeComponent();

            openFilePickerViewModel.FileChoosen += s => _tcs.SetResult(s);
            _directory = directory;
            _openFilePickerViewModel = openFilePickerViewModel;
            _mainView = mainView;
            BindingContext = openFilePickerViewModel;
        }

        public async Task<string> ShowAsync(IEnumerable<string> allowedFileTypes)
        {
            _openFilePickerViewModel.AllowedFileTypes = allowedFileTypes;
            _openFilePickerViewModel.ChangeListedDirectory(_directory);

            _tcs = new TaskCompletionSource<string>();

            await _mainView.Navigation.PushAsync(this);

            var result = await _tcs.Task;

            await _mainView.Navigation.PopAsync();

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